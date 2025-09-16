using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMicroservices.EventBus.Manager;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderMicroservices.EventBus
{
    public class RabbitMqEventBus : IEventBus, IDisposable
    {
        private readonly ILogger<RabbitMqEventBus> _logger;
        private readonly RabbitMQSettings _settings;
        private readonly RabbitMQConnectionManager _connectionManager;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public RabbitMqEventBus(
            IOptions<RabbitMQSettings> options,
            ILogger<RabbitMqEventBus> logger
        )
        {
            _logger = logger;
            _settings = options.Value;
            _connectionManager = new RabbitMQConnectionManager(_settings);
        }

        public async Task PublishAsync<T>(T integrationEvent)
            where T : IIntegrationEvent
        {
            try
            {
                IChannel channel = await _connectionManager.GetChannelAsync();

                var eventName = typeof(T).Name;
                var message = JsonSerializer.Serialize(integrationEvent, _jsonOptions);

                var body = Encoding.UTF8.GetBytes(message);

                var properties = new BasicProperties
                {
                    MessageId = integrationEvent.Id.ToString(),
                    Timestamp = new AmqpTimestamp(
                        ((DateTimeOffset)integrationEvent.CreatedAt).ToUnixTimeSeconds()
                    ),
                    DeliveryMode = DeliveryModes.Persistent,
                };

                var routingKey = $"orders.{eventName.ToLowerInvariant()}";

                await channel.BasicPublishAsync(
                    exchange: _settings.ExchangeName,
                    routingKey: routingKey,
                    false,
                    basicProperties: properties,
                    body: body
                );

                _logger.LogInformation(
                    "Published integration event {EventName} on routingKey {routingKey} with ID {EventId}",
                    eventName,
                    routingKey,
                    integrationEvent.Id
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtaining RabbitMQ channel.");
                throw;
            }      

            await Task.CompletedTask;
        }

        public async void Dispose()
        {
            GC.SuppressFinalize(this);
            await _connectionManager.DisposeAsync();
        }
    }
}
