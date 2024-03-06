//-----------------------------------------------------------------------
// <copyright file="ExpenseCreation.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses
{
    public class ExpenseCreation
    {
        public ExpenseCreation(decimal amount, Currency currency, DateTime date, string description, int userId, ExpenseType type)
        {
            this.Amount = amount;
            this.Currency = currency;
            this.Date = date;
            this.Description = description;
            this.UserId = userId;
            this.Type = type;
        }

        public decimal Amount { get; }

        public Currency Currency { get; }

        public DateTime Date { get; }

        public string Description { get; }

        public int UserId { get; }

        public ExpenseType Type { get; }
    }
}
