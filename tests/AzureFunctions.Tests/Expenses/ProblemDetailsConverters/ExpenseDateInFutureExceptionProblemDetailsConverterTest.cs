//---------------------------------------------------------------------------------
// <copyright file="ExpenseDateInFutureExceptionProblemDetailsConverterTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions.Tests
{
    using WalletSystem.Expenses.RulesValidator;

    public class ExpenseDateInFutureExceptionProblemDetailsConverterTest
    {
        [Fact]
        public void Instance()
        {
            ExpenseDateInFutureExceptionProblemDetailsConverter.Instance.Should().NotBeNull();
            ExpenseDateInFutureExceptionProblemDetailsConverter.Instance.Should().BeSameAs(ExpenseDateInFutureExceptionProblemDetailsConverter.Instance);
        }

        [Fact]
        public void Convert()
        {
            var exception = new ExpenseDateInFutureException("The message description");

            var problemDetailsConverter = ExpenseDateInFutureExceptionProblemDetailsConverter.Instance.Convert(exception);

            problemDetailsConverter.Detail.Should().Be("The message description");
            problemDetailsConverter.Extensions.Should().BeEmpty();
            problemDetailsConverter.Instance.Should().BeNull();
            problemDetailsConverter.Status.Should().Be(422);
            problemDetailsConverter.Title.Should().Be("ExpenseDateInFuture");
            problemDetailsConverter.Type.Should().BeNull();
        }
    }
}
