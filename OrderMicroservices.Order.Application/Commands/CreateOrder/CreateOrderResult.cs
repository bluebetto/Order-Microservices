namespace OrderMicroservices.Order.Application.Commands.CreateOrder
{
    public record CreateOrderResult (Guid OrderId, decimal TotalAmount);
}
