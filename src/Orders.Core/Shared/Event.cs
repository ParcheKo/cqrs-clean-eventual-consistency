using System;

namespace Orders.Core.Shared;

public class Event : IEvent
{
    public Event()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.Now;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime OccurredOn { get; set; }
}