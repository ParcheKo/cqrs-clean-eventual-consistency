using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Orders.GetOrdersByEmail;
using SampleProject.Application.Customers;
using SampleProject.Application.Orders.PlaceCustomerOrder;

namespace SampleProject.API.Orders
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(
            typeof(List<OrderViewModel>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _mediator.Send(new GetOrdersQuery());

            return Ok(orders);
        }

        [Route("~/api/persons/{personEmail}/orders")]
        [HttpGet]
        [ProducesResponseType(
            typeof(List<OrderViewModel>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetOrdersByPersonEmail(string personEmail)
        {
            var orders = await _mediator.Send(new GetOrdersByEmailQuery(personEmail));

            return Ok(orders);
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegisterOrder([FromBody] RegisterOrderRequest request)
        {
            await _mediator.Send(
                new RegisterOrderCommand(
                    request.OrderDate,
                    request.CreatedBy,
                    request.OrderNo,
                    request.ProductName,
                    request.Total,
                    request.Price
                )
            );

            return Created(
                string.Empty,
                null
            );
        }
    }
}