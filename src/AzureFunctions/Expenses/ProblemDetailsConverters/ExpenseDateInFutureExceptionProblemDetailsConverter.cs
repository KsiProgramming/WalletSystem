//---------------------------------------------------------------------------------
// <copyright file="ExpenseDateInFutureExceptionProblemDetailsConverter.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions
{
    using System.Net;
    using WalletSystem.AzureFunctions;
    using WalletSystem.Expenses.RulesValidator;

    public sealed class ExpenseDateInFutureExceptionProblemDetailsConverter : IExceptionProblemDetailsConverter<ExpenseDateInFutureException>
    {
        private ExpenseDateInFutureExceptionProblemDetailsConverter()
        {
        }

        public static ExpenseDateInFutureExceptionProblemDetailsConverter Instance { get; } = new ExpenseDateInFutureExceptionProblemDetailsConverter();

        public ProblemDetails Convert(ExpenseDateInFutureException exception)
        {
            return new ProblemDetails()
            {
                Status = (int)HttpStatusCode.UnprocessableContent,
                Title = "ExpenseDateInFuture",
                Detail = exception.Message,
            };
        }
    }
}
