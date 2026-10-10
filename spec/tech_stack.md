# Tech Stack

## Current stack

| Layer | Technology | Project |
|---|---|---|
| Runtime | .NET 8 (C# latest, nullable enabled) | all |
| API host | Azure Functions v4, isolated worker, ASP.NET Core integration | `src/AzureFunctions` |
| Domain | Plain C#, DI abstractions only | `src/Core` |
| Persistence | EF Core 8 + SQL Server | `src/Sql` |
| DB schema | SQL Database project (`.sqlproj`) | `src/Sql.Database` |
| API docs | OpenAPI (`API.yaml`) served through a Swagger UI function | `src/AzureFunctions/ApiDocumentations` |
| Errors | RFC 7807 Problem Details, built by middleware and per-exception converters | `src/AzureFunctions/ExceptionHandling` |
| Telemetry | Application Insights (worker service) | `src/AzureFunctions` |
| Tests | xUnit, Moq, FluentAssertions, coverlet | `tests/*` |
| Code quality | StyleCop.Analyzers, SonarAnalyzer.CSharp, `CodeAnalysisRules.ruleset` | `Directory.Build.props` |
| CI | GitHub Actions: restore, build, test on push/PR to `master` | `.github/workflows/dotnet.yml` |

### Architecture conventions
- **Core has no infrastructure dependencies.** Repositories are interfaces in Core
  (`IExpenseRepository`, `IUserRepository`) and are implemented in `src/Sql`.
- **Composition roots.** Core uses `AddCore()` and Sql uses `AddPersistence()`, both wired in
  `Program.cs`.
- **Validation.** Each rule implements `IExpenseRuleCheck<ExpenseValidationRequest>`, throws a
  dedicated exception, and has a matching `IExceptionProblemDetailsConverter`.
- **Time.** All code reads the current time through `ISystemClock` so tests can control it.

## Gaps to close

### 1. Authentication and authorization
- **Today:** every function is `AuthorizationLevel.Anonymous`, and `userId` is passed in the request
  body. Any caller can read or write any user's wallet.
- **Target:** callers authenticate with JWT bearer tokens, for example from Microsoft Entra ID
  (external ID for consumers). The user id comes from the token claims and never from the request
  body. A user can only access their own resources.

### 2. Database migrations
- **Today:** `InitialzeDatabase()` calls `EnsureDeleted()` and then `EnsureCreated()` on every
  startup. **All data is wiped each time the app restarts.** There are also two sources of schema
  truth: the EF model and `src/Sql.Database`.
- **Target:** keep one source of schema truth (EF Core migrations is recommended) and apply schema
  changes without losing data. Remove `EnsureDeleted` from startup.

### 3. Integration tests
- **Today:** `src/Sql` has no tests. The repositories and EF mappings are never checked against a
  real database.
- **Target:** add a `tests/Sql.Tests` project that uses Testcontainers (SQL Server) and runs in CI.

### 4. Spending policy configuration storage
- **Decision:** spending policies are **per-user data stored in SQL**, in a new `SpendingPolicy`
  table. They are not app configuration, because each user defines their own limits.
- Users manage their policies through the API, and the domain reads them through a new repository
  interface in Core.

### 5. Deployment and IaC
- **Today:** CI only builds and tests. There is no deployment, and `ci-pipeline.yml` at the repo root
  is an outdated copy of the workflow.
- **Target:** Bicep templates for the Function App, Azure SQL, Application Insights and Key Vault,
  plus a GitHub Actions CD workflow that authenticates through OIDC federated credentials. Remove the
  duplicate pipeline file.
