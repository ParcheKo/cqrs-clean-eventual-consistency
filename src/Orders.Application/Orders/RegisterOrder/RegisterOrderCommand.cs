using System;
using Orders.Application.Configuration.Commands;

namespace Orders.Application.Orders.RegisterOrder;

public class RegisterOrderCommand : CommandBase<OrderDto>
{
    public RegisterOrderCommand(
        DateTime orderDate,
        string personEmail,
        string orderNo,
        string productName,
        int total,
        decimal price
    )
    {
        OrderDate = orderDate;
        PersonEmail = personEmail;
        OrderNo = orderNo;
        ProductName = productName;
        Total = total;
        Price = price;
    }

    public DateTime OrderDate { get; }
    public string PersonEmail { get; }
    public string OrderNo { get; }
    public string ProductName { get; }
    public int Total { get; }
    public decimal Price { get; }
}