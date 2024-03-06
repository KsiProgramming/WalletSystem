//-----------------------------------------------------------------------
// <copyright file="IExpenseRulesValidator.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public interface IExpenseRulesValidator
    {
        void Validate(ExpenseValidationRequest request);
    }
}