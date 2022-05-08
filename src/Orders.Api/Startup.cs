﻿using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Orders.Api.Extensions;
using Orders.Core;
using Orders.Core.Cards;
using Orders.Core.Shared;
using Orders.Core.Transactions;
using Orders.Infrastructure.IoC;
using Orders.Infrastructure.Persistence;

namespace Orders.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public virtual void ConfigureServices(IServiceCollection services)
    {
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
        services.AddScoped<ValidationNotificationHandler>();
        var appConfiguration = Configuration.Get<AppConfiguration>();
        services.AddSingleton(appConfiguration);

        services.AddControllers();

        services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo { Title = "My API", Version = "v1" }
                );
            }
        );

        var sqlConnString = Configuration.GetConnectionString("SqlServerConnectionString");

        services
            .AddDbContext<WriteDbContext>(
                options => options.UseSqlServer(
                    sqlConnString,
                    b => b.MigrationsAssembly(typeof(WriteDbContext).Assembly.GetName().Name)
                ).ConfigureDatabaseNamingConvention(appConfiguration.DatabaseNamingConvention)
            );

        var redisConnString = Configuration.GetConnectionString("RedisCache");

        services
            .AddHealthChecks()
            .AddSqlServer(sqlConnString)
            .AddRedis(redisConnString);
    }

    public virtual void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new CommandModule());
        builder.RegisterModule(new EventModule());
        builder.RegisterModule(new InfrastructureModule());
        builder.RegisterModule(new QueryModule());
    }

    private void ConfigureEventBus(IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<CardCreatedEvent>();
        eventBus.Subscribe<TransactionCreatedEvent>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public virtual void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env
    )
    {
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(
            c =>
            {
                c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "My API V1"
                );
            }
        );

        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        if (env.EnvironmentName == "Docker" || env.EnvironmentName == Environments.Development)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<WriteDbContext>()!;
            // var hasPendingMigrations = context.Database.GetPendingMigrations().Any();
            context.Database.Migrate();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck");
            }
        );

        ConfigureEventBus(app);
    }
}