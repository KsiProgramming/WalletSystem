//-----------------------------------------------------------------------
// <copyright file="Amount.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses
{
    public class Amount
    {
        public Amount(decimal value, Currency currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        public decimal Value { get; }

        public Currency Currency { get; }
    }
}
