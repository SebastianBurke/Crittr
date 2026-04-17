# 🦎 Crittr — Smart Exotic Pet Companion

Crittr is a full-stack **Blazor WebAssembly + ASP.NET Core** app designed for keepers of reptiles and exotic critters. From feeding and growth tracking to fully customizable enclosures, **Crittr brings your pet’s world to life.**

**Current release: v1.0.0** — accounts, enclosures & critters, per-critter **feeding log** (API + UI), Blazor Hybrid (MAUI) shell sharing the same UI library (`Crittr.App`).

---

## Ship v1 (publish checklist)

1. **API (`Crittr.Server`)**  
   - Set `Jwt:Key` to a long random secret (environment variable or user secrets), not the dev placeholder.  
   - Set `ConnectionStrings:DefaultConnection` for production (or migrate off SQLite if you need concurrency).  
   - In `appsettings.Production.json`, set `Cors:AllowedOrigins` to the **exact** HTTPS origin(s) of your hosted WASM app (scheme + host + port).  
   - Run with `ASPNETCORE_ENVIRONMENT=Production` and **without** the dev-only `UseUrls` block (it only runs in Development).

2. **Web client (`Crittr.Client`)**  
   - Before `dotnet publish`, set `wwwroot/appsettings.json` → `ApiBaseUrl` to your public API URL (trailing slash optional; the app normalizes it).  
   - Optional: keep `appsettings.Production.json` as a template; your CI can copy it over `appsettings.json` for release builds.

3. **MAUI (`Crittr.Maui`)**  
   - Update `ApiConfiguration.cs` (or your chosen config story) so `ApiBaseUrl` points at the same production API.

4. **CSS**  
   - From `Crittr.App`, run `npm install` and `npm run build:css` (or a one-off Tailwind build) so `wwwroot/css/output.css` is current before publishing.

---

## 🚀 What's New (May 2025)

### 🎨 1. Critter Customizer & Enclosure Builder (In Progress)
- 🎨 Design your critter with **custom colors, markings, features**
- 🏡 Build your enclosure with **visual elements that match your real setup**
- 🔧 This system will replace external species APIs, moving toward **fully manual control + personalization**

### 🧠 2. Intelligent Species Entry (Current)
- 🔍 Autocomplete via iNaturalist (with images + names)
- 🐍 Type-first entry flow: select type → species → name → stats

### 🔔 3. Logging, Alerts & Notifications (Planned)
- 🕒 Feeding, shedding, cleaning, vet reminders
- 📊 Historical tracking for growth, health, and behavior
- 🔄 Offline-first support and future **reporting dashboard**

### 🤖 4. AI Integration (Future)
- 🧬 Breeding compatibility
- 📋 Smart logging suggestions
- 🧠 Species-specific enrichment tips

---

## 🧩 Tech Stack

| Area        | Stack / Tooling                          |
|-------------|------------------------------------------|
| Frontend    | Blazor WebAssembly, TailwindCSS, Flowbite |
| Backend     | ASP.NET Core Web API, EF Core            |
| Auth        | ASP.NET Identity + JWT (LocalStorage)    |
| Extras      | Anime.js (critter animation), custom modals |

---

## ✅ Feature Overview (v1)

- 🔐 Register / log in, JWT in browser storage, per-user data on the API
- 🏠 Dashboard with enclosure cards, animated critter icons, **critter detail** route
- 🍽️ **Feeding history + log feeding** per critter (owned data only on the API)
- 📦 Modal flow to add critters (species search) and enclosures
- 📱 Mobile-friendly layout; shared **Razor UI** for WASM + MAUI Blazor Hybrid
- 🎨 **v1 UI**: emerald / slate theme, DM Sans, polished empty & loading states

---

## 🧭 Roadmap

| Priority | Feature                              | Notes                                         |
|----------|--------------------------------------|-----------------------------------------------|
| 🔥       | Custom Critter & Enclosure Builder   | Top priority: full visual personalization     |
| ✅       | iNaturalist Autocomplete Integration | Complete with images + common names           |
| 🔜       | Logging + Notifications              | Feed, vet, health, etc.                        |
| 🧠       | AI Recommendations & Suggestions     | Breeding, logs, reminders                      |
| 🌐       | Full Rename to `Crittr`              | All namespaces, UI labels, routes, etc.        |

---

## 🧪 Demo Users

All accounts below are created automatically on server startup (if they do not already exist).

| Email                   | Password     | Notes                                      |
|-------------------------|--------------|--------------------------------------------|
| demo@critterapp.com     | Demo123!     | Sample critters / enclosures use this user |
| demo@reptilecare.com    | Demo123!     | Same password; empty data unless you add your own |
| demo@demo.com           | Password123! | Empty account for quick login tests        |

---

## ⚠️ Known Limitations

| Area                  | Risk   | Notes                               |
|-----------------------|--------|-------------------------------------|
| JWT Expiration        | ⚠️     | No refresh token yet                |
| Role Support          | 💤     | Infrastructure ready, not used yet  |
| Manual Enclosure Entry| 🛠️     | Soon replaced with visual builder   |

---

## 💬 Final Word

Crittr is built to empower exotic pet lovers — not just with forms, but with **visual, meaningful digital companions**. Our next updates will give you a **true reflection of your pets and space**, powered by customization and care-first features.

> _Crittr isn’t just software — it’s your vivarium's second brain._

---

Built with ❤️ by **Sebastian Canales Burke**