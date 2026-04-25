# Crittr — Claude Code Guide

## Project Overview
Exotic pet companion app for serious keepers of reptiles, birds, amphibians, fish, and other exotic animals. Full-stack Blazor WebAssembly + ASP.NET Core. Every feature is informed by real husbandry knowledge — this is not a generic pet tracker.

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

## Current Status (last updated: April 25 2026)
- App live at https://crittr.ca — **v1.3.0**
- Auth, enclosures, critters, feeding log all working
- Enclosure type system: 11 active types + "Other" (Coming Soon), themed canvases, 3-step wizard
- CritterCondition health scoring: Thriving/Good/Fair/Attention/Critical from feeding cadence, shedding, tasks
- **Per-species enclosure compatibility**: species-level data (not just SpeciesType bucket) drives the wizard and dropdown filters
- **Cohabitation system live**: social profiles (SoloOnly/PairsOk/GroupsOk/Community) + gender pairing rules (25 species with male-male conflict data) + predatory checks — all run client-side via `CohabitationCatalog` in Shared with zero server dependency
- **Species-specific feeding form**: reptiles/amphibians get count+stepper+consumed toggle; fish get portion selector; birds get serving log; all feedings capture time not just date; `FeedingUnit` field stored in DB
- **Dashboard three-state drag visual**: green (compatible), orange (cohabitation conflict), dimmed (type mismatch) — with hover messages showing specific reasons including pet name, species, and behavioral notes
- "Other" enclosure type disabled in wizard — Coming Soon (reserved for isometric enclosure builder)
- Tailscale connected — dev server accessible from anywhere
- MAUI project present but excluded from server build (iOS SDK unavailable on Linux) — cosmetic errors only

## Roadmap (updated April 25 2026)

### Completed ✅
1. Auth, enclosures, critters, feeding log
2. iNaturalist-backed species catalog (110+ species, care profiles, accepted foods)
3. CritterCondition health scoring (Thriving → Critical from behavioral logs)
4. Per-species enclosure compatibility (Ball Python → Terrarium only, not the whole Reptile bucket)
5. Cohabitation system — social profiles, predatory checks, gender pairing rules for 25 species
6. Species-specific feeding form — count/portion/serving modes + FeedingUnit field
7. Dashboard three-state drag visual (green/orange/dimmed) with specific conflict messages
8. Client-side CohabitationCatalog in Shared — all checks run in browser, zero server dependency

### Near Term
9. **Notification / reminder system** — feeding, shedding, and task alerts (C# engine, no AI)
10. **Deep species profiles** — modular per-species tracking concepts (see vision below)

### Medium Term
11. **Aquarium intelligence** — water type, parameter logging (pH, ammonia, nitrate, salinity), stocking calculator, nitrogen cycle tracker
12. **Terrarium intelligence** — temperature gradient, UVB bulb tracking, shedding predictor
13. **Vivarium intelligence** — microfauna health, plant log, cleanup crew guidance
14. **Aviary / cage intelligence** — out-of-cage time, enrichment tracker, social needs check
15. **Enclosure detail upgrades** — live sensor integration (Herpstat, SensorPush)

### Long Term
16. **Isometric enclosure builder** — 3D visual builder unlocking the "Other" type; produces structured data so intelligence features still work
17. **Live critter representation** — species-accurate animated critters in the enclosure canvas with idle behaviours and enclosure-type-appropriate movement
18. **Ollama AI integration** — care tips, smart log parsing, pattern analysis via local llama3 (dev server GPU, requires Tailscale on prod)
19. **Auto-deploy pipeline** — deploy on git push

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
dotnet build          # whole solution (MAUI errors are expected on Linux — cosmetic)
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
| demo@crittr.ca  | Demo123! | One of every enclosure type, each with a species-matched critter |
| empty@crittr.ca | Demo123! | Blank account — no enclosures, no critters                       |

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
- `SpeciesService` — species catalog lookup (`GetInfoAsync(name)`, `GetCareProfileAsync(name)`, `SearchAsync`)

### Enclosure Type System (`Crittr.Shared/Utilities/EnclosureCompatibility.cs`)
Static utility with the species ↔ enclosure compatibility matrix. Key methods:
- `FormatEnclosureType(t)` — display name (e.g. `RackSystem` → "Rack System")
- `GetCompatibleEnclosureTypes(species)` — fallback list for a SpeciesType (used when species-specific data unavailable)
- `GetCompatibleSpeciesTypes(enclosure)` — which species types can live in this enclosure
- `GetIdealEnclosureType(species)` — best enclosure type for a SpeciesType
- `IsCompatible(species, enclosure)` — boolean check (SpeciesType-level)
- `GetEnclosureRequirementLabel(IEnumerable<EnclosureType>)` — generates "a terrarium or vivarium" label from a type list
- `GetEnclosureRequirementLabel(SpeciesType)` — overload using the fallback map

**Per-species compatibility flow** (added v1.2): The critter detail page fetches `SpeciesInfo` via `GET /api/species/info?name={commonName}` and uses its `CompatibleEnclosureTypes` list directly. Falls back to `EnclosureCompatibility.GetCompatibleEnclosureTypes(speciesType)` if species not found in catalog. This means a Ball Python correctly gets only `[Terrarium]`, not the whole Reptile fallback list.

### CohabitationCatalog (`Crittr.Shared/Utilities/CohabitationCatalog.cs`)
Static dictionary keyed by CommonName (case-insensitive) → `CohabitationProfile`. Lives in **Shared** so the WASM client runs all cohabitation checks locally — no server round-trips. Contains all 110+ species with:
- `SocialNeeds` (SoloOnly / PairsOk / GroupsOk / Community)
- `IsTerritorial`, `IsPredatory`
- `GenderRule` (AnyGenderOk / OneMalePerEnclosure)
- `MaleMaleIsLethal` — true = hard block, false = strong warning
- `CohabNote` — specific behavioral note shown in conflict messages
- `IncompatibleWith` — explicit species name pairs

`CohabitationCatalog.CheckLocal(incomingDto, residents)` is the primary entry point used by both `Dashboard.razor` and `CritterDetail.razor`. The server's `EnclosureCohabitationService` calls `CohabitationChecker.Check` directly with sex context from the DB.

### Species Catalog (`Crittr.Server/Services/SpeciesCatalogService.cs`)
In-memory catalog of 110+ common exotic pet species. Each `SpeciesInfo` entry has:
- `CommonName`, `ScientificName`, `Type` (SpeciesType enum)
- `CompatibleEnclosureTypes` — the source of truth for per-species enclosure compatibility
- `CareProfile` — feeding cadence, shedding interval, temp/humidity ranges, nocturnal flag
- `AcceptedFoods` — quick-pick food chips for the feeding form

Key methods: `GetAllAsync()`, `SearchAsync(query, type)`, `GetByNameAsync(commonName)`, `GetCareProfileByCommonName(name)`.
Exposed via `SpeciesController` at `/api/species`, `/api/species/search`, `/api/species/info`, `/api/species/careprofile`.

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

---

## CSS / Tailwind
CSS lives in `Crittr.App/wwwroot/css/`. Tailwind is compiled via npm:
```bash
cd Crittr.App
npm install
npm run build:css
```
Run `build:css` before publishing to ensure `output.css` is current. The WASM client serves the compiled output — new Tailwind classes added to Razor files won't appear until CSS is rebuilt.

---

## Product Vision & Design Philosophy

### What Crittr Is
Crittr is a species-specific exotic pet companion — not a generic pet tracker. The core insight: caring for a ball python, a dart frog colony, and a reef aquarium are three completely different disciplines in requirements, workflow, and mental model. Crittr's goal is to feel genuinely made for each of them.

The app is built on the principle that **every intelligence feature must be grounded in real husbandry knowledge**. The species catalog, the cohabitation rules, the feeding form, the health scoring — all of it draws on actual behavioral and care data, not guesswork.

### Data Integrity — No Custom Options
**Custom options are a trap.** Free-text fields and catch-all types produce unstructured data that cannot power intelligent features. Every "Custom" option exposed is a feature we can never build on top of.

Rules:
- Prefer curated, enumerated options: substrate presets, species from catalog, size presets
- `EnclosureType.Other` is disabled — shown as Coming Soon until the isometric enclosure builder ships
- Species must come from the catalog; free-text species names are a degraded fallback for edge cases, not the primary flow
- When a substrate, size, or parameter can be enumerated, enumerate it — the app can then compute on it

This is why "Other" is Coming Soon rather than removed: once we have the custom enclosure builder, it will produce structured data (dimensions, equipment list, layout). Until then, "Other" produces nothing useful.

### Intelligence Flows from the Species
Every piece of intelligence Crittr provides is species-derived. The `SpeciesCatalogService` is the heart of the system:
- Which enclosures a species can live in (`CompatibleEnclosureTypes`) — not just "reptile" vs "bird"
- Care requirements: feeding cadence, shedding interval, ideal temp/humidity
- Accepted foods: powers the quick-chip feeding form
- Cohabitation compatibility (designed, not yet built — see below)

When a feature must behave differently for a Bearded Dragon versus a Ball Python, it must use species-level data — not the coarse `SpeciesType` enum. The enum is a fallback for unknown/catalog-miss cases only.

---

## Cohabitation System (Designed — Not Yet Built)

### The Problem
Currently, any critters can share an enclosure regardless of species compatibility, predator/prey dynamics, territorial behavior, or environmental needs. A ball python and a leopard gecko can cohabit. This must change.

### Data Model Design
Add `CohabitationProfile` to `SpeciesInfo` in `Crittr.Shared/DTOs/`:

```csharp
public class CohabitationProfile
{
    public SocialStructure SocialNeeds { get; set; }
    public bool IsTerritorial { get; set; }          // aggressive toward same species/conspecifics
    public bool IsPredatory { get; set; }             // will eat or harm smaller tank mates
    public string[] CompatibleWith { get; set; } = []; // explicit allowed companions (CommonName)
    public string[] IncompatibleWith { get; set; } = []; // explicit denied companions
    public string? CohabNote { get; set; }            // e.g. "Females only; never two males"
}

public enum SocialStructure
{
    SoloOnly,      // must be kept alone (ball pythons, most chameleons, most tarantulas)
    PairsOk,       // can be kept in same-species pairs, often female-only
    GroupsOk,      // can be kept in same-species groups (dart frogs, some gecko species)
    Community,     // can live with compatible different species (community fish, some birds)
}
```

### Validation Logic (priority order)
When adding a critter to a non-empty enclosure, check in order:
1. If new critter is `SoloOnly` → hard block, cannot be placed with others
2. If any existing critter is `SoloOnly` → hard block
3. Water type mismatch in an Aquarium (freshwater fish + saltwater fish) → hard block
4. Explicit `IncompatibleWith` match → hard block or strong warning
5. `IsPredatory` new critter + small existing critters → warning with size check
6. Environmental needs divergence (temp/humidity ranges don't overlap) → warning
7. Different SpeciesType in a non-Community enclosure → warning
8. Otherwise → allow, optionally with advisory

### Key Cohabitation Rules by Type

**Reptile — almost always SoloOnly:**
- Snakes: all SoloOnly (ball python, corn snake, boa, etc.)
- Chameleons: strictly SoloOnly (extreme stress even from sight of another)
- Bearded dragons: SoloOnly (will suppress smaller cage mates)
- Leopard geckos: PairsOk or GroupsOk — females only, never two males
- Most monitors: SoloOnly

**Amphibian:**
- Dart frogs: GroupsOk within compatible species/morphs; cross-morph breeding risks
- White's Tree Frogs: GroupsOk (same species)
- Axolotls: SoloOnly or same-size pairs only (will cannibalize)
- Pacman frogs: strictly SoloOnly

**Bird — social species benefit from companions:**
- Budgies, cockatiels, lovebirds, finches: GroupsOk (same or compatible species)
- African Grey, macaws: PairsOk (territorial with strangers; large parrots can injure small ones)
- Never mix large parrots (macaws, cockatoos) with small birds (budgies, finches)

**Fish (Aquarium) — the most complex:**
- Community fish: Community (tetras, corydoras, rasboras — compatible across many species)
- Bettas: male is always SoloOnly; females can be grouped in sororities
- Cichlids: IsTerritorial = true; some community-safe, many not
- Oscars: IsPredatory = true; will eat anything that fits in their mouth
- Water type is a hard constraint: freshwater species + saltwater species = hard block

**Mammal — many are social:**
- Rabbits, guinea pigs, degus, rats, ferrets: GroupsOk or Community; benefit strongly from companions
- Syrian (golden) hamster: SoloOnly — will fight to the death if housed together
- Chinchillas: PairsOk (same-sex pairs or bonded pairs)
- Sugar gliders: GroupsOk (highly social; solo often leads to depression)

**Invertebrate:**
- Most tarantulas: SoloOnly (cannibalistic outside of breeding attempts)
- Praying mantis: SoloOnly
- Land hermit crabs: GroupsOk
- Emperor scorpions: GroupsOk (exception among scorpions)
- Isopods / springtails: used as cleanup crew in vivariums, compatible with many species

### UI Implementation Plan
1. **Assign critter to enclosure** — run compatibility check before confirming; show warning modal with specific reason if blocked or cautioned
2. **Enclosure detail page** — "Cohabitation" section listing critters and per-pair compatibility status (green/amber/red)
3. **Dashboard enclosure cards** — cohabitation badge in corner if any conflict exists in that enclosure
4. **Critter catalog** — show SocialStructure tag on species cards ("Social", "Solo Only", "Pairs OK")

---

## Per-Enclosure-Type Intelligence (Vision)

Each enclosure type has a distinct set of parameters, health metrics, and keeper workflows. The long-term vision is a tailored experience per type — the dashboard and logging views morph to fit the keeper's world.

### Aquarium — Most Complex
The aquarium keeper lives by water chemistry. Future features:

**At Creation:**
- `WaterType` enum: `Freshwater`, `Saltwater`, `Brackish` — set once, constrains everything downstream
- Tank volume in gallons (auto-calculated from dimensions or entered directly)
- Filtration type: Sponge, HOB (hang-on-back), Canister, Sump, Internal

**Ongoing Tracking:**
- **Water parameter log**: pH, temperature (°F/°C), ammonia (ppm), nitrite (ppm), nitrate (ppm), phosphate (ppm), KH/GH (freshwater), salinity/specific gravity (saltwater)
- **Nitrogen cycle tracker**: visualize the new tank cycle progression (ammonia spike → nitrite spike → nitrate rise → cycled)
- **Water change log**: date, volume changed (gallons/%), parameters before and after
- **Stocking calculator**: based on tank volume + species bioload; warns when overstocked

**Cohabitation for Aquarium:**
- Hard block on water type mismatch
- Community compatibility matrix: neon tetras + corydoras = OK; Oscar + neon tetras = neon tetra becomes food
- Schooling fish: warn if fewer than 6 of a species that needs a school

### Terrarium — Core Experience
Already partially built. Future additions:
- **Temperature gradient log**: hot spot, cool end, ambient (manual entry; IoT replaces this later)
- **UVB tracking**: bulb type, spectrum (5.0, 10.0, T5, etc.), daily on/off hours, bulb replacement reminder (UVB output degrades before visible light fails)
- **Photoperiod schedule**: light/dark hours; will drive shedding prediction
- **Shedding predictor**: last shed date + species average interval = next predicted shed window

### Vivarium — Bioactive Complexity
- **Microfauna tracking**: springtail/isopod population health (healthy cleanup crew = plant waste managed)
- **Plant log**: species, placement, health notes
- **Substrate composition**: drainage layer depth, substrate depth, moisture level
- **Cleanup crew ratios**: species-specific recommendations (dart frog vivarium vs crested gecko vivarium)

### Aviary / Cage — Bird Experience
- **Out-of-cage time log**: most parrots need daily supervised free flight/interaction; track it
- **Social companion check**: warn if species has `SocialStructure.GroupsOk` but is housed alone
- **Enrichment rotation**: log toy swaps and foraging activities (enrichment prevents feather destructive behavior)
- **Molt tracking**: equivalent to shedding; seasonal and stress-triggered

### Rack System / Bin — Breeder Experience
- **Feeding roster**: batch feeding view across all animals in the rack
- **Per-slot temperature**: rack systems run many animals on shared heat tape; each position may vary
- **Breeding records**: pairing log, egg date, hatch date, clutch size

### Paludarium — Dual Environment
- Both aquatic parameters (water chemistry) and terrestrial parameters (humidity, temperature)
- Waterfall/misting schedule
- Plant health in both zones

---

## Species Profile Vision — Modular, Extensible, Species-Specific

### The Problem with Generic Tracking
A "feeding log" that works the same for a betta fish and a ball python is not a feeding log — it's a notes field. The same is true for shedding, health metrics, enclosure parameters, and every other concept in animal care. Different species have fundamentally different trackable dimensions.

### Modular Concept Architecture (Planned)
Each species in the catalog will eventually have a `TrackingConcepts` list — a set of named dimensions that apply to that species and no others:

```
Ball Python: [FeedingLog, SheddingCycle, WeightTracking, HumidityLog]
Betta Fish:  [FeedingLog, WaterParameters, TemperatureLog, BehaviorLog]
Cockatiel:   [FeedingLog, MoltCycle, OutOfCageTime, EnrichmentLog]
Axolotl:     [FeedingLog, WaterParameters, NitrogenCycle, RegrowthLog]
```

**Design rules:**
- A concept is only shown if the species has it in its profile
- Adding a new concept to a species doesn't affect others
- Concepts can carry species-specific defaults (e.g., feeding frequency from care profile)
- The UI renders the appropriate input/display component per concept type
- Future: users can add custom concepts per critter

This is what "modular" means — a dart frog keeper never sees "water parameters." A reef aquarium keeper never sees "shedding cycle." The software understands what each animal needs.

### Critter Representation Vision
Currently critters are represented by static SVG icons in the enclosure canvas. The long-term vision is **animated, species-accurate visual representation**:

- Each species has idle animations, movement patterns, and enclosure-appropriate behaviors
- Canvas context matters: a fish swims, a gecko climbs, a tortoise grazes, a parrot preens
- Multiple critters in the same enclosure interact — or avoid each other based on cohabitation status
- The canvas is not decorative; it's a live, readable state of the enclosure
- When a cohabitation conflict exists, the visual shows tension between the critters
- When health is critical, the critter's behaviour changes visibly

This is why the enclosure canvas uses a 2:1 aspect ratio with a themed gradient and SVG overlays — the foundation for per-species animation layers is already in place.

## Enclosure Builder — "Other" / Custom Type (Planned)
The `EnclosureType.Other` slot is reserved for a future isometric 3D enclosure builder. Until it ships, it displays as "Coming Soon" in the wizard. The builder will need to produce structured data — custom dimensions, equipment items, layout elements — so that intelligent features can still operate on it. This is explicitly why we don't accept `Other` as a real type yet: unstructured custom data breaks every downstream feature that operates on enclosure type.

The builder will render an isometric 3D visualization of the enclosure — the same visual language as the themed canvases but editable — with drag-and-drop placement of hides, water dishes, plants, and equipment.

---

## Known Stubs (not yet implemented)
- `NotificationEngine` — feeding/shedding/task reminders
- `HealthAnalyticsEngine` — health score computation (stub exists; CritterCondition is the live implementation)
- `NaturalLanguageParser` — natural language log entry
- `HerpstatIntegration` — hardware thermostat integration
- Cohabitation rules — designed above, not yet coded
- Aquarium water parameter logging — designed above, not yet coded
- Per-type keeper dashboard experience — designed above, not yet coded
