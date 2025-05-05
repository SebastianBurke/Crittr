# 🦎 ReptileCare

ReptileCare is a Blazor WebAssembly + ASP.NET Core app that helps reptile owners track the health, behavior, and care schedule of their pets. Built with modularity, real-time updates, and data-driven insights in mind.


## ⚙️ Tech Stack

- **Frontend:** Blazor WebAssembly
- **Backend:** ASP.NET Core Web API (.NET 8)
- **Database:** In-Memory DB (EF Core) for now
- **Architecture:** Modular solution with Client, Server, and Shared projects
- **Tooling:** Rider, Git, Swagger for API testing

## 🚀 Features

- View a list of your reptiles with names/species
- REST API to retrieve reptile data (`/api/reptile`)
- Swagger UI for easy backend testing
- Modular design for future features:
  - Behavior tracking
  - Feeding logs
  - Environmental monitoring
  - Health analytics
  - Notifications and scheduling

## 🧠 Future Plans

- Switch from in-memory to persistent DB (SQL Server / PostgreSQL)
- Auth system for multiple users
- Full CRUD functionality for reptiles and logs
- Integration with smart sensors (e.g., Herpstat, temp/humidity probes)
- Graphs and insights for animal health over time
