using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SampleProject.Domain.Customers;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Domain.Customers.Orders.Events;

namespace SampleProject.Application.Orders.PlaceCustomerOrder
{
    public class OrderRegisteredEventHandler : INotificationHandler<OrderRegisteredEvent>
    {
        private readonly IPersonRepository _personRepository;

        public OrderRegisteredEventHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task Handle(
            OrderRegisteredEvent e,
            CancellationToken cancellationToken
        )
        {
            var personExists = await _personRepository.ExistsWithEmail(e.Email);
            if (!personExists)
            {
                var person = Person.From(
                    e.Email,
                    e.Email,
                    true
                );
                await this._personRepository.Add(person);
            }
        }
    }
}