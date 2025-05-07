# ReptileCareApp 🦎

A modern full-stack **Blazor WebAssembly + ASP.NET Core** application for reptile enthusiasts to track care routines. Supports feeding, shedding, measurements, environmental needs — all scoped per user. Now with visual enclosures and animated critters for a more engaging dashboard.

---

## 📁 Project Structure

| Project | Description |
|---------|-------------|
| `ReptileCare.Client` | Blazor WebAssembly SPA frontend |
| `ReptileCare.Server` | ASP.NET Core API backend |
| `ReptileCare.Shared` | Shared DTOs/models used by both |

---

## 🔐 Authentication (JWT + Identity)

- 🔑 Auth system built using ASP.NET Core Identity and JWT tokens
- 🧠 Token stored in `localStorage` via `Blazored.LocalStorage`
- 🧰 `AuthService` injects tokens automatically into HTTP calls
- 🔁 SPA-style login/logout without page reloads
- 🧪 Seeded demo users included

### ⚙️ Login Flow

1. User logs in via `/login`
2. Token issued by backend
3. Stored in localStorage, used for authenticated API requests
4. API endpoints protected with `[Authorize]` and claims-based user resolution

---

## 🧪 Demo Users

| Email | Password | Notes |
|-------|----------|-------|
| `demo@demo.com` | `Password123!` | Starter user — no reptiles |
| `demo@reptilecare.com` | `Demo123!` | Pre-loaded with reptiles and enclosures |

---

## ✅ Features Implemented

- [x] JWT auth + secure API endpoints
- [x] Per-user data isolation
- [x] Dashboard now shows:
  - Enclosures
  - Pets grouped per enclosure
- [x] Persistent **critter animation** using Anime.js
- [x] TailwindCSS + Flowbite integrated for clean responsive UI
- [x] Login/logout SPA-style with token state detection
- [x] Modal-based forms for adding reptiles/enclosures
- [x] Initial dark mode support

---

## 🎨 UI Overhaul Highlights (2025)

- ✅ Swipable enclosure "cards" styled with Tailwind
- ✅ Reptiles grouped into visual enclosures
- ✅ Empty enclosures prompt to add a new critter
- ✅ Critters animate gently in their "habitats"
- ✅ Buttons, layout, and structure simplified

---

## ⚠️ Known Issues / Caution Points

| Area | Risk | Notes |
|------|------|-------|
| JWT expiration | ⚠️ | No refresh token; silent timeout |
| Login UI state | ⚠️ | Requires layout refresh if token changes outside SPA |
| Token in localStorage | ⚠️ | Consider migrating to secure cookies |
| Secrets in appsettings | ❌ | JWT key must move to secure storage |
| Route protection | ⚠️ | UI blocks access, but backend routes aren't yet guarded |
| Role support | ⏳ | Not implemented, but structure is ready |

---

## 🧭 Next Steps

| Task | Priority | Notes |
|------|----------|-------|
| 👥 User Registration | High | Self-service account creation |
| ✍️ Full Reptile/Enclosure CRUD | High | Add/edit/delete for all entities |
| 🔔 Scheduled Reminders | Medium | Feeding, cleaning, vet, etc. |
| ☁️ Azure Deployment | Medium | SQLite or SQL Server backend |
| 🔁 Token Refresh | Medium | Silent renewal of JWTs |
| 🔒 Security Hardening | High | Secure auth cookies, HTTPS enforcement |
| 🎨 UX Polish | Medium | Accessibility, animations, error feedback |
| ✅ Tests | Medium | Especially around auth/login flow |

---

## 🧰 Stack + Tools

- `Blazor WebAssembly` SPA frontend
- `ASP.NET Core` Web API backend
- `EF Core + SQLite` (dev) / SQL Server (prod-ready)
- `JWT Bearer Auth`
- `Microsoft.AspNetCore.Identity`
- `Blazored.LocalStorage`
- `TailwindCSS + Flowbite`
- `Anime.js` for ambient critter animations

---

## 🏁 Recent Milestones

✅ Integrated persistent critter animation with Anime.js  
✅ Displayed all reptiles grouped by enclosure in a swipeable layout  
✅ Implemented modal flows for reptile + enclosure creation  
✅ UI refresh using Tailwind and semantic layout improvements  
✅ Finalized per-user data scoping end-to-end  

---

## 🧠 Final Thoughts

> ReptileCareApp is now a functional, user-friendly foundation for keepers to manage their pets with confidence and clarity.  
> With CRUD polish and deployment, it's nearly production-ready — now with personality.

---

Built with 💚 by **Sebastian Canales Burke**  
Contributions welcome. Pull requests reviewed with care.
