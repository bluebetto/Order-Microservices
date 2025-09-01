namespace OrderMicroservices.EventBus.Events
{
    public record StockUpdatedIntegrationEvent(
        Guid ProductId,
        int PreviousQuantity,
        int NewQuantity
    ) : IIntegrationEvent
        {
            public Guid Id { get; } = Guid.NewGuid();
            public DateTime CreatedAt { get; } = DateTime.UtcNow;
        }
}
