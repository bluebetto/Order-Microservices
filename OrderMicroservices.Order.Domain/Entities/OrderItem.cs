using OrderMicroservices.Order.Domain.ValueObjects;

namespace OrderMicroservices.Order.Domain.Entities
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public Money UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
