# SQL Persistence

`src/Sql` provides WalletSystem’s SQL Server persistence implementation. It implements repository contracts from `WalletSystem.Core` and is registered by the Azure Functions host through `AddPersistence`.

## Main parts and public interfaces

- **`CompositionRoot`**: `AddPersistence(IServiceCollection, IConfiguration)` requires the `WalletSystem_SQL_CONNECTIONSTRING` setting, configures `WalletSystemDbContext` with SQL Server, and registers scoped `IExpenseRepository` and `IUserRepository` implementations. `InitialzeDatabase(IServiceProvider)` calls `EnsureDeleted()` and then `EnsureCreated()`; this deletes the existing database before recreating it.
- **`WalletSystemDbContext`**: exposes `Users`, `Expenses`, and `Currencies` sets and applies the module’s entity configurations.
- **Repositories**: `ExpenseRepository` implements `IExpenseRepository` to add expenses and find them by optional amount, date, and user ID, with optional amount/date sorting. `UsersRepository` implements `IUserRepository` to find users, optionally by ID. Both map database records to Core domain objects.
- **Data models and configurations**: `UserData`, `ExpenseData`, `CurrencyData`, and `ExpenseTypeData` represent stored records. Their configurations map tables and columns, define keys and relationships, and provide seed data for currencies, expense types, and users. Expense queries include the related user and currency for domain mapping.

The sibling `src/Sql.Database` project contains SQL Server table scripts for Currency, Expense, ExpenseType, and User; it is separate from this EF Core project.

## Build and test

Build the module with `dotnet build src/Sql/WalletSystem.Sql.csproj`. The project is a class library and has no dedicated SQL test project in the solution. Repository CI restores and builds the solution, then runs `dotnet test --no-build --verbosity normal`.

## Dependencies

Targets .NET 8 and references `Microsoft.EntityFrameworkCore` and `Microsoft.EntityFrameworkCore.SqlServer` version 8.0.3, plus `WalletSystem.Core`.
