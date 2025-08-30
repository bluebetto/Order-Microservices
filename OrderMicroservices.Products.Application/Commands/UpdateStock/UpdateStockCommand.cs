using MediatR;
using OrderMicroservices.Products.Domain.Enums;

namespace OrderMicroservices.Products.Application.Commands.UpdateStock
{
    public record UpdateStockCommand(
        Guid ProductId,
        int Quantity,
        StockOperation Operation
    ) : IRequest<UpdateStockResult>;
}
