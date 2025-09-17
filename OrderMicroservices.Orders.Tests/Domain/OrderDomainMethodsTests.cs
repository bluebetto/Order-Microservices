using FluentAssertions;
using OrderMicroservices.Common.ValueObjects;
using OrderMicroservices.Orders.Domain.Entities;
using OrderMicroservices.Orders.Domain.Enums;
using Xunit;

namespace OrderFlow.Orders.UnitTests.Domain;

public class OrderDomainMethodsTests
{
    [Fact]
    public void ConfirmPayment_ShouldChangeStatus()
    {
        var order = CreateValidOrder();
        order.ConfirmPayment();
        order.Status.Should().Be(OrderStatus.PaymentConfirmed);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        var order = CreateValidOrder();
        order.ClearDomainEvents();
        order.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void SumTotalAmount_ShouldCalculateCorrectly()
    {
        var items = new List<OrderItem>
        {
            new(Guid.NewGuid(), "Produto 1", new Money(10, "BRL"), 2),
            new(Guid.NewGuid(), "Produto 2", new Money(5, "BRL"), 1)
        };
        var address = new Address("Rua", "1", "", "Cidade", "UF", "00000-000", "BR");
        var order = Order.Create(Guid.NewGuid(), address, items);

        // Forçar recalculo
        var method = typeof(Order).GetMethod("SumTotalAmount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method!.Invoke(order, null);

        order.TotalAmount.Amount.Should().Be(25);
    }

    private static Order CreateValidOrder()
    {
        var items = new List<OrderItem>
        {
            new(Guid.NewGuid(), "Produto", new Money(10, "BRL"), 1)
        };
        var address = new Address("Rua", "1", "", "Cidade", "UF", "00000-000", "BR");
        return Order.Create(Guid.NewGuid(), address, items);
    }
}