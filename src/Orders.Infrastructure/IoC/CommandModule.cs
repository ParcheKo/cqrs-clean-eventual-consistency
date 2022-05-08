using Autofac;
using System.Reflection;
using Orders.Command.Abstractions;
using Orders.Core.Cards;
using Orders.Core.Transactions;
using Orders.Infrastructure.Dispatchers;
using Orders.Infrastructure.Persistence.Repository;

namespace Orders.Infrastructure.IoC
{
    public class CommandModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
               .RegisterType<CardWriteOnlyRepository>()
               .As<ICardWriteOnlyRepository>()
               .InstancePerLifetimeScope();

            builder
               .RegisterType<TransactionWriteOnlyRepository>()
               .As<ITransactionWriteOnlyRepository>()
               .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(typeof(ICommandHandler<,>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<,>))
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(typeof(ICommandHandler<>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .SingleInstance();
        }
    }
}