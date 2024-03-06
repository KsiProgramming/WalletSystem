//-----------------------------------------------------------------------
// <copyright file="ExpenseDateOlderThanThreeMonthsRuleTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules.Tests
{
    using Moq;

    public class ExpenseDateOlderThanThreeMonthsRuleTest
    {
        [Fact]
        public void Check()
        {
            var request = new ExpenseValidationRequest(
                date: new DateTime(2024, 02, 07, 0, 0, 0, DateTimeKind.Utc),
                description: default!,
                expenseCurrency: default,
                isDuplicatedExpense: default,
                userCurrency: default);

            var systemClock = new Mock<ISystemClock>(MockBehavior.Strict);
            systemClock
                .Setup(x => x.UtcNow)
                .Returns(new DateTime(2024, 02, 07, 0, 0, 0, DateTimeKind.Utc));

            var rule = new DateOlderThanThreeMonthsRule(systemClock.Object);

            rule.Check(request);

            systemClock.VerifyAll();
        }

        [Fact]
        public void Check_WithExpenseDateOlderThanThreeMonths()
        {
            var request = new ExpenseValidationRequest(
                date: new DateTime(2023, 11, 01, 0, 0, 0, DateTimeKind.Utc),
                description: default!,
                expenseCurrency: default,
                isDuplicatedExpense: default,
                userCurrency: default);

            var systemClock = new Mock<ISystemClock>(MockBehavior.Strict);
            systemClock
                .Setup(x => x.UtcNow)
                .Returns(new DateTime(2024, 02, 01, 0, 0, 0, DateTimeKind.Utc));

            var rule = new DateOlderThanThreeMonthsRule(systemClock.Object);

            var act = () => { rule.Check(request); };

            act.Should()
                .ThrowExactly<ExpenseDateOlderThanThreeMonthsException>()
                .WithMessage($"Expense creation failed: The expense date is greater than 3 months old.");

            systemClock.VerifyAll();
        }
    }
}
