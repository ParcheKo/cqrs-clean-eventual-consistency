using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Orders.Domain.Orders.Events;
using Orders.Domain.Persons;

namespace Orders.Application.Orders.RegisterOrder;

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
            await _personRepository.Add(person);
        }
    }
}