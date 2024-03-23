//-----------------------------------------------------------------------
// <copyright file="IExceptionProblemDetailsConverter.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.AzureFunctions
{
    public interface IExceptionProblemDetailsConverter<in TException>
         where TException : Exception
    {
        ProblemDetails Convert(TException exception);
    }
}