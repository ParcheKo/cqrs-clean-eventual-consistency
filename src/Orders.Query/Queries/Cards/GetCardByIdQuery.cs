using System;
using Orders.Query.Abstractions;
using Orders.Query.QueryModel;

namespace Orders.Query.Queries.Cards;

public class GetCardByIdQuery : IQuery<CardViewQueryModel>
{
    public GetCardByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}