//-----------------------------------------------------------------------
// <copyright file="CreateExpenseRequestJson.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions
{
    using System.Text.Json.Serialization;

    public sealed class CreateExpenseRequestJson
    {
        public CreateExpenseRequestJson(decimal amount, string currencyCode, DateTime date, string description, string expenseType, int userId)
        {
            this.Amount = amount;
            this.CurrencyCode = currencyCode;
            this.Date = date;
            this.Description = description;
            this.ExpenseType = expenseType;
            this.UserId = userId;
        }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; }

        [JsonPropertyName("CurrencyCode")]
        public string CurrencyCode { get; }

        [JsonPropertyName("Date")]
        public DateTime Date { get; }

        [JsonPropertyName("Description")]
        public string Description { get; }

        [JsonPropertyName("ExpenseType")]
        public string ExpenseType { get; }

        [JsonPropertyName("UserId")]
        public int UserId { get; }
    }
}
