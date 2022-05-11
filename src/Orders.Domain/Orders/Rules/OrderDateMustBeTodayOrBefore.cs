using System;
using Orders.Domain.SeedWork;

namespace Orders.Domain.Orders.Rules;

public class OrderDateMustBeTodayOrBefore : IBusinessRule
{
    private readonly DateTime _orderDate;

    public OrderDateMustBeTodayOrBefore(DateTime orderDate)
    {
        _orderDate = orderDate;
    }

    public bool IsBroken()
    {
        return _orderDate.Date > DateTime.Today;
    }

    public string Message => "Order date can not be after today.";
}