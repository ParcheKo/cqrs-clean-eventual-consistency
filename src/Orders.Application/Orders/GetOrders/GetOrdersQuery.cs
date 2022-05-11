using System.Collections.Generic;
using Orders.Application.Configuration.Queries;
using Orders.Application.Orders.GetOrdersByEmail;

namespace Orders.Application.Orders.GetOrders;

public class GetOrdersQuery : IQuery<List<OrderViewModel>>
{
}