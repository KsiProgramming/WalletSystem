//---------------------------------------------------------------------------------
// <copyright file="ExpenseMissingDescriptionExceptionProblemDetailsConverterTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions.Tests
{
    using WalletSystem.Expenses.RulesValidator;

    public class ExpenseMissingDescriptionExceptionProblemDetailsConverterTest
    {
        [Fact]
        public void Instance()
        {
            ExpenseMissingDescriptionExceptionProblemDetailsConverter.Instance.Should().NotBeNull();
            ExpenseMissingDescriptionExceptionProblemDetailsConverter.Instance.Should().BeSameAs(ExpenseMissingDescriptionExceptionProblemDetailsConverter.Instance);
        }

        [Fact]
        public void Convert()
        {
            var exception = new ExpenseMissingDescriptionException("The message description");

            var problemDetailsConverter = ExpenseMissingDescriptionExceptionProblemDetailsConverter.Instance.Convert(exception);

            problemDetailsConverter.Detail.Should().Be("The message description");
            problemDetailsConverter.Extensions.Should().BeEmpty();
            problemDetailsConverter.Instance.Should().BeNull();
            problemDetailsConverter.Status.Should().Be(422);
            problemDetailsConverter.Title.Should().Be("ExpenseMissingDescription");
            problemDetailsConverter.Type.Should().BeNull();
        }
    }
}
