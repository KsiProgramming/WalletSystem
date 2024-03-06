//-----------------------------------------------------------------------
// <copyright file="ExpenseMissingDescriptionExceptionTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Tests
{
    public class ExpenseMissingDescriptionExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            var exception = new ExpenseMissingDescriptionException();

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Exception of type 'WalletSystem.Expenses.RulesValidator.ExpenseMissingDescriptionException' was thrown.");
        }

        [Fact]
        public void Constructor_WithMessage()
        {
            var exception = new ExpenseMissingDescriptionException("Expense description is required.");

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Expense description is required.");
        }

        [Fact]
        public void Constructor_WithInnerException()
        {
            var innerException = new InvalidOperationException();
            var exception = new ExpenseMissingDescriptionException("Expense description is required.", innerException);

            exception.InnerException.Should().BeSameAs(innerException);
            exception.Message.Should().Be("Expense description is required.");
        }
    }
}
