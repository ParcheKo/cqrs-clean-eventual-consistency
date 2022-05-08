using System;
using System.Collections.Generic;
using System.Linq;
using Orders.Core.Shared;

namespace Orders.Core.Cards;

public class Card : IAggregateRoot
{
    protected Card()
    {
    }

    private Card(
        string number,
        string cardHolder,
        DateTime expirationDate
    )
    {
        Id = Guid.NewGuid();
        Number = number;
        CardHolder = cardHolder;
        ExpirationDate = expirationDate;
    }

    public string CardHolder { get; }

    public DateTime ExpirationDate { get; }

    public Guid Id { get; }

    public string Number { get; }

    public bool Valid { get; private set; }

    public void Validate(ValidationNotificationHandler notificationHandler)
    {
        var validator = new CardValidator(notificationHandler);

        validator.Validate(this);

        Valid = !notificationHandler.Notifications.Any();
    }

    public static Card CreateNewCard(
        string number,
        string cardHolder,
        DateTime expirationDate
    )
    {
        return new Card(
            number,
            cardHolder,
            expirationDate
        );
    }

    public override bool Equals(object obj)
    {
        var card = obj as Card;
        return card != null &&
               Number == card.Number;
    }

    public override int GetHashCode()
    {
        var hashCode = 1924120557;
        hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Number);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CardHolder);
        hashCode = hashCode * -1521134295 + ExpirationDate.GetHashCode();
        return hashCode;
    }
}