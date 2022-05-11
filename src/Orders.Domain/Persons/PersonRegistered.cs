using Orders.Domain.SeedWork;

namespace Orders.Domain.Persons
{
    public class PersonRegistered : DomainEventBase
    {
        public PersonId PersonId { get; }

        public PersonRegistered(PersonId personId)
        {
            this.PersonId = personId;
        }
    }
}