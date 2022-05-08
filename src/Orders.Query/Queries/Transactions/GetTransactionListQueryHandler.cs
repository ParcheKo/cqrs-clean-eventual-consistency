﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Orders.Query.Abstractions;
using Orders.Query.QueryModel;

namespace Orders.Query.Queries.Transactions
{
    public class GetTransactionListQueryHandler : IQueryHandler<GetTransactionListQuery, IEnumerable<TransactionListQueryModel>>
    {
        private readonly ReadDbContext _readDbContext;

        public GetTransactionListQueryHandler(ReadDbContext readDbContext)
        {
            this._readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
        }

        public async Task<IEnumerable<TransactionListQueryModel>> HandleAsync(GetTransactionListQuery query)
        {
            try
            {
                var result = _readDbContext
                .TransactionListMaterializedView
                .AsQueryable()
                .WhereIf(!string.IsNullOrEmpty(query.CardNumber), x => x.CardNumber == query.CardNumber)
                .WhereIf(!string.IsNullOrEmpty(query.CardHolder), x => x.CardHolder.Contains(query.CardNumber))
                .WhereIf(query.ChargeDate.HasValue, x => x.ChargeDate == query.ChargeDate)
                .WhereIf(query.BetweenAmount.HasValue, x => x.Amount >= query.BetweenAmount && x.Amount <= query.BetweenAmount);

                var itemsTask = await result
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .ToListAsync();

                return itemsTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
