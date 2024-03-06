//-----------------------------------------------------------------------
// <copyright file="CompositionRoot.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem
{
    using WalletSystem.Expenses;
    using WalletSystem.Expenses.RulesValidator;
    using WalletSystem.Expenses.RulesValidator.Rules;
    using WalletSystem.Expenses.Users;
    using Microsoft.Extensions.DependencyInjection;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class CompositionRoot
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            // Expense
            services.AddScoped<IExpenseManager, ExpenseManager>();

            // Expense Business Rules
            services.AddSingleton<IExpenseRuleCheck<ExpenseValidationRequest>, CurrencyMissMatchRule>();
            services.AddSingleton<IExpenseRuleCheck<ExpenseValidationRequest>, DateInFutureRule>();
            services.AddSingleton<IExpenseRuleCheck<ExpenseValidationRequest>, DateOlderThanThreeMonthsRule>();
            services.AddSingleton<IExpenseRuleCheck<ExpenseValidationRequest>, ExpenseDuplicateRule>();
            services.AddSingleton<IExpenseRuleCheck<ExpenseValidationRequest>, MissingDescriptionRule>();

            // Expense Business Validator
            services.AddSingleton<IExpenseRulesValidator, ExpenseRulesValidator>();

            // User
            services.AddScoped<IUserManager, UserManager>();

            // SystemClock
            services.AddSingleton<ISystemClock, SystemClock>();

            return services;
        }
    }
}
