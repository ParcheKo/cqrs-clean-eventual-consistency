using System;
using System.Linq;
using System.Threading.Tasks;
using Orders.Core.Transactions;

namespace Orders.Infrastructure.Persistence.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly WriteDbContext _writeDbContext;

    public TransactionRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext ?? throw new ArgumentNullException(nameof(writeDbContext));
    }

    public async Task<bool> Add(Transaction entity)
    {
        _writeDbContext.Set<Transaction>().Add(entity);
        return await _writeDbContext.SaveChangesAsync() > 0;
    }

    public async Task<Transaction> GetBy(Guid id)
    {
        return await _writeDbContext.Set<Transaction>().FindAsync(id);
    }
}