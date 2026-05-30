# PostgreSQL Setup for TNTTBD.Services

## Overview

The backend is configured to use **PostgreSQL** for both development and production environments.

- **Development**: Local PostgreSQL instance (Docker or native)
- **Production**: PostgreSQL from environment variables (connection string)

## Configuration Strategy

### Base Config (`appsettings.json`)
- Contains logging, CORS origins (empty by default)
- Connection string placeholder (empty) — falls back to in-memory DB if not set

### Development Config (`appsettings.Development.json`)
- **NOT tracked in git** (add to `.gitignore` if needed)
- Local PostgreSQL connection: `Host=localhost;Port=5432;Database=TNTTBD_Dev;Username=postgres;Password=postgres`
- CORS allows `http://localhost:4200` (Angular dev server)
- Debug logging enabled

### Production Config (`appsettings.Production.json`)
- Tracked in git (safe placeholder)
- Connection string injected via environment variable `ConnectionStrings__DefaultConnection`
- Warning-level logging

## Local Setup

### Option 1: Docker (Recommended)

```powershell
# Pull PostgreSQL image
docker pull postgres:latest

# Run container
docker run -d `
  --name tnttbd-postgres `
  -e POSTGRES_PASSWORD=postgres `
  -e POSTGRES_DB=TNTTBD_Dev `
  -p 5432:5432 `
  postgres:latest

# Verify
docker ps | grep tnttbd-postgres
```

### Option 2: Native PostgreSQL Install

1. Download from [postgresql.org](https://www.postgresql.org/download/)
2. Install (keep default port 5432, set password)
3. Create database:
```sql
CREATE DATABASE TNTTBD_Dev;
```

## Running the API

### Development
```powershell
cd C:\TNTTBD\TNTTBD.Services\TNTTBD.Services.Api
dotnet run
```

- Reads `appsettings.Development.json` automatically
- Connects to local Postgres at `localhost:5432`
- Schema migrations run automatically on startup

### Production

Set environment variable before running:
```powershell
$env:ConnectionStrings__DefaultConnection = "Host=prod-postgres.example.com;Port=5432;Database=TNTTBD;Username=app_user;Password=<secret>"
$env:ASPNETCORE_ENVIRONMENT = "Production"

dotnet run
```

Or in Docker:
```bash
docker run -e ConnectionStrings__DefaultConnection="..." -e ASPNETCORE_ENVIRONMENT=Production myapi:latest
```

## Database Migrations

### Generate Migration (Do NOT run yet)
```powershell
cd C:\TNTTBD\TNTTBD.Services\TNTTBD.Services.Infrastructure

dotnet ef migrations add InitialCreate --startup-project ..\TNTTBD.Services.Api --project .
```

### Apply Migration on Startup
The API automatically runs pending migrations at startup via:
```csharp
// In Program.cs or Startup
await using var scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<CatechismDbContext>().Database.MigrateAsync();
```

## Connection String Format

**Standard PostgreSQL connection string:**
```
Host=<server>;Port=<port>;Database=<dbname>;Username=<user>;Password=<password>
```

**Examples:**
- Local Docker: `Host=localhost;Port=5432;Database=TNTTBD_Dev;Username=postgres;Password=postgres`
- Azure Database for PostgreSQL: `Host=myserver.postgres.database.azure.com;Port=5432;Database=mydb;Username=user@myserver;Password=...;SSL Mode=Require`
- AWS RDS: `Host=mydb.xxxx.us-east-1.rds.amazonaws.com;Port=5432;Database=mydb;Username=postgres;Password=...`

## Verification

After starting the API:

```powershell
# Test connection
curl -X GET https://localhost:5001/api/catechism-years

# Expected: Empty paginated list or 200 OK
# If 500: Check connection string, verify Postgres is running
```

Check logs for migration output:
```
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=]
      SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema = 'public' AND table_name = '__EFMigrationsHistory')
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| `Connection refused on localhost:5432` | Start Docker container or Postgres service |
| `FATAL: password authentication failed` | Check connection string credentials |
| `Database "TNTTBD_Dev" does not exist` | Create database or let migrations create it |
| `Column does not exist` | Migrations haven't run; restart API or run manually |

## Environment Variables Reference

| Variable | Example | Notes |
|----------|---------|-------|
| `ConnectionStrings__DefaultConnection` | `Host=localhost;...` | Replaces `DefaultConnection` in appsettings |
| `ASPNETCORE_ENVIRONMENT` | `Production` | Triggers `appsettings.Production.json` |
| `ASPNETCORE_URLS` | `https://0.0.0.0:443` | Override listening port/protocol |

---

**Status**: PostgreSQL configured, ready for migrations.
