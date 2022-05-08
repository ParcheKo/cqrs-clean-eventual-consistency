using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Orders.Query.Abstractions;
using Orders.Query.QueryModel;

namespace Orders.Query.Queries.Cards;

public class GetCardByIdQueryHandler : IQueryHandler<GetCardByIdQuery, CardViewQueryModel>
{
    private readonly ReadDbContext _readDbContext;

    public GetCardByIdQueryHandler(ReadDbContext readDbContext)
    {
        _readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
    }

    public async Task<CardViewQueryModel> HandleAsync(GetCardByIdQuery query)
    {
        var result = await _readDbContext.CardViewMaterializedView.Find(p => p.Id == query.Id)
            .FirstOrDefaultAsync();

        return result;
    }
}