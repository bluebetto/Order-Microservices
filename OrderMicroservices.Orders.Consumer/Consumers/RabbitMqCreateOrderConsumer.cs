using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMicroservices.EventBus;

namespace OrderMicroservices.Order.Consumers
{
    class RabbitMqCreateOrderConsumer : RabbitMqBaseConsumer
    {
        protected readonly ILogger<RabbitMqBaseConsumer> _logger;
        public RabbitMqCreateOrderConsumer(IOptions<RabbitMQSettings> settings, ILogger<RabbitMqCreateOrderConsumer> logger) 
            : base(settings, logger){
            _logger = logger;
            _routingKey = "orders.ordercreatedintegrationevent";
        }

        protected override Task ProcessMessageAsync(string message, CancellationToken cancellationToken)
        {
            _logger.LogInformation(message);
            return Task.CompletedTask;
        }
    }
}