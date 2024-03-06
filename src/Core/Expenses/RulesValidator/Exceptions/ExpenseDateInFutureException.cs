//---------------------------------------------------------------------------------
// <copyright file="ExpenseDateInFutureException.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public class ExpenseDateInFutureException : Exception
    {
        public ExpenseDateInFutureException()
        {
        }

        public ExpenseDateInFutureException(string message)
            : base(message)
        {
        }

        public ExpenseDateInFutureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
