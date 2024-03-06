//-----------------------------------------------------------------------
// <copyright file="ExpenseManager.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses
{
    using WalletSystem.Expenses.RulesValidator;
    using WalletSystem.Expenses.Users;

    public class ExpenseManager : IExpenseManager
    {
        private readonly IExpenseRepository repository;
        private readonly IUserManager userManager;
        private readonly IExpenseRulesValidator validator;

        public ExpenseManager(IExpenseRepository repository, IUserManager userManager, IExpenseRulesValidator validator)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.validator = validator;
        }

        public async Task CreateAsync(ExpenseCreation expenseCreation)
        {
            var user = await this.userManager.FindByIdAsync(expenseCreation.UserId);

            var query = new ExpenseQuery()
            {
                Amount = expenseCreation.Amount,
                Date = expenseCreation.Date,
                UserId = expenseCreation.UserId,
            };

            var isDuplacated = (await this.repository.FindAsync(query)).Any();

            var validationRequest = new ExpenseValidationRequest(
                date: expenseCreation.Date,
                description: expenseCreation.Description,
                expenseCurrency: expenseCreation.Currency,
                isDuplicatedExpense: isDuplacated,
                userCurrency: user.Currency);

            this.validator.Validate(validationRequest);

            await this.repository.AddAsync(expenseCreation);
        }

        public async Task<IReadOnlyCollection<Expense>> FindAsync(ExpenseQuery query)
        {
            return await this.repository.FindAsync(query);
        }
    }
}
