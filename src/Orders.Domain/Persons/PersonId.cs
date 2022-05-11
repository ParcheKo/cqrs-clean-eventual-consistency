using System;
using SampleProject.Domain.SeedWork;

namespace SampleProject.Domain.Customers
{
    public class PersonId : TypedIdValueBase
    {
        public PersonId(Guid value) : base(value)
        {
        }
    }
}