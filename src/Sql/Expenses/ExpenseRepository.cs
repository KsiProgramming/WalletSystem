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
            var expenses = this.context.Expenses
                .Include(e => e.User)
                .ThenInclude(u => u!.Currency)
                .AsNoTracking()
                .AsQueryable();

            if (query.Date.HasValue)
            {
                expenses = expenses.Where(e => e.Date.Date == query.Date.Value.Date);
            }

            if (query.Amount.HasValue)
            {
                expenses = expenses.Where(e => e.Amount == query.Amount);
            }

            if (query.UserId.HasValue)
            {
                expenses = expenses.Where(e => e.UserId == query.UserId);
            }

            if (query.SortBy.HasValue)
            {
                if (query.SortBy == SortBy.Amount)
                {
                    expenses = query.SortOption?.ToUpper() == "DESC"
                    ? expenses.OrderByDescending(e => e.Amount)
                    : expenses.OrderBy(e => e.Amount);
                }

                if (query.SortBy == SortBy.Date)
                {
                    expenses = query.SortOption?.ToUpper() == "DESC"
                    ? expenses.OrderByDescending(e => e.Date)
                    : expenses.OrderBy(e => e.Date);
                }
            }

            var result = await expenses.ToArrayAsync();

            return result
                .Select(ExpenseModelToExpense)
                .ToArray();
        }

        private static Expense ExpenseModelToExpense(ExpenseData model)
        {
            var amount = new Amount(model.Amount, currency: (Currency)model.User!.CurrencyId);

            return new Expense(
                date: model.Date,
                amount: amount,
                description: model.Description,
                user: $"{model.User?.FirstName} {model.User?.LastName}",
                type: (ExpenseType)model.Type);
        }
    }
}
