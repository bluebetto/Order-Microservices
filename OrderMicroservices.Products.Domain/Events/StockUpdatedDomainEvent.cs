using OrderMicroservices.Common;
using OrderMicroservices.Products.Domain.Enums;

namespace OrderMicroservices.Products.Domain.Events
{
    public record StockUpdatedDomainEvent(
    Guid ProductId,
    int PreviousQuantity,
    int NewQuantity,
    StockOperation Operation
) : IDomainEvent;
}
