//-----------------------------------------------------------------------
// <copyright file="UserNotFoundExceptionProblemDetailsConverterTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.AzureFunctions.Tests
{
    public class UserNotFoundExceptionProblemDetailsConverterTest
    {
        [Fact]
        public void Instance()
        {
            UserNotFoundExceptionProblemDetailsConverter.Instance.Should().NotBeNull();
            UserNotFoundExceptionProblemDetailsConverter.Instance.Should().BeSameAs(UserNotFoundExceptionProblemDetailsConverter.Instance);
        }

        [Fact]
        public void Convert()
        {
            var exception = new UserNotFoundException("The message description");

            var problemDetailsConverter = UserNotFoundExceptionProblemDetailsConverter.Instance.Convert(exception);

            problemDetailsConverter.Detail.Should().Be("The message description");
            problemDetailsConverter.Extensions.Should().BeEmpty();
            problemDetailsConverter.Instance.Should().BeNull();
            problemDetailsConverter.Status.Should().Be(404);
            problemDetailsConverter.Title.Should().Be("UserNotFound");
            problemDetailsConverter.Type.Should().BeNull();
        }
    }
}
