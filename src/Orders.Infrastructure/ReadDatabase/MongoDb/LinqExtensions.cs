using System;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;

namespace Orders.Infrastructure.ReadDatabase.MongoDb;

public static class LinqExtensions
{
    public static IMongoQueryable<TSource> WhereIf<TSource>(
        this IMongoQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> predicate
    )
    {
        if (condition)
            return (IMongoQueryable<TSource>)Queryable.Where(
                source,
                predicate
            );
        return source;
    }
}