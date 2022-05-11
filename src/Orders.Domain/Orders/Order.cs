using System;
using SampleProject.Domain.Customers.Orders.Events;
using SampleProject.Domain.Customers.Orders.Rules;
using SampleProject.Domain.Customers.Rules;
using SampleProject.Domain.SeedWork;

namespace SampleProject.Domain.Customers.Orders
{
    public class Order : Entity, IAggregateRoot
    {
        public OrderId Id { get; private set; }
        public DateTime OrderDate { get; private set; }
        public string CreatedBy { get; private set; } // convert to value-object
        public string OrderNo { get; private set; }
        public string ProductName { get; private set; }
        public int Total { get; private set; }
        public decimal Price { get; private set; } // convert to value-object
        public decimal TotalPrice => Total * Price; // todo : does it persist to db using ef? 

        protected Order()
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
}