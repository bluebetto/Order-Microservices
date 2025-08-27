namespace OrderMicroservices.EventBus
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
    }
}
