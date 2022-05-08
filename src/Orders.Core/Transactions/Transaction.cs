using System;
using System.Collections.Generic;
using Orders.Core.Shared;

namespace Orders.Core.Transactions;

public class Transaction : IAggregateRoot
{
    protected Transaction()
    {
    }

    private Transaction(
        Guid cardGuid,
        string uniqueId,
        DateTimeOffset chargeDate,
        Money charge
    )
    {
        Id = Guid.NewGuid();
        CardId = cardGuid;
        UniqueId = uniqueId ?? throw new ArgumentNullException(nameof(uniqueId));
        ChargeDate = chargeDate;
        Charge = charge ?? throw new ArgumentNullException(nameof(charge));
    }

    public Guid CardId { get; }

    public Money Charge { get; }

    public DateTimeOffset ChargeDate { get; }

    public Guid Id { get; }

    public string UniqueId { get; }

    public bool Valid { get; private set; }

    public void Validate(ValidationNotificationHandler notificationHandler)
    {
        throw new NotImplementedException();
    }

    public static Transaction CreateTransactionForCard(
        Guid cardGuid,
        string uniqueId,
        DateTimeOffset chargeDate,
        Money charge
    )
    {
        return new Transaction(
            cardGuid,
            uniqueId,
            chargeDate,
            charge
        );
    }

    public override bool Equals(object obj)
    {
        var transaction = obj as Transaction;
        return transaction != null &&
               UniqueId == transaction.UniqueId;
    }

    public override int GetHashCode()
    {
        return -401120461 + EqualityComparer<string>.Default.GetHashCode(UniqueId);
    }
}