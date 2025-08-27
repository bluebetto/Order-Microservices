namespace OrderMicroservices.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T integrationEvent) where T : IIntegrationEvent;
    }

}
