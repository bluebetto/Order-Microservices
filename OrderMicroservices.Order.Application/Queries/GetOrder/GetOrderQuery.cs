using MediatR;
using OrderMicroservices.Orders.Application.Dtos;

namespace OrderMicroservices.Orders.Application.Queries.GetOrder
{
    public record GetOrderQuery(Guid Id) : IRequest<OrderDto?>;
}
