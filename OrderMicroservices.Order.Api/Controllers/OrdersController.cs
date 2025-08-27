using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderMicroservices.Orders.Application.Commands.CreateOrder;
using OrderMicroservices.Orders.Application.Dtos;
using OrderMicroservices.Orders.Application.Queries.GetOrder;

namespace OrderMicroservices.Orders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(CreatedResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetOrder), new { Id= result.OrderId}, result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetOrderQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
