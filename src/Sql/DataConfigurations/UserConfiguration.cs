//-----------------------------------------------------------------------
// <copyright file="UserConfiguration.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Sql
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<UserData>
    {
        public void Configure(EntityTypeBuilder<UserData> builder)
        {
            builder.ToTable("User");

            builder.HasKey(e => e.Id)
                .IsClustered()
                .HasName("PK_User");

            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .UseIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.FirstName)
                .HasColumnName("FirstName")
                .HasColumnType("NVARCHAR(100)")
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasColumnName("LastName")
                .HasColumnType("NVARCHAR(100)")
                .IsRequired();

            builder.Property(e => e.CurrencyId)
                .HasColumnName("CurrencyId")
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .HasConstraintName("FK_UserCurrency")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
                new UserData(currencyId: 1, firstName: "Anthony", lastName: "Stark") { Id = 1 },
                new UserData(currencyId: 2, firstName: "Natasha", lastName: "Romanova") { Id = 2 });
        }
    }
}
