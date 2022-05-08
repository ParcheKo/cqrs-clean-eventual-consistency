using Orders.Core.Transactions;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using Orders.Core.Shared;
using Orders.Query.Materializers;
using Orders.Query.QueryModel;

namespace Orders.Query.EventHandlers
{
    public class TransactionCreatedEventHandler : IEventHandler<TransactionCreatedEvent>
    {
        private readonly ReadDbContext _readDbContext;
        private readonly ICache _cache;
        private readonly ITransactionListQueryModelMaterializer _transactionMaterializer;
        private readonly ICardListQueryModelMaterializer _cardListMaterializer;

        public TransactionCreatedEventHandler(ReadDbContext readDbContext, ICache cache, 
            ITransactionListQueryModelMaterializer transactionMaterializer, 
            ICardListQueryModelMaterializer cardListMaterializer)
        {
            _readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _transactionMaterializer = transactionMaterializer ?? throw new ArgumentNullException(nameof(transactionMaterializer));
            _cardListMaterializer = cardListMaterializer ?? throw new ArgumentNullException(nameof(cardListMaterializer));
        }

        public async Task Handle(TransactionCreatedEvent e)
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