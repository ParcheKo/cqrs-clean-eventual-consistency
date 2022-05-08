using Orders.Core.Transactions;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using Orders.Core.Shared;
using Orders.Query.Materializers;
using Orders.Query.QueryModel;

namespace Orders.Query.EventHandlers
{
    public class TransactionCreatedEventHandler : IEventHandler<Core.Transactions.TransactionCreatedEvent>
    {
        private readonly ReadDbContext _readDbContext;
        private readonly ICache _cache;
        private readonly ITransactionListQueryModelMaterializer _transactionMaterializer;
        private readonly ICardListQueryModelMaterializer _cardListMaterializer;

        public TransactionCreatedEventHandler(ReadDbContext readDbContext, ICache cache, 
            ITransactionListQueryModelMaterializer transactionMaterializer, 
            ICardListQueryModelMaterializer cardListMaterializer)
        {
            this._readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
            this._cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this._transactionMaterializer = transactionMaterializer ?? throw new ArgumentNullException(nameof(transactionMaterializer));
            this._cardListMaterializer = cardListMaterializer ?? throw new ArgumentNullException(nameof(cardListMaterializer));
        }

        public async Task Handle(Core.Transactions.TransactionCreatedEvent e)
        {
            FilterDefinition<CardListQueryModel> filter = Builders<CardListQueryModel>.Filter.Eq("Id", e.Data.CardId);
            var cardList = await _readDbContext.CardListMaterializedView
                .Find(filter)
                .FirstOrDefaultAsync();

            var transactionList = _transactionMaterializer.Materialize(e.Data, cardList);
            cardList = _cardListMaterializer.Materialize(e.Data, cardList);

            await _cache.Delete(nameof(CardListQueryModel));
            await _readDbContext.TransactionListMaterializedView.InsertOneAsync(transactionList);
            await _readDbContext.CardListMaterializedView.ReplaceOneAsync(filter, cardList, new UpdateOptions { IsUpsert = true });
        }
    }
}