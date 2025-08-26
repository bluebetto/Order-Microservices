using MediatR;

namespace OrderMicroservices.Orders.Application.Commands.CreateOrder
{
    public record CreateOrderCommand(
        Guid CustomerId, 
        AddressDto Address, 
        List<OrderItemDto> Items) : IRequest<CreateOrderResult>;
}
