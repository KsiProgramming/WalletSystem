//-----------------------------------------------------------------------
// <copyright file="User.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users
{
    public class User
    {
        public User(string firstName, string lastName, Currency currency, int id)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Currency = currency;
            this.Id = id;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public Currency Currency { get; }

        public int Id { get; }
    }
}
