using System;
using Orders.Domain.SeedWork;

namespace Orders.Domain.Orders;

public class OrderId : TypedIdValueBase
{
    public OrderId(Guid value) : base(value)
    {
    }
}