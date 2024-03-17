//-----------------------------------------------------------------------
// <copyright file="ExpenseData.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Sql
{
    public class ExpenseData
    {
        public ExpenseData(decimal amount, DateTime date, string description, int userId, int type)
        {
            this.Amount = amount;
            this.Date = date;
            this.Description = description;
            this.UserId = userId;
            this.Type = type;
        }

        public int Id { get; set; }

        public decimal Amount { get; }

        public DateTime Date { get; }

        public string Description { get; }

        public int UserId { get; }

        public UserData? User { get; set; }

        public int Type { get; }
    }
}
