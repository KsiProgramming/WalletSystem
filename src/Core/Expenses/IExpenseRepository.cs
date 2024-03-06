//-----------------------------------------------------------------------
// <copyright file="IExpenseRepository.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses
{
    public interface IExpenseRepository
    {
        Task AddAsync(ExpenseCreation expenseCreation);

        Task<IReadOnlyCollection<Expense>> FindAsync(ExpenseQuery query);
    }
}