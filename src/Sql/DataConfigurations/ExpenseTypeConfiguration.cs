//-----------------------------------------------------------------------
// <copyright file="ExpenseTypeConfiguration.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Sql
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExpenseTypeConfiguration : IEntityTypeConfiguration<ExpenseTypeData>
    {
        public void Configure(EntityTypeBuilder<ExpenseTypeData> builder)
        {
            builder.ToTable("ExpenseType");

            builder.HasKey(e => e.Id)
                .IsClustered()
                .HasName("PK_ExpenseType");

            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(e => e.Label)
                .HasColumnName("Label")
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();

            builder.HasData(
                new ExpenseTypeData(label: "Restaurant") { Id = 1 },
                new ExpenseTypeData(label: "Hotel") { Id = 2 },
                new ExpenseTypeData(label: "Misc") { Id = 3 });
        }
    }
}
