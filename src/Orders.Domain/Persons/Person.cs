using System;
using SampleProject.Domain.Customers.Rules;
using SampleProject.Domain.SeedWork;

namespace SampleProject.Domain.Customers
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