//---------------------------------------------------------------------------------
// <copyright file="ExpenseCurrencyMissMatchException.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public class ExpenseCurrencyMissMatchException : Exception
    {
        public ExpenseCurrencyMissMatchException()
        {
        }

        public ExpenseCurrencyMissMatchException(string message)
            : base(message)
        {
        }

        public ExpenseCurrencyMissMatchException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
