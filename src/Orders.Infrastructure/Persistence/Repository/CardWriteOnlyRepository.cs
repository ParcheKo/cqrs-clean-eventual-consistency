using System;
using System.Linq;
using System.Threading.Tasks;
using Orders.Core.Cards;

namespace Orders.Infrastructure.Persistence.Repository
{
    public class CardWriteOnlyRepository : ICardWriteOnlyRepository
    {
        private readonly WriteDbContext _writeDbContext;

        public CardWriteOnlyRepository(WriteDbContext writeDbContext)
        {
            this._writeDbContext = writeDbContext ?? throw new ArgumentNullException(nameof(writeDbContext));
        }

        public async Task<bool> Add(Card entity)
        {
            _writeDbContext.Cards.Add(entity);
            return await _writeDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Card entity)
        {
            _writeDbContext.Cards.Remove(entity);
            return await _writeDbContext.SaveChangesAsync() > 0;
        }

        public IQueryable<Card> FindAll()
        {
            return _writeDbContext.Cards;
        }

        public async Task<Card> FindAsync(Guid id)
        {
            return await _writeDbContext.Cards.FindAsync(id);
        }

        public bool IsDuplicatedCardNumber(string cardNamber)
        {
            try
            {
                return _writeDbContext.Cards.Any(x => x.Number == cardNamber);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> Update(Card entity)
        {
            _writeDbContext.Cards.Update(entity);
            return await _writeDbContext.SaveChangesAsync() > 0;
        }
    }
}