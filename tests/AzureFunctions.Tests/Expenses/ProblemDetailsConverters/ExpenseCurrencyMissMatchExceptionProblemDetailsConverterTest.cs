//---------------------------------------------------------------------------------
// <copyright file="ExpenseCurrencyMissMatchExceptionProblemDetailsConverterTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions.Tests
{
    using WalletSystem.Expenses.RulesValidator;

    public class ExpenseCurrencyMissMatchExceptionProblemDetailsConverterTest
    {
        [Fact]
        public void Instance()
        {
            ExpenseCurrencyMissMatchExceptionProblemDetailsConverter.Instance.Should().NotBeNull();
            ExpenseCurrencyMissMatchExceptionProblemDetailsConverter.Instance.Should().BeSameAs(ExpenseCurrencyMissMatchExceptionProblemDetailsConverter.Instance);
        }

        [Fact]
        public void Convert()
        {
            var exception = new ExpenseCurrencyMissMatchException("The message description");

            var problemDetailsConverter = ExpenseCurrencyMissMatchExceptionProblemDetailsConverter.Instance.Convert(exception);

            problemDetailsConverter.Detail.Should().Be("The message description");
            problemDetailsConverter.Extensions.Should().BeEmpty();
            problemDetailsConverter.Instance.Should().BeNull();
            problemDetailsConverter.Status.Should().Be(422);
            problemDetailsConverter.Title.Should().Be("CurrencyMissMatch");
            problemDetailsConverter.Type.Should().BeNull();
        }
    }
}
