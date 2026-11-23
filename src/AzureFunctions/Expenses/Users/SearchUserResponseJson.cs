//-----------------------------------------------------------------------
// <copyright file="SearchUserResponseJson.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.AzureFunctions
{
    using System.Text.Json.Serialization;

    public class SearchUserResponseJson
    {
        public SearchUserResponseJson(int id, string firstName, string lastName, string currency)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Currency = currency;
        }

        [JsonPropertyName("id")]
        public int Id { get; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; }

        [JsonPropertyName("lastName")]
        public string LastName { get; }

        [JsonPropertyName("currency")]
        public string Currency { get; }
    }
}
