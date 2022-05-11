using System;
using System.Threading.Tasks;
using CqrsEssentials;
using Orders.Core.Cards;
using Orders.Core.Shared;
using Orders.Query.QueryModel;
using SampleProject.Domain.Customers.Orders.Events;

namespace Orders.Query.EventHandlers;

public class MaterializeCardEventHandler : IAsyncEventHandler<OrderRegisteredEvent>
{
    private readonly ICache _cache;
    private readonly ReadDbContext _readDbContext;

    public MaterializeCardEventHandler(
        ReadDbContext readDbContext,
        ICache cache
    )
    {
        _readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task HandleAsync(CardCreatedEvent e)
    {
        var cardView = new CardViewQueryModel
        {
            CardHolder = e.Data.CardHolder,
            ExpirationDate = e.Data.ExpirationDate,
            Id = e.Data.Id,
            Number = e.Data.Number
        };

        var cardList = new CardListQueryModel
        {
            Id = e.Data.Id,
            Number = e.Data.Number,
            CardHolder = e.Data.CardHolder,
            ExpirationDate = e.Data.ExpirationDate,
            HighestChargeDate = null,
            HighestTransactionAmount = null,
            HighestTransactionId = null,
            TransactionCount = 0
        };

        await _readDbContext.CardViewMaterializedView.InsertOneAsync(cardView);
        await _readDbContext.CardListMaterializedView.InsertOneAsync(cardList);
    }
}