using MediatR;
using OrderMicroservices.Products.Application.DTOs;

namespace OrderMicroservices.Products.Application.Queries.GetProduct
{
    public record GetProductQuery(Guid ProductId) : IRequest<ProductDto?>;
}
