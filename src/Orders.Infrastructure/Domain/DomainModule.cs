using Autofac;

namespace Orders.Infrastructure.Domain;

public class DomainModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // builder.RegisterType<PersonEmailUniquenessChecker>()
        //     .As<IPersonEmailUniquenessChecker>()
        //     .InstancePerLifetimeScope();
        //
        // builder.RegisterType<OrderNoUniquenessChecker>()
        //     .As<IOrderNoUniquenessChecker>()
        //     .InstancePerLifetimeScope();

        // builder.RegisterType<ForeignExchange>()
        //     .As<IForeignExchange>()
        //     .InstancePerLifetimeScope();
    }
}