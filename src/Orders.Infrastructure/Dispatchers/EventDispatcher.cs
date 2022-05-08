using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Orders.Core.Shared;

namespace Orders.Infrastructure.Dispatchers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IComponentContext _componentContext;

        public EventDispatcher(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public async Task Dispatch<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
            {
                throw new System.ArgumentNullException(nameof(@event));
            }

            var eventType = @event.GetType();
            var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            dynamic handlers = _componentContext.Resolve(
                typeof(IEnumerable<>).MakeGenericType(eventHandlerType));

            var tasks = new List<Task>(handlers.Length);

            foreach (var handler in handlers)
            {
                tasks.Add(handler.Handle((dynamic)@event));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}