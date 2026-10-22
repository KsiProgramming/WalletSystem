<!-- module: tests/AzureFunctions.Tests -->
# AzureFunctions.Tests

Unit tests for the Azure Functions host project `src/AzureFunctions` (`WalletSystem.AzureFunctions`). The test project is `WalletSystem.AzureFunctions.Tests.csproj`. It targets `net8.0` and uses xUnit.

## Purpose and place in the repository

The tests cover the HTTP-facing layer of WalletSystem. They check three things:

- The function classes that handle requests.
- The request and response JSON types.
- The code that turns domain exceptions into RFC 7807-style `ProblemDetails` responses.

The business logic behind these functions (`IExpenseManager` and similar) is replaced with Moq mocks. No real database or Functions host is involved.

## Project references

- `src/AzureFunctions/WalletSystem.AzureFunctions.csproj`: the code under test.
- `src/Core/WalletSystem.Core.csproj`: domain types such as `Currency`, `ExpenseType`, `ExpenseCreation`, `ExpenseQuery`, `SortBy` and the `Expense*Exception` classes.

## Layout

| Path | What it tests |
|---|---|
| `ApiDocumentations/ApiDocumentationsFunctionsTest.cs` | `ApiDocumentationsFunctions.GetApiDocumentationsAsync` returns `ApiDocumentations/API.yaml` with `Content-Type: text/yaml; charset=utf-8`. `GetApiDocumentationsUIAsync` returns `ApiDocumentations/swagger-ui.html` with `text/html; charset=utf-8`. Each test compares the response body, minus the BOM, with the file content on disk. |
| `Expenses/ExpenseFuctionsTest.cs` | `ExpenseFunctions.CreateExpenseAsync` and `SearchAsync` (see below). |
| `Expenses/CreateExpenseRequestJsonTest.cs` | The `CreateExpenseRequestJson` constructor maps `amount`, `currencyCode`, `date`, `description`, `expenseType` and `userId` to properties. |
| `Expenses/ProblemDetailsConverters/` | One test class per expense exception converter: currency mismatch, date in future, date older than three months, duplicate found, missing description. |
| `Expenses/Users/ProblemDetailsConverters/` | The converter for `UserNotFoundException`. |
| `ExceptionHandling/ProblemDetailsTest.cs` | `ProblemDetails` defaults (all properties null, `Extensions` non-null) and property setters. |
| `ExceptionHandling/ProblemDetailsOptionsTest.cs` | `ProblemDetailsOptions.AddExceptionConverter` and `TryConvert`. |
| `GlobalUsings.cs` | Global usings for `FluentAssertions` and `Xunit`. The SA1200 StyleCop warning is suppressed for them. |

## What the main tests check

**Expense functions**
- `CreateExpenseAsync`:
  - The test mocks the Worker `ObjectSerializer` to deserialize the body into a `CreateExpenseRequestJson`.
  - It asserts that `IExpenseManager.CreateAsync` receives an `ExpenseCreation` with the matching amount, `Currency.USD`, date, description, `ExpenseType.Hotel` and user ID.
  - It asserts that the response status is set to `200 OK`.
- `SearchAsync`:
  - Query-string values are mapped to an `ExpenseQuery` passed to `IExpenseManager.FindAsync`. The tests cover no parameters, `userId=5`, `sortBy=amount` (→ `SortBy.Amount`) and `sortOption=ASC`. Parameters that aren't supplied stay `null`.
  - The response is `200 OK` with a single `Content-Type: application/json; charset=utf-8` header.
  - The body is serialized as `SearchExpenseResponseJson[]`.

**Problem details**
- `ProblemDetailsOptions`:
  - A registered converter is used by `TryConvert`.
  - `TryConvert` returns `null` for an exception type with no converter.
  - Registering a second converter for the same exception type throws `ArgumentException`. The message is "A converter has already been registered for the 'FormatException' exception type." and the parameter name is `converter`.
- Each converter test follows the same pattern:
  - The converter exposes a singleton `Instance`.
  - `Convert` copies the exception message into `Detail`.
  - `Convert` produces a fixed `Title`, an HTTP `Status`, empty `Extensions`, and null `Type` and `Instance`.
  - For example, `ExpenseDuplicateFoundExceptionProblemDetailsConverter` yields status `422` and title `ExpenseDuplicateFound`.

## Test conventions

- Tests use xUnit `[Fact]` methods, FluentAssertions for assertions, and Moq for mocks. Most mocks use `MockBehavior.Strict` and are checked with `VerifyAll()`.
- The Functions Worker types (`HttpRequestData`, `HttpResponseData`, `FunctionContext`, `WorkerOptions`) are mocked by hand. Each test builds a mock `IServiceProvider` that returns `IOptions<WorkerOptions>` with the mocked serializer.
- Source files carry a standard copyright header, and the project enables nullable reference types.

## Building and running

The repository doesn't include a dedicated script for these tests. The standard .NET CLI applies:

```
dotnet test tests/AzureFunctions.Tests/WalletSystem.AzureFunctions.Tests.csproj
```

The API documentation tests read `ApiDocumentations/API.yaml` and `ApiDocumentations/swagger-ui.html` from the working directory. These files reach the test output folder because `src/AzureFunctions/WalletSystem.AzureFunctions.csproj` marks them as `Content` with `CopyToOutputDirectory=PreserveNewest`, and the test project references it.

## Dependencies

- `xunit` 2.7.0 and `xunit.runner.visualstudio` 2.5.7
- `Microsoft.NET.Test.Sdk` 17.9.0
- `FluentAssertions` 6.12.0
- `Moq` 4.20.70
- `coverlet.collector` 6.0.2, for code coverage
- `SonarAnalyzer.CSharp` 9.22.0.87781, an analyzer applied through a package `Update`
- Indirectly, through the `src/AzureFunctions` project reference: `Microsoft.Azure.Functions.Worker` and related packages.
