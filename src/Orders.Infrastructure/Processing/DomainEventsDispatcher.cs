using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using MediatR;
using Newtonsoft.Json;
using Orders.Application.Configuration.DomainEvents;
using Orders.Domain.SeedWork;
using Orders.Infrastructure.Processing.Outbox;
using Orders.Infrastructure.WriteDatabase;

namespace Orders.Infrastructure.Processing;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IMediator _mediator;
    private readonly OrdersContext _ordersContext;
    private readonly ILifetimeScope _scope;

    public DomainEventsDispatcher(
        IMediator mediator,
        ILifetimeScope scope,
        OrdersContext ordersContext
    )
    {
        _mediator = mediator;
        _scope = scope;
        _ordersContext = ordersContext;
    }

    public async Task DispatchEventsAsync()
    {
        var domainEntities = _ordersContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();
        foreach (var domainEvent in domainEvents)
        {
            var domainEvenNotificationType = typeof(IDomainEventNotification<>);
            var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());
            var domainNotification = _scope.ResolveOptional(
                domainNotificationWithGenericType,
                new List<Parameter>
                {
                    new NamedParameter(
                        "domainEvent",
                        domainEvent
                    )
                }
            );

            if (domainNotification != null)
                domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
        }

        domainEntities
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        var tasks = domainEvents
            .Select(async domainEvent => { await _mediator.Publish(domainEvent); });

        await Task.WhenAll(tasks);

        foreach (var domainEventNotification in domainEventNotifications)
        {
            var type = domainEventNotification.GetType().FullName;
            var data = JsonConvert.SerializeObject(domainEventNotification);
            var outboxMessage = new OutboxMessage(
                domainEventNotification.DomainEvent.OccurredOn,
                type,
                data
            );
            _ordersContext.OutboxMessages.Add(outboxMessage);
        }
    }
}