using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderMicroservices.Orders.Domain.Entities;

namespace OrderMicroservices.Orders.Infra.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.OrderDate)
                .IsRequired();

            builder.Property(o => o.Status)
                .HasConversion<int>()
                .IsRequired();

            // Value Object: Money
            builder.OwnsOne(o => o.TotalAmount, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("TotalAmount")
                    .HasColumnType("decimal(18,2)");
                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3);
            });

            // Value Object: Address
            builder.OwnsOne(o => o.DeliveryAddress, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
                address.Property(a => a.Number).HasColumnName("Number").HasMaxLength(10);
                address.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
                address.Property(a => a.State).HasColumnName("State").HasMaxLength(50);
                address.Property(a => a.ZipCode).HasColumnName("ZipCode").HasMaxLength(10);
                address.Property(a => a.Country).HasColumnName("Country").HasMaxLength(50);
            });

            // Relacionamento com OrderItems
            builder.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
