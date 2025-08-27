using OrderMicroservices.Common;

namespace OrderMicroservices.Orders.Domain.Events
{
    public record OrderCancelledDomainEvent(Guid OrderId) : IDomainEvent;
}
