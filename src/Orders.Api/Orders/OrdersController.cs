using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Orders;
using Orders.Application.Orders.GetOrders;
using Orders.Application.Orders.GetOrdersByEmail;
using Orders.Application.Orders.RegisterOrder;

namespace Orders.Api.Orders;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("")]
    [HttpGet]
    [ProducesResponseType(
        typeof(List<OrderViewModel>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(
            new GetOrdersQuery(),
            cancellationToken
        );

        return Ok(orders);
    }

    [Route("~/api/persons/{personEmail}/orders")]
    [HttpGet]
    [ProducesResponseType(
        typeof(List<OrderViewModel>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetOrdersByPersonEmail(
        string personEmail,
        CancellationToken cancellationToken
    )
    {
        var orders = await _mediator.Send(
            new GetOrdersByEmailQuery(personEmail),
            cancellationToken
        );

        return Ok(orders);
    }

    [Route("")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<IActionResult> RegisterOrder(
        [FromBody] RegisterOrderRequest request,
        CancellationToken cancellationToken
    )
    {
        var orderDto = await _mediator.Send(
            new RegisterOrderCommand(
                request.OrderDate,
                request.PersonEmail,
                request.OrderNo,
                request.ProductName,
                request.Total,
                request.Price
            ), cancellationToken
        );

        return Created(
            string.Empty,
            orderDto
        );
    }
}