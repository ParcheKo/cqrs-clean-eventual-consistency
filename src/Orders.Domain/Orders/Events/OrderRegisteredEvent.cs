using System;
using SampleProject.Domain.SeedWork;
using SampleProject.Domain.SharedKernel;

namespace SampleProject.Domain.Customers.Orders.Events
{
    public class OrderRegisteredEvent : DomainEventBase
    {
        public OrderId OrderId { get; }
        public string Email { get; }

        // todo: put [JsonConstructor] if any problem
        public OrderRegisteredEvent(
            OrderId orderId,
            string email
        )
        {
            this.OrderId = orderId;
            Email = email;
        }
    }
}