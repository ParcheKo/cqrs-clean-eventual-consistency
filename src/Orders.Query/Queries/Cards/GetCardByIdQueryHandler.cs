using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Orders.Query.Abstractions;
using Orders.Query.QueryModel;

namespace Orders.Query.Queries.Cards
{
    public class GetCardByIdQueryHandler : IQueryHandler<GetCardByIdQuery, CardViewQueryModel>
    {
        private readonly ReadDbContext _readDbContext;

        public GetCardByIdQueryHandler(ReadDbContext readDbContext)
        {
            this._readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
        }

        public async Task<CardViewQueryModel> HandleAsync(GetCardByIdQuery query)
        {
            try
            {
                FilterDefinition<CardViewQueryModel> filter = Builders<CardViewQueryModel>.Filter.Eq("Id", query.Id);
                var result = await _readDbContext.CardViewMaterializedView.FindAsync(filter);

                return await result.FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}