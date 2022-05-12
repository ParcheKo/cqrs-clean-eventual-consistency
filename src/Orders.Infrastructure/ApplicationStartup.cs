using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Configuration;
using Orders.Application.Configuration.Emails;
using Orders.Infrastructure.Domain;
using Orders.Infrastructure.Emails;
using Orders.Infrastructure.Logging;
using Orders.Infrastructure.Processing;
using Orders.Infrastructure.Processing.InternalCommands;
using Orders.Infrastructure.Processing.Outbox;
using Orders.Infrastructure.Quartz;
using Orders.Infrastructure.ReadDatabase;
using Orders.Infrastructure.SeedWork;
using Orders.Infrastructure.WriteDatabase;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace Orders.Infrastructure;

public class ApplicationStartup
{
    public static IServiceProvider Initialize(
        IServiceCollection services,
        AppConfiguration appConfiguration,
        IEmailSender emailSender,
        EmailSettings emailSettings,
        ILogger logger,
        IExecutionContextAccessor executionContextAccessor,
        bool runQuartz = true
    )
    {
        if (runQuartz)
            StartQuartz(
                appConfiguration,
                emailSettings,
                logger,
                executionContextAccessor
            );


        var serviceProvider = CreateAutofacServiceProvider(
            services,
            appConfiguration,
            emailSender,
            emailSettings,
            logger,
            executionContextAccessor
        );

        return serviceProvider;
    }

    private static IServiceProvider CreateAutofacServiceProvider(
        IServiceCollection services,
        AppConfiguration appConfiguration,
        IEmailSender emailSender,
        EmailSettings emailSettings,
        ILogger logger,
        IExecutionContextAccessor executionContextAccessor
    )
    {
        var container = new ContainerBuilder();

        container.Populate(services);

        container.RegisterModule(new LoggingModule(logger));
        container.RegisterModule(new DataAccessModule(appConfiguration));
        container.RegisterModule(new QueryModule());
        container.RegisterModule(new MediatorModule());
        container.RegisterModule(new DomainModule());

        if (emailSender != null)
            container.RegisterModule(
                new EmailModule(
                    emailSender,
                    emailSettings
                )
            );
        else
            container.RegisterModule(new EmailModule(emailSettings));

        container.RegisterModule(new ProcessingModule());

        container.RegisterInstance(executionContextAccessor);

        var buildContainer = container.Build();

        ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));
        
        var serviceProvider = new AutofacServiceProvider(buildContainer);

        CompositionRoot.SetContainer(buildContainer);

        return serviceProvider;
    }
    
    private static void StartQuartz(
        AppConfiguration appConfiguration,
        EmailSettings emailSettings,
        ILogger logger,
        IExecutionContextAccessor executionContextAccessor
    )
    {
        var schedulerFactory = new StdSchedulerFactory();
        var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

        var container = new ContainerBuilder();

        container.RegisterModule(new LoggingModule(logger));
        container.RegisterModule(new QuartzModule());
        container.RegisterModule(new MediatorModule());
        container.RegisterModule(new DataAccessModule(appConfiguration));
        container.RegisterModule(new QueryModule());
        container.RegisterModule(new EmailModule(emailSettings));
        container.RegisterModule(new ProcessingModule());

        container.RegisterInstance(executionContextAccessor);
        container.Register(
            c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<OrdersContext>();
                dbContextOptionsBuilder.UseSqlServer(appConfiguration.ConnectionStrings.SqlServerConnectionString)
                    .ConfigureDatabaseNamingConvention(appConfiguration.DatabaseNamingConvention);
                dbContextOptionsBuilder
                    .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                return new OrdersContext(dbContextOptionsBuilder.Options);
            }
        ).AsSelf().InstancePerLifetimeScope();

        scheduler.JobFactory = new JobFactory(container.Build());

        scheduler.Start().GetAwaiter().GetResult();

        var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
        var trigger =
            TriggerBuilder
                .Create()
                .StartNow()
                .WithCronSchedule("0/15 * * ? * *")
                .Build();

        scheduler.ScheduleJob(
            processOutboxJob,
            trigger
        ).GetAwaiter().GetResult();

        var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
        var triggerCommandsProcessing =
            TriggerBuilder
                .Create()
                .StartNow()
                .WithCronSchedule("0/15 * * ? * *")
                .Build();
        scheduler.ScheduleJob(
            processInternalCommandsJob,
            triggerCommandsProcessing
        ).GetAwaiter().GetResult();
    }
}