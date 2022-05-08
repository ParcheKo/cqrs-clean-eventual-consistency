namespace Orders.Core.Shared
{
    public interface IEventBus
    {
        void Publish(IEvent @event);

        void Subscribe<T>() where T : IEvent;
    }
}