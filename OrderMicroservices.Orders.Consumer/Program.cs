using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderMicroservices.EventBus;
using OrderMicroservices.Order.Consumers;

namespace OrderMicroservices.Orders.Consumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services)=>
                {
                    services.Configure<RabbitMQSettings>(context.Configuration.GetSection("RabbitMQ"));
                    services.AddHostedService<RabbitMqCreateOrderConsumer>();
                }).Build();

            host.Run();
        }
    }
}
