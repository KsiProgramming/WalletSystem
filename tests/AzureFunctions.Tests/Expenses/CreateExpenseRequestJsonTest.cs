//-----------------------------------------------------------------------
// <copyright file="CreateExpenseRequestJsonTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions.Tests
{
    using FluentAssertions;

    public class CreateExpenseRequestJsonTest
    {
        [Fact]
        public void Constructor()
        {
            var request = new CreateExpenseRequestJson(
                amount: 12_23m,
                currencyCode: "USD",
                date: new DateTime(2024, 02, 11, 0, 0, 0, DateTimeKind.Utc),
                description: "This is description section.",
                expenseType: "Restaurant",
                userId: 3);

            request.Amount.Should().Be(12_23m);
            request.CurrencyCode.Should().Be("USD");
            request.Date.Should().Be(new DateTime(2024, 02, 11, 0, 0, 0, DateTimeKind.Utc));
            request.Description.Should().Be("This is description section.");
            request.ExpenseType.Should().Be("Restaurant");
            request.UserId.Should().Be(3);
        }
    }
}
