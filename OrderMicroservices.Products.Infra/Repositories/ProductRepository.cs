using Microsoft.EntityFrameworkCore;
using OrderMicroservices.Products.Domain.Entities;
using OrderMicroservices.Products.Infra.Data;

namespace OrderMicroservices.Products.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(
            string category,
            CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(p => p.Category == category && p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            var result = await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
