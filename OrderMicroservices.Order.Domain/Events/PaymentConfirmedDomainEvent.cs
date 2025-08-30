using OrderMicroservices.Common;
using OrderMicroservices.Common.ValueObjects;

namespace OrderMicroservices.Orders.Domain.Events
{
    public record PaymentConfirmedDomainEvent(Guid OrderId, Money TotalAmount) : IDomainEvent;
}
