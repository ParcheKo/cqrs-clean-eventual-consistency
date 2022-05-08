using Orders.Core.Shared;

namespace Orders.Core.Transactions;

public interface ITransactionWriteOnlyRepository : IWriteOnlyRepository<Transaction>
{
}