using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Orders.Core;
using Orders.Query.QueryModel;

namespace Orders.Query;

public class ReadDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoClient _mongoClient;

    public ReadDbContext(AppConfiguration appConfiguration)
    {
        _mongoClient = new MongoClient(appConfiguration.ConnectionStrings.MongoConnectionString);
        _database = _mongoClient.GetDatabase(appConfiguration.ConnectionStrings.MongoDatabase);
        ConfigureDatabaseNamingConvention(appConfiguration.DatabaseNamingConvention);
        Map();
    }

    internal IMongoCollection<CardViewQueryModel> CardViewMaterializedView =>
        _database.GetCollection<CardViewQueryModel>("CardViewMaterializedView");

    internal IMongoCollection<CardListQueryModel> CardListMaterializedView =>
        _database.GetCollection<CardListQueryModel>("CardListMaterializedView");

    internal IMongoCollection<TransactionListQueryModel> TransactionListMaterializedView =>
        _database.GetCollection<TransactionListQueryModel>("TransactionListMaterializedView");

    private static void ConfigureDatabaseNamingConvention(DatabaseNamingConvention namingConvention)
    {
        IConvention elementNameConvention = null;
        switch (namingConvention)
        {
            case DatabaseNamingConvention.AsIs:
                break;
            case DatabaseNamingConvention.CamelCase:
                elementNameConvention = new CamelCaseElementNameConvention();
                break;
            case DatabaseNamingConvention.SnakeCase:
                elementNameConvention = new SnakeCaseElementNameConvention();
                break;
            default:
                throw new InvalidOperationException("Database naming convention not specified.");
        }

        if (elementNameConvention is not null)
        {
            var elementNamingConventionPack = new ConventionPack
            {
                elementNameConvention
            };
            ConventionRegistry.Register(
                elementNameConvention.GetType().Name,
                elementNamingConventionPack,
                _ => true
            );
        }
    }

    private void Map()
    {
        BsonClassMap.RegisterClassMap<CardViewQueryModel>(cm => { cm.AutoMap(); });

        BsonClassMap.RegisterClassMap<CardListQueryModel>(cm => { cm.AutoMap(); });

        BsonClassMap.RegisterClassMap<TransactionListQueryModel>(cm => { cm.AutoMap(); });
    }
}