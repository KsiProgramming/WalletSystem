# WalletSystem.Core

`src/Core` is the WalletSystem’s business-logic library. It defines expense and user models, manager and repository contracts, expense-creation validation, and a system-clock abstraction. The Azure Functions project uses `AddCore()` to register the core services; persistence adapters are registered separately by the SQL module.

## Main parts and public interfaces

- **Expense domain** (`WalletSystem.Expenses`): `Expense`, `Amount`, `ExpenseCreation`, `ExpenseQuery`, and the `ExpenseType` and `SortBy` enums. `IExpenseManager` exposes `CreateAsync` and `FindAsync`; `IExpenseRepository` defines the persistence operations those manager methods use.
- **Expense validation** (`WalletSystem.Expenses.RulesValidator`): `IExpenseRulesValidator` and `IExpenseRuleCheck<T>` define the validation contracts. `ExpenseRulesValidator` runs the registered checks for currency mismatch, future dates, expenses older than three months, duplicates, and missing descriptions. Each failure has a corresponding exception type.
- **Users** (`WalletSystem.Expenses.Users`): `User` and `UserQuery` model user data and lookup criteria. `IUserManager` provides `FindByIdAsync`; `IUserRepository` defines the lookup contract. A missing user results in `UserNotFoundException`.
- **Shared types and clock** (`WalletSystem`): `Currency` defines USD and RUB. `ISystemClock` exposes `UtcNow`, with `SystemClock` providing the current UTC time.

`CompositionRoot.AddCore(IServiceCollection)` registers the expense and user managers as scoped services, the validation rules and validator as singletons, and the system clock as a singleton. Repository implementations are not registered there.

## Build and tests

Build the library with:

```sh
dotnet build src/Core/WalletSystem.Core.csproj
```

Run its tests with:

```sh
dotnet test tests/Core.Tests/WalletSystem.Core.Tests.csproj
```

The project targets .NET 8, enables nullable reference types and implicit usings, and depends on `Microsoft.Extensions.DependencyInjection.Abstractions` 8.0.0. The test project uses xUnit, Moq, and FluentAssertions.
