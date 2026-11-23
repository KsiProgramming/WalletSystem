# Azure Functions Tests

## Purpose and repository role

This .NET test project verifies behavior in the `src/AzureFunctions` application layer, which exposes WalletSystem functionality through Azure Functions. It references both the Azure Functions and Core projects, so its tests can exercise HTTP-facing functions and their interaction with core interfaces.

## Main parts and tested interfaces

- **Expenses** — `ExpenseFuctionsTest` covers `ExpenseFunctions.CreateExpenseAsync` and `SearchAsync`, including mapping request data to `IExpenseManager` calls and producing JSON responses. Search cases cover user ID, sort field, and sort option query parameters. `CreateExpenseRequestJsonTest` checks the expense request DTO.
- **Exception handling** — `ProblemDetailsTest` checks the `ProblemDetails` data model, and `ProblemDetailsOptionsTest` checks converter registration, conversion, and duplicate registration errors.
- **Problem-details converters** — focused tests check the converter singletons and output for expense currency mismatch, future or old expense dates, duplicate expenses, missing descriptions, and missing users.
- **API documentation functions** — tests check that the YAML and HTML endpoints return the contents of their respective documentation files with the expected content types.

Tests use xUnit, FluentAssertions, and Moq. Global imports provide xUnit and FluentAssertions.

## Build and test

The project targets .NET 8. Run its tests from the repository root with:

```sh
dotnet test tests/AzureFunctions.Tests/WalletSystem.AzureFunctions.Tests.csproj
```

The repository’s GitHub Actions workflow uses .NET 8, restores dependencies, builds projects, and runs tests.

## Dependencies

The project references `WalletSystem.AzureFunctions.csproj` and `WalletSystem.Core.csproj`. Test packages include xUnit, `Microsoft.NET.Test.Sdk`, FluentAssertions, Moq, and coverlet; the project also references the SonarAnalyzer.CSharp analyzer.
