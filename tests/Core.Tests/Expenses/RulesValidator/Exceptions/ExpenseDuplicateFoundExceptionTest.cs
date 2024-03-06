//-----------------------------------------------------------------------
// <copyright file="ExpenseDuplicateFoundExceptionTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Tests
{
    public class ExpenseDuplicateFoundExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            var exception = new ExpenseDuplicateFoundException();

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Exception of type 'WalletSystem.Expenses.RulesValidator.ExpenseDuplicateFoundException' was thrown.");
        }

        [Fact]
        public void Constructor_WithMessage()
        {
            var exception = new ExpenseMissingDescriptionException("Duplicate expense was found.");

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Duplicate expense was found.");
        }

        [Fact]
        public void Constructor_WithInnerException()
        {
            var innerException = new InvalidOperationException();
            var exception = new ExpenseDuplicateFoundException("Duplicate expense was found.", innerException);

            exception.InnerException.Should().BeSameAs(innerException);
            exception.Message.Should().Be("Duplicate expense was found.");
        }
    }
}
