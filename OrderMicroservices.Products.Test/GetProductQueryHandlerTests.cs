using AutoMapper;
using Moq;
using OrderMicroservices.Products.Application.DTOs;
using OrderMicroservices.Products.Application.Queries.GetProduct;
using OrderMicroservices.Products.Domain.Entities;
using OrderMicroservices.Products.Infra.Repositories;
using OrderMicroservices.Common.ValueObjects;
using Xunit;

namespace OrderMicroservices.Products.Test;

public class GetProductQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsNullIfNotFound()
    {
        var repo = new Mock<IProductRepository>();
        var mapper = new Mock<IMapper>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync((Product)null);
        var handler = new GetProductQueryHandler(repo.Object, mapper.Object);

        var result = await handler.Handle(new GetProductQuery(Guid.NewGuid()), default);
        Assert.Null(result);
    }
}