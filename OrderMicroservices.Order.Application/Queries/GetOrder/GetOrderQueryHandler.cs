using AutoMapper;
using MediatR;
using OrderMicroservices.Orders.Application.Dtos;
using OrderMicroservices.Orders.Infra.Repositories;

namespace OrderMicroservices.Orders.Application.Queries.GetOrder
{
    public class GetOrderQueryHandler(IOrderRepository _repository, IMapper _mapper)
        : IRequestHandler<GetOrderQuery, OrderDto?>
    {
        public async Task<OrderDto?> Handle(
            GetOrderQuery request,
            CancellationToken cancellationToken
        )
        {
            var order = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return _mapper.Map<OrderDto?>(order);
        }
    }
}
