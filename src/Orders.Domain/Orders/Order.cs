using System;
using Orders.Domain.Orders.Events;
using Orders.Domain.Orders.Rules;
using Orders.Domain.SeedWork;
using Orders.Domain.SharedKernel.Email;

namespace Orders.Domain.Orders;

public class Order : Entity, IAggregateRoot
{
    private Order()
    {
    }

    private Order(
        DateTime orderDate,
        Email createdBy,
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
                CreatedBy.Value,
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
    public Email CreatedBy { get; private set; }
    public string OrderNo { get; private set; }
    public string ProductName { get; private set; }
    public int Total { get; private set; }
    public decimal Price { get; private set; } // convert to value-object

    private decimal _totalPrice;

    public decimal TotalPrice
    {
        get => Total * Price;
        set => _totalPrice = value;
    }

    public static Order From(
        DateTime orderDate,
        Email createdBy,
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