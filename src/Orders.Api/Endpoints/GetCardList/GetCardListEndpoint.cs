using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Orders.Query.Abstractions;
using Orders.Query.Queries;
using Orders.Query.Queries.Cards;

namespace Orders.Api.Endpoints.GetCardList
{
    [Route("api/cards")]
    [Produces("application/json")]
    public class GetCardListEndpoint : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public GetCardListEndpoint(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCardListRequest request)
        {
            var query = new GetCardListQuery()
            {
                CardHolder = request.CardHolder,
                ChargeDate = request.ChargeDate,
                Number = request.Number,
                Limit = request.Limit,
                Offset = request.Offset
            };

            var result = await _queryDispatcher.ExecuteAsync(query);

            if (!result.Any())
            {
                return NotFound(query);
            }

            var respose = result.Select(x => new GetCardListResponse()
            {
                Id = x.Id,
                Number = x.Number,
                CardHolder = x.CardHolder,
                ExpirationDate = x.ExpirationDate,
                HighestTransactionAmount = x.HighestTransactionAmount,
                HighestTransactionId = x.HighestTransactionId
            });

            return Ok(respose);
        }
    }
}