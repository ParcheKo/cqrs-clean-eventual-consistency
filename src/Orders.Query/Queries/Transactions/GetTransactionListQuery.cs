using System;
using System.Collections.Generic;
using Orders.Query.Abstractions;
using Orders.Query.QueryModel;

namespace Orders.Query.Queries.Transactions
{
    public class GetTransactionListQuery : IQuery<IEnumerable<TransactionListQueryModel>>
    {
        public decimal? BetweenAmount { get; set; }
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public DateTimeOffset? ChargeDate { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
