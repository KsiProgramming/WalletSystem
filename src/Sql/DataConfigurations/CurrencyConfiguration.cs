//-----------------------------------------------------------------------
// <copyright file="CurrencyConfiguration.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Sql
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CurrencyConfiguration : IEntityTypeConfiguration<CurrencyData>
    {
        public void Configure(EntityTypeBuilder<CurrencyData> builder)
        {
            builder.ToTable("Currency");

            builder.HasKey(e => e.Id)
                .IsClustered()
                .HasName("PK_Currency");

            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(e => e.Code)
                .HasColumnName("Code")
                .HasColumnType("VARCHAR(3)")
                .IsRequired();

            builder.Property(e => e.Name)
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired();

            builder.Property(e => e.Symbol)
                .HasColumnName("Symbol")
                .HasColumnType("NVARCHAR(10)")
                .IsRequired();

            builder.HasData(
                new CurrencyData(code: "USD", name: "American Dollar", symbol: "$") { Id = 1 },
                new CurrencyData(code: "RUB", name: "Russian Ruble", symbol: "₽") { Id = 2 });
        }
    }
}
