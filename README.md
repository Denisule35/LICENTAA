# LICENTAA — Gimnaziu / Sports Club Management System

A dual-component management application for a sports club / gym:

- **WPF Desktop App (Modern/)** — internal server, attendance validation, member/finance/inventory management
- **ASP.NET Core Web API (WebApi/)** — HTTP bridge that external clients (web browser, mobile) use to check subscriptions remotely

---

## Table of Contents

1. [Architecture](#architecture)
2. [Prerequisites](#prerequisites)
3. [IP Configuration — Read This First](#ip-configuration--read-this-first)
4. [Startup Order](#startup-order)
5. [Running the Application](#running-the-application)
6. [How the Subscription Flow Works](#how-the-subscription-flow-works)
7. [Database](#database)
8. [Project Structure](#project-structure)
9. [Features](#features)
10. [Troubleshooting](#troubleshooting)
11. [License](#license)

---

## Architecture

```
+-------------------+          POST /api/check-subscription          +-------------------+
|   WebApi (port    | -----------------------------------------------> |   WPF App         |
|   5152)           |                                                |   (port 8000)     |
|   ASP.NET Core    |  <---------------------------------------------- |   HttpListener    |
|   + Static Files  |        "Valid Subscription" / "Invalid"       |   + SQLite DB     |
+-------------------+                                                +-------------------+
        |                                                                    |
        | (browser / mobile)                                                |
        v                                                                    v
   index.html                                                            Admin UI
   test page                                                             (WPF)
```

**Two programs must run together:**

1. **Modern (WPF)** — the core application. Starts an embedded HTTP server on `localhost:8000`. Manages the SQLite database. Provides the admin interface.
2. **WebApi (ASP.NET Core)** — a thin API layer. Receives external requests and forwards them to the WPF app's server. Serves a test web page.

---

## Prerequisites

| Component | Version | Notes |
|-----------|---------|-------|
| .NET SDK | 9.0 or later | [Download](https://dotnet.microsoft.com/download) |
| OS | Windows | WPF requires Windows |
| IDE (optional) | Visual Studio 2022 or VS Code | `dotnet` CLI works without IDE |

Verify your installation:

```bash
dotnet --version
# Should print 9.0.x
```

---

## IP Configuration — Read This First

**You must update hardcoded IP addresses before running.** The codebase currently contains your machine's previous LAN IP (`192.168.100.4`) in multiple places. You need to replace it with your **current** LAN IP.

### Step 1: Find your LAN IP

Open Command Prompt and run:

```cmd
ipconfig
```

Look for your active network adapter and find the **IPv4 Address** (e.g., `192.168.1.XX` or `10.0.0.XX`). That is your IP.

### Step 2: Update the three files

#### File 1: `WebApi/Properties/launchSettings.json`

```json
"applicationUrl": "http://localhost:5152;http://0.0.0.0:5152;http://<YOUR_IP>:5152"
```

Replace `<YOUR_IP>` with your LAN IP. The `0.0.0.0` binding allows the API to accept connections from any network interface.

#### File 2: `WebApi/wwwroot/index.html`

```javascript
const fetchResponse = await fetch("http://<YOUR_IP>:5152/api/check-subscription", {
```

Replace `<YOUR_IP>` with the same LAN IP. This is the test page's endpoint.

#### File 3: `WebApi/Controllers/SubscriptionController.cs`

```csharp
var response = await _httpClient.PostAsync("http://localhost:8000/server/", content);
```

**Leave this as `localhost:8000`** — it is the internal communication between the WebApi and the WPF app's embedded server. Both run on the same machine, so `localhost` is correct. Only change this if the WPF app runs on a **different machine** from the WebApi.

### Why these IPs matter

- **`192.168.x.x:5152`** — this is how external devices (phone, another computer's browser) reach the WebApi. If the IP is wrong, the test page and any remote client will get a connection error.
- **`localhost:8000`** — this is how the WebApi reaches the WPF app's internal server. It must stay `localhost` when both run on the same machine.

---

## Startup Order

**Start the WPF app first, then the WebApi.**

```
1. WPF (Modern)  →  starts HttpListener on port 8000
2. WebApi        →  connects to localhost:8000
```

If you start WebApi first, it will fail to forward requests because nothing is listening on port 8000 yet.

---

## Running the Application

### Option A: Visual Studio 2022 (Recommended)

1. Open `LICENTAA.sln` in Visual Studio 2022
2. Set both projects to start:
   - Right-click the solution → **Set StartUp Projects…**
   - Select **Multiple startup projects**
   - Set **Modern** = **Start**
   - Set **WebApi** = **Start**
   - Use the **up/down arrows** to ensure **Modern** is above **WebApi** (starts first)
3. Press **F5** or click **Start**

Visual Studio will launch both projects. The WPF login window appears first; the WebApi console window starts alongside it.

### Option B: .NET CLI (Two terminals)

**Terminal 1 — WPF App:**

```bash
cd /c/Users/Denis/LICENTAA/Modern
dotnet run
```

The WPF login window opens. The embedded HTTP server starts on port 8000.

**Terminal 2 — WebApi:**

```bash
cd /c/Users/Denis/LICENTAA/WebApi
dotnet run
```

The WebApi starts on `http://localhost:5152`. The console shows the assigned URL.

### Option C: Build and Run the Executables

```bash
# Build both projects
cd /c/Users/Denis/LICENTAA
dotnet build LICENTAA.sln -c Release

# Run WPF app
cd Modern/bin/Release/net9.0-windows
./Modern.exe

# In a separate terminal, run WebApi
cd /c/Users/Denis/LICENTAA/WebApi/bin/Release/net9.0
./WebApi.exe
```

---

## How the Subscription Flow Works

This is the heart of the system — the communication chain between external clients and the attendance system.

```
[Browser / Mobile]              [WebApi]                      [WPF App]
     |                              |                              |
     |  POST name + deviceId        |                              |
     |  to http://<IP>:5152/        |                              |
     |  api/check-subscription       |                              |
     |----------------------------->|                              |
     |                              |  Forward to                  |
     |                              |  localhost:8000/server/      |
     |                              |----------------------------->|
     |                              |                              |  Check SQLite:
     |                              |                              |  - Is person in Oameni?
     |                              |                              |  - Is Abonament < 1 month old?
     |                              |                              |
     |                              |<-----------------------------|  Return:
     |                              |  "Valid Subscription"        |  or
     |                              |  "Invalid Subscription"      |
     |                              |                              |  If valid:
     |                              |                              |  - Add name to today's Prezenta
     |                              |                              |  - Remove from Absenti list
     |<-----------------------------|                              |
     |  "Valid Subscription"        |                              |
     |  or "Invalid Subscription"   |                              |
```

**Device ID locking:** Each device (browser) generates a unique `deviceId` stored in `localStorage`. Once a device validates a subscription, it is **locked** — it cannot validate another person's subscription. This prevents one person from checking in multiple members from the same device.

**Subscription expiry:** A member's subscription is valid for **1 month** from the `Abonament` date. After that, the system returns `"Invalid Subscription"`.

---

## Database

**SQLite** — file-based, zero configuration.

- **File:** `DataFile.db` — auto-created in the WPF app's output directory on first run
- **ORM:** Entity Framework Core with `Microsoft.EntityFrameworkCore.Sqlite`
- **No migrations needed** — EF Core creates tables automatically from the model classes on first use

### Tables

| Table | Purpose |
|-------|---------|
| `Users` | Login credentials (username, password) — plain text |
| `Oameni` | Club members: name, subscription date, level, strengths/weaknesses, punch stats, attendance percentage |
| `Prezenti` | Daily attendance: date, list of present members, list of absent members |
| `Articole` | Inventory items: name, sell price, purchase price, stock count, photo flag |
| `Tranzactii` | Financial transactions: description, date, amount, income/expense flag |

### Seeding the Database

The database is created empty on first run. You need to add data through the WPF app:

1. **Create an admin account:** The `Users` table starts empty. You need to insert at least one user to log in. This can be done programmatically or by adding a record directly to the SQLite database.

2. **Add members (Oameni):** Through the WPF app's "Adăugare Om" (Add Member) function. Each member gets a subscription date, which starts their 1-month validity window.

3. **Set subscription price:** The `PretAbonament` field in MainWindowViewModel controls the price used for renewal transactions.

4. **Inventory:** Add items through the "Inventar" module.

---

## Project Structure

```
LICENTAA/
├── Modern/
│   ├── Modern.sln              # Solution file
│   ├── Modern.csproj           # WPF project (.NET 9.0-windows)
│   ├── View/                   # XAML windows & user controls
│   │   ├── MainWindow.xaml    # Main dashboard
│   │   ├── Login.xaml         # Login screen
│   │   ├── PrezentaView.xaml  # Attendance management
│   │   ├── FinanteView.xaml   # Finance / transactions
│   │   ├── InventarView.xaml  # Inventory management
│   │   ├── DetaliiSportiviView.xaml  # Athlete detail profiles
│   │   └── ...
│   ├── ViewModel/             # MVVM ViewModels
│   │   ├── MainWindowViewModel.cs   # Starts HttpListener, loads data
│   │   ├── LoginViewModel.cs        # Login logic
│   │   ├── PrezentaViewModel.cs     # Attendance view model
│   │   ├── FinanteViewModel.cs      # Finance view model
│   │   ├── InventarViewModel.cs     # Inventory view model
│   │   ├── DetaliSportiviViewModel.cs  # Athlete stats & chart
│   │   ├── OameniViewModel.cs       # Member row model
│   │   └── ...
│   ├── Model/                 # Data models & server
│   │   ├── Bazadateconnect.cs # EF Core DbContext (SQLite)
│   │   ├── HttpServer.cs      # Embedded HttpListener (port 8000)
│   │   ├── Oameni.cs          # Member entity
│   │   ├── Prezenta.cs        # Attendance entity
│   │   ├── Inventar.cs        # Inventory entity
│   │   ├── Tranzactie.cs      # Transaction entity
│   │   └── User.cs            # Login entity
│   ├── Commands/              # MVVM commands (ICommand implementations)
│   │   ├── LoginCommand.cs
│   │   ├── AdaugareOmCommand.cs
│   │   ├── StergereOmCommand.cs
│   │   ├── RenoireAbonamentCommand.cs
│   │   ├── VanzareArticolCommand.cs
│   │   └── ...
│   └── imagini/              # Image assets (member photos, icons)
│       ├── bec.png, gym.jpg, kickboxer.png, somo.jpg, ...
│       └── placeholder.png, placeholderarticole.png
│
├── WebApi/
│   ├── WebApi.csproj          # ASP.NET Core project (.NET 9.0)
│   ├── Program.cs             # Entry point, configures services & pipeline
│   ├── Controllers/
│   │   └── SubscriptionController.cs  # POST /api/check-subscription
│   ├── wwwroot/
│   │   └── index.html         # Test page for subscription checking
│   ├── Properties/
│   │   └── launchSettings.json  # Ports & bindings (EDIT YOUR IP HERE)
│   ├── appsettings.json       # Logging configuration
│   └── WebApi.http           # Visual Studio HTTP test file
│
└── .gitignore
```

---

## Features

### Attendance (Prezenta)
- Daily attendance sheets — one record per day
- Members start as absent; subscription validation via web API marks them present
- View attendance history by day
- Tracks present/absent counts per member over time

### Member Management (Oameni)
- Add new members with subscription date
- Remove members
- Renew subscriptions (records a financial transaction)
- Subscription expiry highlighted in red (expired) or gold (active)

### Finance (Finante / Tranzactii)
- Monthly finance views
- Income and expense tracking
- Add manual transactions (income or expense)
- Net balance calculation per month
- Subscription renewals auto-generate transactions

### Inventory (Inventar)
- Add, edit, delete items
- Track stock levels
- Sell items (reduces stock, creates income transaction)
- Purchase items (increases stock, creates expense transaction)
- Associate photos with items (saved in `imagini/` folder)

### Athlete Profiles (Detalii Sportivi)
- Technical level (Începător → Elit)
- Strengths and weaknesses lists
- Punch statistics: hits to head/body, missed punches, punches landed
- Attendance percentage
- Win/loss/draw counts
- Pie chart visualization of punch statistics (LiveCharts2 / SkiaSharp)

### Admin Management
- Add and remove admins/trainers

### AI Analysis (Analiza AI)
- Filtered view of all members for AI-based analysis

### Theme Toggle
- Dark theme (default) and light theme
- Applied across all views consistently

### Web Subscription Check
- External clients (browser, mobile) can check subscription validity
- Device-level locking prevents duplicate validations
- Returns plain text: `"Hello {name}! Valid Subscription"` or `"Hello {name}! Invalid Subscription"`

---

## Troubleshooting

### "Error communicating with WPF server" / Connection refused on port 8000

**Cause:** The WPF app is not running, or it hasn't finished starting the HttpListener yet.

**Fix:**
1. Make sure the WPF app (Modern) is running — the login window should be visible
2. Wait a few seconds after the login window appears for the server to start
3. Check that nothing else is using port 8000:
   ```bash
   netstat -ano | findstr :8000
   ```
4. Restart the WPF app

### WebApi fails to start — port 5152 already in use

**Fix:**
```bash
netstat -ano | findstr :5152
```
Kill the process using the port, or change the port in `launchSettings.json`.

### Login fails — "Usernameul sau Parola este grestia"

**Cause:** The `Users` table is empty — no accounts have been created yet.

**Fix:** Insert a user directly into the SQLite database, or add user creation logic to the application:

```bash
# Using the SQLite CLI (if installed)
sqlite3 Modern/bin/Debug/net9.0-windows/DataFile.db
INSERT INTO Users (name, password) VALUES ('admin', 'admin');
.exit
```

Or add a user via code in `LoginCommand.cs` / a setup screen.

### Cannot access WebApi from phone / another computer

**Cause:** Wrong IP in `launchSettings.json` or `index.html`, or firewall blocking port 5152.

**Fix:**
1. Verify the IP in both files matches your current LAN IP (`ipconfig`)
2. Windows Firewall may block port 5152 — allow `dotnet.exe` through the firewall for private networks
3. Make sure the WebApi is listening on `0.0.0.0` (not just `localhost`) — check `launchSettings.json` has `http://0.0.0.0:5152`

### `DataFile.db` not found or database errors

**Cause:** The database file is created in the output directory next to the executable. If you run from a different working directory, the path may resolve differently.

**Fix:** Check that `Bazadateconnect.cs` uses a relative path (`Data Source = DataFile.db`) which resolves to the application's working directory. Run the application from its output folder.

### Images not showing for inventory items or athletes

**Cause:** Photo files must be in the `imagini/` folder next to the executable, named exactly as the member/item name with a supported extension (`.jpg`, `.jpeg`, `.png`, `.jfif`, `.webp`).

**Fix:** Place photos in the `imagin

───

───ii/` folder in the output directory. For members: `imagini/{MemberName}.jpg`. For inventory items: `imagini/{ItemName}.png` etc.

---

## License

This project is a personal academic application (licență — bachelor's thesis). Adjust licensing as needed for your use case.

---

*ReadME generated after full code review — BALTHASAR-2 / MAGI System, NERV Headquarters.*
