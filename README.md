# Crittr

Exotic pet companion app for serious keepers of reptiles, birds, amphibians, fish, and other exotic animals. Full-stack Blazor WebAssembly + ASP.NET Core.

**Live:** [crittr.ca](https://crittr.ca)

---

## Overview

Crittr is built around a single conviction: caring for a ball python, a dart frog colony, and a reef aquarium are three completely different disciplines. A generic pet tracker fails all three. Every feature in Crittr is grounded in real husbandry knowledge — species-specific enclosure requirements, cohabitation rules, feeding modes, health scoring — and the architecture is designed to grow into a fully personalized experience for each type of keeper.

---

## Features

### v1.3.0 (Current)

- **Authentication** — register / log in, JWT in browser localStorage, per-user data isolation
- **Dashboard** — enclosure carousel with type-themed canvases, animated critter icons, drag-and-drop assignment with three-state cohabitation visual (green / orange / dimmed)
- **11 active enclosure types** — Terrarium, Aquarium, Paludarium, Vivarium, Insectarium, Aviary, Cage, Bin, Rack System, Free-Roam Room, Tank (Custom/Other is Coming Soon)
- **Per-species enclosure compatibility** — Ball Python gets Terrarium only; Red-eared Slider gets Tank + Aquarium; species-level data, not just category-level
- **Species catalog** — 110+ common exotic species with compatible enclosure types, care profiles (feeding cadence, shedding, temp/humidity), and accepted food quick-picks
- **Cohabitation system** — social profiles (Solo Only, Pairs OK, Groups OK, Community) + gender pairing rules for 25 species + predatory checks — all run client-side, instant, no server calls
  - Hard blocks prevent fatal pairings (two male leopard geckos, any critter with a Bearded Dragon)
  - Soft warnings show specific behavioral reasons ("Two male Cockatiels may fight in breeding season")
  - Available enclosure dropdown filtered at load time — incompatible enclosures are hidden before the user sees them
- **CritterCondition health system** — Thriving / Good / Fair / Needs Attention / Critical from feeding cadence, shedding history, and pending tasks
- **Species-specific feeding form** — reptiles/amphibians get count+stepper+consumed toggle; fish get portion selector; birds get serving log; all feedings now capture time of day
- **Feeding history** — unit-aware display: "4× Crickets", "Large portion · Betta Pellets", "Serving · Pellets"
- **"How It Works" guide** — in-app explanation of cohabitation rules, species compatibility, health scoring, and the product roadmap — accessible via the ⓘ icon in the nav
- **MAUI Blazor Hybrid** — same `Crittr.App` Razor Class Library serves both WASM and mobile (iOS/Android)

### Planned

The architecture is designed around a core belief: **every keeper's experience should feel like it was built for their specific animals**. What's coming:

- **Deep species profiles** — modular per-species tracking concepts (water parameters for aquariums, UVB tracking for terrariums, out-of-cage time for birds); concepts are additive — a dart frog keeper never sees "water parameters"
- **Live critter representation** — animated, species-accurate critters in the enclosure canvas with idle behaviours appropriate to their enclosure type; cohabitation conflicts visually readable
- **Per-type keeper experience** — aquarium dashboard shows water chemistry; terrarium dashboard shows temperature gradient; each enclosure type becomes its own tailored interface
- **Isometric enclosure builder** — 3D visual editor unlocking the "Other" enclosure type with structured data so intelligence features continue to work
- **Notification system** — feeding, shedding, and task reminders
- **Ollama AI integration** — care tips, natural language log entry, pattern analysis via local llama3 (GPU-accelerated on dev server)

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Frontend | Blazor WebAssembly (.NET 8) |
| Mobile | MAUI Blazor Hybrid (iOS, Android) |
| Shared UI | Razor Class Library (`Crittr.App`) |
| Shared Logic | `Crittr.Shared` — models, utilities, CohabitationCatalog, CohabitationChecker |
| Styling | Tailwind CSS (compiled via npm) |
| Backend | ASP.NET Core 8 Web API |
| ORM | EF Core + SQLite |
| Auth | ASP.NET Identity + JWT |
| Local AI | Ollama (llama3, GPU via CUDA) — not yet wired |

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Node.js 18+ (for Tailwind CSS compilation)
- dotnet-ef: `dotnet tool install -g dotnet-ef`

### Running Locally

```bash
# 1. Clone
git clone <repo-url> && cd Crittr

# 2. Compile Tailwind CSS
cd Crittr.App && npm install && npm run build:css && cd ..

# 3. Terminal A — API server
cd Crittr.Server && dotnet run

# 4. Terminal B — WASM client
cd Crittr.Client && dotnet run
```

| Service | URL |
|---------|-----|
| API | https://localhost:7282 |
| Swagger | https://localhost:7282/swagger |
| Client | http://localhost:5267 |

### Demo Accounts

| Email | Password | Notes |
|-------|----------|-------|
| demo@crittr.ca | Demo123! | One of every enclosure type, each with a species-matched critter |
| empty@crittr.ca | Demo123! | Blank account |

---

## Project Structure

```
Crittr.sln
├── Crittr.Server/    # ASP.NET Core 8 API — auth, EF Core, SQLite, species catalog
├── Crittr.Shared/    # Models, DTOs, enums, CohabitationCatalog, CohabitationChecker
├── Crittr.App/       # Razor Class Library — all pages, components, client services
├── Crittr.Client/    # Blazor WASM host (thin wrapper)
└── Crittr.Maui/      # MAUI Blazor Hybrid host (thin wrapper)
```

**Key rule:** New pages, components, and client-side services go in `Crittr.App`. Intelligence logic shared between server and client goes in `Crittr.Shared`.

---

## Architecture Highlights

**Species-first intelligence.** All compatibility, cohabitation, and feeding decisions flow from `SpeciesCatalogService` (server) and `CohabitationCatalog` (shared/client). A Ball Python isn't just a "Reptile" — it has specific compatible enclosures, predatory status, social profile, gender rules, care cadence, and accepted foods.

**Client-side cohabitation.** All cohabitation checks on the dashboard and critter detail page run in the WASM client using `CohabitationCatalog` — zero server round-trips, instant feedback, works offline.

**No custom options by design.** Enclosure types, substrates, and species are enumerated and curated. Custom/Other is Coming Soon pending a builder that produces structured data. Unstructured data breaks every downstream intelligence feature.

**Shared UI between web and mobile.** `Crittr.Client` and `Crittr.Maui` are thin hosts. All logic is in `Crittr.App` and `Crittr.Shared`.

---

## Deployment

Hosted on DigitalOcean (Toronto) behind Nginx + Let's Encrypt.

```bash
ssh root@165.22.226.103
cd /root/Crittr && git pull
systemctl restart crittr-server crittr-client
```

---

## Known Limitations

| Area | Notes |
|------|-------|
| JWT | No refresh token — sessions expire after token lifetime |
| MAUI on Linux | VS Code shows MAUI build errors — cosmetic, does not affect the web app |
| IoT | Temp/humidity fields exist but require manual entry until sensor integration ships |
| Custom enclosures | "Other" type disabled — reserved for the structured custom builder |

---

Built by Sebastian Canales Burke.
