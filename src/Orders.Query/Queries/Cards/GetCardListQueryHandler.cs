using System;
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
        private readonly ReadDbContext _readDbContext;
        private readonly ICache _cache;

        public GetCardListQueryHandler(ReadDbContext readDbContext, ICache cache)
        {
            _readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<CardListQueryModel>> HandleAsync(GetCardListQuery query)
        {
            try
            {
                var cached = await _cache.Get<IEnumerable<CardListQueryModel>>(nameof(CardListQueryModel));

                if (cached != null && cached.Any())
                {
                    return cached;
                }

                var result = _readDbContext
                   .CardListMaterializedView
                   .AsQueryable()
                   .WhereIf(!string.IsNullOrEmpty(query.Number), x => x.Number.Contains(query.Number))
                   .WhereIf(!string.IsNullOrEmpty(query.CardHolder), x => x.CardHolder.Contains(query.CardHolder))
                   .WhereIf(query.ChargeDate.HasValue, x => x.ExpirationDate == query.ChargeDate);

                var itemsTask = await result
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .ToListAsync();

                await _cache.Store<IEnumerable<CardListQueryModel>>(nameof(GetCardListQuery), itemsTask, null);

                return itemsTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
