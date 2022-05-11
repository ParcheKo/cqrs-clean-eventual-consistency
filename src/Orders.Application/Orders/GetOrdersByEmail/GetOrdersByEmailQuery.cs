using System.Collections.Generic;
using Orders.Application.Configuration.Queries;

namespace Orders.Application.Orders.GetOrdersByEmail;

public class GetOrdersByEmailQuery : IQuery<List<OrderDto>>
{
    public GetOrdersByEmailQuery(string email)
    {
        Email = email;
    }

    public string Email { get; }
}