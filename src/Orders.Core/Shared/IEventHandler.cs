using System.Threading.Tasks;

namespace Orders.Core.Shared;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    Task Handle(TEvent e);
}