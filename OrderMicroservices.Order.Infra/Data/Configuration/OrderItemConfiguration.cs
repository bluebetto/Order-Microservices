using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderMicroservices.Orders.Domain.Entities;

namespace OrderMicroservices.Orders.Infra.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.ProductId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            // Value Object: Price
            builder.OwnsOne(oi => oi.UnitPrice, price =>
            {
                price.Property(p => p.Amount)
                    .HasColumnName("Price")
                    .HasColumnType("decimal(18,2)");
                price.Property(p => p.Currency)
                    .HasColumnName("PriceCurrency")
                    .HasMaxLength(3);
            });
        }
    }
}
