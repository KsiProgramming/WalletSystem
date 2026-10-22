<!-- module: src/Core -->
# WalletSystem.Core

`src/Core` is the domain and business-logic library of WalletSystem (project `WalletSystem.Core`, target `net8.0`). It holds the expense and user models and the rules for creating an expense. It also defines the repository interfaces that the infrastructure projects must implement. `spec/mission.md` says business rules belong in `WalletSystem.Core` as small, testable units.

## Place in the repository

- `src/AzureFunctions` and `src/Sql` both reference this project through a `ProjectReference`.
- `tests/Core.Tests` and `tests/AzureFunctions.Tests` also reference it.
- The solution `WalletSystem.sln` lists the project as `WalletSystem.Core`. Its tests project is `WalletSystem.Core.Tests`, in `tests/Core.Tests`.
- The only NuGet dependency is `Microsoft.Extensions.DependencyInjection.Abstractions` 8.0.0, used for service registration.
- The project enables `ImplicitUsings` and `Nullable`.
- The Core project has no storage code. `IExpenseRepository` and `IUserRepository` are interfaces only. The files in this module don't show where the implementations live.

## Structure

| Path | Contents |
|---|---|
| `CompositionRoot.cs` | `AddCore(this IServiceCollection)` extension that registers all services. |
| `Commun/Currency.cs` | `Currency` enum: `USD = 1`, `RUB`. |
| `Expenses/` | `IExpenseManager` / `ExpenseManager`, `IExpenseRepository`, `ExpenseCreation`. |
| `Expenses/Domain/` | `Expense`, `Amount`, `ExpenseType` (`Restaurant`, `Hotel`, `Misc`), `ExpenseQuery` and the `SortBy` enum (`Amount`, `Date`). |
| `Expenses/RulesValidator/` | Validator, rule interface, validation request, extension methods, rules and exceptions. |
| `Expenses/Users/` | `IUserManager` / `UserManager`, `IUserRepository`, `User`, `UserQuery`, `UserNotFoundException`. |
| `Helpers/SystemClock/` | `ISystemClock` and `SystemClock`, which returns `DateTime.UtcNow` as a `DateTimeOffset`. |

Namespaces are flat and don't follow the folder names. Types live in `WalletSystem`, `WalletSystem.Expenses`, `WalletSystem.Expenses.RulesValidator`, `WalletSystem.Expenses.RulesValidator.Rules` and `WalletSystem.Expenses.Users`.

## Public interfaces

### `IExpenseManager`
- `Task CreateAsync(ExpenseCreation expenseCreation)`
- `Task<IReadOnlyCollection<Expense>> FindAsync(ExpenseQuery query)`

### `IExpenseRepository` (implemented by the host)
- `Task AddAsync(ExpenseCreation expenseCreation)`
- `Task<IReadOnlyCollection<Expense>> FindAsync(ExpenseQuery query)`

### `IUserManager` and `IUserRepository`
- `UserManager.FindByIdAsync(int id)` queries `IUserRepository.FindAsync(UserQuery)`. It throws `UserNotFoundException` if no user matches, and otherwise returns the single result.
- `IUserRepository` (implemented by the host) exposes `Task<IReadOnlyCollection<User>> FindAsync(UserQuery query)`.

### Models
- `ExpenseCreation` has `Amount`, `Currency`, `Date`, `Description`, `UserId` and `Type`. It is immutable.
- `Expense` has `Amount` (an `Amount` of value and currency), `Description`, `Date`, `User` (a string) and `Type`.
- `ExpenseQuery` has optional `Date`, `Amount`, `UserId`, `SortBy` and `SortOption`.
- `User` has `FirstName`, `LastName`, `Currency` and `Id`.

## Expense creation flow

`ExpenseManager.CreateAsync` does the following:

1. It loads the user with `IUserManager.FindByIdAsync`. This throws `UserNotFoundException` if the user is missing.
2. It queries the repository for existing expenses with the same amount, date and user ID. If any exist, the new expense is flagged as a duplicate.
3. It builds an `ExpenseValidationRequest` from the date, description, expense currency, duplicate flag and user currency.
4. It calls `IExpenseRulesValidator.Validate`. Any violated rule throws an exception.
5. If validation passes, it calls `IExpenseRepository.AddAsync`.

`FindAsync` passes the query straight through to the repository.

## Business rules

`ExpenseRulesValidator` receives every registered `IExpenseRuleCheck<ExpenseValidationRequest>` and runs each one in turn. The first rule to fail throws. The rules are in `RulesValidator/Rules/`. Their conditions are implemented as extension methods in `ExpenseValidationRequestExtensions`.

| Rule | Fails when | Exception |
|---|---|---|
| `CurrencyMissMatchRule` | The user currency differs from the expense currency. | `ExpenseCurrencyMissMatchException` |
| `DateInFutureRule` | The expense date is after today (UTC, from `ISystemClock`). | `ExpenseDateInFutureException` |
| `DateOlderThanThreeMonthsRule` | The age in days, converted to months as `12 * days / 365.25`, is greater than 3. | `ExpenseDateOlderThanThreeMonthsException` |
| `ExpenseDuplicateRule` | A duplicate expense was found. | `ExpenseDuplicateFoundException` |
| `MissingDescriptionRule` | The description is null, empty or whitespace. | `ExpenseMissingDescriptionException` |

All the exceptions derive directly from `Exception`. To add a rule, implement `IExpenseRuleCheck<ExpenseValidationRequest>` and register it in `CompositionRoot`.

## Dependency injection

Call `services.AddCore()` from the host. It registers:

- Scoped: `IExpenseManager` → `ExpenseManager`, and `IUserManager` → `UserManager`.
- Singleton: the five rules as `IExpenseRuleCheck<ExpenseValidationRequest>`, `IExpenseRulesValidator` → `ExpenseRulesValidator`, and `ISystemClock` → `SystemClock`.

`AddCore` does not register `IExpenseRepository` or `IUserRepository`. The host application must register them.

## Build and test

Build the project with `dotnet build src/Core/WalletSystem.Core.csproj`. The unit tests are in `tests/Core.Tests` and can be run with `dotnet test tests/Core.Tests`.
