using System;
using System.Threading.Tasks;
using CqrsEssentials;
using Orders.Core.Cards;
using Orders.Core.Shared;
using Orders.Query.QueryModel;

namespace Orders.Query.EventHandlers;

public class SampleEventHandler : IAsyncEventHandler<CardCreatedEvent>
{
    public async Task HandleAsync(CardCreatedEvent e)
    {
       
    }
}