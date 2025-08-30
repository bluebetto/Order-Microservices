﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderMicroservices.Products.Domain.Entities;

namespace OrderMicroservices.Products.Infra.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Property(p => p.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.IsActive)
                .IsRequired();

            // Value Object: Price (Money)
            builder.OwnsOne(p => p.Price, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("Price")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                price.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            // Value Object: Stock
            builder.OwnsOne(p => p.Stock, stock =>
            {
                stock.Property(s => s.Quantity)
                    .HasColumnName("StockQuantity")
                    .IsRequired();
            });

            // Ignore domain events
            builder.Ignore(p => p.DomainEvents);

            // Indexes
            builder.HasIndex(p => p.Category);
            builder.HasIndex(p => p.IsActive);
        }
    }
}
