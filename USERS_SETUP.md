# Users Management Feature - Setup Guide

This document provides instructions for running the ClientServiceRazor application and handling common issues.

## Prerequisites

- .NET 10.0 SDK
- PostgreSQL 12+ or Docker for running PostgreSQL in a container
- Entity Framework Core tools: `dotnet tool install --global dotnet-ef`

## Database Setup

### Option 1: Using Docker (Recommended)

Start a PostgreSQL container:

```bash
docker run -d --name postgres-clientservice \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=client_service \
  -p 5433:5432 \
  postgres:latest
```

### Option 2: Local PostgreSQL Installation

Ensure PostgreSQL is running and create the database:

```bash
createdb -U postgres client_service
```

Update the connection string in `appsettings.json` if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5433;Database=client_service;Username=postgres;Password=postgres"
}
```

## Running the Application

1. **Apply Database Migrations**:
   ```bash
   cd ClientServiceRazor
   dotnet ef database update
   ```

2. **Build the Application**:
   ```bash
   dotnet build
   ```

3. **Run the Application**:
   ```bash
   dotnet run
   ```

   The application will start on:
   - HTTP: http://localhost:5163
   - HTTPS: https://localhost:7239

## Troubleshooting Port Conflicts

### Issue: "Failed to bind to address http://127.0.0.1:5163: address already in use"

This error occurs when port 5163 is already in use by another process.

**Solution 1: Find and Stop the Process**

```bash
# On Linux/macOS
lsof -i :5163
# Note the PID and stop it
kill <PID>

# On Windows (PowerShell)
Get-Process -Id (Get-NetTCPConnection -LocalPort 5163).OwningProcess | Stop-Process
```

**Solution 2: Use a Different Port**

Update `Properties/launchSettings.json`:

```json
{
  "profiles": {
    "http": {
      "applicationUrl": "http://localhost:5000"
    }
  }
}
```

Or use environment variable:
```bash
dotnet run --urls "http://localhost:5000"
```

## Features

### Users Management

The Users feature includes:

- **User Model**: Login, Password, Email, Role, Status, CreatedAt, UpdatedAt
- **Roles**: Admin, User, Manager (automatically seeded)
- **Statuses**: Active, Inactive, Suspended (automatically seeded)
- **CRUD Operations**: Create and view users via web interface
- **Validation**: Email format, password length (6-255 chars), login length (3-50 chars)
- **Database Constraints**: Unique email and login

### Accessing the Features

- Users Management: http://localhost:5163/Users
- Clients Management: http://localhost:5163/Clients
- Home: http://localhost:5163/

## Running Tests

```bash
dotnet test
```

All 10 tests (5 for Clients, 5 for Users) should pass:
- CreateUser_SavesAndReadsBack
- GetAllUsers_ReturnsAllSavedUsers
- DeleteUser_RemovesFromDatabase
- UpdateUser_ChangesPersisted
- User_WithRoleAndStatus_SavesAndIncludesRelations

## Security Considerations

⚠️ **Important**: This application stores passwords in plain text. For production use, implement proper password hashing using BCrypt, Argon2, or ASP.NET Core Identity.

## Database Schema

### Users Table
- Id (int, PK)
- Login (string, unique, 50 chars)
- Password (string, 255 chars)
- Email (string, unique, 100 chars)
- RoleId (int, FK)
- StatusId (int, FK)
- CreatedAt (datetime)
- UpdatedAt (datetime)

### Roles Table
- Id (int, PK)
- Name (string, 50 chars)

### Statuses Table
- Id (int, PK)
- Name (string, 50 chars)

## Architecture

The application follows Vertical Slice Architecture:

```
Features/
├── Clients/
│   ├── Models/
│   ├── Pages/
│   └── ViewModels/
└── Users/
    ├── Models/
    │   ├── User.cs
    │   ├── Role.cs
    │   └── Status.cs
    ├── ViewModels/
    │   └── UserViewModel.cs
    ├── Pages/
    │   ├── Index.cshtml
    │   ├── Index.cshtml.cs
    │   └── _ViewImports.cshtml
    └── Services/
```

Each feature is self-contained with its own models, pages, and view models.
