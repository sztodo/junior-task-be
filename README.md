# Device Management System — Phase 1 (Backend)

## A RESTful ASP.NET Core 10 Web API for tracking company-owned mobile devices, their specifications, locations, and user assignments.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Mac — Apple Silicon or Intel)
- Git

---

## Project Structure — Layer Dependency Rules

```
Api  →  Application  →  Domain
 ↓
Infrastructure  →  Domain
```

- **Domain** has zero external dependencies
- **Application** depends only on Domain (no EF Core, no HTTP)
- **Infrastructure** implements Domain interfaces using EF Core
- **Api** wires everything together via DI and exposes HTTP endpoints

---

## Run project

Run this command in the terminal `docker compose up -d` to start both the containers
Before running the application the first time, apply the migrations. The command for that is below.
Run this command in the terminal `dotnet run --project src/Api/Api.csproj` to start the API server

## Apply migrations

Run `dotnet ef database update --project src/Infrastructure --startup-project src/Api` in the terminal to apply migrations
