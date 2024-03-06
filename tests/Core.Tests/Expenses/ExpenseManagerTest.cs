//-----------------------------------------------------------------------
// <copyright file="ExpenseManagerTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Tests
{
    using WalletSystem.Expenses.RulesValidator;
    using WalletSystem.Expenses.Users;
    using Moq;

    public class ExpenseManagerTest
    {
        [Fact]
        public async Task CreateAsync()
        {
            var expenseCreation = new ExpenseCreation(
                amount: 11_22m,
                currency: Currency.USD,
                date: new DateTime(2024, 02, 08, 0, 0, 0, DateTimeKind.Utc),
                description: "Commentaire",
                userId: 5,
                type: ExpenseType.Hotel);

            var user = new User(
                firstName: "First Name 1",
                lastName: "Last Name 1",
                currency: Currency.USD,
                id: 5);

            var expenses = Array.Empty<Expense>();

            var userManager = new Mock<IUserManager>();
            userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<int>()))
                .Callback((int userId) =>
                {
                    userId.Should().Be(5);
                })
                .ReturnsAsync(user);

            var expenseRepository = new Mock<IExpenseRepository>(MockBehavior.Strict);
            expenseRepository
                .Setup(e => e.FindAsync(It.IsAny<ExpenseQuery>()))
                .Callback((ExpenseQuery query) =>
                {
                    query.Amount.Should().Be(11_22m);
                    query.Date.Should().Be(new DateTime(2024, 02, 08, 0, 0, 0, DateTimeKind.Utc));
                    query.UserId.Should().Be(5);
                })
                .ReturnsAsync(expenses);
            expenseRepository
                .Setup(e => e.AddAsync(expenseCreation))
                .Returns(Task.CompletedTask);

            var rulesValidator = new Mock<IExpenseRulesValidator>();
            rulesValidator
                .Setup(r => r.Validate(It.IsAny<ExpenseValidationRequest>()))
                .Callback((ExpenseValidationRequest request) =>
                {
                    request.Date.Should().Be(new DateTime(2024, 02, 08, 0, 0, 0, DateTimeKind.Utc));
                    request.Description.Should().Be("Commentaire");
                    request.ExpenseCurrency.Should().Be(Currency.USD);
                    request.IsDuplicatedExpense.Should().Be(false);
                    request.UserCurrency.Should().Be(Currency.USD);
                })
                .Verifiable();

            var expenseManger = new ExpenseManager(
                repository: expenseRepository.Object,
                userManager: userManager.Object,
                validator: rulesValidator.Object);

            await expenseManger.CreateAsync(expenseCreation);

            expenseRepository.VerifyAll();
            userManager.VerifyAll();
            rulesValidator.VerifyAll();
        }

        [Fact]
        public async Task FindAsync()
        {
            var query = new ExpenseQuery();

            var expenses = Array.Empty<Expense>();

            var repository = new Mock<IExpenseRepository>();
            repository
                .Setup(r => r.FindAsync(query))
                .ReturnsAsync(expenses);

            var expenseManger = new ExpenseManager(repository: repository.Object, default!, default!);

            var result = await expenseManger.FindAsync(query);

            result.Should().BeSameAs(expenses);

            repository.VerifyAll();
        }
    }
}
