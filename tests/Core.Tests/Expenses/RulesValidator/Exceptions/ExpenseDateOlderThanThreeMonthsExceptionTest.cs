//-----------------------------------------------------------------------
// <copyright file="ExpenseDateOlderThanThreeMonthsExceptionTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Tests
{
    public class ExpenseDateOlderThanThreeMonthsExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            var exception = new ExpenseDateOlderThanThreeMonthsException();

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Exception of type 'WalletSystem.Expenses.RulesValidator.ExpenseDateOlderThanThreeMonthsException' was thrown.");
        }

        [Fact]
        public void Constructor_WithMessage()
        {
            var exception = new ExpenseDateOlderThanThreeMonthsException("Expense date in future.");

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Expense date in future.");
        }

        [Fact]
        public void Constructor_WithInnerException()
        {
            var innerException = new InvalidOperationException();
            var exception = new ExpenseDateOlderThanThreeMonthsException("Expense date is older than three months.", innerException);

            exception.InnerException.Should().BeSameAs(innerException);
            exception.Message.Should().Be("Expense date is older than three months.");
        }
    }
}
