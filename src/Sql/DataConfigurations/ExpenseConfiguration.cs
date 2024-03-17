//-----------------------------------------------------------------------
// <copyright file="ExpenseConfiguration.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Sql
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExpenseConfiguration : IEntityTypeConfiguration<ExpenseData>
    {
        public void Configure(EntityTypeBuilder<ExpenseData> builder)
        {
            builder.ToTable("Expense");

            builder.HasKey(e => e.Id)
                .IsClustered()
                .HasName("PK_Expense");

            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .UseIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.Date)
                .HasColumnName("DateTime")
                .HasColumnType("DateTime2(7)")
                .IsRequired();

            builder.Property(e => e.Amount)
                .HasColumnName("Amount")
                .HasColumnType("DECIMAL(18,4)")
                .IsRequired();

            builder.Property(e => e.Description)
                .HasColumnName("Description")
                .HasColumnType("NVARCHAR(MAX)")
                .IsRequired();

            builder.Property(e => e.UserId)
                .HasColumnName("UserId")
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(e => e.Type)
                .HasColumnName("TypeId")
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_ExpenseUser")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.Amount)
                .HasDatabaseName("IX_Expense_Amount")
                .IsClustered(false);

            builder.HasIndex(e => e.Date)
                .HasDatabaseName("IX_Expense_DateTime")
                .IsClustered(false);

            builder.HasIndex(e => e.UserId)
                .HasDatabaseName("IX_Expense_UserId")
                .IsClustered(false);
        }
    }
}
