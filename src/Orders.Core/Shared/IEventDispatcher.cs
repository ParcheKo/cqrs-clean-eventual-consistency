using System.Threading.Tasks;

namespace Orders.Core.Shared
{
    public interface IEventDispatcher
    {
        Task Dispatch<TEvent>(TEvent e) where TEvent : IEvent;
    }
}