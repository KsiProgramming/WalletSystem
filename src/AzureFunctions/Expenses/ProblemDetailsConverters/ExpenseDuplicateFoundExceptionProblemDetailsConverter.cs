//-----------------------------------------------------------------------
// <copyright file="ExpenseDuplicateFoundExceptionProblemDetailsConverter.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions
{
    using System.Net;
    using WalletSystem.AzureFunctions;
    using WalletSystem.Expenses.RulesValidator;

    public sealed class ExpenseDuplicateFoundExceptionProblemDetailsConverter : IExceptionProblemDetailsConverter<ExpenseDuplicateFoundException>
    {
        private ExpenseDuplicateFoundExceptionProblemDetailsConverter()
        {
        }

        public static ExpenseDuplicateFoundExceptionProblemDetailsConverter Instance { get; } = new ExpenseDuplicateFoundExceptionProblemDetailsConverter();

        public ProblemDetails Convert(ExpenseDuplicateFoundException exception)
        {
            return new ProblemDetails()
            {
                Status = (int)HttpStatusCode.UnprocessableContent,
                Title = "ExpenseDuplicateFound",
                Detail = exception.Message,
            };
        }
    }
}
