//-----------------------------------------------------------------------
// <copyright file="ExpenseValidationRequest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public class ExpenseValidationRequest
    {
        public ExpenseValidationRequest(DateTime date, string description, Currency expenseCurrency, bool isDuplicatedExpense, Currency userCurrency)
        {
            this.Date = date;
            this.Description = description;
            this.ExpenseCurrency = expenseCurrency;
            this.IsDuplicatedExpense = isDuplicatedExpense;
            this.UserCurrency = userCurrency;
        }

        public DateTime Date { get; }

        public string Description { get; }

        public Currency ExpenseCurrency { get; }

        public bool IsDuplicatedExpense { get; }

        public Currency UserCurrency { get; }
    }
}
