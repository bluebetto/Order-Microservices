using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMicroservices.EventBus;
using OrderMicroservices.EventBus.Manager;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime;
using System.Text;

public abstract class RabbitMqBaseConsumer : BackgroundService
{
    private readonly RabbitMQConnectionManager _connectionManager;
    private IChannel? _channel;

    private ILogger<RabbitMqBaseConsumer> _logger;
    private RabbitMQSettings _settings;

    protected string? _routingKey;

    protected RabbitMqBaseConsumer(
            IOptions<RabbitMQSettings> options,
            ILogger<RabbitMqBaseConsumer> logger
        )
    {
        _logger = logger;
        _settings = options.Value;
        _connectionManager = new RabbitMQConnectionManager(_settings);
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = await _connectionManager.GetChannelAsync(_routingKey);

        await base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Received message from [{Routing}]: {Message}", ea.RoutingKey, message);

            try
            {
                await ProcessMessageAsync(message, stoppingToken);
                _channel?.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                // Trate erro || por exemplo: log, rejeitar mensagem
                _logger.LogError(ex, "Error processing message: {Message}", message);
                _logger.LogError("StackTrace: {StackTrace}", ex.StackTrace);
                _channel?.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsumeAsync(queue: _settings.QueueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    // Método abstrato para processar mensagens
    protected abstract Task ProcessMessageAsync(string message, CancellationToken cancellationToken);

    public override async void Dispose()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }
        await _connectionManager.DisposeAsync();
        base.Dispose();
    }
}
