using System.Collections.Generic;
using SampleProject.Application.Configuration.Queries;

namespace SampleProject.Application.Customers;

public class GetOrdersByEmailQuery : IQuery<List< OrderDto>>
{
    public GetOrdersByEmailQuery(string email)
    {
        Email = email;
    }

    public string Email { get; }
}