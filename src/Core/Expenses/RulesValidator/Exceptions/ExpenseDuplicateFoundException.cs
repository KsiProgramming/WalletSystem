//-----------------------------------------------------------------------
// <copyright file="ExpenseDuplicateFoundException.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public class ExpenseDuplicateFoundException : Exception
    {
        public ExpenseDuplicateFoundException()
        {
        }

        public ExpenseDuplicateFoundException(string message)
            : base(message)
        {
        }

        public ExpenseDuplicateFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
