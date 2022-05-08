using System;
using System.Threading.Tasks;
using Orders.Core.Cards;
using Orders.Core.Shared;
using Orders.Query.QueryModel;

namespace Orders.Query.EventHandlers;

public class SampleEventHandler : IEventHandler<CardCreatedEvent>
{
    public async Task Handle(CardCreatedEvent e)
    {
       
    }
}