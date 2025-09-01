using MediatR;
using OrderMicroservices.EventBus;
using OrderMicroservices.EventBus.Events;
using OrderMicroservices.Products.Domain.Entities;
using OrderMicroservices.Products.Domain.Events;
using OrderMicroservices.Products.Infra.Repositories;

namespace OrderMicroservices.Products.Application.Commands.UpdateStock
{
    public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, UpdateStockResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventBus _eventBus;

        public UpdateStockCommandHandler(IProductRepository productRepository, IEventBus eventBus)
        {
            _productRepository = productRepository;
            _eventBus = eventBus;
        }

        public async Task<UpdateStockResult> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product == null)
                throw new ArgumentException($"Product with ID {request.ProductId} not found");

            var previousStock = product.Stock.Quantity;

            product.UpdateStock(request.Quantity, request.Operation);

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);

            // Publicar eventos
            await PublishDomainEvents(product);

            return new UpdateStockResult(product.Id, previousStock, product.Stock.Quantity);
        }

        private async Task PublishDomainEvents(Product product)
        {
            foreach (var domainEvent in product.DomainEvents)
            {
                switch (domainEvent)
                {
                    case StockUpdatedDomainEvent stockUpdated:
                        await _eventBus.PublishAsync(new StockUpdatedIntegrationEvent(
                            stockUpdated.ProductId,
                            stockUpdated.PreviousQuantity,
                            stockUpdated.NewQuantity
                        ));
                        break;
                }
            }

            product.ClearDomainEvents();
        }
    }
}
