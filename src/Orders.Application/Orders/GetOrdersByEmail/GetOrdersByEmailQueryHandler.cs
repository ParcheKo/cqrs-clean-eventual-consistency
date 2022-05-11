using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Orders.Application.Configuration.Queries;

namespace Orders.Application.Orders.GetOrdersByEmail;

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