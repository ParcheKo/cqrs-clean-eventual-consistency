using System;
using Orders.Domain.Persons.Rules;
using Orders.Domain.SeedWork;
using Orders.Domain.SharedKernel.Email;

namespace Orders.Domain.Persons;

public class Person : Entity, IAggregateRoot
{
    private Person()
    {
    }

    private Person(
        Email email,
        string name
    )
    {
        Id = new PersonId(Guid.NewGuid());
        Email = email;
        Name = name;

        AddDomainEvent(new PersonRegistered(Id, Email.Value, Name));
    }

    public PersonId Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }

    public static Person From(
        Email email,
        string name,
        bool emailIsUnique
    )
    {
        CheckRule(new PersonEmailMustBeUnique(emailIsUnique));

        return new Person(
            email,
            name
        );
    }
}