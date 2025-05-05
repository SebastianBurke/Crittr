# 🦎 ReptileCareApp

**ReptileCareApp** is a modular web app designed to help reptile owners monitor and manage the health, environment, and feeding of their pets—all in one place.

This is a **work-in-progress** project using Blazor WebAssembly for the client and ASP.NET Core Web API for the backend. So far, we’ve set up the core structure, implemented a dashboard view, and wired up the services to display real reptile data (via DTOs).

---

## ✅ Current Functionality

- Fetch and display a list of reptiles
- View each reptile’s:
  - Name, species, and acquisition date
  - Last fed date
  - Last recorded weight date
  - Health score
  - Count of pending tasks

---

## 🔧 Tech Stack

| Layer         | Technology                      |
|--------------|----------------------------------|
| Frontend     | Blazor WebAssembly (.NET 8)     |
| Backend      | ASP.NET Core Web API            |
| Data Sharing | DTOs via Shared Project         |
| API Docs     | Swagger/OpenAPI                 |

---

## 📍 Progress & What’s Next

### Done
- REST endpoints set up for feeding, health, environment, shedding, measurements, and scheduled tasks
- Dashboard layout fetching real data via DTOs
- Nav and layout component updates

### Up Next
- Enable creation and editing of records (CRUD operations)
- Add componentized views for:
  - Feeding logs
  - Shedding records
  - Environmental readings
- Start integrating a persistent database (currently using in-memory)

---

## 💡 How to Run It

```bash
# Backend
dotnet run --project ReptileCare.Server

# Then open:
https://localhost:7110
```

---

## 📁 Project Structure

```
ReptileCare/
├── Client/         # Blazor WebAssembly frontend
├── Server/         # ASP.NET Core backend
├── Shared/         # DTOs and shared models
```

---

## ✍️ Author

Developed by **Sebastian Canales Burke**  
Backend, frontend, UI, and integration done solo so far.

---

## 📜 License

MIT – use, remix, and expand.
