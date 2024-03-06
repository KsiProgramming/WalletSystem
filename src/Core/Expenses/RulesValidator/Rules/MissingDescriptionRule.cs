//-----------------------------------------------------------------------
// <copyright file="MissingDescriptionRule.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules
{
    public class MissingDescriptionRule : IExpenseRuleCheck<ExpenseValidationRequest>
    {
        public void Check(ExpenseValidationRequest request)
        {
            if (request.IsDescriptionNullOrWhiteSpace())
            {
                throw new ExpenseMissingDescriptionException("Expense creation failed: Description is a required field.");
            }
        }
    }
}
