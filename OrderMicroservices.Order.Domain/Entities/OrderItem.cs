using OrderMicroservices.Common;
using OrderMicroservices.Common.ValueObjects;

namespace OrderMicroservices.Orders.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Money UnitPrice { get; set; }

        public int Quantity { get; set; }

        public OrderItem(Guid productId, string productName, Money unitPrice, int quantity)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));
        }   
        protected OrderItem() { }
    }
}
