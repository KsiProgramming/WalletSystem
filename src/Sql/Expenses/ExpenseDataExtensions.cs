//-----------------------------------------------------------------------
// <copyright file="ExpenseDataExtensions.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Sql
{
    using Microsoft.EntityFrameworkCore;
    using WalletSystem.Sql;

    public static class ExpenseDataExtensions
    {
        public static IQueryable<ExpenseData> WhereAmountEquals(this IQueryable<ExpenseData> expenses, decimal? amount)
        {
            if (amount.HasValue)
            {
                expenses = expenses.Where(e => e.Amount == amount);
            }

            return expenses;
        }

        public static IQueryable<ExpenseData> WhereDateEquals(this IQueryable<ExpenseData> expenses, DateTime? date)
        {
            if (date.HasValue)
            {
                expenses = expenses.Where(e => e.Date == date);
            }

            return expenses;
        }

        public static IQueryable<ExpenseData> WhereUserIdEquals(this IQueryable<ExpenseData> expenses, int? userId)
        {
            if (userId.HasValue)
            {
                expenses = expenses.Where(e => e.UserId == userId);
            }

            return expenses;
        }

        public static IQueryable<ExpenseData> Sort(this IQueryable<ExpenseData> expenses, SortBy? sortBy, string? sortOption)
        {
            return (sortBy, sortOption?.ToUpper()) switch
            {
                (SortBy.Amount, "DESC") => expenses.OrderByDescending(e => e.Amount),
                (SortBy.Amount, _) => expenses.OrderBy(e => e.Amount),
                (SortBy.Date, "DESC") => expenses.OrderByDescending(e => e.Date),
                (SortBy.Date, _) => expenses.OrderBy(e => e.Date),
                _ => expenses
            };
        }

        public static async Task<IReadOnlyCollection<Expense>> ToExpensesAsync(this IQueryable<ExpenseData> expensesQuery)
        {
            var expensesData = await expensesQuery.ToArrayAsync();

            var expenseTasks = expensesData.Select(ExpenseModelToExpenseAsync);

            return await Task.WhenAll(expenseTasks);
        }

        private static async Task<Expense> ExpenseModelToExpenseAsync(ExpenseData model)
        {
            var amount = new Amount(model.Amount, currency: (Currency)model.User!.CurrencyId);

            return await Task.FromResult(new Expense(
                date: model.Date,
                amount: amount,
                description: model.Description,
                user: $"{model.User?.FirstName} {model.User?.LastName}",
                type: (ExpenseType)model.Type));
        }
    }
}
