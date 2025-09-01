using AutoMapper;
using FluentAssertions;
using Moq;
using OrderMicroservices.Common.ValueObjects;
using OrderMicroservices.EventBus;
using OrderMicroservices.Products.Application.Commands.UpdateStock;
using OrderMicroservices.Products.Application.DTOs;
using OrderMicroservices.Products.Application.Queries.GetProduct;
using OrderMicroservices.Products.Application.Request;
using OrderMicroservices.Products.Domain.Entities;
using OrderMicroservices.Products.Domain.Enums;
using OrderMicroservices.Products.Infra.Repositories;

namespace OrderMicroservices.Products.Test
{
    public class ProductApplicationTests
    {
        [Fact]
        public void ProductDto_CanBeCreated()
        {
            var dto = new ProductDto(Guid.NewGuid(), "Name", "Desc", 10, "USD", "Cat", 5, true);
            dto.Should().NotBeNull();
        }

        [Fact]
        public void GetProductsResult_CanBeCreated()
        {
            var result = new GetProductsResult(new List<ProductDto>(), 0, 1, 10);
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetProductQuery_CanBeCreated()
        {
            var query = new GetProductQuery(Guid.NewGuid());
            query.Should().NotBeNull();
        }

        [Fact]
        public void UpdateStockCommand_CanBeCreated()
        {
            var cmd = new UpdateStockCommand(Guid.NewGuid(), 1, StockOperation.Add);
            cmd.Should().NotBeNull();
        }

        [Fact]
        public void UpdateStockResult_CanBeCreated()
        {
            var result = new UpdateStockResult(Guid.NewGuid(), 1, 2);
            result.Should().NotBeNull();
        }

        [Fact]
        public void UpdateStockRequest_CanBeCreated()
        {
            var req = new UpdateStockRequest(1, StockOperation.Add);
            req.Should().NotBeNull();
        }

        [Fact]
        public void GetProductsQuery_CanBeCreated()
        {
            var query = new GetProductsQuery(1, 10, "Cat");
            query.Should().NotBeNull();
        }

        [Fact]
        public async Task GetProductQueryHandler_ReturnsProductDto()
        {
            var repo = new Mock<IProductRepository>();
            var mapper = new Mock<IMapper>();
            var product = Product.Create("Name", "Desc", new Money(10, "USD"), "Cat", 5);
            var handler = new GetProductQueryHandler(repo.Object, mapper.Object);
            repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(product);
            mapper.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto(product.Id, product.Name, product.Description, product.Price.Amount, product.Price.Currency, product.Category, product.Stock.Quantity, product.IsActive));
            var result = await handler.Handle(new GetProductQuery(product.Id), CancellationToken.None);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetProductsQueryHandler_ReturnsGetProductsResult()
        {
            var repo = new Mock<IProductRepository>();
            var mapper = new Mock<IMapper>();
            var handler = new GetProductsQueryHandler(repo.Object, mapper.Object);
            repo.Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Product>());
            mapper.Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>())).Returns(new List<ProductDto>());
            var result = await handler.Handle(new GetProductsQuery(1, 10, null), CancellationToken.None);
            result.Should().NotBeNull();
        }

        [Fact]
        public void UpdateStockCommandValidator_ValidatesCorrectly()
        {
            var validator = new UpdateStockCommandValidator();
            var valid = validator.Validate(new UpdateStockCommand(Guid.NewGuid(), 1, StockOperation.Add));
            valid.IsValid.Should().BeTrue();
            var invalid = validator.Validate(new UpdateStockCommand(Guid.Empty, 0, (StockOperation)999));
            invalid.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateStockCommandHandler_ThrowsIfProductNotFound()
        {
            var repo = new Mock<IProductRepository>();
            var eventBus = new Mock<IEventBus>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Product)null);
            var handler = new UpdateStockCommandHandler(repo.Object, eventBus.Object);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new UpdateStockCommand(Guid.NewGuid(), 1, StockOperation.Add), CancellationToken.None));
        }
    }
}
