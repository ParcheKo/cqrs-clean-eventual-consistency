using System;
using System.Linq;
using System.Threading.Tasks;
using Orders.Core.Cards;

namespace Orders.Infrastructure.Persistence.Repository;

public class CardWriteOnlyRepository : ICardWriteOnlyRepository
{
    private readonly WriteDbContext _writeDbContext;

    public CardWriteOnlyRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext ?? throw new ArgumentNullException(nameof(writeDbContext));
    }

    public async Task<bool> Add(Card entity)
    {
        _writeDbContext.Set<Card>().Add(entity);
        return await _writeDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(Card entity)
    {
        _writeDbContext.Set<Card>().Remove(entity);
        return await _writeDbContext.SaveChangesAsync() > 0;
    }

    public async Task<Card> FindAsync(Guid id)
    {
        return await _writeDbContext.Set<Card>().FindAsync(id);
    }

    public bool IsDuplicatedCardNumber(string cardNamber)
    {
        return _writeDbContext.Set<Card>().Any(x => x.Number == cardNamber);
    }

    public async Task<bool> Update(Card entity)
    {
        _writeDbContext.Set<Card>().Update(entity);
        return await _writeDbContext.SaveChangesAsync() > 0;
    }

    public IQueryable<Card> FindAll()
    {
        return _writeDbContext.Set<Card>();
    }
}