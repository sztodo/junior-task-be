# Device Management System (Backend)

## A RESTful ASP.NET Core 10 Web API for tracking company-owned mobile devices, their specifications, locations, and user assignments.

## Prerequisites

- sqlcmd (required to run SQL scripts manually)
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

- Run this command in the terminal `docker compose up -d` to start both the containers
- Before running the application the first time, apply the migrations. The command for that is below.
- Copy the contents of `appsettings.json` into `appsettings.Development.json` making the needed changes for your local configurations.
- Run this command in the terminal `dotnet run --project src/Api/Api.csproj` to start the API server

## Apply migrations

Run `dotnet ef database update --project src/Infrastructure --startup-project src/Api` in the terminal to apply migrations

---

## Run tests

Run `dotnet test`

---

## Create an auth user account

To link your account to one of the seeded users, use the **exact same values**
as that user's record in the database. For example, to link to Alice Johnson:

| Field    | Value             |
| -------- | ----------------- |
| Name     | Alice Johnson     |
| Role     | Software Engineer |
| Location | New York, USA     |

You can use any email address and password you like — those are your login
credentials and are not tied to the user record.

When you register, the system:

1. Creates an `AuthUser` record with your email and hashed password
2. Creates a `User` record with the name/role/location you provided or finds the user with the same properties
3. Links them together automatically

> The seed script users do not have login accounts by default.
> Registration is how you create the link on your local environment,
> giving you flexibility to use any credentials you prefer.

---

## Admin access

By default all registered accounts are **Employee** role.
To promote your account to Admin, run the following script after registering:

```bash
# Edit the email in the script first:
# open scripts/04_MakeAdmin.sql and set @TargetEmail to your email

sqlcmd -S localhost,1433 -U sa -P 'YourStrong!Passw0rd' \
       -d DeviceManagementDb \
       -i src/Infrastructure/scripts/Make_Auth_User_Admin.sql -C
```

Then **log out and log back in** — the new role is embedded in the JWT token,
so you need a fresh token for it to take effect.
