//-----------------------------------------------------------------------
// <copyright file="UserQueryTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.Tests
{
    public class UserQueryTest
    {
        [Fact]
        public void Constructor()
        {
            var query = new UserQuery();

            query.Id.Should().BeNull();
        }

        [Fact]
        public void Id_ValueChanged()
        {
            var query = new UserQuery();
            query.Id = 1;

            query.Id.Should().Be(1);
        }
    }
}
