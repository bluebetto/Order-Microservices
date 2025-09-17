using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OrderMicroservices.Common.ValueObjects;
using OrderMicroservices.Orders.Domain.Entities;
using OrderMicroservices.Orders.Infra.Data;
using OrderMicroservices.Orders.Infra.Repositories;
using Xunit;

namespace OrderMicroservices.Orders.Tests.Infra;

public class OrderRepositoryExtraTests
{
    [Fact]
    public async Task SaveChangesAsync_ShouldReturnFalseIfNoChanges()
    {
        using var context = new OrderDbContext(new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var repo = new OrderRepository(context);

        var result = await repo.SaveChangesAsync();
        result.Should().BeFalse();
    }

    [Fact]
    public void Update_NonExistentOrder_ShouldNotThrow()
    {
        using var context = new OrderDbContext(new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var repo = new OrderRepository(context);

        var order = new Order();
        repo.Update(order);
        // Não deve lançar exceção
    }
}