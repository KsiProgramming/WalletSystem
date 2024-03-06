//-----------------------------------------------------------------------
// <copyright file="ExpenseQuery.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses
{
    public enum SortBy
    {
        Amount = 1,
        Date,
    }

    public class ExpenseQuery
    {
        public ExpenseQuery()
        {
        }

        public DateTime? Date { get; set; }

        public decimal? Amount { get; set; }

        public int? UserId { get; set; }

        public SortBy? SortBy { get; set; }

        public string? SortOption { get; set; }
    }
}