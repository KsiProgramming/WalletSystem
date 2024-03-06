//-----------------------------------------------------------------------
// <copyright file="ExpenseRulesValidatorTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator.Tests
{
    using Moq;

    public class ExpenseRulesValidatorTest
    {
        [Fact]
        public void Validate()
        {
            var rule1 = new Mock<IExpenseRuleCheck<ExpenseValidationRequest>>();
            rule1.Setup(r => r.Check(It.IsAny<ExpenseValidationRequest>()));

            var rule2 = new Mock<IExpenseRuleCheck<ExpenseValidationRequest>>();
            rule2.Setup(r => r.Check(It.IsAny<ExpenseValidationRequest>()));

            var request = new ExpenseValidationRequest(
                date: default,
                description: default!,
                expenseCurrency: default,
                isDuplicatedExpense: default,
                userCurrency: default);

            var validator = new ExpenseRulesValidator(new IExpenseRuleCheck<ExpenseValidationRequest>[2]
            {
                rule1.Object,
                rule2.Object,
            });

            validator.Validate(request);

            rule1.Verify(x => x.Check(request), Times.Exactly(1));
            rule2.Verify(x => x.Check(request), Times.Exactly(1));
            rule1.VerifyAll();
            rule2.VerifyAll();
        }
    }
}
