# 🦎 Crittr — Smart Exotic Pet Companion

Crittr is a full-stack **Blazor WebAssembly + ASP.NET Core** app designed for keepers of reptiles and exotic critters. From feeding and growth tracking to fully customizable enclosures, **Crittr brings your pet's world to life.**

**Current release: v1.0.0** — accounts, enclosures & critters, per-critter **feeding log** (API + UI), Blazor Hybrid (MAUI) shell sharing the same UI library (`Crittr.App`).

---

## 🚀 Features (v1)

- 🔐 Register / log in, JWT in browser storage, per-user data on the API
- 🏠 Dashboard with enclosure cards, animated critter icons, **critter detail** route
- 🍽️ **Feeding history + log feeding** per critter (owned data only on the API)
- 📦 Modal flow to add critters (species search via iNaturalist) and enclosures
- 📱 Mobile-friendly layout; shared **Razor UI** (`Crittr.App`) for both WASM and MAUI Blazor Hybrid
- 🎨 Emerald / slate theme, DM Sans, polished empty & loading states

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
| ✅     | Auth, enclosures, critters, feeding log  | v1 complete                                        |
| ✅     | iNaturalist species autocomplete         | Images + common names                              |
| ✅     | MAUI Blazor Hybrid shell                 | Shares `Crittr.App` with the WASM client           |
| ✅     | Full rename to `Crittr`                  | Namespaces, UI labels, routes                      |
| 🔜     | Notifications & reminders                | Feeding / shedding / task alerts — next up         |
| 🔜     | Systemd auto-start                       | Servers restart automatically on reboot            |
| 🔜     | Tailscale remote access                  | Access app outside home network                    |
| 🧠     | Ollama AI integration                    | Species care tips, smart logging via local llama3  |
| 🌐     | Nginx + domain                           | Public deployment                                  |

---

## 🧪 Demo Users

All accounts are created automatically on server startup.

| Email                | Password     | Notes                                           |
|----------------------|--------------|-------------------------------------------------|
| demo@critterapp.com  | Demo123!     | Has sample critters / enclosures pre-loaded     |
| demo@reptilecare.com | Demo123!     | Empty account                                   |
| demo@demo.com        | Password123! | Empty account for quick login tests             |

---

## 🛳️ Ship v1 (publish checklist)

1. **API (`Crittr.Server`)**
   - Set `Jwt:Key` to a long random secret (env var or user secrets), not the dev placeholder.
   - Set `ConnectionStrings:DefaultConnection` for production (or migrate off SQLite if you need concurrency).
   - In `appsettings.Production.json`, set `Cors:AllowedOrigins` to the exact HTTPS origin(s) of your hosted WASM app.
   - Run with `ASPNETCORE_ENVIRONMENT=Production` (the dev-only `UseUrls` block is skipped automatically).

2. **Web client (`Crittr.Client`)**
   - Before `dotnet publish`, set `wwwroot/appsettings.json` → `ApiBaseUrl` to your public API URL.
   - Or let CI copy `appsettings.Production.json` over `appsettings.json` for release builds.

3. **MAUI (`Crittr.Maui`)**
   - Update `ApiConfiguration.cs` so `ApiBaseUrl` points at the production API.

4. **CSS**
   - From `Crittr.App`, run `npm install && npm run build:css` so `wwwroot/css/output.css` is current before publishing.

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
