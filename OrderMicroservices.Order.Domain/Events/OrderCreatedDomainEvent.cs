using OrderMicroservices.Common;
using OrderMicroservices.Orders.Domain.Entities;

namespace OrderMicroservices.Orders.Domain.Events
{
    public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
}
