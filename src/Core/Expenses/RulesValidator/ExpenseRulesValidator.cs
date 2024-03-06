//-----------------------------------------------------------------------
// <copyright file="ExpenseRulesValidator.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.RulesValidator
{
    public class ExpenseRulesValidator : IExpenseRulesValidator
    {
        private readonly IEnumerable<IExpenseRuleCheck<ExpenseValidationRequest>> rules;

        public ExpenseRulesValidator(IEnumerable<IExpenseRuleCheck<ExpenseValidationRequest>> rules)
        {
            this.rules = rules;
        }

        public void Validate(ExpenseValidationRequest request)
        {
            foreach (var validator in this.rules)
            {
                validator.Check(request);
            }
        }
    }
}
