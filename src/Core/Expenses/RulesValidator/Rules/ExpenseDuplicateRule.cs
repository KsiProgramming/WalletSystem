//-----------------------------------------------------------------------
// <copyright file="ExpenseDuplicateRule.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules
{
    public class ExpenseDuplicateRule : IExpenseRuleCheck<ExpenseValidationRequest>
    {
        public void Check(ExpenseValidationRequest request)
        {
            if (request.IsDuplicatedExpense)
            {
                throw new ExpenseDuplicateFoundException($"Expense creation failed: Duplicated expense was found.");
            }
        }
    }
}
