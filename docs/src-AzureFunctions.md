# Azure Functions

## Purpose and repository role

This .NET 8 Azure Functions v4 isolated-worker project exposes the Wallet System expense operations over HTTP. It connects the HTTP layer to the Core business services and SQL persistence project, and serves the API specification and Swagger UI.

## Main parts and public interfaces

- **`Program.cs`** configures the Functions web application, registers Application Insights, Core and persistence services, and installs the exception-handling middleware. At startup, it initializes the database through the persistence service.
- **`Expenses/ExpenseFunctions.cs`** exposes two anonymous HTTP endpoints:
  - `POST /api/Expenses/Create` reads an expense request containing `amount`, `currencyCode`, `date`, `description`, `expenseType`, and `userId`, then calls `IExpenseManager.CreateAsync`. Success returns HTTP 200.
  - `GET /api/Expenses/search` accepts optional `UserId`, `SortBy`, and `SortOption` query parameters, calls `IExpenseManager.FindAsync`, and returns a JSON array of expense results.
- **`ApiDocumentations/ApiDocumentationsFunctions.cs`** serves the OpenAPI document at `GET /api/api.yaml` and the Swagger UI at `GET /api`. The project copies `API.yaml` and `swagger-ui.html` to the output directory.
- **`ExceptionHandling/`** defines the RFC 7807-style `ProblemDetails` response, the `IExceptionProblemDetailsConverter<TException>` interface, converter registration options, and middleware that maps exceptions to HTTP responses. Registered expense validation errors return 422; a missing user returns 404. Unmapped exceptions return 500, with exception details included in development.
- **`Expenses/`** also contains the JSON request and response models and the expense-specific problem-detail converters. The user-not-found converter is under `Expenses/Users/`.

## Build, run, and test

Build the module with:

```sh
dotnet build src/AzureFunctions/WalletSystem.AzureFunctions.csproj
```

Run its tests with:

```sh
dotnet test tests/AzureFunctions.Tests/WalletSystem.AzureFunctions.Tests.csproj
```

The CI workflow restores dependencies, builds projects, and runs tests using .NET 8. The project includes an `AzureFunctions` launch profile configured for port 7159 and launch URL `api`. The Functions runtime requires the `WalletSystem_SQL_CONNECTIONSTRING` setting. Startup calls `EnsureDeleted()` followed by `EnsureCreated()` on the configured database, so running the host recreates that database.

## Dependencies

The project uses the Azure Functions Worker, HTTP and ASP.NET Core HTTP extensions, the ASP.NET Core shared framework, and Application Insights Worker packages. It references `WalletSystem.Core` for expense and user services and `WalletSystem.Sql` for SQL Server persistence.
