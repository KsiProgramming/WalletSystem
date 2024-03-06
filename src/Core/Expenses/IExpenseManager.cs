//-----------------------------------------------------------------------
// <copyright file="IExpenseManager.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses
{
    public interface IExpenseManager
    {
        Task CreateAsync(ExpenseCreation expenseCreation);

        Task<IReadOnlyCollection<Expense>> FindAsync(ExpenseQuery query);
    }
}