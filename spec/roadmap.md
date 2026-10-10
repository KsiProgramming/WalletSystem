# Roadmap

The order follows [TODO.md](../TODO.md): **spending policy comes first**. The tech stack gaps from
[tech_stack.md](tech_stack.md) follow it. Each phase is a single small, mergeable change with its
own tests. CI must be green before the next phase starts.

---

## Milestone 1: Spending policy (TODO: "ADD spending policy")

A user sets personal limits, for example "at most 300 EUR on Restaurant per month". An expense that
would push spending over a limit is rejected.

- [ ] **Phase 1.1: Domain model.** Add a `SpendingPolicy` record in Core with `UserId`, an optional
  `ExpenseType` (null means all types), `Period` (start with `Monthly`) and `Limit`. Unit tests
  only.
- [ ] **Phase 1.2: Repository contract.** Add `ISpendingPolicyRepository` in Core with
  `FindByUserAsync(int userId)`. No implementation yet.
- [ ] **Phase 1.3: Spend calculation.** Add a pure domain function that computes the amount
  already spent in the current period for a type, from a list of expenses. Use `ISystemClock`.
  Unit tests.
- [ ] **Phase 1.4: Rule and exception.** Add `SpendingLimitExceededRule` and
  `SpendingLimitExceededException`, and extend `ExpenseValidationRequest` with the policy and
  spend data. Unit tests.
- [ ] **Phase 1.5: Wire into ExpenseManager.** `CreateAsync` loads the policies and the current
  spend, and passes them to the validator. Update `ExpenseManagerTest`.
- [ ] **Phase 1.6: Problem Details.** Add `SpendingLimitExceededExceptionProblemDetailsConverter`
  and register it in `Program.cs`. Converter test.
- [ ] **Phase 1.7: SQL storage.** Add the `SpendingPolicy` table (`.sqlproj` + EF configuration),
  `SpendingPolicyData`, and the repository implementation. Register it in `AddPersistence()`.
- [ ] **Phase 1.8: API: create and list policies.** Add `POST api/SpendingPolicies` and
  `GET api/SpendingPolicies`, and update `API.yaml`. Function tests.
- [ ] **Phase 1.9: API: update and delete policies.** Add `PUT` and `DELETE` endpoints, and update
  `API.yaml`.
- [ ] **Phase 1.10: Docs.** Add the spending policy flow to the README sequence diagram and the
  ERD.

## Milestone 2: Safe database lifecycle

- [ ] **Phase 2.1:** Remove `EnsureDeleted()` from `InitialzeDatabase()` so restarts no longer wipe
  data.
- [ ] **Phase 2.2:** Add an initial EF Core migration that matches the current schema, and replace
  `EnsureCreated()` with `Migrate()`.
- [ ] **Phase 2.3:** Choose a single schema source (EF migrations or `.sqlproj`) and retire the
  other.
- [ ] **Phase 2.4:** Add a `tests/Sql.Tests` project that uses Testcontainers, with a first test
  for `UsersRepository`.
- [ ] **Phase 2.5:** Add integration tests for `ExpenseRepository` and `SpendingPolicyRepository`.
- [ ] **Phase 2.6:** Make sure the integration tests run in the GitHub Actions workflow.

## Milestone 3: Authentication and ownership

- [ ] **Phase 3.1:** Add JWT bearer validation middleware. Configure it only, and do not enforce it
  yet.
- [ ] **Phase 3.2:** Add an `ICurrentUser` abstraction that resolves the user id from token claims.
- [ ] **Phase 3.3:** Expenses endpoints: take the user id from `ICurrentUser`, and remove `userId`
  from the request body and query.
- [ ] **Phase 3.4:** Spending policy endpoints: take the user id from `ICurrentUser`.
- [ ] **Phase 3.5:** Enforce authentication on all endpoints except the API docs, and return 401
  or 403 as Problem Details.
- [ ] **Phase 3.6:** Update `API.yaml` with the security scheme.

## Milestone 4: Deployment

- [ ] **Phase 4.1:** Delete the stale root `ci-pipeline.yml`.
- [ ] **Phase 4.2:** Add a Bicep template for the Function App and Application Insights.
- [ ] **Phase 4.3:** Add Azure SQL and Key Vault (connection string as a Key Vault reference) to
  the Bicep template.
- [ ] **Phase 4.4:** Add a CD workflow that uses OIDC login, deploys the infrastructure, and then
  deploys the Function App to a dev environment.
- [ ] **Phase 4.5:** Run migrations as part of the deployment.

---

## Open questions
- Spending policy **periods** other than monthly, such as weekly or yearly. This is deferred to
  after Milestone 1.
- Should going over a limit be a **hard reject** (the current plan) or a **soft warning** that
  still accepts the expense?
- Should **user registration** (creating a `User` with a currency) go through the API once auth
  exists?
