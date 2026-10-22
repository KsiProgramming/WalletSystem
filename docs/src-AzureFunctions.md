<!-- module: src/AzureFunctions -->
# WalletSystem.AzureFunctions

This is the HTTP entry point of the Wallet System. It is an Azure Functions app (isolated worker model, Functions v4, .NET 8). It exposes the expense API and hosts the API documentation. Business logic is not in this project. It lives in the referenced `Core` and `Sql` projects.

## Role in the repository

- The project is `WalletSystem.AzureFunctions.csproj`, with `OutputType` `Exe`.
- It references `..\Core\WalletSystem.Core.csproj` and `..\Sql\WalletSystem.Sql.csproj`.
- The functions use `IExpenseManager`, `ExpenseCreation`, `ExpenseQuery`, `Expense`, `Currency`, `ExpenseType` and `SortBy`. These types come from the Core project.
- Core's rule exceptions (`WalletSystem.Expenses.RulesValidator`) and `UserNotFoundException` are translated into HTTP error responses here.
- `Program.cs` wires dependencies with `AddPersistence(...)` and `AddCore()`. Both are extension methods defined outside this project. It then calls `InitialzeDatabase()` on a scoped service provider before the host runs.

## Main parts

### Startup (`Program.cs`)
`Program.Main` builds the host with `ConfigureFunctionsWebApplication`. It does the following:
- Registers `ExceptionHandlingMiddleware`.
- Enables Application Insights (`AddApplicationInsightsTelemetryWorkerService`, `ConfigureFunctionsApplicationInsights`).
- Registers persistence and core services.
- Registers each exception-to-problem-details converter as a singleton `IExceptionProblemDetailsConverter<T>`.
- Registers the same converters, through their `Instance` singletons, in `ProblemDetailsOptions`.
- Initialises the database.

### HTTP functions

**Expenses** (`Expenses/ExpenseFunctions.cs`). All functions use `AuthorizationLevel.Anonymous`.

| Function | Method and route | Behaviour |
|---|---|---|
| `CreateExpense` | `POST api/Expenses/Create` | Reads a `CreateExpenseRequestJson` body and parses `CurrencyCode` and `ExpenseType` into the `Currency` and `ExpenseType` enums. Calls `IExpenseManager.CreateAsync` and returns 200. |
| `SearchExpenses` | `GET api/Expenses/search` | Reads the optional query parameters `UserId`, `SortBy` (case-insensitive enum) and `SortOption`. Calls `IExpenseManager.FindAsync` and returns a JSON array of `SearchExpenseResponseJson`. |

**API documentation** (`ApiDocumentations/ApiDocumentationsFunctions.cs`)

| Function | Method and route | Behaviour |
|---|---|---|
| `ApiDocumentations` | `GET api/api.yaml` | Serves `ApiDocumentations/API.yaml` as `text/yaml`. |
| `ApiDocumentationsUI` | `GET api` | Serves `ApiDocumentations/swagger-ui.html`. |

`host.json` sets `routePrefix` to an empty string. The `api/` part of each route therefore comes from the route templates. `API.yaml` is an OpenAPI description titled "Wallet System". It documents `/Expenses/Create` and `/Expenses/search`. The `.csproj` copies `API.yaml` and `swagger-ui.html` to the output directory.

### JSON contracts (`Expenses/`)
- `CreateExpenseRequestJson` has the properties `Amount`, `CurrencyCode`, `Date`, `Description`, `ExpenseType` and `UserId`.
- `SearchExpenseResponseJson` has the properties `amountValue`, `amountCurrency`, `date`, `description`, `type` and `userFullName`.

### Error handling (`ExceptionHandling/`)
- **`ExceptionHandlingMiddleware`**
  - It is an `IFunctionsWorkerMiddleware` that catches exceptions from HTTP-triggered functions. If the invocation has no HTTP request, it rethrows.
  - It looks up a converter in `ProblemDetailsOptions`. The lookup matches the exact exception type.
  - If none is found, it returns a 500 with the title `InternalServerError`. The detail is the full exception text in the Development environment. Otherwise it is a generic support message.
  - It fills in a default `title` and `type` (RFC 9110 links) for known status codes.
  - It writes the response as `application/problem+json`.
- **`ProblemDetails`** is an RFC 7807 model with `type`, `title`, `status`, `detail`, `instance` and extension data.
- **`IExceptionProblemDetailsConverter<TException>`** is the interface for converting an exception into a `ProblemDetails`.
- **`ProblemDetailsOptions`** holds the registered converters. `AddExceptionConverter<T>` throws an `ArgumentException` if a converter is already registered for that type. `TryConvert` returns `null` if there is no match.

Converters, each exposed as a singleton `Instance`:

| Exception | Status | Title |
|---|---|---|
| `ExpenseCurrencyMissMatchException` | 422 | `CurrencyMissMatch` |
| `ExpenseDateOlderThanThreeMonthsException` | 422 | `ExpenseDateOlderThanThreeMonths` |
| `ExpenseDateInFutureException` | 422 | `ExpenseDateInFuture` |
| `ExpenseDuplicateFoundException` | 422 | `ExpenseDuplicateFound` |
| `ExpenseMissingDescriptionException` | 422 | `ExpenseMissingDescription` |
| `UserNotFoundException` | 404 | `UserNotFound` |

The first five are in `Expenses/ProblemDetailsConverters/`. The `UserNotFound` converter is in `Expenses/Users/ProblemDetailsConverters/`. Each converter uses the exception's `Message` as the `detail`.

To add a new mapping, write a converter and register it in both places in `Program.cs`.

## Running locally

- `Properties/launchSettings.json` defines an `AzureFunctions` profile. It starts the app on port 7159 and opens the browser at `api`, which is the Swagger UI.
- The profile sets `WalletSystem_SQL_CONNECTIONSTRING` to a LocalDB database: `Server=(LocalDB)\MSSQLLocalDB;Database=WalletSystemDB;Trusted_Connection=True`.
- `local.settings.json` is copied to the output directory but never to the publish directory. It is git-ignored in this folder, so you must create it yourself.
- `Properties/serviceDependencies.json` lists Application Insights and a storage account (`AzureWebJobsStorage`) as dependencies.
- There are no tests in this folder.

## Notable dependencies

- `Microsoft.Azure.Functions.Worker` 1.20.1
- `Microsoft.Azure.Functions.Worker.Sdk` 1.16.4
- `Microsoft.Azure.Functions.Worker.Extensions.Http` 3.1.0
- `Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore` 1.2.0
- `Microsoft.ApplicationInsights.WorkerService` 2.21.0
- `Microsoft.Azure.Functions.Worker.ApplicationInsights` 1.1.0
- `Microsoft.AspNetCore.App` framework reference
- The project references `WalletSystem.Core` and `WalletSystem.Sql`.
