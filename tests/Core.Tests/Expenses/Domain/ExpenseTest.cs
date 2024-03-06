//-----------------------------------------------------------------------
// <copyright file="ExpenseTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Tests
{
    public class ExpenseTest
    {
        [Fact]
        public void Constructor()
        {
            var amount = new Amount(value: 11_22m, currency: Currency.USD);
            var expense = new Expense(
                amount: amount,
                description: "Description for expense",
                date: new DateTime(2023, 02, 05, 0, 0, 0, DateTimeKind.Utc),
                user: "FirstName LastName",
                type: ExpenseType.Restaurant);

            expense.Amount.Should().BeSameAs(amount);
            expense.Description.Should().Be("Description for expense");
            expense.Date.Should().Be(new DateTime(2023, 02, 05, 0, 0, 0, DateTimeKind.Utc));
            expense.User.Should().Be("FirstName LastName");
            expense.Type.Should().Be(ExpenseType.Restaurant);
        }
    }
}
