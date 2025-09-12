using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMicroservices.EventBus.Manager;
using RabbitMQ.Client;

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
            var channel = await _connectionManager.GetChannelAsync();
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

            await channel.BasicPublishAsync(
                exchange: _settings.QueueName,
                routingKey: $"orders.{eventName.ToLowerInvariant()}",
                false,
                basicProperties: properties,
                body: body
            );

            _logger.LogInformation(
                "Published integration event {EventName} with ID {EventId}",
                eventName,
                integrationEvent.Id
            );

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _connectionManager.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
