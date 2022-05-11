using System;
using Orders.Core.Shared;
using Orders.Core.Transactions;
using Orders.UnitTest.Builders;

namespace Orders.UnitTest.Mothers;

public static class TransactionMother
{
    public static Transaction CreateSimpleTransaction()
    {
        return new TransactionBuilder()
            .ForCard(Guid.NewGuid())
            .HavingUniqueId(Guid.NewGuid().ToString("N"))
            .ChargedAt(DateTimeOffset.Now)
            .ContainingChargeAmount(
                new Money(
                    100,
                    "EUR"
                )
            )
            .Build();
    }
}