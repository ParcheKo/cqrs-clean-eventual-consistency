using System;
using Orders.Domain.Persons.Rules;
using Orders.Domain.SeedWork;

namespace Orders.Domain.Persons
{
    public class Person : Entity, IAggregateRoot
    {
        public PersonId Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; } // todo : make it value-object 

        private Person()
        {
        }

        private Person(
            string email,
            string name
        )
        {
            this.Id = new PersonId(Guid.NewGuid());
            Email = email;
            Name = name;

            this.AddDomainEvent(new PersonRegistered(this.Id));
        }

        public static Person From(
            string email,
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
}