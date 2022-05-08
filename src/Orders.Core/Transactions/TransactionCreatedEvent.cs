using Orders.Core.Shared;

namespace Orders.Core.Transactions
{
    public class TransactionCreatedEvent : Event
    {
        public Transaction Data { get; set; }
    
        public TransactionCreatedEvent(Transaction transaction)
        {
            Data = transaction;
            Name = (nameof(TransactionCreatedEvent));
        }
    }
}