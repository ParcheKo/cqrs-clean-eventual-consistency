using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Persons;
using Orders.Application.Persons.GetPersons;

namespace Orders.Api.Persons
{
    [Route("api/persons")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(
            typeof(List<PersonViewModel>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetPersons(CancellationToken cancellationToken)
        {
            var persons = await _mediator.Send(
                new GetPersonsQuery(),
                cancellationToken
            );

            return Ok(persons);
        }  
    }
}

