using Autofac;
using System.Reflection;
using Orders.Core.Cards;
using Orders.Core.Shared;
using Orders.Core.Transactions;
using Orders.Infrastructure.Dispatchers;
using Orders.Query;
using Orders.Query.EventHandlers;

namespace Orders.Infrastructure.IoC
{
    public class EventModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(IEventHandler<>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<MaterializeCardEventHandler>()
                .As<IEventHandler<CardCreatedEvent>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<TransactionCreatedEventHandler>()
                .As<IEventHandler<TransactionCreatedEvent>>()
                .InstancePerLifetimeScope();

            builder
               .RegisterType<ReadDbContext>()
               .AsSelf()
               .InstancePerLifetimeScope();

            builder
                .RegisterType<EventDispatcher>()
                .As<IEventDispatcher>()
                .SingleInstance();
        }
    }
}