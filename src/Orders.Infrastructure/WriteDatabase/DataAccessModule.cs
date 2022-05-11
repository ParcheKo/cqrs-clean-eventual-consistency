using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orders.Api.Extensions;
using Orders.Core;
using SampleProject.Application.Configuration.Data;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Domain.SeedWork;
using SampleProject.Infrastructure.Domain;
using SampleProject.Infrastructure.Domain.Customers;
using SampleProject.Infrastructure.SeedWork;

namespace SampleProject.Infrastructure.Database
{
    public class DataAccessModule : Module
    {
        private readonly string _databaseConnectionString;
        private readonly AppConfiguration _appConfiguration;

        public DataAccessModule(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            this._databaseConnectionString = appConfiguration.ConnectionStrings.SqlServerConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter(
                    "connectionString",
                    _databaseConnectionString
                )
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PersonRepository>()
                .As<IPersonRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StronglyTypedIdValueConverterSelector>()
                .As<IValueConverterSelector>()
                .SingleInstance();

            builder
                .Register(
                    c =>
                    {
                        var dbContextOptionsBuilder = new DbContextOptionsBuilder<OrdersContext>();
                        dbContextOptionsBuilder.UseSqlServer(
                                _databaseConnectionString,
                                b => b.MigrationsAssembly(typeof(OrdersContext).Assembly.GetName().Name)
                            )
                            .ConfigureDatabaseNamingConvention(_appConfiguration.DatabaseNamingConvention);
                        dbContextOptionsBuilder
                            .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                        return new OrdersContext(dbContextOptionsBuilder.Options);
                    }
                )
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();
        }
    }
}