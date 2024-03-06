//-----------------------------------------------------------------------
// <copyright file="AmountTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Tests
{
    public class AmountTest
    {
        [Fact]
        public void Constructor()
        {
            var amount = new Amount(
                value: 12_003m,
                currency: Currency.USD);

            amount.Currency.Should().Be(Currency.USD);
            amount.Value.Should().Be(12_003m);
        }
    }
}
