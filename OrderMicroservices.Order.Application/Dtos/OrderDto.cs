using OrderMicroservices.Orders.Application.Commands.CreateOrder;

namespace OrderMicroservices.Orders.Application.Dtos
{
    public record OrderDto(
        Guid Id,
        string CustomerId,
        DateTime OrderDate,
        string Status,
        decimal TotalAmount,
        string Currency,
        AddressDto DeliveryAddress,
        List<OrderItemDto> Items
    );
}
