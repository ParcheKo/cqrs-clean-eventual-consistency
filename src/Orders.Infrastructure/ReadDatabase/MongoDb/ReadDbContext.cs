using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Orders.Core;
using Orders.Infrastructure.ReadDatabase.Models;

namespace Orders.Infrastructure.ReadDatabase.MongoDb;

public class ReadDbContext
{
    private readonly IMongoDatabase _database;

    public ReadDbContext(AppConfiguration appConfiguration)
    {
        var mongoClient = new MongoClient(appConfiguration.ConnectionStrings.MongoConnectionString);
        _database = mongoClient.GetDatabase(appConfiguration.ConnectionStrings.MongoDatabase);
        ConfigureDatabaseNamingConvention(appConfiguration.DatabaseNamingConvention);
        // todo: a bug must be solved -> 
        // Map();
    }
    
    internal IMongoCollection<OrderFlatQueryModel> OrderFlatMaterializedView =>
        _database.GetCollection<OrderFlatQueryModel>(nameof(OrderFlatMaterializedView));
    
    internal IMongoCollection<PersonFlatQueryModel> PersonFlatMaterializedView =>
        _database.GetCollection<PersonFlatQueryModel>(nameof(PersonFlatMaterializedView));

    private static void ConfigureDatabaseNamingConvention(DatabaseNamingConvention namingConvention)
    {
        IConvention elementNameConvention = null;
        switch (namingConvention)
        {
            case DatabaseNamingConvention.Normal:
                break;
            case DatabaseNamingConvention.CamelCase:
                elementNameConvention = new CamelCaseElementNameConvention();
                break;
            case DatabaseNamingConvention.SnakeCase:
                elementNameConvention = new SnakeCaseElementNameConvention();
                break;
            default:
                throw new DatabaseNamingConventionNotSpecifiedException();
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
        // BsonClassMap.RegisterClassMap<OrderViewQueryModel>(cm => { cm.AutoMap(); });
        BsonClassMap.RegisterClassMap<OrderFlatQueryModel>(cm => { cm.AutoMap(); });
        BsonClassMap.RegisterClassMap<PersonFlatQueryModel>(cm => { cm.AutoMap(); });
    }
}