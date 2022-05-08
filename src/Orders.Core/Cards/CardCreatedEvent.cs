using Orders.Core.Shared;

namespace Orders.Core.Cards
{
    public class CardCreatedEvent : Event
    {
        public Card Data { get; set; }
        public CardCreatedEvent(Card card)
        {
            Data = card;
            Name = (nameof(CardCreatedEvent));
        }
    }
}