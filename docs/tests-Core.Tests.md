# Core.Tests

`Core.Tests` is the .NET unit-test project for `WalletSystem.Core`. It checks the core expense and user logic, validation rules, and system clock behavior. The project references `src/Core/WalletSystem.Core.csproj` and sits alongside the repository’s Azure Functions test project.

## Main parts

- `Expenses/Domain` tests expense values and entities, creation data, and expense queries.
- `Expenses` tests `ExpenseManager`, including expense creation and search.
- `Expenses/RulesValidator` tests validation requests, their helper extensions, the validator’s rule orchestration, individual rules, and validation exceptions. The rules cover currency mismatch, future or older-than-three-months dates, duplicate expenses, and missing descriptions.
- `Expenses/Users` tests users and user queries, `UserManager`, and the not-found exception.
- `Helpers/SystemClock` tests the `ISystemClock` implementation.

Tests use xUnit facts and theories, FluentAssertions for assertions, and Moq to substitute dependencies such as repositories, managers, validators, and the clock. The module tests Core interfaces and types; it does not define a separate application API.

## Build and test

From the repository root, run:

```sh
dotnet test tests/Core.Tests/WalletSystem.Core.Tests.csproj
```

The project targets .NET 8, enables nullable reference types and implicit usings, and is marked as a test project. The GitHub Actions workflow restores dependencies and builds and tests project files with `dotnet`.

## Dependencies

The test project references `WalletSystem.Core` and uses `Microsoft.NET.Test.Sdk`, xUnit and its Visual Studio runner, FluentAssertions, Moq, and Coverlet’s collector.
