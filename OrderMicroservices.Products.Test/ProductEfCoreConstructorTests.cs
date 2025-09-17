using OrderMicroservices.Products.Domain.Entities;
using Xunit;

namespace OrderMicroservices.Products.Test;

public class ProductEfCoreConstructorTests
{
    [Fact]
    public void ProtectedConstructor_ShouldCreateInstance()
    {
        var product = (Product)Activator.CreateInstance(typeof(Product), true)!;
        Assert.NotNull(product);
    }
}