using OrderMicroservices.Common;
using OrderMicroservices.Orders.Domain.ValueObjects;

namespace OrderMicroservices.Orders.Domain.Events
{
    public record PaymentConfirmedDomainEvent(Guid OrderId, Money TotalAmount) : IDomainEvent;
}
