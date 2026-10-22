<!-- module: src/Sql -->
# WalletSystem.Sql (`src/Sql`)

## Purpose

`WalletSystem.Sql` is the SQL Server persistence layer of WalletSystem. It uses Entity Framework Core to implement the repository interfaces that the `Core` project defines. `IExpenseRepository` and `IUserRepository` are those interfaces. The module maps database rows to the domain types `Expense`, `User`, `Amount`, `Currency` and `ExpenseType`, and back.

## Project facts

- Project file: `WalletSystem.Sql.csproj`. It targets `net8.0` with implicit usings and nullable reference types enabled.
- NuGet packages:
  - `Microsoft.EntityFrameworkCore` 8.0.3
  - `Microsoft.EntityFrameworkCore.SqlServer` 8.0.3
- Project reference: `..\Core\WalletSystem.Core.csproj`. This is where the domain types and repository interfaces come from.
- The code is split across the namespaces `WalletSystem`, `WalletSystem.Sql`, `WalletSystem.Expenses.Sql` and `WalletSystem.Expenses.Users.Sql`.

## Structure

| Path | Contents |
|---|---|
| `CompositionRoot.cs` | DI registration (`AddPersistence`) and database initialisation (`InitialzeDatabase`). |
| `WalletSystemDbContext.cs` | The EF Core `DbContext`. |
| `DataModels/` | Persistence entities: `UserData`, `ExpenseData`, `CurrencyData`, `ExpenseTypeData`. |
| `DataConfigurations/` | `IEntityTypeConfiguration` classes that define the tables, plus `DataModelConfigurationModule`. |
| `Expenses/` | `ExpenseRepository` and the `ExpenseDataExtensions` query helpers. |
| `Users/` | `UsersRepository`. |

## Public interfaces

### `CompositionRoot` (static class, namespace `WalletSystem`)

- `IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)`
  - It requires the configuration setting **`WalletSystem_SQL_CONNECTIONSTRING`**. If the setting is missing or blank, it throws `InvalidOperationException`.
  - It registers `WalletSystemDbContext` with `UseSqlServer(...)`.
  - It registers `IExpenseRepository` → `ExpenseRepository` and `IUserRepository` → `UsersRepository`, both as scoped services.
- `void InitialzeDatabase(this IServiceProvider serviceProvider)` (the spelling is as in the code)
  - It resolves `WalletSystemDbContext`, then calls `Database.EnsureDeleted()` followed by `Database.EnsureCreated()`.
  - **It drops and recreates the database every time it is called.** The seed data is re-applied on each call.

### `WalletSystemDbContext`

It exposes `DbSet<UserData> Users`, `DbSet<ExpenseData> Expenses` and `DbSet<CurrencyData> Currencies`. `OnModelCreating` calls `ApplyDataModulesConfiguration()`. That extension applies the Currency, ExpenseType, User and Expense configurations, in that order.

### `ExpenseRepository` (`IExpenseRepository`)

- `AddAsync(ExpenseCreation)` builds an `ExpenseData` from the description, amount, user id, date and type (the type is cast to `int`). It then adds the row and calls `SaveChangesAsync`.
- `FindAsync(ExpenseQuery)` returns `IReadOnlyCollection<Expense>`.
  - It loads expenses with `User` and `User.Currency` included, using `AsNoTracking`.
  - It applies the filters `WhereAmountEquals`, `WhereDateEquals` and `WhereUserIdEquals`. Each filter applies only when the query value is set.
  - It then applies `Sort(query.SortBy, query.SortOption)` and maps the rows to domain `Expense` objects.

### `ExpenseDataExtensions`

These are `IQueryable<ExpenseData>` helpers:

- `WhereAmountEquals`, `WhereDateEquals` and `WhereUserIdEquals`.
- `Sort(SortBy?, string?)` sorts by `SortBy.Amount` or `SortBy.Date`. It sorts descending when the option is `"DESC"`, compared case-insensitively. Any other option sorts ascending. With no `SortBy`, the query is left unsorted.
- `ToExpensesAsync()` materialises the query and maps each row to an `Expense`. The `Amount` takes its currency from the user's `CurrencyId`. The user name is `"{FirstName} {LastName}"`. The type is cast to `ExpenseType`.

### `UsersRepository` (`IUserRepository`)

`FindAsync(UserQuery)` loads users with their currency (no tracking). It filters by `Id` when `query.Id` has a value. It maps each row to a `User` (first name, last name, `Currency` taken from the currency id, id).

## Database schema

The schema is defined in `DataConfigurations/` and created by `EnsureCreated`. This module has no EF migrations.

| Table | Columns | Notes |
|---|---|---|
| `Currency` | `Id`, `Code` VARCHAR(3), `Name`, `Symbol` | Seeded with 1 = USD, 2 = RUB. |
| `ExpenseType` | `Id`, `Label` | Seeded with 1 = Restaurant, 2 = Hotel, 3 = Misc. |
| `User` | `Id` (identity), `FirstName`, `LastName` (NVARCHAR(100)), `CurrencyId` | FK `FK_UserCurrency`. Seeded with Anthony Stark (USD) and Natasha Romanova (RUB). |
| `Expense` | `Id` (identity), `DateTime` DATETIME2(7), `Amount` DECIMAL(18,4), `Description` NVARCHAR(MAX), `UserId`, `TypeId` | FK `FK_ExpenseUser` with no-action delete. Non-clustered indexes on Amount, DateTime and UserId. |

## Build and test

The repository shows no module-specific commands. As a standard .NET project it builds with `dotnet build src/Sql/WalletSystem.Sql.csproj`, or through `WalletSystem.sln`. This module has no test project of its own. The `tests/` folder contains only `AzureFunctions.Tests` and `Core.Tests`.

## Place in the repository

- It depends on `src/Core` for the domain model and repository contracts.
- `src/Sql.Database` is a sibling project. Its contents were not examined for this page.
- The host application (probably `src/AzureFunctions`) is expected to call `AddPersistence` with a configuration that contains `WalletSystem_SQL_CONNECTIONSTRING`. A repository search found no callers, so this page does not confirm it.
