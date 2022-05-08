using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Orders.Core.Shared;

namespace Orders.Infrastructure.Dispatchers;

public class EventDispatcher : IEventDispatcher
{
    private readonly IComponentContext _componentContext;
    private readonly ILifetimeScope _lifetimeScope;

    public EventDispatcher(IComponentContext componentContext,
        ILifetimeScope lifetimeScope
    )
    {
        _componentContext = componentContext;
        _lifetimeScope = lifetimeScope;
    }

    public async Task Dispatch<TEvent>(TEvent @event) where TEvent : IEvent
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event));
        
        // todo: why doesnt it work ?
        // await using var scope = _lifetimeScope.BeginLifetimeScope();
        // var asyncHandlers = scope.Resolve<IEnumerable<IEventHandler<TEvent>>>();
        // var asyncTasks = new List<Task>(/*asyncHandlers.Count*/);
        // foreach (var asyncHandler in asyncHandlers)
        // {
        //     asyncTasks.Add(asyncHandler.Handle(@event));
        // }
        // await Task.WhenAll(asyncTasks).ConfigureAwait(false);

        var eventType = @event.GetType();
        var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        dynamic handlers = _componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(eventHandlerType));
        
        var tasks = new List<Task>(handlers.Length);
        foreach (var handler in handlers) tasks.Add(handler.Handle((dynamic)@event));
        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}