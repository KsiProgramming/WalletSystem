//-----------------------------------------------------------------------
// <copyright file="ExpenseValidationRequestTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Tests
{
    public class ExpenseValidationRequestTest
    {
        [Fact]
        public void Costructor()
        {
            var request = new ExpenseValidationRequest(
                date: new DateTime(2024, 02, 06, 0, 0, 0, DateTimeKind.Utc),
                description: "Description forthe expense",
                expenseCurrency: Currency.USD,
                isDuplicatedExpense: false,
                userCurrency: Currency.USD);

            request.Date.Should().Be(new DateTime(2024, 02, 06, 0, 0, 0, DateTimeKind.Utc));
            request.Description.Should().Be("Description forthe expense");
            request.ExpenseCurrency.Should().Be(Currency.USD);
            request.IsDuplicatedExpense.Should().Be(false);
            request.UserCurrency.Should().Be(Currency.USD);
        }
    }
}
