//-----------------------------------------------------------------------
// <copyright file="DateInFutureRuleTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Rules.Tests
{
    using Moq;

    public class DateInFutureRuleTest
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

            var rule = new DateInFutureRule(systemClock.Object);

            rule.Check(request);

            systemClock.VerifyAll();
        }

        [Fact]
        public void Check_WithDateInFuture()
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
                .Returns(new DateTime(2024, 02, 01, 0, 0, 0, DateTimeKind.Utc));

            var rule = new DateInFutureRule(systemClock.Object);

            var act = () => { rule.Check(request); };

            act.Should()
                .ThrowExactly<ExpenseDateInFutureException>()
                .WithMessage($"Expense creation failed: The expense date cannot be in the future. Please provide a valid date. '07/02/2024 00:00:00'");

            systemClock.VerifyAll();
        }
    }
}
