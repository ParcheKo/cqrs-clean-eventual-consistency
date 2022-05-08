using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orders.Command.Abstractions;
using Orders.Command.CreateCard;

namespace Orders.Api.Endpoints.CreateCard;

[Route("api/cards")]
[Produces("application/json")]
public class CreateCardEndpoint : Controller
{
    private readonly ICommandDispatcher _commandDispatcher;

    public CreateCardEndpoint(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCardRequest request)
    {
        var command = new CreateCardCommand(
            request.Number,
            request.CardHolder,
            request.ExpirationDate
        );
        var result = await _commandDispatcher.Dispatch(command);

        if (result.Success)
        {
            var response = new CreateCardResponse
            {
                Id = result.Id,
                Number = result.Number,
                CardHolder = result.CardHolder,
                ExpirationDate = result.ExpirationDate
            };

            return CreatedAtAction(
                null,
                response
            );
        }

        return BadRequest(request);
    }
}