using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Orders.Application.Configuration.Queries;

namespace Orders.Application.Persons.GetPersons;

internal sealed class GetPersonsQueryHandler : IQueryHandler<GetPersonsQuery, List<PersonDto>>
{
    public Task<List<PersonDto>> Handle(
        GetPersonsQuery request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}