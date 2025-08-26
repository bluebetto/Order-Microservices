using OrderMicroservices.Common;
using OrderMicroservices.Orders.Domain.Enums;
using OrderMicroservices.Orders.Domain.ValueObjects;

namespace OrderMicroservices.Orders.Domain.Entities
{
    public class Order : BaseEntity
    {
        private readonly List<OrderItem> _items = new();
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; }
        public Money TotalAmount { get; set; }
        public Address DeliveryAddress { get; set; }
        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

        public static Order Create(Guid customerId, Address deliveryAddress, List<OrderItem> items)
        {
            var order = new Order
            {
                CustomerId = customerId,
                DeliveryAddress = deliveryAddress,
                Status = OrderStatus.Pending,
            };
            order._items.AddRange(items);

            order.SumTotalAmount();
            return order;
        }

        private void SumTotalAmount()
        {
            var total = _items.Sum(i => i.UnitPrice.Amount * i.Quantity);
            TotalAmount = new Money(total, "BRL");
        }
    }
}
