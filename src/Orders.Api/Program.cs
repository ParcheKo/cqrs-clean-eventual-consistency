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
                .WriteTo.Seq("http://localhost:5341")
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
            .UseStartup<Startup>();
    }
}