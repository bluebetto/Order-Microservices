using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.OrderMicroservices_Orders_Api>("ordermicroservices-orders-api");

builder.AddProject<Projects.OrderMicroservices_Products_Api>("ordermicroservices-products-api");

builder.Build().Run();
