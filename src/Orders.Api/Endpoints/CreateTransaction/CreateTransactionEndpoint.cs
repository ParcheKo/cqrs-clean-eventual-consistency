using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Orders.Command.Abstractions;
using Orders.Command.CreateTransaction;
using Orders.Core;
using Orders.Core.Shared;

namespace Orders.Api.Endpoints.CreateTransaction
{
    [Route("api/transactions")]
    [ApiController]
    public class CreateTransactionEndpoint : ControllerBase
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly ValidationNotificationHandler validationNotificationHandler;

        public CreateTransactionEndpoint(ICommandDispatcher commandDispatcher, ValidationNotificationHandler validationNotificationHandler)
        {
            this.commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            this.validationNotificationHandler = validationNotificationHandler ?? throw new ArgumentNullException(nameof(validationNotificationHandler));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTransactionRequest request)
        {
            var command = new CreateTransactionCommand(request.Amount, request.CurrencyCode, request.CardId, request.UniqueId, request.ChargeDate);
            var result = await commandDispatcher.Dispatch(command);

            if (result.Success)
            {
                var response = new CreateTransactionResponse()
                {
                    Amount = result.Amount,
                    CardId = result.CardId,
                    ChargeDate = result.ChargeDate,
                    CurrencyCode = result.CurrencyCode,
                    Id = result.Id,
                    UniqueId = result.UniqueId
                };

                return CreatedAtAction(null, response);
            }

            return BadRequest(validationNotificationHandler.Notifications);
        }
    }
}