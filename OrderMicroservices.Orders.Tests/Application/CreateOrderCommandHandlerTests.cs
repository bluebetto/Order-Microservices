using AutoMapper;
using FluentAssertions;
using Moq;
using OrderMicroservices.EventBus;
using OrderMicroservices.EventBus.Events;
using OrderMicroservices.Orders.Application.Commands.CreateOrder;
using OrderMicroservices.Orders.Domain.Entities;
using OrderMicroservices.Orders.Infra.Repositories;

namespace OrderFlow.Orders.UnitTests.Application;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IEventBus> _mockEventBus;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockEventBus = new Mock<IEventBus>();

        _handler = new CreateOrderCommandHandler(
            _mockOrderRepository.Object,
            _mockEventBus.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateOrderAndPublishEvent()
    {
        // Arrange
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new AddressDto("Rua A", "123", "", "Jardins", "São Paulo", "SP", "01234-567", "Brazil"),
            new List<OrderItemDto>
            {
                new(Guid.NewGuid(), "Product 1", 10.00m, 2)
            }            
        );

        _mockOrderRepository
            .Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order o, CancellationToken ct) => o);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.OrderId.Should().NotBeEmpty();
        result.TotalAmount.Should().Be(20.00m);

        _mockOrderRepository.Verify(
            r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _mockEventBus.Verify(
            e => e.PublishAsync(It.IsAny<OrderCreatedIntegrationEvent>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_EmptyItems_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new AddressDto("Rua A", "123", "", "Jardins", "São Paulo", "SP", "01234-567", "Brazil"),
            new List<OrderItemDto>()
        );

        var validator = new CreateOrderCommandValidator();

        // Act
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e =>
            e.ErrorMessage == "Orders must have at least one item");
    }
}