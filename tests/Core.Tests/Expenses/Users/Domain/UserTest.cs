//-----------------------------------------------------------------------
// <copyright file="UserTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.Tests
{
    public class UserTest
    {
        [Fact]
        public void Constructor()
        {
            var user = new User(
                firstName: "First Name 1",
                lastName: "Last Name 1",
                currency: Currency.USD,
                id: 5);

            user.Currency.Should().Be(Currency.USD);
            user.FirstName.Should().Be("First Name 1");
            user.Id.Should().Be(5);
            user.LastName.Should().Be("Last Name 1");
        }
    }
}
