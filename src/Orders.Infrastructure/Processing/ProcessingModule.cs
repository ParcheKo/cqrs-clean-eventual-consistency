using System.Reflection;
using Autofac;
using MediatR;
using Orders.Application.Configuration.Commands;
using Orders.Application.Configuration.DomainEvents;
using Orders.Application.Configuration.Processing;
using Orders.Domain.Orders.Events;
using Orders.Infrastructure.Logging;
using Orders.Infrastructure.Processing.InternalCommands;
using Module = Autofac.Module;

namespace Orders.Infrastructure.Processing;

public class ProcessingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventsDispatcher>()
            .As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(OrderRegisteredEvent).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();

        builder.RegisterGenericDecorator(
            typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
            typeof(INotificationHandler<>)
        );

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerDecorator<>),
            typeof(ICommandHandler<>)
        );

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>)
        );

        builder.RegisterType<CommandsDispatcher>()
            .As<ICommandsDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterType<CommandsScheduler>()
            .As<ICommandsScheduler>()
            .InstancePerLifetimeScope();

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerDecorator<>),
            typeof(ICommandHandler<>)
        );

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>)
        );
    }
}