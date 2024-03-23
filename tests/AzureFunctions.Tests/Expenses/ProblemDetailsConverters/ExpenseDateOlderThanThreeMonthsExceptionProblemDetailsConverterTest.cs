//---------------------------------------------------------------------------------
// <copyright file="ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverterTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions.Tests
{
    using WalletSystem.Expenses.RulesValidator;

    public class ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverterTest
    {
        [Fact]
        public void Instance()
        {
            ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter.Instance.Should().NotBeNull();
            ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter.Instance.Should().BeSameAs(ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter.Instance);
        }

        [Fact]
        public void Convert()
        {
            var exception = new ExpenseDateOlderThanThreeMonthsException("The message description");

            var problemDetailsConverter = ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter.Instance.Convert(exception);

            problemDetailsConverter.Detail.Should().Be("The message description");
            problemDetailsConverter.Extensions.Should().BeEmpty();
            problemDetailsConverter.Instance.Should().BeNull();
            problemDetailsConverter.Status.Should().Be(422);
            problemDetailsConverter.Title.Should().Be("ExpenseDateOlderThanThreeMonths");
            problemDetailsConverter.Type.Should().BeNull();
        }
    }
}
