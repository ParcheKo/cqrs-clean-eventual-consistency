using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SampleProject.Application.Configuration.Queries;
using SampleProject.Application.Customers;

namespace SampleProject.Application.Orders.GetCustomerOrders
{
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
}