namespace OrderMicroservices.Order.Application.Commands.CreateOrder
{
    public record OrderItemDto(
        Guid ProductId,
        string ProductName,
        decimal Price,
        int Quantity
        );
}
