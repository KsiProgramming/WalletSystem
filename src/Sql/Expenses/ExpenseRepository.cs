//-----------------------------------------------------------------------
// <copyright file="ExpenseRepository.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Sql
{
    using Microsoft.EntityFrameworkCore;
    using WalletSystem.Sql;

    public class ExpenseRepository : IExpenseRepository
    {
        private readonly WalletSystemDbContext context;

        public ExpenseRepository(WalletSystemDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(ExpenseCreation expenseCreation)
        {
            var expenseData = new ExpenseData(
                description: expenseCreation.Description,
                amount: expenseCreation.Amount,
                userId: expenseCreation.UserId,
                date: expenseCreation.Date,
                type: (int)expenseCreation.Type);

            await this.context.Expenses.AddAsync(expenseData);

            await this.context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Expense>> FindAsync(ExpenseQuery query)
        {
            var expenses = await this.context.Expenses
                .Include(e => e.User)
                .ThenInclude(u => u!.Currency)
                .AsNoTracking()
                .AsQueryable()
                .WhereAmountEquals(query.Amount)
                .WhereDateEquals(query.Date)
                .WhereUserIdEquals(query.UserId)
                .Sort(query.SortBy, query.SortOption)
                .ToExpensesAsync();

            return expenses;
        }
    }
}
