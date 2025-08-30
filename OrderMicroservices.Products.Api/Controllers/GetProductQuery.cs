using MediatR;
using OrderMicroservices.Products.Application.DTOs;

namespace OrderMicroservices.Products.Api.Controllers
{
    public record GetProductQuery(Guid ProductId) : IRequest<ProductDto?>;
}