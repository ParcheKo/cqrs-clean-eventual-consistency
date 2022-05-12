using Orders.Domain.SeedWork;

namespace Orders.Domain.Persons;

public class PersonRegistered : DomainEventBase
{
    public PersonRegistered(PersonId personId,
        string email,
        string name
    )
    {
        PersonId = personId;
        Email = email;
        Name = name;
    }

    public PersonId PersonId { get; }
    public string Email { get;  }
    public string Name { get;  }
}