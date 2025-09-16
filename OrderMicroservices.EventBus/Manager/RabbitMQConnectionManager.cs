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

        public async Task<IChannel> GetChannelAsync(string? routingKey = "orders.#")
        {
            string receivedRoutingKey = routingKey ?? "orders.#";
            try
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

                    if (string.IsNullOrWhiteSpace(_settings.ExchangeName) || _settings.ExchangeName == "default")
                        throw new InvalidOperationException("ExchangeName inválido. Não pode ser vazio ou 'default'.");

                    await _channel.ExchangeDeclareAsync(exchange: _settings.ExchangeName, type: ExchangeType.Topic, durable: true);

                    await _channel.QueueDeclareAsync(
                        queue: _settings.QueueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    await _channel.QueueBindAsync(
                        queue: _settings.QueueName,
                        exchange: _settings.ExchangeName,
                        routingKey: receivedRoutingKey
                    );
                }

                return _channel;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao criar o channel RabbitMQ.", ex);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null && _channel.IsClosed != true)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }

        }
    }
}