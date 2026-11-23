//-----------------------------------------------------------------------
// <copyright file="SearchUserResponseJsonTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.AzureFunctions.Tests
{
    using FluentAssertions;

    public class SearchUserResponseJsonTest
    {
        [Fact]
        public void Constructor()
        {
            var response = new SearchUserResponseJson(
                id: 3,
                firstName: "John",
                lastName: "Doe",
                currency: "USD");

            response.Id.Should().Be(3);
            response.FirstName.Should().Be("John");
            response.LastName.Should().Be("Doe");
            response.Currency.Should().Be("USD");
        }
    }
}
