using System;
using Orders.Domain.Orders.Events;
using Orders.Domain.Orders.Rules;
using Orders.Domain.SeedWork;

namespace Orders.Domain.Orders;

public class Order : Entity, IAggregateRoot
{
    private decimal _totalPrice;

    private Order()
    {
    }

    private Order(
        DateTime orderDate,
        string personEmail,
        string orderNo,
        string productName,
        int total,
        decimal price
    )
    {
        Id = new OrderId(Guid.NewGuid());
        OrderDate = orderDate;
        CreatedBy = personEmail;
        OrderNo = orderNo;
        ProductName = productName;
        Total = total;
        Price = price;

        AddDomainEvent(
            new OrderRegisteredEvent(
                new OrderId(Guid.NewGuid()),
                personEmail
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

    public decimal TotalPrice
    {
        get => Total * Price;
        set => _totalPrice = value;
    }

    public static Order From(
        DateTime orderDate,
        string personEmail,
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
            personEmail,
            orderNo,
            productName,
            total,
            price
        );
    }
}