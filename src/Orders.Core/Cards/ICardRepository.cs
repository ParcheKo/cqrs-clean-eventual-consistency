using Orders.Core.Shared;

namespace Orders.Core.Cards;

public interface ICardRepository : IRepository<Card>
{
    bool IsDuplicatedCardNumber(string cardNumber);
}