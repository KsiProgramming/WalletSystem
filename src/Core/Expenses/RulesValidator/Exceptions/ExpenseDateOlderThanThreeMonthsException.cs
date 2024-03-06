//-----------------------------------------------------------------------
// <copyright file="ExpenseDateOlderThanThreeMonthsException.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public class ExpenseDateOlderThanThreeMonthsException : Exception
    {
        public ExpenseDateOlderThanThreeMonthsException()
        {
        }

        public ExpenseDateOlderThanThreeMonthsException(string message)
            : base(message)
        {
        }

        public ExpenseDateOlderThanThreeMonthsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
