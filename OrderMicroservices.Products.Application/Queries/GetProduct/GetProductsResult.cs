using OrderMicroservices.Products.Application.DTOs;

namespace OrderMicroservices.Products.Application.Queries.GetProduct
{
    public record GetProductsResult(
    List<ProductDto> Products,
    int TotalCount,
    int Page,
    int PageSize
);
}
