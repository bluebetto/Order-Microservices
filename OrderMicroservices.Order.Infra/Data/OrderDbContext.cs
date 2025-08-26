using Microsoft.EntityFrameworkCore;
using OrderMicroservices.Orders.Domain.Entities;
using OrderMicroservices.Orders.Infra.Data.Configuration;

namespace OrderMicroservices.Orders.Infra.Data
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
