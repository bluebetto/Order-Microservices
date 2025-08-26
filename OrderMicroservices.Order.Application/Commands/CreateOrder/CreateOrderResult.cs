namespace OrderMicroservices.Orders.Application.Commands.CreateOrder
{
    public record CreateOrderResult (Guid OrderId, decimal TotalAmount);
}
