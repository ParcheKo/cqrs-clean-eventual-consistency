using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Orders.Application.Orders.GetOrdersByEmail;
using SampleProject.Application.Configuration.Queries;
using SampleProject.Application.Customers;

namespace SampleProject.Application.Orders.GetCustomerOrders
{
    internal sealed class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, List<OrderViewModel>>
    {
        public Task<List<OrderViewModel>> Handle(
            GetOrdersQuery request,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }
    }
}