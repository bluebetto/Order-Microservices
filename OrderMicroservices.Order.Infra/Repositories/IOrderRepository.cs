using OrderMicroservices.Orders.Domain.Entities;

namespace OrderMicroservices.Orders.Infra.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default
    );
    Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
    void Update(Order order);
}
