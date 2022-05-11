using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SampleProject.Application.Configuration.Queries;
using SampleProject.Application.Customers;

namespace SampleProject.Application.Orders.GetCustomerOrders
{
    internal sealed class GetOrdersByEmailQueryHandler : IQueryHandler<GetOrdersByEmailQuery, List<OrderDto>>
    {
        public Task<List<OrderDto>> Handle(
            GetOrdersByEmailQuery request,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }
    }
}