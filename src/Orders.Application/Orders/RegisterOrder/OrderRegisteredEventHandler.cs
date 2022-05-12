using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Orders.Domain.Orders.Events;
using Orders.Domain.Persons;
using Orders.Domain.SharedKernel.Email;

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
        var personExists = await _personRepository.ExistsWithEmail(e.CreatedBy);
        if (!personExists)
        {
            var person = Person.From(
                Email.Of(e.CreatedBy),
                e.CreatedBy, // todo: name??
                true
            );
            await _personRepository.Add(person);
        }
    }
}