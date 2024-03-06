//-----------------------------------------------------------------------
// <copyright file="ExpenseDateInFutureExceptionTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Tests
{
    public class ExpenseDateInFutureExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            var exception = new ExpenseDateInFutureException();

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Exception of type 'WalletSystem.Expenses.RulesValidator.ExpenseDateInFutureException' was thrown.");
        }

        [Fact]
        public void Constructor_WithMessage()
        {
            var exception = new ExpenseDateInFutureException("Expense date in future.");

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Expense date in future.");
        }

        [Fact]
        public void Constructor_WithInnerException()
        {
            var innerException = new InvalidOperationException();
            var exception = new ExpenseDateInFutureException("Expense date in future.", innerException);

            exception.InnerException.Should().BeSameAs(innerException);
            exception.Message.Should().Be("Expense date in future.");
        }
    }
}
