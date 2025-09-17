using FluentAssertions;
using OrderMicroservices.Common.ValueObjects;
using OrderMicroservices.Orders.Domain.Entities;
using Xunit;

namespace OrderFlow.Orders.UnitTests.Domain;

public class OrderItemTests
{
    [Fact]
    public void Constructor_InvalidQuantity_ShouldThrowException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productName = "Produto";
        var unitPrice = new Money(10, "BRL");

        // Act
        var act = () => new OrderItem(productId, productName, unitPrice, 0);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Quantity must be positive*");
    }

    [Fact]
    public void ProtectedConstructor_ShouldCreateInstance()
    {
        // Act
        var item = (OrderItem)Activator.CreateInstance(typeof(OrderItem), true)!;

        // Assert
        item.Should().NotBeNull();
    }
}