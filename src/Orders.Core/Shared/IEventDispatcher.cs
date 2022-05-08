using System.Threading.Tasks;

namespace Orders.Core.Shared
{
    public interface IEventDispatcher
    {
        Task Dispatch<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}