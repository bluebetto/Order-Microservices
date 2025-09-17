using FluentAssertions;
using OrderMicroservices.Orders.Application.Commands.CreateOrder;
using Xunit;

namespace OrderFlow.Orders.UnitTests.Application;

public class DtoAndValidatorTests
{
    [Fact]
    public void AddressDto_CanBeCreated()
    {
        var dto = new AddressDto("Rua", "1", "", "Bairro", "Cidade", "UF", "00000-000", "BR");
        dto.Should().NotBeNull();
    }

    [Fact]
    public void OrderItemDto_CanBeCreated()
    {
        var dto = new OrderItemDto(Guid.NewGuid(), "Produto", 10, 1);
        dto.Should().NotBeNull();
    }

    [Fact]
    public void AddressValidator_ShouldValidateCorrectly()
    {
        var validator = new AddressValidator();
        var valid = validator.Validate(new AddressDto("Rua", "1", "", "Bairro", "Cidade", "UF", "00000-000", "BR"));
        valid.IsValid.Should().BeTrue();
    }
}