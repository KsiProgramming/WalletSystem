//-----------------------------------------------------------------------
// <copyright file="WalletSystemDbContext.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Sql
{
    using Microsoft.EntityFrameworkCore;

    public class WalletSystemDbContext : DbContext
    {
        public WalletSystemDbContext(DbContextOptions<WalletSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserData> Users { get; set; }

        public DbSet<ExpenseData> Expenses { get; set; }

        public DbSet<CurrencyData> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyDataModulesConfiguration();
        }
    }
}
