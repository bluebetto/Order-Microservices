
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderMicroservices.Common.Middleware;
using OrderMicroservices.EventBus;
using OrderMicroservices.Orders.Application.Commands.CreateOrder;
using OrderMicroservices.Orders.Infra;

namespace OrderMicroservices.Orders.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.AddServiceDefaults();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfra(builder.Configuration);

        var licenseKey = builder.Configuration["Resources:MediatrKey"];

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(CreateOrderCommand).Assembly);
            cfg.LicenseKey = licenseKey;
        });

        builder.Services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = licenseKey;
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly);
        });

        builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
        builder.Services.AddEventBus();

        // Add FluentValidation and register validators from the specified assembly
        builder.Services.AddValidatorsFromAssembly(typeof(CreateOrderCommandValidator).Assembly);

        var app = builder.Build();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
