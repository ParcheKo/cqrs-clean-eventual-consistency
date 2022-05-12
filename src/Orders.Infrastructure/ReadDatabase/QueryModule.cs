using Autofac;
using Orders.Infrastructure.ReadDatabase.MongoDb;

namespace Orders.Infrastructure.ReadDatabase;

public class QueryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<ReadDbContext>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}