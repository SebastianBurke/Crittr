# ReptileCareApp 🦎

A modern full-stack **Blazor WebAssembly + ASP.NET Core** application to help reptile keepers track care routines. It supports feeding, shedding, measurements, health logs, and environmental needs — all scoped per user.

---

## 📁 Project Structure

| Project | Description |
|---------|-------------|
| `ReptileCare.Client` | Blazor WebAssembly SPA frontend |
| `ReptileCare.Server` | ASP.NET Core API backend |
| `ReptileCare.Shared` | Shared DTOs/models used by both |

---

## 🔐 Authentication (JWT + Identity)

- 🔑 **Auth system** built using ASP.NET Core Identity and JWT tokens.
- 🧠 Token is stored in `localStorage` using `Blazored.LocalStorage`.
- 🔐 `AuthService` handles login logic and injects tokens into authorized API calls.
- ✅ Logout implemented SPA-style — no page reload required.
- 🧪 Supports demo login with a seeded user.

### ⚙️ Login Flow

1. User logs in via `/login`.
2. Token is issued by the backend.
3. Token saved to `localStorage`, used on protected endpoints.
4. API endpoints use `[Authorize]` and extract the user via claims.

---

## 🧪 Demo User (Seeded)

This user is created on backend startup and has demo reptiles and enclosures:

Email: demo@demo.com
Password: Password123!

This user is empty

Email: demo@reptilecare.com
Password: Demo123!


---

## ✅ Features Implemented

- [x] Full JWT login flow
- [x] Secure API access with per-user data
- [x] Dashboard displaying reptiles + enclosures
- [x] Logout/Login button that updates without page reload
- [x] TailwindCSS + Flowbite for responsive UI
- [x] Clean layout and navigation via `MainLayout.razor`

---

## ⚠️ Known Issues / Caution Points

| Area | Risk | Notes |
|------|------|-------|
| **JWT expiration** | ⚠️ | No refresh token; token expires silently |
| **Login State on Nav** | ⚠️ | Login/logout button updates only if the layout component is refreshed |
| **Token in localStorage** | ⚠️ | Vulnerable to XSS. Consider token hardening or secure cookie auth later |
| **Hardcoded secrets** | ❌ | JWT key in `appsettings.json` — **must** be moved to secret store before prod |
| **Swagger auth** | ⚠️ | Public dev-only; should support JWT or be protected |
| **Unprotected routes** | ⚠️ | UI restricts access, but routes themselves don’t redirect yet |
| **Role-based auth** | ⏳ | Not implemented yet, but infrastructure is in place to support it |

---

## 🧭 Next Steps

| Task | Priority | Notes |
|------|----------|-------|
| 🧑‍💻 User Registration | High | Allow users to register (create accounts) |
| ✍️ Reptile & Enclosure CRUD | High | Full create/update/delete from client |
| 📅 Scheduled Reminders | Medium | For feedings, cleanings, checkups |
| ☁️ Cloud Deployment | Medium | Azure App Service + SQLite or Azure DB |
| 🔐 Token Refresh | Medium | Implement silent refresh mechanism |
| 🔒 Harden Security | High | Protect token, validate roles, enforce HTTPS |
| 🎨 Improved UX | Medium | Conditional UI, modals, error feedback |
| 🧪 Testing | Medium | Add integration/unit tests especially for auth flow |

---

## 🧰 Stack + Tools

- `Blazor WebAssembly` (SPA frontend)
- `ASP.NET Core` (API backend)
- `Entity Framework Core` (SQLite)
- `Microsoft.AspNetCore.Identity`
- `JWT Bearer Auth`
- `Blazored.LocalStorage`
- `TailwindCSS + Flowbite`

---

## 🏁 Summary of Today’s Milestones

✅ Integrated TailwindCSS and Flowbite cleanly via `index.html`  
✅ Refactored `MainLayout.razor` to support SPA-aware login/logout  
✅ Resolved token state sync issues without relying on page reloads  
✅ Set up proper dependency injection, error handling, and layout rendering  
✅ Cleaned up all legacy nav/menu components  
✅ Unified the visual theme and structure of the app across pages  

---

## 🧠 Final Thoughts

> The architecture is now clean, modular, and scalable.  
> With some backend polish (refresh tokens, secure config) and frontend CRUD, this is ready for production-level polish.

---

Built with 💚 by **Sebastian Canales Burke**  
