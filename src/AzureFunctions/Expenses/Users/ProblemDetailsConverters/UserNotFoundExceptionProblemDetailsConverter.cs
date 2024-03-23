//-----------------------------------------------------------------------
// <copyright file="UserNotFoundExceptionProblemDetailsConverter.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.AzureFunctions
{
    using System.Net;
    using WalletSystem.AzureFunctions;

    public class UserNotFoundExceptionProblemDetailsConverter : IExceptionProblemDetailsConverter<UserNotFoundException>
    {
        private UserNotFoundExceptionProblemDetailsConverter()
        {
        }

        public static UserNotFoundExceptionProblemDetailsConverter Instance { get; } = new UserNotFoundExceptionProblemDetailsConverter();

        public ProblemDetails Convert(UserNotFoundException exception)
        {
            return new ProblemDetails()
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "UserNotFound",
                Detail = exception.Message,
            };
        }
    }
}
