//-----------------------------------------------------------------------
// <copyright file="ExpenseMissingDescriptionException.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public class ExpenseMissingDescriptionException : Exception
    {
        public ExpenseMissingDescriptionException()
        {
        }

        public ExpenseMissingDescriptionException(string message)
            : base(message)
        {
        }

        public ExpenseMissingDescriptionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
