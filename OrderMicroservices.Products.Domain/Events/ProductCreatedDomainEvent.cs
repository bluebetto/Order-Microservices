using OrderMicroservices.Common;

namespace OrderMicroservices.Products.Domain.Events
{
    public record ProductCreatedDomainEvent(
        Guid ProductId,
        string ProductName,
        decimal Price
    ) : IDomainEvent;
}
