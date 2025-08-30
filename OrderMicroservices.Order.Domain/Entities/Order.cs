using OrderMicroservices.Common;
using OrderMicroservices.Common.ValueObjects;
using OrderMicroservices.Orders.Domain.Enums;
using OrderMicroservices.Orders.Domain.Events;

namespace OrderMicroservices.Orders.Domain.Entities
{
    public class Order : BaseEntity
    {
        private readonly List<OrderItem> _items = new();
        private readonly List<IDomainEvent> _domainEvents = new();
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; }
        public Money TotalAmount { get; set; }
        public Address DeliveryAddress { get; set; }
        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public static Order Create(Guid customerId, Address deliveryAddress, List<OrderItem> items)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                DeliveryAddress = deliveryAddress,
                Status = OrderStatus.Pending,
            };
            order._items.AddRange(items);

            order.SumTotalAmount();
            order.AddDomainEvent(new OrderCreatedDomainEvent(order));
            return order;
        }

        private void SumTotalAmount()
        {
            var total = _items.Sum(i => i.UnitPrice.Amount * i.Quantity);
            TotalAmount = new Money(total, "BRL");
        }

        public void Cancel()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be cancelled");

            Status = OrderStatus.Cancelled;
            AddDomainEvent(new OrderCancelledDomainEvent(Id));
        }

        public void ConfirmPayment()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can have payment confirmed");

            Status = OrderStatus.PaymentConfirmed;
            AddDomainEvent(new PaymentConfirmedDomainEvent(Id, TotalAmount));
        }

        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
