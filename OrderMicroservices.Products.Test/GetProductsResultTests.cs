using OrderMicroservices.Products.Application.DTOs;
using OrderMicroservices.Products.Application.Queries.GetProduct;
using Xunit;

namespace OrderMicroservices.Products.Test;

public class GetProductsResultTests
{
    [Fact]
    public void GetProductsResult_PropertiesShouldBeSet()
    {
        var products = new List<ProductDto>
        {
            new ProductDto(Guid.NewGuid(), "Name", "Desc", 10, "BRL", "Cat", 5, true)
        };
        var result = new GetProductsResult(products, 1, 1, 10);

        Assert.Equal(products, result.Products);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
    }
}