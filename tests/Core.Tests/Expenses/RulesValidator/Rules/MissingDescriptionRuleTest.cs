//-----------------------------------------------------------------------
// <copyright file="MissingDescriptionRuleTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules.Tests
{
    public class MissingDescriptionRuleTest
    {
        [Fact]
        public void Check()
        {
            var request = new ExpenseValidationRequest(
                date: default,
                description: "Bus ticket",
                expenseCurrency: default,
                isDuplicatedExpense: default,
                userCurrency: default);

            var rule = new MissingDescriptionRule();

            rule.Check(request);
        }

        [Fact]
        public void Check_WithExpenseMissingDescription()
        {
            var request = new ExpenseValidationRequest(
                date: default,
                description: default!,
                expenseCurrency: default,
                isDuplicatedExpense: default,
                userCurrency: default);

            var rule = new MissingDescriptionRule();

            var act = () => { rule.Check(request); };

            act.Should()
                .ThrowExactly<ExpenseMissingDescriptionException>()
                .WithMessage($"Expense creation failed: Description is a required field.");
        }
    }
}
