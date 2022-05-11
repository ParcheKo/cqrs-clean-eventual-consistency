using System;
using Newtonsoft.Json;
using Orders.Domain.SeedWork;

namespace Orders.Application.Configuration.DomainEvents;

public class DomainNotificationBase<T> : IDomainEventNotification<T> where T : IDomainEvent
{
    public DomainNotificationBase(T domainEvent)
    {
        Id = Guid.NewGuid();
        DomainEvent = domainEvent;
    }

    [JsonIgnore] public T DomainEvent { get; }

    public Guid Id { get; }
}