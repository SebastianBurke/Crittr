# Crittr — Claude Code Guide

## Project Overview
Exotic pet companion app for reptile/critter keepers. Full-stack Blazor WebAssembly + ASP.NET Core.

## Dev Server Environment
- Ubuntu Server 24.04 LTS (headless, no GUI) — hostname: sudoserver
- PC specs: NVIDIA RTX 2070 (8GB VRAM), 1TB Crucial SATA SSD
- Server IP: 192.168.0.38 / username: sbsudo
- SSH: `ssh sbsudo@192.168.0.38` (passwordless — SSH key installed)
- VS Code Remote SSH connected to server; C# Dev Kit installed

### Software Stack
- .NET 8 SDK + .NET 9 SDK (both installed via `dotnet-install.sh` at `~/.dotnet`)
- dotnet-ef global tool installed
- dotnet workloads: `maui-tizen`, `wasm-tools-net8`
- Node.js v24.15.0 (via nvm), npm 11.12.1
- Docker Engine 29.4.0 (sbsudo in docker group)
- NVIDIA drivers + CUDA 13.2 (GPU recognized by Ollama)
- tmux, git, curl, build-essential
- Claude Code (npm global, installed on both server and MacBook)

### dotnet Path Setup
Installed at `/home/sbsudo/.dotnet/dotnet`, symlinked to `/usr/local/bin/dotnet`.
`~/.bashrc` exports:
```bash
export PATH=$PATH:$HOME/.dotnet
export PATH=$PATH:$HOME/.dotnet/tools
export DOTNET_ROOT=$HOME/.dotnet
```
VS Code `settings.json`: `"dotnet.dotnetPath": "/home/sbsudo/.dotnet/dotnet"`

## Running via tmux (persistent sessions)
```bash
tmux attach -t crittr-server   # backend  (cd ~/Crittr/Crittr.Server && dotnet run)
tmux attach -t crittr-client   # frontend (cd ~/Crittr/Crittr.Client && dotnet run)
```
Access from any device on local network:
- Client: http://192.168.0.38:5267
- API: http://192.168.0.38:5099

> **Note:** Servers must be started manually after reboot — systemd auto-start not yet configured.

## Local AI (Ollama)
- Running on server at http://192.168.0.38:11434
- Model: llama3 (4.7 GB, GPU-accelerated via CUDA)
- Starts automatically as a systemd service on boot — no tmux needed
- Query: `ollama run llama3`
- Not yet wired into Crittr; future AI features will call `http://localhost:11434` from `Crittr.Server`

## Current Status (last updated: April 18 2026)
- App running locally on dev server
- Auth, enclosures, critters, feeding log all working
- MAUI project present in solution but excluded from server build (iOS SDK unavailable on Linux)
  - MAUI errors shown in VS Code are cosmetic/expected — do not affect the web app
- Next: build notification/reminder feature

## Roadmap (discussed April 18 2026)
1. Walk through app as a user — find bugs and missing features
2. Build notification/reminder feature (pure C# practice, no AI)
3. Wire Ollama into Crittr (species care tips, smart logging, etc.)
4. Configure systemd services so servers auto-start on reboot
5. Set up Tailscale for remote access outside the home network
6. Nginx reverse proxy + domain for public deployment

## Solution Structure
```
Crittr.sln
├── Crittr.Server/      # ASP.NET Core 8 Web API (auth, EF Core, SQLite)
├── Crittr.Shared/      # Shared models + DTOs (referenced by Server and App)
├── Crittr.App/         # Razor Class Library — ALL pages, components, client services
├── Crittr.Client/      # Blazor WASM thin host (references Crittr.App)
└── Crittr.Maui/        # MAUI Blazor Hybrid thin host (references Crittr.App)
```
**Key rule:** New pages, components, and client-side services go in `Crittr.App`, not in the host projects (`Crittr.Client` or `Crittr.Maui`).

## Running the App
Start both processes (they must run simultaneously):
```bash
# Terminal 1 — API
cd Crittr.Server && dotnet run
# Terminal 2 — Web client
cd Crittr.Client && dotnet run
```

| Process | URL |
|---------|-----|
| API | https://localhost:7282 |
| Swagger | https://localhost:7282/swagger |
| WASM client | http://localhost:5267 |

The client reads its API base URL from `Crittr.Client/wwwroot/appsettings.json` → `ApiBaseUrl`.
Currently set to the LAN IP (`http://192.168.0.38:5099`) for mobile testing.
Falls back to `https://localhost:7282/` if the file is absent.

## Build
```bash
dotnet build          # whole solution
dotnet build Crittr.Server
dotnet build Crittr.App
```

## Dev Workflow
1. Open MacBook → VS Code auto-reconnects via Remote SSH
2. Verify tmux sessions are running (`tmux ls`)
3. Open http://192.168.0.38:5267 in browser
4. Edit code in VS Code (files live on the server)
5. Run `claude` from `~/Crittr` in VS Code terminal for AI assistance
6. Commit: `git add . && git commit -m "message" && git push`

## Demo Accounts (auto-seeded on startup)
| Email | Password |
|-------|----------|
| demo@critterapp.com | Demo123! |
| demo@reptilecare.com | Demo123! |
| demo@demo.com | Password123! |

Sample critters/enclosures are attached to `demo@critterapp.com`.

## Architecture Notes

### Auth
- ASP.NET Identity + JWT. Token stored in browser `localStorage` under key `authToken`.
- `AuthorizedHandler` (`Crittr.App/Services/AuthorizedHandler.cs`) is a `DelegatingHandler` that reads the token from localStorage and injects it as a `Bearer` header on every outbound request.
- All client API services are registered via `AddHttpClient<T>().AddHttpMessageHandler<AuthorizedHandler>()` in `Crittr.App/DependencyInjection.cs`. Do not add manual token injection inside service methods.

### Client Services (Crittr.App)
- `AuthService` — login, logout, register
- `CritterService` — CRUD for critters
- `EnclosureService` — CRUD for enclosures
- `FeedingService` — feeding log CRUD
- `SheddingService`, `MeasurementService`, `HealthService`, `ScheduledTaskService` — supporting logs
- `SpeciesService` — iNaturalist species search autocomplete

### Static Assets
Razor Class Library assets are served under `_content/Crittr.App/`. Always use `AppAssets.Resolve(path)` to reference critter icons and images — it handles both relative RCL paths and absolute external URLs.

### Server Binding / CORS
- In Development, the server explicitly binds `http://0.0.0.0:5099` and `https://localhost:7282`.
- CORS allows any loopback origin plus any private-network IPv4 (10.x, 172.16–31.x, 192.168.x) in Development so phones/emulators on LAN can reach the API without whitelisting specific IPs.
- `UseHttpsRedirection` is disabled in Development to avoid redirect loops when the WASM client calls plain HTTP on LAN.
- Production CORS origins are read from `Cors:AllowedOrigins` in config (`appsettings.Production.json`).

### Database
SQLite in development (`app.db` in server working directory). EF Core migrations live in `Crittr.Server/Migrations/`. After modifying a model:
```bash
cd Crittr.Server
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

### Server-Side Services
All server services implement interfaces in `Crittr.Server/Services/Interfaces/`. Register new services as `AddScoped` in `Program.cs`.

### Controllers
All controllers require JWT auth via `[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]`. Always scope DB queries to the current user — use `User.FindFirstValue(ClaimTypes.NameIdentifier)` and filter at the EF level (never fetch all rows and filter in memory).

## CSS / Tailwind
CSS lives in `Crittr.App/wwwroot/css/`. Tailwind is compiled via npm:
```bash
cd Crittr.App
npm install
npm run build:css
```
Run `build:css` before publishing to ensure `output.css` is current.

## Known Stubs (not yet implemented)
- `NotificationEngine` — feeding/shedding/task reminders
- `HealthAnalyticsEngine` — health score computation
- `NaturalLanguageParser` — natural language log entry
- `HerpstatIntegration` — hardware thermostat integration
