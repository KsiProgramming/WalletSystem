//-----------------------------------------------------------------------
// <copyright file="ExpenseDuplicateFoundExceptionProblemDetailsConverterTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions.Tests
{
    using WalletSystem.Expenses.RulesValidator;

    public class ExpenseDuplicateFoundExceptionProblemDetailsConverterTest
    {
        [Fact]
        public void Instance()
        {
            ExpenseDuplicateFoundExceptionProblemDetailsConverter.Instance.Should().NotBeNull();
            ExpenseDuplicateFoundExceptionProblemDetailsConverter.Instance.Should().BeSameAs(ExpenseDuplicateFoundExceptionProblemDetailsConverter.Instance);
        }

        [Fact]
        public void Convert()
        {
            var exception = new ExpenseDuplicateFoundException("The message description");

            var problemDetailsConverter = ExpenseDuplicateFoundExceptionProblemDetailsConverter.Instance.Convert(exception);

            problemDetailsConverter.Detail.Should().Be("The message description");
            problemDetailsConverter.Extensions.Should().BeEmpty();
            problemDetailsConverter.Instance.Should().BeNull();
            problemDetailsConverter.Status.Should().Be(422);
            problemDetailsConverter.Title.Should().Be("ExpenseDuplicateFound");
            problemDetailsConverter.Type.Should().BeNull();
        }
    }
}
