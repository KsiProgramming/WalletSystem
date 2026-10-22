<!-- module: tests/Core.Tests -->
# WalletSystem.Core.Tests

This is the unit-test project for the `WalletSystem.Core` library (`src/Core/WalletSystem.Core.csproj`). Its tests cover the core business logic: expenses, users, expense validation rules and the system clock helper. The sibling project `tests/AzureFunctions.Tests` covers the other source module.

## Project setup

| Item | Value |
|---|---|
| Project file | `WalletSystem.Core.Tests.csproj` |
| Target framework | `net8.0` |
| Nullable / implicit usings | enabled |
| Packable | No (`IsPackable=false`, `IsTestProject=true`) |
| Project reference | `..\..\src\Core\WalletSystem.Core.csproj` |

`GlobalUsings.cs` adds global usings for `FluentAssertions` and `Xunit`, so test files don't import them. Each file uses the repository's StyleCop copyright header. `GlobalUsings.cs` suppresses the SA1200 warning for the global usings.

### Dependencies

- `xunit` 2.6.6 and `xunit.runner.visualstudio` 2.5.6: the test framework and runner.
- `Microsoft.NET.Test.Sdk` 17.9.0: test host.
- `FluentAssertions` 6.12.0: assertions (`Should()`, `ThrowExactly<>()`, `BeCloseTo`, and so on).
- `Moq` 4.20.70: mocks for the interfaces in Core, such as `IUserManager`, `IExpenseRepository`, `IExpenseRulesValidator` and `ISystemClock`.
- `coverlet.collector` 6.0.0: code coverage collection.

## Layout

The folders mirror the namespaces of the Core library. Test classes are named `<Type>Test`, or `<Type>Tests` in a few files.

- `Expenses/`
  - `ExpenseManagerTest.cs`: tests `ExpenseManager.CreateAsync`. It mocks the user manager, a strict expense repository and the rules validator. It checks the `ExpenseQuery` used for the duplicate lookup and the `ExpenseValidationRequest` passed to the validator.
  - `ExpenseCreationTest.cs`
  - `Domain/`: `AmountTest`, `ExpenseQueryTest`, `ExpenseTest`.
  - `RulesValidator/`
    - `ExpenseRulesValidatorTest.cs`
    - `Domain/`: `ExpenseValidationRequestTest` and `ExpenseValidationRequestExtensionsTests`.
    - `Rules/`: one test per rule, covering these rules:
      - `CurrencyMissMatchRule`
      - `DateInFutureRule`
      - the three-month age rule (`DateOlderThanThreeMonthsRule`)
      - `ExpenseDuplicateRule`
      - `MissingDescriptionRule`
    - `Exceptions/`: tests for the matching exception types:
      - `ExpenseCurrencyMissMatchException`
      - `ExpenseDateInFutureException`
      - `ExpenseDateOlderThanThreeMonthsException`
      - `ExpenseDuplicateFoundException`
      - `ExpenseMissingDescriptionException`
  - `Users/`: `UserManagerTest`, `UserNotFoundExceptionTest`, and `Domain/` with `UserTest` and `UserQueryTest`.
- `Helpers/SystemClock/SystemClockTest.cs`: checks that `SystemClock.UtcNow` is within one second of `DateTime.UtcNow`.

## Conventions seen in the tests

- xUnit `[Fact]` methods are named after the member under test. A suffix describes the scenario, for example `Check_WithExpenseDateOlderThanThreeMonths`.
- Dependencies are mocked with Moq. `MockBehavior.Strict` and `VerifyAll()` or `Verifiable()` confirm that the expected calls happened. For example, the rule tests mock `ISystemClock` so that `UtcNow` returns a fixed date.
- Validation failures are asserted with FluentAssertions, for example `act.Should().ThrowExactly<ExpenseDateOlderThanThreeMonthsException>().WithMessage(...)`.
- Dates are built as `DateTimeKind.Utc`.

## Running the tests

From the repository root:

```sh
dotnet restore
dotnet build --no-restore
dotnet test --no-build --verbosity normal
```

To run only this project, pass its path:

```sh
dotnet test tests/Core.Tests/WalletSystem.Core.Tests.csproj
```

The GitHub Actions workflow (`.github/workflows/dotnet.yml`) builds and tests every `.csproj` individually. It skips the SQL database project. `ci-pipeline.yml` at the repository root runs `dotnet restore`, `dotnet build` and `dotnet test` with .NET 8.0.x on pushes and pull requests to `master`.
