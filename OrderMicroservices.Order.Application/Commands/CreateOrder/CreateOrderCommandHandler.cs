using AutoMapper;
using MediatR;
using OrderMicroservices.EventBus;
using OrderMicroservices.EventBus.Events;
using OrderMicroservices.Orders.Domain.Entities;
using OrderMicroservices.Orders.Domain.Events;
using OrderMicroservices.Orders.Domain.ValueObjects;
using OrderMicroservices.Orders.Infra.Repositories;

namespace OrderMicroservices.Orders.Application.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(IOrderRepository _repository, IEventBus _eventBus) : IRequestHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(
                    request.Address.Street,
                    request.Address.Number,
                    request.Address.Complement,
                    request.Address.City,
                    request.Address.State,
                    request.Address.ZipCode,
                    request.Address.Country
                );

            var orderItems = request.Items.Select(item => new OrderItem(
                                                                item.ProductId,
                                                                item.ProductName,
                                                                new Money(item.Price, "BRL"),
                                                                item.Quantity
                                                                )).ToList();

            var order = Order.Create(request.CustomerId,
                                     address,
                                     orderItems
                                    );

            await _repository.AddAsync(order, cancellationToken);

            //Publish Events

            return new CreateOrderResult(order.Id, order.TotalAmount.Amount);
        }

        private async Task PublishDomainEvents(Order order)
        {
            foreach (var domainEvent in order.DomainEvents)
            {
                switch (domainEvent)
                {
                    case OrderCreatedDomainEvent orderCreated:
                        var integrationEvent = new OrderCreatedIntegrationEvent(
                            orderCreated.Order.Id,
                            orderCreated.Order.CustomerId,
                            orderCreated.Order.TotalAmount.Amount,
                            orderCreated.Order.Items.Select(i => new OrderItemIntegrationDto(
                                i.ProductId,
                                i.ProductName,
                                i.UnitPrice.Amount,
                                i.Quantity
                            )).ToList()
                        );

                        await _eventBus.PublishAsync(integrationEvent);
                        break;
                }
            }

            order.ClearDomainEvents();
        }
    }
}
