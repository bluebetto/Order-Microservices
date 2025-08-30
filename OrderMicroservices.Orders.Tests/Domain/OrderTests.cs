using FluentAssertions;
using OrderMicroservices.Common.ValueObjects;
using OrderMicroservices.Orders.Domain.Entities;
using OrderMicroservices.Orders.Domain.Enums;
using OrderMicroservices.Orders.Domain.Events;

namespace OrderFlow.Orders.UnitTests.Domain;

public class OrderTests
{
    [Fact]
    public void Create_ValidOrder_ShouldCreateSuccessfully()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var items = new List<OrderItem>
        {
            new(Guid.NewGuid(), "Product 1", new Money(10.50m, "BRL"), 2),
            new(Guid.NewGuid(), "Product 2", new Money(25.00m, "BRL"), 1)
        };
        var address = new Address("Rua A", "123","", "São Paulo", "SP", "01234-567", "Brazil");

        // Act
        var order = Order.Create(customerId, address, items);

        // Assert
        order.Should().NotBeNull();
        order.Id.Should().NotBeEmpty();
        order.CustomerId.Should().Be(customerId);
        order.Status.Should().Be(OrderStatus.Pending);
        order.Items.Should().HaveCount(2);
        order.TotalAmount.Amount.Should().Be(46.00m); // (10.50 * 2) + 25.00
        order.TotalAmount.Currency.Should().Be("BRL");

        // Verificar evento de domínio
        order.DomainEvents.Should().HaveCount(1);
        order.DomainEvents.First().Should().BeOfType<OrderCreatedDomainEvent>();

        var domainEvent = order.DomainEvents.First() as OrderCreatedDomainEvent;
        domainEvent!.Order.Should().Be(order);
    }

    [Fact]
    public void Cancel_PendingOrder_ShouldCancelSuccessfully()
    {
        // Arrange
        var order = CreateValidOrder();

        // Act
        order.Cancel();

        // Assert
        order.Status.Should().Be(OrderStatus.Cancelled);
        order.DomainEvents.Should().HaveCount(2); // OrderCreated + OrderCancelled
        order.DomainEvents.Last().Should().BeOfType<OrderCancelledDomainEvent>();
    }

    [Fact]
    public void Cancel_NonPendingOrder_ShouldThrowException()
    {
        // Arrange
        var order = CreateValidOrder();
        order.ConfirmPayment(); // Muda status para PaymentConfirmed

        // Act & Assert
        order.Invoking(o => o.Cancel())
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Only pending orders can be cancelled");
    }

    private static Order CreateValidOrder()
    {
        var items = new List<OrderItem>
        {
            new(Guid.NewGuid(), "Product 1", new Money(10.00m, "BRL"), 1)
        };
        var address = new Address("Rua A", "123", "", "São Paulo", "SP", "01234-567", "Brazil");

        return Order.Create(Guid.NewGuid(), address, items);
    }
}