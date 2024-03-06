//-----------------------------------------------------------------------
// <copyright file="ExpenseDuplicateRuleTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules.Tests
{
    public class ExpenseDuplicateRuleTest
    {
        [Fact]
        public void Check()
        {
            var request = new ExpenseValidationRequest(
                date: default,
                description: default!,
                expenseCurrency: default,
                isDuplicatedExpense: false,
                userCurrency: default);

            var rule = new ExpenseDuplicateRule();

            rule.Check(request);
        }

        [Fact]
        public void Check_WithExpenseDuplicateFound()
        {
            var request = new ExpenseValidationRequest(
                date: default,
                description: default!,
                expenseCurrency: default,
                isDuplicatedExpense: true,
                userCurrency: default);

            var rule = new ExpenseDuplicateRule();

            var act = () => { rule.Check(request); };

            act.Should()
                .ThrowExactly<ExpenseDuplicateFoundException>()
                .WithMessage($"Expense creation failed: Duplicated expense was found.");
        }
    }
}
