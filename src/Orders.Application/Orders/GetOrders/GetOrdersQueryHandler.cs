using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Orders.Application.Configuration.Queries;
using Orders.Application.Orders.GetOrdersByEmail;

namespace Orders.Application.Orders.GetOrders;

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