//-----------------------------------------------------------------------
// <copyright file="ExpenseQueryTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Tests
{
    public class ExpenseQueryTest
    {
        [Fact]
        public void Constructor()
        {
            var query = new ExpenseQuery();

            query.Amount.Should().BeNull();
            query.Date.Should().BeNull();
            query.UserId.Should().BeNull();
            query.SortBy.Should().BeNull();
            query.SortOption.Should().BeNull();
        }

        [Fact]
        public void Amount_ValueChange()
        {
            var query = new ExpenseQuery()
            {
                Amount = 12_34m,
            };

            query.Amount.Should().Be(12_34m);
        }

        [Fact]
        public void Date_ValueChange()
        {
            var query = new ExpenseQuery() { Date = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc) };

            query.Date.Should().Be(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc));
        }

        [Fact]
        public void UserId_ValueChange()
        {
            var query = new ExpenseQuery() { UserId = 1 };

            query.UserId.Should().Be(1);
        }

        [Fact]
        public void SortBy_ValueChange()
        {
            var query = new ExpenseQuery() { SortBy = SortBy.Amount };

            query.SortBy.Should().Be(SortBy.Amount);
        }

        [Fact]
        public void SortOption_ValueChange()
        {
            var query = new ExpenseQuery() { SortOption = "ASC" };

            query.SortOption.Should().Be("ASC");
        }
    }
}
