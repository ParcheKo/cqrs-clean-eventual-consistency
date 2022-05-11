using System;
using Orders.Domain.SeedWork;

namespace Orders.Domain.Persons;

public class PersonId : TypedIdValueBase
{
    public PersonId(Guid value) : base(value)
    {
    }
}