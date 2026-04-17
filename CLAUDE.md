# Crittr — Claude Code Guide

## Project Overview

Exotic pet companion app for reptile/critter keepers. Full-stack Blazor WebAssembly + ASP.NET Core.

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

The client reads its API base URL from `Crittr.Client/wwwroot/appsettings.json` → `ApiBaseUrl`. Defaults to `https://localhost:7282/` if absent.

## Build

```bash
dotnet build          # whole solution
dotnet build Crittr.Server
dotnet build Crittr.App
```

## Demo Accounts (auto-seeded on startup)

| Email | Password |
|-------|----------|
| demo@critterapp.com | Demo123! |
| demo@reptilecare.com | Demo123! |
| demo@demo.com | Password123! |

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
