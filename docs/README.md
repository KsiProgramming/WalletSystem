# WalletSystem documentation

WalletSystem is a .NET 8 application that exposes expense operations through Azure Functions, keeps business logic in a Core library, and persists data with SQL Server. This documentation set describes its application, domain, persistence, and test modules.

## Module index

- [Azure Functions](src-AzureFunctions.md) — HTTP API, application startup, and exception handling.
- [Core](src-Core.md) — expense and user domain, business services, and validation.
- [SQL Persistence](src-Sql.md) — SQL Server database context and repository implementations.
- [Azure Functions Tests](tests-AzureFunctions.Tests.md) — tests for HTTP functions and exception handling.
- [Core Tests](tests-Core.Tests.md) — unit tests for Core logic, validation, and clock behavior.
