using System;
using Orders.Domain.Orders.Events;
using Orders.Domain.Orders.Rules;
using Orders.Domain.SeedWork;

namespace Orders.Domain.Orders;

public class Order : Entity, IAggregateRoot
{
    private Order()
    {
    }

    private Order(
        DateTime orderDate,
        string createdBy,
        string orderNo,
        string productName,
        int total,
        decimal price
    )
    {
        Id = new OrderId(Guid.NewGuid());
        OrderDate = orderDate;
        CreatedBy = createdBy;
        OrderNo = orderNo;
        ProductName = productName;
        Total = total;
        Price = price;

        AddDomainEvent(
            new OrderRegisteredEvent(
                new OrderId(Guid.NewGuid()),
                CreatedBy,
                OrderDate,
                OrderNo,
                ProductName,
                Total,
                Price,
                TotalPrice
            )
        );
    }

    public OrderId Id { get; private set; }
    public DateTime OrderDate { get; private set; }
    public string CreatedBy { get; private set; } // convert to value-object
    public string OrderNo { get; private set; }
    public string ProductName { get; private set; }
    public int Total { get; private set; }
    public decimal Price { get; private set; } // convert to value-object

    public decimal TotalPrice => Total * Price;
    // {
    //     get => Total * Price;
    //     set => _totalPrice = value;
    // }

    public static Order From(
        DateTime orderDate,
        string createdBy,
        string orderNo,
        string productName,
        int total,
        decimal price,
        bool orderNoIsUnique
    )
    {
        CheckRule(new OrderDateMustBeTodayOrBefore(orderDate));
        CheckRule(new OrderNoMustBeUnique(orderNoIsUnique));

        return new Order(
            orderDate,
            createdBy,
            orderNo,
            productName,
            total,
            price
        );
    }
}