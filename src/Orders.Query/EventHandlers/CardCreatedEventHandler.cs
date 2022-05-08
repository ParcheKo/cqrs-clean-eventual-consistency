﻿using System;
using System.Threading.Tasks;
using Orders.Core.Cards;
using Orders.Core.Shared;
using Orders.Query.QueryModel;

namespace Orders.Query.EventHandlers
{
    public class MaterializeCardEventHandler : IEventHandler<CardCreatedEvent>
    {
        private readonly ReadDbContext readDbContext;
        private readonly ICache cache;

        public MaterializeCardEventHandler(ReadDbContext readDbContext, ICache cache)
        {
            this.readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task Handle(CardCreatedEvent e)
        {
            var cardView = new CardViewQueryModel()
            {
                CardHolder = e.Data.CardHolder,
                ExpirationDate = e.Data.ExpirationDate,
                Id = e.Data.Id,
                Number = e.Data.Number
            };

            var cardList = new CardListQueryModel()
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

            await readDbContext.CardViewMaterializedView.InsertOneAsync(cardView);
            await readDbContext.CardListMaterializedView.InsertOneAsync(cardList);
        }
    }
}
