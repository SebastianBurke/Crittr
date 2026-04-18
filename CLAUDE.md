# Crittr — Claude Code Guide

## Project Overview
Exotic pet companion app for reptile/critter keepers. Full-stack Blazor WebAssembly + ASP.NET Core.

## Dev Server Environment
- Ubuntu Server 24.04 LTS (headless, no GUI) — hostname: sudoserver
- PC specs: NVIDIA RTX 2070 (8GB VRAM), 1TB Crucial SATA SSD
- LAN IP: 192.168.0.38 / Tailscale IP: 100.64.80.93 / username: sbsudo
- SSH: `ssh sbsudo@100.64.80.93` (Tailscale, passwordless — SSH key installed)
- VS Code Remote SSH connected via Tailscale (`Host sudoserver → 100.64.80.93`); C# Dev Kit installed

### Software Stack
- .NET 8 SDK + .NET 9 SDK (both installed via `dotnet-install.sh` at `~/.dotnet`)
- dotnet-ef global tool installed
- dotnet workloads: `maui-tizen`, `wasm-tools-net8`
- Node.js v24.15.0 (via nvm), npm 11.12.1
- Docker Engine 29.4.0 (sbsudo in docker group)
- NVIDIA drivers + CUDA 13.2 (GPU recognized by Ollama)
- Tailscale (connected — enables remote access from anywhere)
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

## Dev Server — Services (systemd)
Both app services auto-start on reboot via systemd — no manual tmux needed.
```bash
sudo systemctl status crittr-server   # or crittr-client
sudo journalctl -u crittr-server -f   # live logs
sudo journalctl -u crittr-client -f
```
Dev server live URLs (accessible from anywhere via Tailscale):
- Client: http://100.64.80.93:5267
- API: http://100.64.80.93:5099
- LAN fallback: http://192.168.0.38:5267

## Local AI (Ollama)
- Running on dev server at http://100.64.80.93:11434
- Model: llama3 (4.7 GB, GPU-accelerated via CUDA on RTX 2070)
- Starts automatically as a systemd service on boot
- Query: `ollama run llama3`
- Not yet wired into Crittr; future AI features will call this from `Crittr.Server`

## Production Server (DigitalOcean)
- Provider: DigitalOcean — $8/month (2GB RAM, 1 CPU, 50GB SSD), Toronto (TOR1)
- OS: Ubuntu 24.04 LTS — SSH: `ssh root@165.22.226.103`
- Public URL: **https://crittr.ca** (and https://www.crittr.ca)
- Nginx reverse proxy: `/api/` → port 5099, `/` → port 5267
- SSL: Let's Encrypt via Certbot — expires 2026-07-17 (auto-renews)
- DNS: Porkbun — A records for crittr.ca and www.crittr.ca → 165.22.226.103
- Both `crittr-server` and `crittr-client` run as systemd services (`ASPNETCORE_ENVIRONMENT=Production`)
- `appsettings.json` ApiBaseUrl: `https://crittr.ca/`

### Deploying to Production
```bash
ssh root@165.22.226.103
cd /root/Crittr
git pull
systemctl restart crittr-server crittr-client
```

## Current Status (last updated: April 18 2026)
- App live at https://crittr.ca — **v1.1.0**
- Auth, enclosures, critters, feeding log all working
- Enclosure type system in place: compatibility matrix, themed canvases, 3-step creation wizard
- Bug fix: enclosure type was not being persisted to DB (always defaulted to Terrarium)
- Systemd auto-start configured on both dev and prod servers
- Tailscale connected — dev server accessible from anywhere
- MAUI project present in solution but excluded from server build (iOS SDK unavailable on Linux)
  - MAUI errors shown in VS Code are cosmetic/expected — do not affect the web app
- Next: build notification/reminder feature

## Roadmap (discussed April 18 2026)
1. ~~Walk through app as a user — find bugs and missing features~~ ✅
2. Build notification/reminder feature (pure C# practice, no AI)
3. Wire Ollama into Crittr via Tailscale (prod → dev server at 100.64.80.93:11434)
4. Add Tailscale to prod server so it can reach Ollama on dev server
5. Set up a proper deployment pipeline (auto-deploy on git push)

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
Dev is set to `http://100.64.80.93:5099/` (Tailscale). Prod is set to `https://crittr.ca/`.
Falls back to `https://localhost:7282/` if the file is absent.

## Build
```bash
dotnet build          # whole solution
dotnet build Crittr.Server
dotnet build Crittr.App
```

## Dev Workflow
1. Open MacBook → VS Code auto-reconnects to sudoserver via Tailscale Remote SSH
2. Open http://100.64.80.93:5267 in browser (services auto-start on boot)
3. Edit code in VS Code (files live on the dev server)
4. Run `claude` from `~/Crittr` in VS Code terminal for AI assistance
5. Commit & deploy: `git add . && git commit -m "message" && git push`
6. On prod: `ssh root@165.22.226.103` → `cd /root/Crittr && git pull && systemctl restart crittr-server crittr-client`

## Demo Accounts (auto-seeded on startup)
| Email           | Password | Notes                                                            |
|-----------------|----------|------------------------------------------------------------------|
| demo@crittr.ca  | Test123! | One of every enclosure type, each with a species-matched critter |
| empty@crittr.ca | Test123! | Blank account — no enclosures, no critters                       |

Sample data attaches to `demo@crittr.ca`. Seeding is skipped if any critters already exist in the DB.

## Architecture Notes

### Auth
- ASP.NET Identity + JWT. Token stored in browser `localStorage` under key `authToken`.
- `AuthorizedHandler` (`Crittr.App/Services/AuthorizedHandler.cs`) is a `DelegatingHandler` that reads the token from localStorage and injects it as a `Bearer` header on every outbound request.
- All client API services are registered via `AddHttpClient<T>().AddHttpMessageHandler<AuthorizedHandler>()` in `Crittr.App/DependencyInjection.cs`. Do not add manual token injection inside service methods.

### Client Services (Crittr.App)
- `AuthService` — login, logout, register
- `CritterService` — CRUD for critters
- `EnclosureService` — CRUD for enclosures (includes `GetCompatibleAsync(speciesType)`)
- `FeedingService` — feeding log CRUD
- `SheddingService`, `MeasurementService`, `HealthService`, `ScheduledTaskService` — supporting logs
- `SpeciesService` — iNaturalist species search autocomplete

### Enclosure Type System (`Crittr.Shared/Utilities/EnclosureCompatibility.cs`)
Static utility with the species ↔ enclosure compatibility matrix. Key methods:
- `FormatEnclosureType(t)` — display name (e.g. `RackSystem` → "Rack System")
- `GetIdealEnclosureType(species)` — best enclosure type for a species
- `GetCompatibleEnclosureTypes(species)` / `GetCompatibleSpeciesTypes(enclosure)` — compatibility lookups
- `IsCompatible(species, enclosure)` — boolean check
Used by the critter wizard, enclosure wizard, and server-side filtering (`GET /api/enclosure/dto/compatible`).

### Enclosure Theming (`Crittr.App/EnclosureTheme.cs`)
Static helpers that return Tailwind class strings and SVG `MarkupString` overlays keyed to `EnclosureType`:
- `CanvasBg(t)` / `CanvasBorder(t)` — gradient + border classes for the dashboard canvas
- `CanvasOverlay(t)` — decorative inline SVG (bubbles for aquatic, leaves for green, bars for cage, etc.)
- `HeroBg(t)` / `HeroOverlay(t)` — used on the enclosure detail page hero section
- `TypeBadge(t)` — coloured badge classes for enclosure type labels

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
