using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Orders.Query.Abstractions;
using Orders.Query.Queries;
using Orders.Query.Queries.Cards;

namespace Orders.Api.Endpoints.GetCardView
{
    [Route("api/cards")]
    [Produces("application/json")]
    public class GetCardViewEndpoint : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public GetCardViewEndpoint(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetCardByIdQuery(id);

            var queryResult = await _queryDispatcher.ExecuteAsync(query);

            if (queryResult == null)
            {
                return BadRequest(id);
            }

            var response = new GetCardViewResponse()
            {
                CardHolder = queryResult.CardHolder,
                ExpirationDate = queryResult.ExpirationDate,
                Id = queryResult.Id,
                Number = queryResult.Number
            };

            return Ok(response);
        }
    }
}