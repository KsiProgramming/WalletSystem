//-----------------------------------------------------------------------
// <copyright file="UserManagerTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.Tests
{
    using Moq;

    public class UserManagerTest
    {
        [Fact]
        public async Task FindByIdAsync()
        {
            var users = new User[1] { new User("First 1", "Last 1", Currency.USD, 1) };

            var repository = new Mock<IUserRepository>(MockBehavior.Strict);
            repository
                .Setup(r => r.FindAsync(It.IsAny<UserQuery>()))
                .Callback((UserQuery query) =>
                {
                    query.Id.Should().Be(1);
                })
                .ReturnsAsync(users);

            var manager = new UserManager(repository.Object);

            var user = await manager.FindByIdAsync(1);
            user.Currency.Should().Be(Currency.USD);
            user.FirstName.Should().Be("First 1");
            user.Id.Should().Be(1);
            user.LastName.Should().Be("Last 1");

            repository.VerifyAll();
        }

        [Fact]
        public async Task FindByIdAsync_UserNotFound()
        {
            var users = Array.Empty<User>();

            var repository = new Mock<IUserRepository>(MockBehavior.Strict);
            repository
                .Setup(r => r.FindAsync(It.IsAny<UserQuery>()))
                .Callback((UserQuery query) =>
                {
                    query.Id.Should().Be(2);
                })
                .ReturnsAsync(users);

            var manager = new UserManager(repository.Object);

            var act = async () => { await manager.FindByIdAsync(id: 2); };
            await act.Should()
                .ThrowExactlyAsync<UserNotFoundException>()
                .WithMessage("No user has been found with the specified '2' identifier.");

            repository.VerifyAll();
        }

        [Fact]
        public async Task FindByNameAsync()
        {
            var users = new User[2]
            {
                new User("John", "Doe", Currency.USD, 1),
                new User("Mary", "Johnson", Currency.RUB, 2),
            };

            UserQuery? capturedQuery = null;
            var repository = new Mock<IUserRepository>(MockBehavior.Strict);
            repository
                .Setup(r => r.FindAsync(It.IsAny<UserQuery>()))
                .Callback((UserQuery query) =>
                {
                    capturedQuery = query;
                })
                .ReturnsAsync(users);

            var manager = new UserManager(repository.Object);

            var result = await manager.FindByNameAsync("john");

            capturedQuery.Should().NotBeNull();
            capturedQuery!.Name.Should().Be("john");
            capturedQuery.Id.Should().BeNull();
            result.Should().HaveCount(2);
            result.Select(u => u.Id).Should().Equal(1, 2);
            result.Select(u => u.FirstName).Should().Equal("John", "Mary");

            repository.VerifyAll();
        }

        [Fact]
        public async Task FindByNameAsync_NoMatch()
        {
            var users = Array.Empty<User>();

            UserQuery? capturedQuery = null;
            var repository = new Mock<IUserRepository>(MockBehavior.Strict);
            repository
                .Setup(r => r.FindAsync(It.IsAny<UserQuery>()))
                .Callback((UserQuery query) =>
                {
                    capturedQuery = query;
                })
                .ReturnsAsync(users);

            var manager = new UserManager(repository.Object);

            var result = await manager.FindByNameAsync("unknown");

            capturedQuery.Should().NotBeNull();
            capturedQuery!.Name.Should().Be("unknown");
            result.Should().BeEmpty();

            repository.VerifyAll();
        }
    }
}