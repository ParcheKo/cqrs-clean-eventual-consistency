using Orders.Domain.SeedWork;

namespace Orders.Domain.Persons;

public class PersonRegistered : DomainEventBase
{
    public PersonRegistered(PersonId personId)
    {
        PersonId = personId;
    }

    public PersonId PersonId { get; }
}