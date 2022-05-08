using System;
using System.Linq;
using System.Threading.Tasks;
using Orders.Core.Cards;

namespace Orders.Infrastructure.Persistence.Repository;

public class CardRepository : ICardRepository
{
    private readonly WriteDbContext _writeDbContext;

    public CardRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext ?? throw new ArgumentNullException(nameof(writeDbContext));
    }

    public async Task<bool> Add(Card entity)
    {
        _writeDbContext.Set<Card>().Add(entity);
        return await _writeDbContext.SaveChangesAsync() > 0;
    }

    public async Task<Card> GetBy(Guid id)
    {
        return await _writeDbContext.Set<Card>().FindAsync(id);
    }

    public bool IsDuplicatedCardNumber(string cardNumber)
    {
        return _writeDbContext.Set<Card>().Any(x => x.Number == cardNumber);
    }
}