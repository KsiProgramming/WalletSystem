//---------------------------------------------------------------------------------
// <copyright file="ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions
{
    using System.Net;
    using WalletSystem.AzureFunctions;
    using WalletSystem.Expenses.RulesValidator;

    public sealed class ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter : IExceptionProblemDetailsConverter<ExpenseDateOlderThanThreeMonthsException>
    {
        private ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter()
        {
        }

        public static ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter Instance { get; } = new ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter();

        public ProblemDetails Convert(ExpenseDateOlderThanThreeMonthsException exception)
        {
            return new ProblemDetails()
            {
                Status = (int)HttpStatusCode.UnprocessableContent,
                Title = "ExpenseDateOlderThanThreeMonths",
                Detail = exception.Message,
            };
        }
    }
}