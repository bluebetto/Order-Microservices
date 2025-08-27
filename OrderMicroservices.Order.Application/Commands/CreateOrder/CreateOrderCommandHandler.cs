using MediatR;
using OrderMicroservices.Orders.Infra.Repositories;

namespace OrderMicroservices.Orders.Application.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(IOrderRepository _repository) : IRequestHandler<CreateOrderCommand, CreateOrderResult>
    {
        public Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
