# WalletSystem documentation

WalletSystem is a .NET 8 expense-tracking system. An Azure Functions app exposes the HTTP API. A Core library holds the business rules. A SQL Server persistence layer built on Entity Framework Core stores the data. Two test projects cover the Core library and the Functions host.

## Module index

- [src/AzureFunctions](src-AzureFunctions.md): HTTP entry point (Azure Functions, isolated worker). It exposes the expense API and the Swagger UI, and turns domain exceptions into problem-details responses.
- [src/Core](src-Core.md): domain models, expense-creation rules and repository interfaces. It holds the business logic and has no storage code.
- [src/Sql](src-Sql.md): SQL Server persistence with EF Core. It implements the Core repository interfaces and defines the database schema and seed data.
- [tests/AzureFunctions.Tests](tests-AzureFunctions.Tests.md): unit tests for the HTTP functions, the JSON contracts and the exception-to-problem-details converters.
- [tests/Core.Tests](tests-Core.Tests.md): unit tests for the Core business logic: expense manager, validation rules, users and the system clock.
