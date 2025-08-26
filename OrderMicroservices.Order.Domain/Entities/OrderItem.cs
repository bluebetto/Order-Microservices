using OrderMicroservices.Common;
using OrderMicroservices.Orders.Domain.ValueObjects;

namespace OrderMicroservices.Orders.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public Money UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
