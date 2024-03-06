//-----------------------------------------------------------------------
// <copyright file="DateOlderThanThreeMonthsRule.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules
{
    public class DateOlderThanThreeMonthsRule : IExpenseRuleCheck<ExpenseValidationRequest>
    {
        private readonly ISystemClock systemClock;

        public DateOlderThanThreeMonthsRule(ISystemClock systemClock)
        {
            this.systemClock = systemClock;
        }

        public void Check(ExpenseValidationRequest request)
        {
            if (request.IsDateOlderThanThreeMonths(this.systemClock.UtcNow.Date))
            {
                throw new ExpenseDateOlderThanThreeMonthsException("Expense creation failed: The expense date is greater than 3 months old.");
            }
        }
    }
}
