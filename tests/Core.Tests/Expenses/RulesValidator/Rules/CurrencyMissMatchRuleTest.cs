//-----------------------------------------------------------------------
// <copyright file="CurrencyMissMatchRuleTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules.Tests
{
    public class CurrencyMissMatchRuleTest
    {
        [Fact]
        public void Check()
        {
            var request = new ExpenseValidationRequest(
                date: default,
                description: default!,
                expenseCurrency: Currency.USD,
                isDuplicatedExpense: default,
                userCurrency: Currency.USD);

            var rule = new CurrencyMissMatchRule();

            rule.Check(request);
        }

        [Fact]
        public void Check_WithExpenseCurrencyMissMatch()
        {
            var request = new ExpenseValidationRequest(
                date: default,
                description: default!,
                expenseCurrency: Currency.RUB,
                isDuplicatedExpense: default,
                userCurrency: Currency.USD);

            var rule = new CurrencyMissMatchRule();

            var act = () => { rule.Check(request); };

            act.Should()
                .ThrowExactly<ExpenseCurrencyMissMatchException>()
                .WithMessage("Expense creation failed: The user currency and the expense currency are different.");
        }
    }
}
