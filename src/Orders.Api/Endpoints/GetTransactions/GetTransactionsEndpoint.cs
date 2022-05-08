using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Orders.Query.Abstractions;
using Orders.Query.Queries;
using Orders.Query.Queries.Transactions;

namespace Orders.Api.Endpoints.GetTransactions
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;

        public TransactionController(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetTransactionsRequest request)
        {
            var query = new GetTransactionListQuery()
            {
                BetweenAmount = request.BetweenAmount,
                CardHolder = request.CardHolder,
                CardNumber = request.CardNumber,
                ChargeDate = request.ChargeDate,
                Limit = request.Limit,
                Offset = request.Offset
            };

            var result = await queryDispatcher.ExecuteAsync(query);

            if (!result.Any())
            {
                return NotFound(query);
            }

            var response = result.Select(x => new GetTransactionsResponse()
            {
                Amount = x.Amount,
                CardHolder = x.CardHolder,
                CardNumber = x.CardNumber,
                ChargeDate = x.ChargeDate,
                CurrencyCode = x.CurrencyCode,
                UniqueId = x.UniqueId
            });

            return Ok(response);
        }
    }
}