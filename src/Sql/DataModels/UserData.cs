//-----------------------------------------------------------------------
// <copyright file="UserData.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Sql
{
    public class UserData
    {
        public UserData(int currencyId, string firstName, string lastName)
        {
            this.CurrencyId = currencyId;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public int Id { get; set; }

        public string FirstName { get; }

        public string LastName { get; }

        public int CurrencyId { get; }

        public CurrencyData? Currency { get; set; }
    }
}
