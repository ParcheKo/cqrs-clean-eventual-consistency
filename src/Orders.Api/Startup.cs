using System;
using System.Linq;
using System.Net.Http;
using Autofac;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Api.Configuration;
using Orders.Api.SeedWork;
using Orders.Application.Configuration;
using Orders.Application.Configuration.Validation;
using Orders.Domain.SeedWork;
using Orders.Infrastructure;
using Orders.Infrastructure.Emails;
using Orders.Infrastructure.WriteDatabase;
using Serilog;
using Serilog.Formatting.Compact;

namespace Orders.Api;

public class Startup
{

    private readonly ILogger _logger;

    public Startup(
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        Configuration = configuration;
        Environment = environment;
        _logger = ConfigureLogger();
        _logger.Information("Logger configured");
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

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
        
        var serviceProvider = services.BuildServiceProvider();

        IExecutionContextAccessor executionContextAccessor =
            new ExecutionContextAccessor(serviceProvider.GetService<IHttpContextAccessor>());
        
        var emailsSettings = appConfiguration.EmailSettings;

        return ApplicationStartup.Initialize(
            services,
            appConfiguration,
            null,
            emailsSettings,
            _logger,
            executionContextAccessor
        );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env
    )
    {
        app.UseSerilogRequestLogging();
        
        app.UseMiddleware<CorrelationMiddleware>();
        
        app.UseProblemDetails();

        app.UseRouting();

        app.UseCors("CorsPolicy");

        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
            }
        );

        app.UseSwaggerDocumentation();

        if (env.EnvironmentName == "Docker" || env.EnvironmentName == Environments.Development)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<OrdersContext>()!;
            // just in case we wanna apply at runtime in dev env
            // var hasPendingMigrations = context.Database.GetPendingMigrations().Any();
            // context.Database.Migrate();
        }
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

    private void ConfigureProblemDetails(ProblemDetailsOptions options)
    {
        // Only include exception details in a development environment. There's really no nee
        // to set this as it's the default behavior. It's just included here for completeness :)
        options.IncludeExceptionDetails = (
            ctx,
            ex
        ) => Environment.IsDevelopment();

        // // Custom mapping function for FluentValidation's ValidationException.
        // options.MapFluentValidationException();

        // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
        // This is useful if you have upstream middleware that needs to do additional handling of exceptions.
        options.Rethrow<NotSupportedException>();

        // This will map NotImplementedException to the 501 Not Implemented status code.
        options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

        // This will map HttpRequestException to the 503 Service Unavailable status code.
        options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

        // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
        // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
        options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
    }
}