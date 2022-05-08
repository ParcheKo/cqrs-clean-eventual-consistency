using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Orders.Core.Shared;
using Orders.Core.Transactions;
using Orders.Query.Materializers;
using Orders.Query.QueryModel;

namespace Orders.Query.EventHandlers;

public class TransactionCreatedEventHandler : IEventHandler<TransactionCreatedEvent>
{
    private readonly ICache _cache;
    private readonly ICardListQueryModelMaterializer _cardListMaterializer;
    private readonly ReadDbContext _readDbContext;
    private readonly ITransactionListQueryModelMaterializer _transactionMaterializer;

    public TransactionCreatedEventHandler(
        ReadDbContext readDbContext,
        ICache cache,
        ITransactionListQueryModelMaterializer transactionMaterializer,
        ICardListQueryModelMaterializer cardListMaterializer
    )
    {
        _readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _transactionMaterializer = transactionMaterializer ??
                                   throw new ArgumentNullException(nameof(transactionMaterializer));
        _cardListMaterializer =
            cardListMaterializer ?? throw new ArgumentNullException(nameof(cardListMaterializer));
    }

    public async Task Handle(TransactionCreatedEvent e)
    {
        var cardList = await _readDbContext.CardListMaterializedView
            .Find(p => p.Id == e.Data.CardId)
            .FirstOrDefaultAsync();

        var transactionList = _transactionMaterializer.Materialize(
            e.Data,
            cardList
        );
        cardList = _cardListMaterializer.Materialize(
            e.Data,
            cardList
        );

        await _cache.Delete(nameof(CardListQueryModel));
        await _readDbContext.TransactionListMaterializedView.InsertOneAsync(transactionList);
        await _readDbContext.CardListMaterializedView.ReplaceOneAsync(
            p => p.Id == e.Data.CardId,
            cardList,
            new ReplaceOptions { IsUpsert = true }
        );
    }
}