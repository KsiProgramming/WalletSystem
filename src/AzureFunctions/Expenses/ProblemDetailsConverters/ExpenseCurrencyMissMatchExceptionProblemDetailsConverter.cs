//---------------------------------------------------------------------------------
// <copyright file="ExpenseCurrencyMissMatchExceptionProblemDetailsConverter.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions
{
    using System.Net;
    using WalletSystem.AzureFunctions;
    using WalletSystem.Expenses.RulesValidator;

    public class ExpenseCurrencyMissMatchExceptionProblemDetailsConverter : IExceptionProblemDetailsConverter<ExpenseCurrencyMissMatchException>
    {
        private ExpenseCurrencyMissMatchExceptionProblemDetailsConverter()
        {
        }

        public static ExpenseCurrencyMissMatchExceptionProblemDetailsConverter Instance { get; } = new ExpenseCurrencyMissMatchExceptionProblemDetailsConverter();

        public ProblemDetails Convert(ExpenseCurrencyMissMatchException exception)
        {
            return new ProblemDetails()
            {
                Status = (int)HttpStatusCode.UnprocessableContent,
                Title = "CurrencyMissMatch",
                Detail = exception.Message,
            };
        }
    }
}
