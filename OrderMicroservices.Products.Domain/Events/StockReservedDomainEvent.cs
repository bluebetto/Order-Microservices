using OrderMicroservices.Common;

namespace OrderMicroservices.Products.Domain.Events
{
    public record StockReservedDomainEvent(
    Guid ProductId,
    int ReservedQuantity
) : IDomainEvent;
}
