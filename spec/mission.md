# Mission

## Purpose
WalletSystem is a **personal finance wallet API**. People use it to record their everyday expenses
and stay within the spending limits they set for themselves.

Every expense goes through a set of rules before it is accepted, so the data a person sees about
their own spending is clean, consistent and trustworthy.

## Target audience
**Individual end users** who manage their own wallet.

- Each user owns their own data. They see and act only on their own expenses and policies.
- There are no admin or finance roles. The user who owns a wallet is the one who configures it.
- The README sequence diagrams use the example user "Donald". He records an expense and later
  searches his history.

## What the product does today
- **Record an expense** (`POST api/Expenses/Create`) with an amount, currency, date, description and
  type (`Restaurant`, `Hotel`, `Misc`).
- **Search expenses** (`GET api/Expenses/search`).
- **Validate every expense** through pluggable rules (`IExpenseRuleCheck`):
  - the date is not in the future
  - the date is not older than 3 months
  - it does not duplicate an existing expense
  - its currency matches the user's currency
  - its description is not empty
- **Report errors** as RFC 7807 Problem Details, so clients get a clear reason when a rule rejects
  an expense.

## What it will do next
- **Spending policy**: the user defines personal spending limits, for example per expense type and
  period. Expenses are checked against those limits (see [roadmap.md](roadmap.md)).

## Guiding principles
1. **The user owns their data.** Every operation is scoped to the authenticated user.
2. **Rules live in the domain.** Business rules belong in `WalletSystem.Core` as small, testable
   rule classes. They do not belong in the API or SQL layers.
3. **Fail clearly.** Every rejection maps to a specific exception and a Problem Details response.
4. **Small, safe increments.** Work ships in small phases, and each phase is covered by tests.

## Out of scope (for now)
- Shared or family wallets, and multi-user roles
- Bank or card integrations and automatic transaction import
- Converting between currencies (a user has a single currency)
