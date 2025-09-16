var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.OrderMicroservices_Orders_Api>("ordermicroservices-orders-api");

builder.AddProject<Projects.OrderMicroservices_Products_Api>("ordermicroservices-products-api");

builder.AddProject<Projects.OrderMicroservices_Orders_Consumer>("ordermicroservices-orders-consumer");

builder.Build().Run();
