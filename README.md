# ReptileCareApp 🦎

This is a full-stack Blazor WebAssembly + ASP.NET Core application for managing reptile care. It supports tracking reptile data, feeding, shedding, measurement, health, and scheduled tasks, with secure authentication via JWT.

---

## 📁 Project Structure

| Project | Purpose |
|--------|---------|
| `ReptileCare.Client` | Blazor WebAssembly frontend |
| `ReptileCare.Server` | ASP.NET Core API backend |
| `ReptileCare.Shared` | Shared DTOs and models (used by both client and server) |

---

## 🔐 Authentication

- **Identity**: Uses ASP.NET Core Identity (`AppUser` in `.Server` project).
- **JWT Auth**: Token issued on login, stored in browser using `Blazored.LocalStorage`.
- **AuthService** (`Client/Services/AuthService.cs`): Handles login, token storage, and setting the `Authorization` header.
- **AuthorizedHandler** (`Client/Services/AuthorizedHandler.cs`): Used to inject the token into protected API requests.

### Login Flow

1. User logs in with email and password.
2. Server validates and returns JWT.
3. Token is saved in `localStorage`, then attached to requests.
4. Authorized API endpoints require `[Authorize]` and extract `User.Identity.Name` or `User.FindFirst(ClaimTypes.NameIdentifier)?.Value`.

### Swagger Support

For development, Swagger endpoints were enabled even with authentication. You may consider adding Swagger JWT support if needed.

---

## ✅ Current Features

- ✅ Login via seeded demo user (`demo@reptileapp.com` / `Demo123!`)
- ✅ Token stored and used in protected requests
- ✅ Reptile data seeded and scoped per-user
- ✅ Blazor client loads reptiles after login

---

## 🔧 Things To Update or Add Later

| Task | Status | Notes |
|------|--------|-------|
| Refresh Token Support | ❌ | Currently JWT expires with no refresh flow |
| Logout Expiration Handling | ⚠️ | User will stay logged in until token expires or is cleared |
| Role-based access | ❌ | Can add role claims to the token if needed |
| API Error Handling | ⚠️ | Add proper toast/UI error messages |
| Production Secrets | ❌ | Move JWT key to a secret manager or environment variable before deploying |
| HTTPS Certs | ⚠️ | Development-only certs used right now |
| Stronger Token Claims | ⚠️ | Consider adding roles, emails, or user metadata to token |

---

## 🧪 Seeded Demo User

In `Program.cs` (Server):

```
Email: demo@reptileapp.com
Password: Demo123!
```

This user is automatically created at startup and linked to seeded reptile data.

---

## 🔒 Security Notes

- JWT secret is hardcoded in `appsettings.json`. This should be **secured in production**.
- CORS is configured for `https://localhost:7110` (Blazor client) only.
- Default token expiry is short. No refresh mechanism is implemented.
- Tokens are stored in localStorage — good for SPA simplicity, but vulnerable to XSS.

---

## 📦 Packages Used

- `Microsoft.AspNetCore.Identity`
- `Microsoft.EntityFrameworkCore.Sqlite`
- `Blazored.LocalStorage`
- `System.Net.Http.Json`

---

## 🧭 Next Steps

- Add user registration
- Add reptile CRUD from the client
- Add reminder system (notifications/emails?)
- Consider cloud deployment (Azure App Service + Azure SQLite or PostgreSQL)

---

Built with 💚 by Sebastian Canales Burke.
