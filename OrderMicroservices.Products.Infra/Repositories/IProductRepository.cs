using OrderMicroservices.Products.Domain.Entities;

namespace OrderMicroservices.Products.Infra.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetAllAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
        Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
        void Update(Product product);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
