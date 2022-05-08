using Orders.Core.Shared;

namespace Orders.Core.Transactions;

public class TransactionCreatedEvent : Event
{
    public TransactionCreatedEvent(Transaction transaction)
    {
        Data = transaction;
        Name = nameof(TransactionCreatedEvent);
    }

    public Transaction Data { get; set; }
}