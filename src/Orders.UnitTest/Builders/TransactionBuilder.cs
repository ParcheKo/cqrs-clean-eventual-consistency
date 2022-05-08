using System;
using Orders.Core;
using Orders.Core.Shared;
using Orders.Core.Transactions;

namespace Orders.UnitTest.Builders
{
    public class TransactionBuilder : IBuilder<Transaction>
    {
        private Guid _cardId;

        private Money _charge;

        private DateTimeOffset _chargeDate;

        private string _uniqueId;

        public TransactionBuilder ForCard(Guid id)
        {
            _cardId = id;

            return this;
        }

        public TransactionBuilder ContainingChargeAmount(Money charge)
        {
            _charge = charge;

            return this;
        }

        public TransactionBuilder ChargedAt(DateTimeOffset date)
        {
            _chargeDate = date;

            return this;
        }

        public TransactionBuilder HavingUniqueId(string uniqueId)
        {
            _uniqueId = uniqueId;

            return this;
        }

        public Transaction Build()
        {
            return Transaction.CreateTransactionForCard(_cardId, _uniqueId, _chargeDate, _charge);
        }
    }
}
