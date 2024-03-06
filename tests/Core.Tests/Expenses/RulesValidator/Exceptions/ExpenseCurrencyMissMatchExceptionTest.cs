//-----------------------------------------------------------------------
// <copyright file="ExpenseCurrencyMissMatchExceptionTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Tests
{
    public class ExpenseCurrencyMissMatchExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            var exception = new ExpenseCurrencyMissMatchException();

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Exception of type 'WalletSystem.Expenses.RulesValidator.ExpenseCurrencyMissMatchException' was thrown.");
        }

        [Fact]
        public void Constructor_WithMessage()
        {
            var exception = new ExpenseCurrencyMissMatchException("Currency Matching Problem.");

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Currency Matching Problem.");
        }

        [Fact]
        public void Constructor_WithInnerException()
        {
            var innerException = new InvalidOperationException();
            var exception = new ExpenseCurrencyMissMatchException("Currency Matching Problem.", innerException);

            exception.InnerException.Should().BeSameAs(innerException);
            exception.Message.Should().Be("Currency Matching Problem.");
        }
    }
}
