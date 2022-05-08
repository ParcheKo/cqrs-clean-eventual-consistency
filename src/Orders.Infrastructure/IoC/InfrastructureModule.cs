using Autofac;
using Orders.Core.Shared;
using Orders.Infrastructure.Bus;
using Orders.Infrastructure.Cache;
using RabbitMQ.Client;

namespace Orders.Infrastructure.IoC
{
    public class InfrastructureModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RabbitMqPersistentConnection>()
                .As<IPersistentConnection<IModel>>()
                .SingleInstance();

            builder
                .RegisterType<RedisCache>()
                .As<ICache>()
                .SingleInstance();

            builder
                .RegisterType<RabbitMqEventBus>()
                .As<IEventBus>()
                .SingleInstance();
        }
    }
}