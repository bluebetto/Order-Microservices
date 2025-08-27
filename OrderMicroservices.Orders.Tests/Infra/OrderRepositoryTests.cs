using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OrderMicroservices.Orders.Domain.Entities;
using OrderMicroservices.Orders.Domain.ValueObjects;
using OrderMicroservices.Orders.Infra.Data;
using OrderMicroservices.Orders.Infra.Repositories;

namespace OrderMicroservices.Orders.Tests.Infra
{
    public class OrderRepositoryTests
    {
        private OrderDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new OrderDbContext(options);
        }

        private Order CreateOrder(Guid? customerId = null)
        {
            var items = new List<OrderItem>
            {
                new(Guid.NewGuid(), "Product 1", new Money(10.00m, "BRL"), 2),
                new(Guid.NewGuid(), "Product 2", new Money(20.00m, "BRL"), 1)
            };
            var address = new Address("Street", "123", "", "City", "State", "00000-000", "Country");
            return Order.Create(customerId ?? Guid.NewGuid(), address, items);
        }

        [Fact]
        public async Task AddAsync_ShouldAddOrder()
        {
            using var context = CreateDbContext();
            var repo = new OrderRepository(context);
            var order = CreateOrder();

            var result = await repo.AddAsync(order);
            result.Should().NotBeNull();
            context.Orders.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrder()
        {
            using var context = CreateDbContext();
            var repo = new OrderRepository(context);
            var order = CreateOrder();
            await repo.AddAsync(order);

            var found = await repo.GetByIdAsync(order.Id);
            found.Should().NotBeNull();
            found!.Id.Should().Be(order.Id);
            found.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByCustomerIdAsync_ShouldReturnOrders()
        {
            using var context = CreateDbContext();
            var repo = new OrderRepository(context);
            var customerId = Guid.NewGuid();
            var order1 = CreateOrder(customerId);
            var order2 = CreateOrder(customerId);
            await repo.AddAsync(order1);
            await repo.AddAsync(order2);

            var orders = await repo.GetByCustomerIdAsync(customerId);
            orders.Should().HaveCount(2);
        }

        [Fact]
        public async Task Update_ShouldModifyOrder()
        {
            using var context = CreateDbContext();
            var repo = new OrderRepository(context);
            var order = CreateOrder();
            await repo.AddAsync(order);

            order.Status = Orders.Domain.Enums.OrderStatus.Shipped;
            repo.Update(order);
            await context.SaveChangesAsync();

            var updated = await repo.GetByIdAsync(order.Id);
            updated!.Status.Should().Be(Orders.Domain.Enums.OrderStatus.Shipped);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldPersistChanges()
        {
            using var context = CreateDbContext();
            var repo = new OrderRepository(context);
            var order = CreateOrder();
            await repo.AddAsync(order);

            order.Status = Orders.Domain.Enums.OrderStatus.Delivered;
            repo.Update(order);
            var saved = await repo.SaveChangesAsync();
            saved.Should().BeTrue();
        }
    }
}
