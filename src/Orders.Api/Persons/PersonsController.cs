// using System.Net;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.AspNetCore.Mvc;
// using SampleProject.Application.Customers;
// using SampleProject.Application.Customers.RegisterCustomer;
//
// namespace SampleProject.API.Customers
// {
//     [Route("api/persons")]
//     [ApiController]
//     public class PersonsController : Controller
//     {
//         private readonly IMediator _mediator;
//
//         public PersonsController(IMediator mediator)
//         {
//             this._mediator = mediator;
//         }
//
//         [Route("")]
//         [HttpPost]
//         [ProducesResponseType(typeof(PersonDto), (int)HttpStatusCode.Created)]
//         public async Task<IActionResult> RegisterCustomer([FromBody]RegisterCustomerRequest request)
//         {
//            var customer = await _mediator.Send(new RegisterPersonCommand(request.Email, request.Name));
//
//            return Created(string.Empty, customer);
//         }       
//     }
// }
