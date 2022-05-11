using Orders.Domain.SeedWork;

namespace Orders.Domain.Orders.Events;

public class OrderRegisteredEvent : DomainEventBase
{
    // todo: put [JsonConstructor] if any problem
    public OrderRegisteredEvent(
        OrderId orderId,
        string email
    )
    {
        OrderId = orderId;
        Email = email;
    }

    public OrderId OrderId { get; }
    public string Email { get; }
}