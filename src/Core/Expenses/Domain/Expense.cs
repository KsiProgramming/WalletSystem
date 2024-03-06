//-----------------------------------------------------------------------
// <copyright file="Expense.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses
{
    public class Expense
    {
        public Expense(Amount amount, string description, DateTime date, string user, ExpenseType type)
        {
            this.Amount = amount;
            this.Description = description;
            this.Date = date;
            this.User = user;
            this.Type = type;
        }

        public Amount Amount { get; }

        public string Description { get; }

        public DateTime Date { get; }

        public string User { get; }

        public ExpenseType Type { get; }
    }
}
