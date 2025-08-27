using Microsoft.Extensions.DependencyInjection;

namespace OrderMicroservices.EventBus
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddScoped<IEventBus, RabbitMqEventBus>();
            return services;
        }
    }
}
