using System;
using System.Linq;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Core;
using SampleProject.API.Configuration;
using SampleProject.API.SeedWork;
using SampleProject.Application.Configuration;
using SampleProject.Application.Configuration.Emails;
using SampleProject.Application.Configuration.Validation;
using SampleProject.Domain.SeedWork;
using SampleProject.Infrastructure;
using SampleProject.Infrastructure.Database;
using Serilog;
using Serilog.Formatting.Compact;

namespace Orders.Api;

public class Startup
{
    private const string OrdersConnectionString = "OrdersConnectionString";


    private readonly ILogger _logger;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _logger = ConfigureLogger();
        _logger.Information("Logger configured");
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        var appConfiguration = Configuration.Get<AppConfiguration>();
        services.AddSingleton(appConfiguration);

        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            }
        );

        services.AddControllers();

        services.AddMemoryCache();

        services.AddSwaggerDocumentation();

        services.AddProblemDetails(
            x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            }
        );

        services.AddHttpContextAccessor();

        // services.AddScoped<ValidationNotificationHandler>();

        // services.AddControllers();
        //
        // services.AddSwaggerGen(
        //     c =>
        //     {
        //         c.SwaggerDoc(
        //             "v1",
        //             new OpenApiInfo { Title = "My API", Version = "v1" }
        //         );
        //     }
        // );

        var serviceProvider = services.BuildServiceProvider();

        IExecutionContextAccessor executionContextAccessor =
            new ExecutionContextAccessor(serviceProvider.GetService<IHttpContextAccessor>());

        // var children = this.Configuration.GetSection("Caching").GetChildren();
        // var cachingConfiguration = children.ToDictionary(
        //     child => child.Key,
        //     child => TimeSpan.Parse(child.Value)
        // );
        var emailsSettings = appConfiguration.EmailSettings;
        // var memoryCache = serviceProvider.GetService<IMemoryCache>();

        // todo: setup health-checks
        // var redisConnString = Configuration.GetConnectionString("RedisCache");
        // services
        //     .AddHealthChecks()
        //     .AddSqlServer(this.Configuration[OrdersConnectionString])
        //     .AddRedis(redisConnString);

        return ApplicationStartup.Initialize(
            services,
            appConfiguration,
            null,
            emailsSettings,
            _logger,
            executionContextAccessor
        );
    }


    // public virtual void ConfigureContainer(ContainerBuilder builder)
    // {
    //     builder.RegisterModule(new CommandModule());
    //     builder.RegisterModule(new EventModule());
    //     builder.RegisterModule(new InfrastructureModule());
    //     builder.RegisterModule(new QueryModule());
    // }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public virtual void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env
    )
    {
        app.UseMiddleware<CorrelationMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseProblemDetails();
        }

        // app.UseStaticFiles();

        app.UseRouting();

        app.UseCors("CorsPolicy");

        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapHealthChecks("/hc");
            }
        );

        app.UseSwaggerDocumentation();

        if (env.EnvironmentName == "Docker" || env.EnvironmentName == Environments.Development)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<OrdersContext>()!;
            var hasPendingMigrations = context.Database.GetPendingMigrations().Any();
            context.Database.Migrate();
        }

        // todo: if another micro was in the picture 
        // ConfigureEventBus(app);
    }

    private static ILogger ConfigureLogger()
    {
        return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.RollingFile(
                new CompactJsonFormatter(),
                "logs/logs"
            )
            .CreateLogger();
    }

    // private void ConfigureEventBus(IApplicationBuilder app)
    // {
    //     var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    //
    //     // todo: if another micro was in the picture 
    //     // eventBus.Subscribe<PersonRegistered>();
    //     // eventBus.Subscribe<CardCreatedEvent>();
    //     // eventBus.Subscribe<TransactionCreatedEvent>();
    // }
}