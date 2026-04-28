# Device Management System — Phase 1 (Backend)

A RESTful ASP.NET Core 10 Web API for tracking company-owned mobile devices, their specifications, locations, and user assignments.
---

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