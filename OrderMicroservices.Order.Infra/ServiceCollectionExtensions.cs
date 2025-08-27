using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderMicroservices.Orders.Infra.Data;
using OrderMicroservices.Orders.Infra.Repositories;

namespace OrderMicroservices.Orders.Infra
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

    }
}
