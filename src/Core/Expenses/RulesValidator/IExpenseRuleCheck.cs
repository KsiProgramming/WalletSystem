//-----------------------------------------------------------------------
// <copyright file="IExpenseRuleCheck.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public interface IExpenseRuleCheck<in T>
        where T : ExpenseValidationRequest
    {
        void Check(T request);
    }
}
