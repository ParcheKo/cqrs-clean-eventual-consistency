﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Orders.Core.Transactions;

namespace Orders.Infrastructure.Persistence.Repository
{
    public class TransactionWriteOnlyRepository : ITransactionWriteOnlyRepository
    {
        private readonly WriteDbContext _writeDbContext;

        public TransactionWriteOnlyRepository(WriteDbContext writeDbContext)
        {
            this._writeDbContext = writeDbContext ?? throw new ArgumentNullException(nameof(writeDbContext));
        }

        public async Task<bool> Add(Transaction entity)
        {
            _writeDbContext.Set<Transaction>().Add(entity);
            return await _writeDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Transaction entity)
        {
            _writeDbContext.Set<Transaction>().Remove(entity);
            return await _writeDbContext.SaveChangesAsync() > 0;
        }

        public IQueryable<Transaction> FindAll()
        {
            return _writeDbContext.Set<Transaction>();
        }

        public async Task<Transaction> FindAsync(Guid id)
        {
            return await _writeDbContext.Set<Transaction>().FindAsync(id);
        }

        public async Task<bool> Update(Transaction entity)
        {
            _writeDbContext.Set<Transaction>().Update(entity);
            return await _writeDbContext.SaveChangesAsync() > 0;
        }
    }
}