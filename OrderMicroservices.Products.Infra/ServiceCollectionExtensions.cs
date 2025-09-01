using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderMicroservices.Products.Infra.Data;
using OrderMicroservices.Products.Infra.Repositories;

namespace OrderMicroservices.Products.Infra
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }

    }
}
