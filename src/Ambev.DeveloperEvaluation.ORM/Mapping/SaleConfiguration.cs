﻿using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.SaleNumber)
            .IsRequired();

        builder.Property(s => s.SaleDate)
            .HasColumnType("timestamp with time zone").HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(s => s.BranchId)
            .IsRequired();

        builder.Property(s => s.BranchName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.IsCancelled)
            .IsRequired();

        builder.Ignore(s => s.TotalAmount);

        builder.HasOne(s => s.Saller)
            .WithMany(i => i.SallerTransactions)
            .HasForeignKey(i => i.SallerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Customer)
            .WithMany(i => i.CustomerTransactions)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Items)
               .WithOne(i => i.Sale)
               .HasForeignKey(i => i.SaleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
