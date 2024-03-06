//-----------------------------------------------------------------------
// <copyright file="UserNotFoundExceptionTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.Tests
{
    public class UserNotFoundExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            var exception = new UserNotFoundException();

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("Exception of type 'WalletSystem.Expenses.Users.UserNotFoundException' was thrown.");
        }

        [Fact]
        public void Constructor_WithMessage()
        {
            var exception = new UserNotFoundException("User not found for the specified Id.");

            exception.InnerException.Should().BeNull();
            exception.Message.Should().Be("User not found for the specified Id.");
        }

        [Fact]
        public void Constructor_WithInnerException()
        {
            var innerException = new InvalidOperationException();
            var exception = new UserNotFoundException("User not found for the specified Id.", innerException);

            exception.InnerException.Should().BeSameAs(innerException);
            exception.Message.Should().Be("User not found for the specified Id.");
        }
    }
}
