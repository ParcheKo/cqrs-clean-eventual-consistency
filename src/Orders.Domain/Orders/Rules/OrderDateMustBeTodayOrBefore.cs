using System;
using SampleProject.Domain.SeedWork;

namespace SampleProject.Domain.Customers.Orders.Rules;

public class OrderDateMustBeTodayOrBefore : IBusinessRule
{
    private readonly DateTime _orderDate;

    public OrderDateMustBeTodayOrBefore(DateTime orderDate)
    {
        _orderDate = orderDate;
    }

    public bool IsBroken() => _orderDate.Date > DateTime.Today;

    public string Message => "Order date can not be after today.";
}