//---------------------------------------------------------------------------------
// <copyright file="ExpenseMissingDescriptionExceptionProblemDetailsConverter.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions
{
    using System.Net;
    using WalletSystem.AzureFunctions;
    using WalletSystem.Expenses.RulesValidator;

    public class ExpenseMissingDescriptionExceptionProblemDetailsConverter : IExceptionProblemDetailsConverter<ExpenseMissingDescriptionException>
    {
        private ExpenseMissingDescriptionExceptionProblemDetailsConverter()
        {
        }

        public static ExpenseMissingDescriptionExceptionProblemDetailsConverter Instance { get; } = new ExpenseMissingDescriptionExceptionProblemDetailsConverter();

        public ProblemDetails Convert(ExpenseMissingDescriptionException exception)
        {
            return new ProblemDetails()
            {
                Status = (int)HttpStatusCode.UnprocessableContent,
                Title = "ExpenseMissingDescription",
                Detail = exception.Message,
            };
        }
    }
}
