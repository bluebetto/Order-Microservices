using OrderMicroservices.Common;
using OrderMicroservices.Common.ValueObjects;
using OrderMicroservices.Products.Domain.Enums;
using OrderMicroservices.Products.Domain.Events;
using OrderMicroservices.Products.Domain.Exceptions;

namespace OrderMicroservices.Products.Domain.Entities
{
    public class Product : BaseEntity
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Money Price { get; private set; } = Money.Zero("BRL");
        public string Category { get; private set; } = string.Empty;
        public Stock Stock { get; private set; } = new(0);
        public bool IsActive { get; private set; } = true;

        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private Product() { } // EF Core

        public static Product Create(
            string name,
            string description,
            Money price,
            string category,
            int initialStock)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                Stock = new Stock(initialStock)
            };

            product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id, product.Name, product.Price.Amount));
            return product;
        }

        public void UpdateStock(int quantity, StockOperation operation)
        {
            var previousQuantity = Stock.Quantity;

            Stock = operation switch
            {
                StockOperation.Add => new Stock(Stock.Quantity + quantity),
                StockOperation.Remove => new Stock(Math.Max(0, Stock.Quantity - quantity)),
                StockOperation.Set => new Stock(quantity),
                _ => throw new ArgumentException("Invalid stock operation")
            };

            AddDomainEvent(new StockUpdatedDomainEvent(
                Id,
                previousQuantity,
                Stock.Quantity,
                operation));
        }

        public bool HasSufficientStock(int requestedQuantity)
        {
            return Stock.Quantity >= requestedQuantity;
        }

        public void ReserveStock(int quantity)
        {
            if (!HasSufficientStock(quantity))
                throw new InsufficientStockException($"Insufficient stock for product {Name}. Available: {Stock.Quantity}, Requested: {quantity}");

            UpdateStock(quantity, StockOperation.Remove);
            AddDomainEvent(new StockReservedDomainEvent(Id, quantity));
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
