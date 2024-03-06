//-----------------------------------------------------------------------
// <copyright file="CurrencyMissMatchRule.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules
{
    public class CurrencyMissMatchRule : IExpenseRuleCheck<ExpenseValidationRequest>
    {
        public void Check(ExpenseValidationRequest request)
        {
            if (request.IsCurrencyMismatch())
            {
                throw new ExpenseCurrencyMissMatchException("Expense creation failed: The user currency and the expense currency are different.");
            }
        }
    }
}
