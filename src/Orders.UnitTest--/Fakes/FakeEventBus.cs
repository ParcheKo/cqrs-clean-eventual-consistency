using CqrsEssentials;
using Orders.Core.Shared;

namespace Orders.UnitTest.Fakes;

internal class FakeEventBus : IEventBus
{
    public void Publish(IEvent @event)
    {
    }

    public void Subscribe<T>() where T : IEvent
    {
    }
}