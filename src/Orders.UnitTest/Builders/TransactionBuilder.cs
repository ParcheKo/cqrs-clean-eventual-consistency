using System;
using Orders.Core;
using Orders.Core.Shared;
using Orders.Core.Transactions;

namespace Orders.UnitTest.Builders
{
    public class TransactionBuilder : IBuilder<Transaction>
    {
        private Guid cardId;

        private Money charge;

        private DateTimeOffset chargeDate;

        private string uniqueId;

        public TransactionBuilder ForCard(Guid id)
        {
            cardId = id;

            return this;
        }

        public TransactionBuilder ContainingChargeAmount(Money charge)
        {
            this.charge = charge;

            return this;
        }

        public TransactionBuilder ChargedAt(DateTimeOffset date)
        {
            this.chargeDate = date;

            return this;
        }

        public TransactionBuilder HavingUniqueId(string uniqueId)
        {
            this.uniqueId = uniqueId;

            return this;
        }

        public Transaction Build()
        {
            return Transaction.CreateTransactionForCard(cardId, uniqueId, chargeDate, charge);
        }
    }
}
