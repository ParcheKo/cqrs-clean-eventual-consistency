using System;
using System.IO;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Orders.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override(
                    "Microsoft",
                    LogEventLevel.Information
                )
                .Enrich.FromLogContext()
                .WriteTo.Console()
                // Add this line:
                .WriteTo.File(
                    System.IO.Path.Combine(
                        Environment.GetEnvironmentVariable("USERPROFILE"),
                        "LogFiles",
                        AppDomain.CurrentDomain.FriendlyName,
                        "diagnostics.txt"
                    ),
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    retainedFileCountLimit: 2,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1)
                )
                .CreateLogger();

            Log.Information("Starting Web Host");
            await CreateWebHostBuilder(args).Build().RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(
                ex,
                "Web Host terminated unexpectedly"
            );
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddAutofac())
            .UseSerilog()
            // .ConfigureAppConfiguration(
            //     (
            //         hostingContext,
            //         config
            //     ) =>
            //     {
            //         config.SetBasePath(Directory.GetCurrentDirectory());
            //         config.AddJsonFile(
            //             "appsettings.json",
            //             false,
            //             true
            //         );
            //         config.AddJsonFile(
            //             $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
            //             true,
            //             true
            //         );
            //     }
            // )
            // .UseKestrel()
            // .UseContentRoot(Directory.GetCurrentDirectory())
            // .ConfigureLogging(
            //     (
            //         hostingContext,
            //         logging
            //     ) =>
            //     {
            //         logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //         logging.AddConsole();
            //         logging.AddDebug();
            //     }
            // )
            // .UseIIS()
            .UseStartup<Startup>();
    }
}