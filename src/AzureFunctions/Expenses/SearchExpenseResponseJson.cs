//-----------------------------------------------------------------------
// <copyright file="SearchExpenseResponseJson.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions
{
    using System.Text.Json.Serialization;

    public class SearchExpenseResponseJson
    {
        public SearchExpenseResponseJson(decimal amountValue, string amountCurrency, DateTime date, string description, string type, string userFulleName)
        {
            this.AmountValue = amountValue;
            this.AmountCurrency = amountCurrency;
            this.Date = date;
            this.Description = description;
            this.Type = type;
            this.UserFulleName = userFulleName;
        }

        [JsonPropertyName("amountValue")]
        public decimal AmountValue { get; }

        [JsonPropertyName("amountCurrency")]
        public string AmountCurrency { get; }

        [JsonPropertyName("date")]
        public DateTime Date { get; }

        [JsonPropertyName("description")]
        public string Description { get; }

        [JsonPropertyName("type")]
        public string Type { get; }

        [JsonPropertyName("userFullName")]
        public string UserFulleName { get; }
    }
}
