using System;
using SampleProject.Application.Configuration.Commands;
using SampleProject.Application.Customers;

namespace SampleProject.Application.Orders.PlaceCustomerOrder;

public class RegisterOrderCommand : CommandBase<OrderDto>
{
    public DateTime OrderDate { get; }
    public string CreatedBy { get; }
    public string OrderNo { get; }
    public string ProductName { get; }
    public int Total { get; }
    public decimal Price { get; }

    public RegisterOrderCommand(
        DateTime orderDate,
        string createdBy,
        string orderNo,
        string productName,
        int total,
        decimal price
    ) 
    {
        OrderDate = orderDate;
        CreatedBy = createdBy;
        OrderNo = orderNo;
        ProductName = productName;
        Total = total;
        Price = price;
    }
}