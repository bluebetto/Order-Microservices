using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderMicroservices.Products.Application.Commands.UpdateStock;
using OrderMicroservices.Products.Application.DTOs;
using OrderMicroservices.Products.Application.Queries.GetProduct;
using OrderMicroservices.Products.Application.Request;

namespace OrderMicroservices.Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetProductsResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? category = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetProductsQuery(page, pageSize, category);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = new GetProductQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id:guid}/stock")]
        [ProducesResponseType(typeof(UpdateStockResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStock(
            Guid id,
            [FromBody] UpdateStockRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateStockCommand(id, request.Quantity, request.Operation);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
