# 🦎 Crittr — Smart Exotic Pet Companion

Crittr is a full-stack **Blazor WebAssembly + ASP.NET Core** app designed for keepers of reptiles and exotic critters. From feeding and growth tracking to fully customizable enclosures, **Crittr brings your pet's world to life.**

**Current release: v1.1.0** — enclosure type system (12 types, compatibility matrix, themed canvases), 3-step enclosure creation wizard, enclosure detail page, and bug fixes. Built on v1.0.0: accounts, enclosures & critters, per-critter **feeding log** (API + UI), Blazor Hybrid (MAUI) shell sharing the same UI library (`Crittr.App`).

**Live at [https://crittr.ca](https://crittr.ca)**

---

## 🚀 Features (v1.1)

- 🔐 Register / log in, JWT in browser storage, per-user data on the API
- 🏠 Dashboard with enclosure carousel, type-themed canvases, animated critter icons
- 🧩 **12 enclosure types** — Terrarium, Aquarium, Paludarium, Vivarium, Insectarium, Aviary, Cage, Bin, Rack System, Free-Roam Room, Tank, Other
- 🪄 **3-step enclosure creation wizard** — pick type → configure environment → name it
- 🐍 Species ↔ enclosure **compatibility matrix** — smart critter filtering per enclosure type
- 🎨 Per-type **themed canvases** with gradient backgrounds and decorative SVG overlays
- 🍽️ **Feeding history + log feeding** per critter (owned data only on the API)
- 📦 Multi-step add critter flow (species search via iNaturalist) with enclosure compatibility hints
- 📱 Mobile-friendly layout; shared **Razor UI** (`Crittr.App`) for both WASM and MAUI Blazor Hybrid
- 🧭 Enclosure detail page with hero section and critter roster

---

## 🧩 Tech Stack

| Area        | Stack / Tooling                              |
|-------------|----------------------------------------------|
| Frontend    | Blazor WebAssembly + MAUI Blazor Hybrid      |
| UI Library  | `Crittr.App` Razor Class Library (shared)    |
| Styling     | TailwindCSS, Flowbite                        |
| Backend     | ASP.NET Core 8 Web API, EF Core, SQLite      |
| Auth        | ASP.NET Identity + JWT (LocalStorage)        |
| Species     | iNaturalist autocomplete API                 |
| AI (local)  | Ollama (llama3) — not yet wired in           |
| Extras      | Anime.js (critter animation), custom modals  |

---

## 🧭 Roadmap

| Status | Feature                                  | Notes                                              |
|--------|------------------------------------------|----------------------------------------------------|
| ✅     | Auth, enclosures, critters, feeding log  | v1.0 complete                                      |
| ✅     | iNaturalist species autocomplete         | Images + common names                              |
| ✅     | MAUI Blazor Hybrid shell                 | Shares `Crittr.App` with the WASM client           |
| ✅     | Full rename to `Crittr`                  | Namespaces, UI labels, routes                      |
| ✅     | Systemd auto-start                       | Both dev and prod servers restart on reboot        |
| ✅     | Tailscale remote access                  | Dev server accessible from anywhere                |
| ✅     | Nginx + domain + SSL                     | Live at https://crittr.ca (Let's Encrypt)          |
| ✅     | Enclosure type system                    | 12 types, compatibility matrix, themed canvases    |
| ✅     | 3-step enclosure creation wizard         | Type picker → environment → name                   |
| ✅     | Enclosure detail page                    | Hero section, critter roster, stats                |
| 🔜     | Notifications & reminders                | Feeding / shedding / task alerts — next up         |
| 🧠     | Ollama AI integration                    | Species care tips, smart logging via local llama3  |
| 🔜     | Auto-deploy pipeline                     | Deploy on git push                                 |

---

## 🧪 Demo Users

All accounts are created automatically on server startup.

| Email            | Password  | Notes                                                              |
|------------------|-----------|--------------------------------------------------------------------|
| demo@crittr.ca   | Test123!  | One of every enclosure type, each with a species-matched critter   |
| empty@crittr.ca  | Test123!  | Blank account — no enclosures, no critters                         |

---

## 🚢 Deploying to Production

Production runs on DigitalOcean (Toronto) behind Nginx + Let's Encrypt.

```bash
ssh root@165.22.226.103
cd /root/Crittr
git pull
systemctl restart crittr-server crittr-client
```

**Server config notes:**
- `ASPNETCORE_ENVIRONMENT=Production` is set in the systemd unit files
- `appsettings.json` → `ApiBaseUrl` is `https://crittr.ca/` on the prod server
- Nginx routes `/api/` → port 5099 (backend), `/` → port 5267 (frontend)
- SSL cert auto-renews via Certbot (expires 2026-07-17)

---

## ⚠️ Known Limitations

| Area           | Notes                                           |
|----------------|-------------------------------------------------|
| JWT Expiration | No refresh token yet                            |
| Role Support   | Infrastructure ready, not used                  |
| MAUI on Linux  | VS Code shows MAUI errors on Linux — cosmetic, harmless |

---

> _Crittr isn't just software — it's your vivarium's second brain._

Built with ❤️ by **Sebastian Canales Burke**
