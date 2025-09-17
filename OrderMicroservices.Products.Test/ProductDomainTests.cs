using FluentAssertions;
using OrderMicroservices.Common.ValueObjects;
using OrderMicroservices.Products.Domain.Entities;
using OrderMicroservices.Products.Domain.Enums;
using OrderMicroservices.Products.Domain.Exceptions;
using Xunit;

namespace OrderMicroservices.Products.Test;

public class ProductDomainTests
{
    [Fact]
    public void UpdateStock_Add_ShouldIncreaseQuantity()
    {
        var product = Product.Create("Teste", "Desc", new Money(10, "BRL"), "Cat", 5);
        product.UpdateStock(3, StockOperation.Add);
        product.Stock.Quantity.Should().Be(8);
    }

    [Fact]
    public void UpdateStock_Remove_ShouldDecreaseQuantity()
    {
        var product = Product.Create("Teste", "Desc", new Money(10, "BRL"), "Cat", 5);
        product.UpdateStock(2, StockOperation.Remove);
        product.Stock.Quantity.Should().Be(3);
    }

    [Fact]
    public void UpdateStock_Set_ShouldSetQuantity()
    {
        var product = Product.Create("Teste", "Desc", new Money(10, "BRL"), "Cat", 5);
        product.UpdateStock(10, StockOperation.Set);
        product.Stock.Quantity.Should().Be(10);
    }

    [Fact]
    public void HasSufficientStock_ShouldReturnTrueOrFalse()
    {
        var product = Product.Create("Teste", "Desc", new Money(10, "BRL"), "Cat", 5);
        product.HasSufficientStock(3).Should().BeTrue();
        product.HasSufficientStock(6).Should().BeFalse();
    }

    [Fact]
    public void ReserveStock_ShouldDecreaseQuantity()
    {
        var product = Product.Create("Teste", "Desc", new Money(10, "BRL"), "Cat", 5);
        product.ReserveStock(2);
        product.Stock.Quantity.Should().Be(3);
    }

    [Fact]
    public void ReserveStock_Insufficient_ShouldNotChangeQuantity()
    {
        var product = Product.Create("Teste", "Desc", new Money(10, "BRL"), "Cat", 2);
        Assert.Throws<InsufficientStockException>(() => product.ReserveStock(5));
        product.Stock.Quantity.Should().Be(2);        
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        var product = Product.Create("Teste", "Desc", new Money(10, "BRL"), "Cat", 5);
        product.UpdateStock(1, StockOperation.Add); // Gera evento
        product.ClearDomainEvents();
        product.DomainEvents.Should().BeEmpty();
    }
}