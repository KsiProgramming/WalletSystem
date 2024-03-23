//-----------------------------------------------------------------------
// <copyright file="DateInFutureRule.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules
{
    using System.Globalization;

    public class DateInFutureRule : IExpenseRuleCheck<ExpenseValidationRequest>
    {
        private readonly ISystemClock systemClock;

        public DateInFutureRule(ISystemClock systemClock)
        {
            this.systemClock = systemClock;
        }

        public void Check(ExpenseValidationRequest request)
        {
            if (request.IsDateInFuture(this.systemClock.UtcNow.Date))
            {
                var date = Convert.ToString(request.Date, CultureInfo.CreateSpecificCulture("fr-FR"));
                throw new ExpenseDateInFutureException($"Expense creation failed: The expense date cannot be in the future. Please provide a valid date. '{date}'");
            }
        }
    }
}
