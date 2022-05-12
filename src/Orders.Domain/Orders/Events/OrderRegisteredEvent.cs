using System;
using Orders.Domain.SeedWork;

namespace Orders.Domain.Orders.Events;

public class OrderRegisteredEvent : DomainEventBase
{
    // todo: put [JsonConstructor] if any problem
    public OrderRegisteredEvent(
        OrderId orderId,
        string createdBy,
        DateTime orderDate,
        string orderNo,
        string productName,
        int total,
        decimal price,
        decimal totalPrice
    )
    {
        OrderId = orderId;
        CreatedBy = createdBy;
        OrderDate = orderDate;
        OrderNo = orderNo;
        ProductName = productName;
        Total = total;
        Price = price;
        TotalPrice = totalPrice;
    }

    public OrderId OrderId { get; }
    public string CreatedBy { get; }

    public DateTime OrderDate { get; }
    public string OrderNo { get; }
    public string ProductName { get; }
    public int Total { get; }
    public decimal Price { get; }
    public decimal TotalPrice { get; }
}