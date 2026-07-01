from ultralytics.models.sam import SAM2VideoPredictor
from ultralytics import YOLO
import cv2
import numpy as np
import json


# ── Models ────────────────────────────────────────────────────────────────────
punch_model = YOLO('topaiever.pt')

# ── Colors ────────────────────────────────────────────────────────────────────
COLORS = {0: (0, 255, 0), 1: (0, 0, 255)}

# ── Tuning ────────────────────────────────────────────────────────────────────
PUNCH_CONF = 0.4
HIT_COOLDOWN = 10
ATTACKER_OVERLAP = 0.20
DEFENDER_OVERLAP = 0.15
ATTACKER_DEFENDER_MARGIN = 0.15


# ─────────────────────────────────────────────────────────────────────────────
# Geometry helpers
# ─────────────────────────────────────────────────────────────────────────────
def box_iou_with_mask(box, mask):
   
    x1, y1, x2, y2 = [int(v) for v in box]
    x1 = max(0, x1)
    y1 = max(0, y1)
    x2 = min(mask.shape[1], x2)
    y2 = min(mask.shape[0], y2)

    if x2 <= x1 or y2 <= y1:
        return 0.0

    crop = mask[y1:y2, x1:x2]
    overlap_pixels = int(crop.sum())
    box_pixels = (x2 - x1) * (y2 - y1)

    return overlap_pixels / max(1, box_pixels)


def get_head_box(mask, head_fraction=0.25):
    
    ys, xs = np.where(mask > 0)
    if len(ys) == 0:
        return None

    y_min = int(ys.min())
    y_max = int(ys.max())
    x_min = int(xs.min())
    x_max = int(xs.max())

    head_y_max = y_min + int((y_max - y_min) * head_fraction)
    return (x_min, y_min, x_max, head_y_max)


def get_body_box(mask, head_fraction=0.25, body_end_fraction=0.80):
    """Bounding box of the body zone (between head and legs)."""
    ys, xs = np.where(mask > 0)
    if len(ys) == 0:
        return None

    y_min = int(ys.min())
    y_max = int(ys.max())
    x_min = int(xs.min())
    x_max = int(xs.max())

    height = y_max - y_min
    body_y_min = y_min + int(height * head_fraction)
    body_y_max = y_min + int(height * body_end_fraction)

    return (x_min, body_y_min, x_max, body_y_max)


def box_overlap_fraction(box_a, box_b):
    """Fraction of box_a that overlaps with box_b."""
    ax1, ay1, ax2, ay2 = [int(v) for v in box_a]
    bx1, by1, bx2, by2 = [int(v) for v in box_b]

    ix1 = max(ax1, bx1)
    iy1 = max(ay1, by1)
    ix2 = min(ax2, bx2)
    iy2 = min(ay2, by2)

    if ix2 <= ix1 or iy2 <= iy1:
        return 0.0

    inter = (ix2 - ix1) * (iy2 - iy1)
    area_a = max(1, (ax2 - ax1) * (ay2 - ay1))
    return inter / area_a


def draw_mask_overlay(frame, mask, color, alpha=0.2):
    overlay = frame.copy()
    overlay[mask > 0] = color
    cv2.addWeighted(overlay, alpha, frame, 1 - alpha, 0, frame)


def get_box_center(box):
    x1, y1, x2, y2 = box
    return ((x1 + x2) / 2, (y1 + y2) / 2)


# ─────────────────────────────────────────────────────────────────────────────
# Main Analysis Function
# ─────────────────────────────────────────────────────────────────────────────
def analyze_fight(
    video_path: str,
    fighter1_name: str,
    fighter2_name: str,
    output_path: str = "output_hit_tracking.mp4"
) -> str:
    """
    Analyze a fight video and track punches for two fighters.
    
    Args:
        video_path: Path to the input video file
        fighter1_name: Name of the first fighter
        fighter2_name: Name of the second fighter
        output_path: Path for the output video (default: "output_hit_tracking.mp4")
    
    Returns:
        JSON string containing fight statistics for both fighters
    """
    
    FIGHTER_NAMES = {0: fighter1_name, 1: fighter2_name}
    
    prev_punch_centers = []
    
    # Hit state tracks head and body shots separately plus missed punches
    hit_state = {
        0: {
            "head_landed": 0, "body_landed": 0,
            "head_received": 0, "body_received": 0,
            "head_missed": 0, "body_missed": 0,
            "last_hit": -999, "flash": 0
        },
        1: {
            "head_landed": 0, "body_landed": 0,
            "head_received": 0, "body_received": 0,
            "head_missed": 0, "body_missed": 0,
            "last_hit": -999, "flash": 0
        },
    }

    # ─────────────────────────────────────────────────────────────────────────
    # Punch attribution function
    # ─────────────────────────────────────────────────────────────────────────
    def attribute_punch(punch_box, sam_masks, frame_idx):
        """Improved attacker detection with STRICT mutual exclusion."""
        nonlocal prev_punch_centers

        if len(sam_masks) < 2:
            return None, None, None

        # ── Compute current punch center ─────────────────────────────────────
        curr_center = get_box_center(punch_box)

        # Find closest previous center (simple tracking)
        prev_center = None
        min_dist = float('inf')
        for pc in prev_punch_centers:
            d = (pc[0] - curr_center[0]) ** 2 + (pc[1] - curr_center[1]) ** 2
            if d < min_dist:
                min_dist = d
                prev_center = pc

        # Motion direction
        dx = 0
        if prev_center is not None:
            dx = curr_center[0] - prev_center[0]

        # Save for next frame
        prev_punch_centers.append(curr_center)
        if len(prev_punch_centers) > 20:
            prev_punch_centers.pop(0)

        # ── Compute overlap scores ───────────────────────────────────────────
        overlap_scores = {}
        for fid, mask in sam_masks.items():
            overlap_scores[fid] = box_iou_with_mask(punch_box, mask)

        # ── Compute direction scores ─────────────────────────────────────────
        direction_scores = {}

        # Get fighter centers
        fighter_centers = {}
        for fid, mask in sam_masks.items():
            ys, xs = np.where(mask > 0)
            if len(xs) == 0:
                continue
            cx = xs.mean()
            fighter_centers[fid] = cx

        for fid, cx in fighter_centers.items():
            score = 0.0

            if prev_center is not None:
                if dx > 2 and cx < curr_center[0]:
                    score = 0.8
                elif dx < -2 and cx > curr_center[0]:
                    score = 0.8

            direction_scores[fid] = score

        # ── Combine scores ───────────────────────────────────────────────────
        combined_scores = {}
        for fid in sam_masks.keys():
            overlap = overlap_scores.get(fid, 0.0)
            direction = direction_scores.get(fid, 0.0)
            combined_scores[fid] = (0.8 * overlap) + (0.4 * direction)

        # ── Select attacker with STRICT validation ───────────────────────────
        sorted_fighters = sorted(combined_scores.items(), key=lambda x: x[1], reverse=True)

        if not sorted_fighters:
            return None, None, None

        attacker_id = sorted_fighters[0][0]
        attacker_score = sorted_fighters[0][1]

        if attacker_score < ATTACKER_OVERLAP:
            return None, None, None

        if len(sorted_fighters) >= 2:
            second_score = sorted_fighters[1][1]
            if attacker_score - second_score < ATTACKER_DEFENDER_MARGIN:
                return None, None, None

        # ── Defender detection with STRICT exclusion ─────────────────────────
        defender_scores = {}
        hit_types = {}

        for fid, mask in sam_masks.items():
            if fid == attacker_id:
                continue

            head_box = get_head_box(mask)
            head_overlap = box_overlap_fraction(punch_box, head_box) if head_box else 0

            body_box = get_body_box(mask)
            body_overlap = box_overlap_fraction(punch_box, body_box) if body_box else 0

            if head_overlap >= body_overlap:
                defender_scores[fid] = head_overlap
                hit_types[fid] = 'head'
            else:
                defender_scores[fid] = body_overlap
                hit_types[fid] = 'body'

        if not defender_scores:
            return attacker_id, None, None

        defender_id = max(defender_scores, key=defender_scores.get)

        if defender_scores[defender_id] < DEFENDER_OVERLAP:
            return attacker_id, None, None

        if attacker_id == defender_id:
            return None, None, None

        hit_type = hit_types[defender_id]

        return attacker_id, defender_id, hit_type

    # ─────────────────────────────────────────────────────────────────────────
    # Hit registration function
    # ─────────────────────────────────────────────────────────────────────────
    def register_hit(attacker_id, defender_id, hit_type, frame_idx):
        """Record hit if both fighters identified and cooldown passed."""
        if attacker_id is None or defender_id is None or hit_type is None:
            return False

        if attacker_id == defender_id:
            return False

        attacker_state = hit_state[attacker_id]
        defender_state = hit_state[defender_id]

        if frame_idx - attacker_state["last_hit"] < HIT_COOLDOWN:
            return False

        if frame_idx - defender_state["last_hit"] < HIT_COOLDOWN // 2:
            return False

        attacker_state["last_hit"] = frame_idx

        if hit_type == 'head':
            attacker_state["head_landed"] += 1
            defender_state["head_received"] += 1
        else:
            attacker_state["body_landed"] += 1
            defender_state["body_received"] += 1

        defender_state["flash"] = 12
        return True

    # ─────────────────────────────────────────────────────────────────────────
    # Missed punch tracking function
    # ─────────────────────────────────────────────────────────────────────────
    def register_missed_punch(attacker_id, hit_type):
        """Track missed punches (punch detected but no defender hit)."""
        if attacker_id is None or hit_type is None:
            return

        if hit_type == 'head':
            hit_state[attacker_id]["head_missed"] += 1
        else:
            hit_state[attacker_id]["body_missed"] += 1

    # ─────────────────────────────────────────────────────────────────────────
    # Select fighters
    # ─────────────────────────────────────────────────────────────────────────
    cap = cv2.VideoCapture(video_path)
    
    ret, frame0 = cap.read()
    
    if not ret:
        raise Exception("Could not read video")
    
    cap.release()

    x, y, w, h = cv2.selectROI(
        f"Select {fighter1_name}",
        frame0,
        showCrosshair=False
    )

    x2, y2, w2, h2 = cv2.selectROI(
        f"Select {fighter2_name}",
        frame0,
        showCrosshair=False
    )

    cv2.destroyAllWindows()

    bboxes = [
        [x, y, x + w, y + h],
        [x2, y2, x2 + w2, y2 + h2]
    ]

    points = [
        [x + w // 2, y + h // 2],
        [x2 + w2 // 2, y2 + h2 // 2]
    ]

    sam_labels = [1, 1]

    # ─────────────────────────────────────────────────────────────────────────
    # SAM2 Setup
    # ─────────────────────────────────────────────────────────────────────────
    overrides = dict(
        conf=0.25, task="segment", mode="predict",
        imgsz=512, model="sam2.1_l.pt",
        half=True, int8=False, batch=4,
        vid_stride=1, device="cuda:0", save=False,
    )

    predictor = SAM2VideoPredictor(overrides=overrides)
    
    sam_results = predictor(
        source=video_path,
        bboxes=bboxes, points=points, labels=sam_labels,
        stream=True,
    )

    cap_info = cv2.VideoCapture(video_path)
    
    fps = cap_info.get(cv2.CAP_PROP_FPS) or 30
    h_f = int(cap_info.get(cv2.CAP_PROP_FRAME_HEIGHT))
    w_f = int(cap_info.get(cv2.CAP_PROP_FRAME_WIDTH))
    
    cap_info.release()

    out = cv2.VideoWriter(
        output_path,
        cv2.VideoWriter_fourcc(*"mp4v"),
        fps, (w_f, h_f)
    )

    frame_idx = 0

    # ─────────────────────────────────────────────────────────────────────────
    # Main processing loop
    # ─────────────────────────────────────────────────────────────────────────
    for sam_result in sam_results:
        frame = sam_result.orig_img.copy()

        # ── Extract SAM2 masks + boxes ────────────────────────────────────────
        sam_masks = {}
        sam_boxes = {}

        if sam_result.masks is not None and sam_result.boxes is not None:
            masks_data = sam_result.masks.data.cpu().numpy()
            obj_ids = sam_result.boxes.cls.cpu().numpy().astype(int)
            boxes_data = sam_result.boxes.xyxy.cpu().numpy().astype(int)

            for obj_id, mask, box in zip(obj_ids, masks_data, boxes_data):
                if obj_id not in FIGHTER_NAMES:
                    continue
                if mask.shape != (h_f, w_f):
                    mask = cv2.resize(
                        mask.astype(np.uint8), (w_f, h_f),
                        interpolation=cv2.INTER_NEAREST
                    )
                sam_masks[obj_id] = mask.astype(np.uint8)
                sam_boxes[obj_id] = box

        # ── SAM2 silhouettes ──────────────────────────────────────────────────
        for fid, mask in sam_masks.items():
            draw_mask_overlay(frame, mask, COLORS[fid], alpha=0.2)

        # ── Run punch detector ────────────────────────────────────────────────
        punch_results = punch_model(frame, verbose=False, conf=PUNCH_CONF)
        punch_boxes_list = []

        if punch_results and punch_results[0].boxes is not None:
            for box in punch_results[0].boxes:
                cls_id = int(box.cls[0])
                name = punch_model.names[cls_id]
                if name == 'punch':
                    x1, y1, x2, y2 = box.xyxy[0].cpu().numpy()
                    conf = float(box.conf[0])
                    punch_boxes_list.append((x1, y1, x2, y2, conf))

        # ── Attribute punches ─────────────────────────────────────────────────
        active_punch_boxes = []

        for (x1, y1, x2, y2, conf) in punch_boxes_list:
            pbox = (x1, y1, x2, y2)
            attacker_id, defender_id, hit_type = attribute_punch(pbox, sam_masks, frame_idx)
            hit = register_hit(attacker_id, defender_id, hit_type, frame_idx)

            # Track missed punches
            if attacker_id is not None and not hit:
                missed_type = hit_type if hit_type else 'body'
                register_missed_punch(attacker_id, missed_type)

            active_punch_boxes.append({
                "box": pbox,
                "conf": conf,
                "attacker_id": attacker_id,
                "defender_id": defender_id,
                "hit_type": hit_type,
                "hit": hit,
            })

        # ── Draw fighters ─────────────────────────────────────────────────────
        for fid, box in sam_boxes.items():
            color = COLORS[fid]
            state = hit_state[fid]
            name = FIGHTER_NAMES[fid]
            mask = sam_masks.get(fid)

            if state["flash"] > 0 and mask is not None:
                overlay = frame.copy()
                overlay[mask > 0] = (0, 0, 255)
                cv2.addWeighted(overlay, 0.5, frame, 0.5, 0, frame)
                state["flash"] -= 1

            if mask is not None:
                hbox = get_head_box(mask)
                if hbox:
                    hx1, hy1, hx2, hy2 = hbox
                    cv2.rectangle(frame, (hx1, hy1), (hx2, hy2), (255, 255, 0), 1)

                bbox = get_body_box(mask)
                if bbox:
                    bx1, by1, bx2, by2 = bbox
                    cv2.rectangle(frame, (bx1, by1), (bx2, by2), (255, 0, 255), 1)

            x1, y1, x2, y2 = box
            cv2.rectangle(frame, (x1, y1), (x2, y2), color, 2)
            cv2.putText(frame, name,
                        (x1, y1 - 44), cv2.FONT_HERSHEY_SIMPLEX, 0.7, color, 2)

            cv2.putText(frame,
                        f"Head: {state['head_landed']}  Body: {state['body_landed']}",
                        (x1, y1 - 26), cv2.FONT_HERSHEY_SIMPLEX, 0.5, color, 2)
            cv2.putText(frame,
                        f"Received: H:{state['head_received']} B:{state['body_received']}",
                        (x1, y1 - 8), cv2.FONT_HERSHEY_SIMPLEX, 0.5, color, 2)

        # ── Draw punch boxes ──────────────────────────────────────────────────
        for p in active_punch_boxes:
            x1, y1, x2, y2 = [int(v) for v in p["box"]]
            atk = p["attacker_id"]
            dfn = p["defender_id"]
            hit_type = p["hit_type"]
            hit = p["hit"]
            conf = p["conf"]

            if hit:
                box_color = (0, 255, 0)
            elif atk is not None:
                box_color = (0, 255, 255)
            else:
                box_color = (200, 200, 200)

            cv2.rectangle(frame, (x1, y1), (x2, y2), box_color, 2)

            atk_name = FIGHTER_NAMES.get(atk, "?")
            dfn_name = FIGHTER_NAMES.get(dfn, "?")
            label = f"punch {conf:.0%}"

            if hit_type:
                sublabel = f"{atk_name} -> {dfn_name} ({hit_type.upper()})"
            elif atk is not None:
                sublabel = f"{atk_name} -> {dfn_name}"
            else:
                sublabel = "unattributed"

            cv2.putText(frame, label,
                        (x1, y1 - 18), cv2.FONT_HERSHEY_SIMPLEX, 0.55, box_color, 2)
            cv2.putText(frame, sublabel,
                        (x1, y1 - 4), cv2.FONT_HERSHEY_SIMPLEX, 0.45, box_color, 1)

        # ── Scoreboard ────────────────────────────────────────────────────────
        cv2.rectangle(frame, (5, 5), (550, 85), (0, 0, 0), -1)
        for i, (fid, fname) in enumerate(FIGHTER_NAMES.items()):
            s = hit_state[fid]
            total_landed = s['head_landed'] + s['body_landed']
            total_received = s['head_received'] + s['body_received']

            cv2.putText(frame,
                        f"{fname}",
                        (10, 22 + i * 36), cv2.FONT_HERSHEY_SIMPLEX,
                        0.6, (255, 255, 255), 2)
            cv2.putText(frame,
                        f"  Landed: H:{s['head_landed']} B:{s['body_landed']} (Total:{total_landed})  Recv: H:{s['head_received']} B:{s['body_received']} (Total:{total_received})",
                        (10, 38 + i * 36), cv2.FONT_HERSHEY_SIMPLEX,
                        0.45, (255, 255, 255), 1)

        out.write(frame)
        frame_idx += 1

    # ─────────────────────────────────────────────────────────────────────────
    # Cleanup
    # ─────────────────────────────────────────────────────────────────────────
    out.release()
    cv2.destroyAllWindows()

    # ─────────────────────────────────────────────────────────────────────────
    # Return JSON results
    # ─────────────────────────────────────────────────────────────────────────
    results = {}
    for fid, fname in FIGHTER_NAMES.items():
        s = hit_state[fid]
        results[fname] = {
            "head_landed": s['head_landed'],
            "body_landed": s['body_landed'],
            "total_landed": s['head_landed'] + s['body_landed'],
            "head_received": s['head_received'],
            "body_received": s['body_received'],
            "total_received": s['head_received'] + s['body_received'],
            "head_missed": s['head_missed'],
            "body_missed": s['body_missed'],
            "total_missed": s['head_missed'] + s['body_missed']
        }

    output = {
        "output_video": output_path,
        "fighter1": fighter1_name,
        "fighter2": fighter2_name,
        "results": results
    }

    return json.dumps(output, indent=2)


# ─────────────────────────────────────────────────────────────────────────────
# Example usage
# ─────────────────────────────────────────────────────────────────────────────
if __name__ == "__main__":
    # Example call
    result = analyze_fight(
        video_path="goym_1pentruai.mp4",
        fighter1_name="Fighter 1",
        fighter2_name="Fighter 2",
        output_path="output_hit_tracking.mp4"
    )
    
    print(result)