using OrderMicroservices.Products.Application.DTOs;
using Xunit;

namespace OrderMicroservices.Products.Test;

public class ProductDtoTests
{
    [Fact]
    public void ProductDto_PropertiesShouldBeSet()
    {
        var id = Guid.NewGuid();
        var dto = new ProductDto(
            id,
            "Produto",
            "Descrição",
            99.99m,
            "BRL",
            "Categoria",
            10,
            true
        );

        Assert.Equal(id, dto.Id);
        Assert.Equal("Produto", dto.Name);
        Assert.Equal("Descrição", dto.Description);
        Assert.Equal(99.99m, dto.Price);
        Assert.Equal("BRL", dto.Currency);
        Assert.Equal("Categoria", dto.Category);
        Assert.Equal(10, dto.StockQuantity);
        Assert.True(dto.IsActive);
    }
}