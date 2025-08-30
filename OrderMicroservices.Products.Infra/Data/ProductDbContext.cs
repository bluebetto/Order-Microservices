using Microsoft.EntityFrameworkCore;
using OrderMicroservices.Products.Domain.Entities;
using OrderMicroservices.Products.Infra.Data.Configuration;

namespace OrderMicroservices.Products.Infra.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
