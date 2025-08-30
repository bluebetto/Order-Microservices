using MediatR;

namespace OrderMicroservices.Products.Application.Queries.GetProduct
{
    public record GetProductsQuery(
    int Page = 1,
    int PageSize = 10,
    string? Category = null
) : IRequest<GetProductsResult>;
}
