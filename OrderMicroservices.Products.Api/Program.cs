
using FluentValidation;
using OrderMicroservices.Common.Middleware;
using OrderMicroservices.EventBus;
using OrderMicroservices.Products.Application.Commands.UpdateStock;
using OrderMicroservices.Products.Infra;

namespace OrderMicroservices.Products.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddInfra(builder.Configuration);

        var licenseKey = builder.Configuration["Resources:MediatrKey"];

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(UpdateStockCommand).Assembly);
            cfg.LicenseKey = licenseKey;
        });

        builder.Services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = licenseKey;
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(UpdateStockCommand).Assembly);
        });

        builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
        builder.Services.AddEventBus();

        builder.Services.AddValidatorsFromAssembly(typeof(UpdateStockCommandValidator).Assembly);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
