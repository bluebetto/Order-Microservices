namespace OrderMicroservices.EventBus.Events
{
    public record OrderCreatedIntegrationEvent(
        Guid OrderId,
        Guid CustomerId,
        decimal TotalAmount,
        List<OrderItemIntegrationDto> Items
    ) : IIntegrationEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
    }

    public record OrderItemIntegrationDto(
        Guid ProductId,
        string ProductName,
        decimal Price,
        int Quantity
    );
}
