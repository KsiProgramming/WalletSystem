//-----------------------------------------------------------------------
// <copyright file="ExpenseCreationTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Tests
{
    public class ExpenseCreationTest
    {
        [Fact]
        public void Constructor()
        {
            var expenseCreation = new ExpenseCreation(
                amount: 11_22,
                currency: Currency.USD,
                date: new DateTime(2024, 02, 08, 0, 0, 0, DateTimeKind.Utc),
                description: "Stadium Hotel",
                userId: 5,
                type: ExpenseType.Hotel);

            expenseCreation.Amount.Should().Be(11_22);
            expenseCreation.Currency.Should().Be(Currency.USD);
            expenseCreation.Date.Should().Be(new DateTime(2024, 02, 08, 0, 0, 0, DateTimeKind.Utc));
            expenseCreation.Description.Should().Be("Stadium Hotel");
            expenseCreation.UserId.Should().Be(5);
            expenseCreation.Type.Should().Be(ExpenseType.Hotel);
        }
    }
}
