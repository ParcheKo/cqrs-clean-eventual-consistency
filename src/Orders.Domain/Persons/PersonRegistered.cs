using SampleProject.Domain.SeedWork;

namespace SampleProject.Domain.Customers
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