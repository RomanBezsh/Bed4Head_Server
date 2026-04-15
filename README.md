# Bed4Head

## Project Structure

The solution follows a layered architecture built around the `Bed4Head` name.

```text
Bed4Head/
├── Bed4Head.Web/              # Web API layer
│   ├── Controllers/
│   ├── Middleware/
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
├── Bed4Head.Application/      # Application and business logic
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Services/
│   ├── Extensions/
│   └── UseCases/
├── Bed4Head.Domain/           # Core domain model
│   ├── Entities/
│   ├── Enums/
│   └── ValueObjects/
├── Bed4Head.Infrastructure/   # Data access and external integrations
│   ├── Data/
│   ├── Repositories/
│   ├── ExternalServices/
│   ├── Extensions/
│   └── Migrations/
├── Bed4Head.Common/           # Shared cross-cutting utilities
│   ├── Exceptions/
│   └── Helpers/
└── Bed4Head.Tests/            # Test project
    ├── Unit/
    └── Integration/
```

## Layer Responsibilities

- `Bed4Head.Web` exposes HTTP endpoints, request handling, and middleware.
- `Bed4Head.Application` contains DTOs, service contracts, and application services.
- `Bed4Head.Domain` contains the core entities and domain-level building blocks.
- `Bed4Head.Infrastructure` contains EF Core, repositories, migrations, and integration code.
- `Bed4Head.Common` is reserved for shared helpers and reusable exceptions.
- `Bed4Head.Tests` contains unit and integration tests for the solution.
