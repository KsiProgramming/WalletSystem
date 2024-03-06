//-----------------------------------------------------------------------
// <copyright file="ExpenseValidationRequestExtensions.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public static class ExpenseValidationRequestExtensions
    {
        public static bool IsCurrencyMismatch(this ExpenseValidationRequest request)
        {
            return request.UserCurrency != request.ExpenseCurrency;
        }

        public static bool IsDateInFuture(this ExpenseValidationRequest request, DateTime currentDate)
        {
            return request.Date.Date > currentDate.Date;
        }

        public static bool IsDateOlderThanThreeMonths(this ExpenseValidationRequest request, DateTime currentDate)
        {
            var monthsCount = 12 * currentDate.Subtract(request.Date).Days / 365.25;

            return monthsCount > 3;
        }

        public static bool IsDescriptionNullOrWhiteSpace(this ExpenseValidationRequest request)
        {
            return string.IsNullOrWhiteSpace(request.Description);
        }
    }
}
