using Orders.Core.Transactions;
using Orders.Query.Abstractions;
using Orders.Query.QueryModel;

namespace Orders.Query.Materializers
{
    public interface ICardListQueryModelMaterializer : IMaterializer<CardListQueryModel, Transaction, CardListQueryModel>
    {

    }

    public class CardListQueryModelMaterializer : ICardListQueryModelMaterializer
    {
        public CardListQueryModel Materialize(Transaction source1, CardListQueryModel source2)
        {
            if (!source2.HighestTransactionAmount.HasValue || source1.Charge.Amount > source2.HighestTransactionAmount)
            {
                source2.HighestChargeDate = source1.ChargeDate;
                source2.HighestTransactionId = source1.Id;
                source2.HighestTransactionAmount = source1.Charge.Amount;
            }

            source2.TransactionCount += 1;

            return source2;
        }
    }
}
