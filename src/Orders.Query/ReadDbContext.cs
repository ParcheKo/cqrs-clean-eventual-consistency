using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Orders.Core;
using Orders.Query.QueryModel;

namespace Orders.Query
{
    public class ReadDbContext
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _database;

        public ReadDbContext(AppConfiguration appConfiguration)
        {
            _mongoClient = new MongoClient(appConfiguration.ConnectionStrings.MongoConnectionString);
            _database = _mongoClient.GetDatabase(appConfiguration.ConnectionStrings.MongoDatabase);
            ConfigureDatabaseNamingConvention(appConfiguration.DatabaseNamingConvention);
            Map();
        }

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

        internal IMongoCollection<CardViewQueryModel> CardViewMaterializedView
        {
            get { return _database.GetCollection<CardViewQueryModel>("CardViewMaterializedView"); }
        }

        internal IMongoCollection<CardListQueryModel> CardListMaterializedView
        {
            get { return _database.GetCollection<CardListQueryModel>("CardListMaterializedView"); }
        }

        internal IMongoCollection<TransactionListQueryModel> TransactionListMaterializedView
        {
            get { return _database.GetCollection<TransactionListQueryModel>("TransactionListMaterializedView"); }
        }

        private void Map()
        {
            BsonClassMap.RegisterClassMap<CardViewQueryModel>(cm => { cm.AutoMap(); });

            BsonClassMap.RegisterClassMap<CardListQueryModel>(cm => { cm.AutoMap(); });

            BsonClassMap.RegisterClassMap<TransactionListQueryModel>(cm => { cm.AutoMap(); });
        }
    }
}