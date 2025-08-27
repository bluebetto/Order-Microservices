using Microsoft.EntityFrameworkCore;
using OrderMicroservices.Orders.Domain.Entities;
using OrderMicroservices.Orders.Infra.Data;

namespace OrderMicroservices.Orders.Infra.Repositories
{
    public class OrderRepository(OrderDbContext _context) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(
            Guid customerId,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Orders.Include(o => o.Items)
                .Where(o => o.CustomerId == customerId)
                .OrderBy(o => o.OrderDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Order> AddAsync(
            Order order,
            CancellationToken cancellationToken = default
        )
        {
            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
