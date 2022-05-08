﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Orders.Core.Shared;
using Orders.Query.Abstractions;
using Orders.Query.QueryModel;

namespace Orders.Query.Queries.Cards
{
    public class GetCardListQueryHandler : IQueryHandler<GetCardListQuery, IEnumerable<CardListQueryModel>>
    {
        private readonly ReadDbContext readDbContext;
        private readonly ICache cache;

        public GetCardListQueryHandler(ReadDbContext readDbContext, ICache cache)
        {
            this.readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<CardListQueryModel>> HandleAsync(GetCardListQuery query)
        {
            try
            {
                var cached = await cache.Get<IEnumerable<CardListQueryModel>>(nameof(CardListQueryModel));

                if (cached != null && cached.Any())
                {
                    return cached;
                }

                var result = readDbContext
                   .CardListMaterializedView
                   .AsQueryable()
                   .WhereIf(!string.IsNullOrEmpty(query.Number), x => x.Number.Contains(query.Number))
                   .WhereIf(!string.IsNullOrEmpty(query.CardHolder), x => x.CardHolder.Contains(query.CardHolder))
                   .WhereIf(query.ChargeDate.HasValue, x => x.ExpirationDate == query.ChargeDate);

                var itemsTask = await result
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .ToListAsync();

                await cache.Store<IEnumerable<CardListQueryModel>>(nameof(GetCardListQuery), itemsTask, null);

                return itemsTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
