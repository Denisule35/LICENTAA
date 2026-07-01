using System.Text.Json.Serialization;


namespace Modern.Model
{
    public class FighterStats
    {

        [JsonPropertyName("head_landed")]
        public int HeadLanded { get; set; }

        [JsonPropertyName("body_landed")]
        public int BodyLanded { get; set; }

        [JsonPropertyName("total_landed")]
        public int TotalLanded { get; set; }

        [JsonPropertyName("head_received")]
        public int HeadReceived { get; set; }

        [JsonPropertyName("body_received")]
        public int BodyReceived { get; set; }

        [JsonPropertyName("total_received")]
        public int TotalReceived { get; set; }

        [JsonPropertyName("head_missed")]
        public int HeadMissed { get; set; }

        [JsonPropertyName("body_missed")]
        public int BodyMissed { get; set; }

        [JsonPropertyName("total_missed")]
        public int TotalMissed { get; set; }
    }

    public class FightResults
    {
        [JsonPropertyName("output_video")]
        public string OutputVideo { get; set; }

        [JsonPropertyName("fighter1")]
        public string Fighter1 { get; set; }

        [JsonPropertyName("fighter2")]
        public string Fighter2 { get; set; }

        [JsonPropertyName("results")]
        public Dictionary<string, FighterStats> Results { get; set; }
    }

}

