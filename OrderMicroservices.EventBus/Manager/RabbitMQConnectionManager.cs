using RabbitMQ.Client;

namespace OrderMicroservices.EventBus.Manager
{
    public class RabbitMQConnectionManager : IAsyncDisposable
    {
        private readonly RabbitMQSettings _settings;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMQConnectionManager(RabbitMQSettings settings)
        {
            _settings = settings;
        }

        public async Task<IChannel> GetChannelAsync()
        {
            if (_connection == null)
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.HostName,
                    UserName = _settings.UserName,
                    Password = _settings.Password,
                    ConsumerDispatchConcurrency = 1
                };
                _connection = await factory.CreateConnectionAsync();
            }

            if (_channel == null)
            {
                _channel = await _connection.CreateChannelAsync();
                await _channel.QueueDeclareAsync(
                    queue: _settings.QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
            }

            return _channel;
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
                await _channel.CloseAsync();
            if (_connection != null)
                await _connection.CloseAsync();
        }
    }
}