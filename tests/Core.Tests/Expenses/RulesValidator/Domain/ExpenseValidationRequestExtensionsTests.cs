//-----------------------------------------------------------------------
// <copyright file="ExpenseValidationRequestExtensionsTests.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Tests
{
    using System.Globalization;

    public class ExpenseValidationRequestExtensionsTests
    {
        [Theory]
        [InlineData(Currency.USD, Currency.USD, false)]
        [InlineData(Currency.USD, Currency.RUB, true)]
        [InlineData(Currency.RUB, Currency.USD, true)]
        public void IsCurrencyMismatch(Currency expenseCurrency, Currency userCurrency, bool expectedResult)
        {
            var request = new ExpenseValidationRequest(
                date: default,
                description: default!,
                expenseCurrency: expenseCurrency,
                isDuplicatedExpense: default,
                userCurrency: userCurrency);

            var result = request.IsCurrencyMismatch();

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("2023-01-01T04:50:06", "2024-02-06T09:50:05", false)]
        [InlineData("2024-02-05T04:50:06", "2024-02-06T09:50:05", false)]
        [InlineData("2024-02-06T09:00:06", "2024-02-06T09:50:05", false)]
        [InlineData("2024-02-06T04:50:06", "2024-02-06T09:50:05", false)]
        [InlineData("2024-02-06T10:50:06", "2024-02-06T09:50:05", false)]
        [InlineData("2024-02-07T04:50:06", "2024-02-06T09:50:05", true)]
        [InlineData("2025-12-31T04:50:06", "2024-02-06T09:50:05", true)]
        public void IsDateInFuture(string expenseDateInString, string currentDateInString, bool expectedResult)
        {
            var expenseDate = DateTime.Parse(expenseDateInString, CultureInfo.InvariantCulture);
            var currentDate = DateTime.Parse(currentDateInString, CultureInfo.InvariantCulture);

            var request = new ExpenseValidationRequest(
                date: expenseDate,
                description: default!,
                expenseCurrency: default,
                isDuplicatedExpense: default,
                userCurrency: default);

            var result = request.IsDateInFuture(currentDate);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("2023-01-01T04:50:06", "2024-02-06T09:50:05", true)]
        [InlineData("2023-11-06T09:00:06", "2024-02-06T09:50:05", true)]
        [InlineData("2023-11-07T09:00:06", "2024-02-06T09:50:05", false)]
        [InlineData("2024-02-06T10:50:06", "2024-02-06T09:50:05", false)]
        [InlineData("2024-02-07T04:50:06", "2024-02-06T09:50:05", false)]
        public void IsDateOlderThanThreeMonths(string expenseDateInString, string currentDateInString, bool expectedResult)
        {
            var expenseDate = DateTime.Parse(expenseDateInString, CultureInfo.InvariantCulture);
            var currentDate = DateTime.Parse(currentDateInString, CultureInfo.InvariantCulture);

            var request = new ExpenseValidationRequest(
                date: expenseDate,
                description: default!,
                expenseCurrency: default,
                isDuplicatedExpense: default,
                userCurrency: default);

            var result = request.IsDateOlderThanThreeMonths(currentDate);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData(" ", true)]
        [InlineData(null!, true)]
        [InlineData("Commentaire", false)]
        public void IsDescriptionNullOrWhiteSpace(string? description, bool expectedResult)
        {
            var request = new ExpenseValidationRequest(
                date: default,
                description: description!,
                expenseCurrency: default,
                isDuplicatedExpense: default,
                userCurrency: default);

            var result = request.IsDescriptionNullOrWhiteSpace();

            result.Should().Be(expectedResult);
        }
    }
}
