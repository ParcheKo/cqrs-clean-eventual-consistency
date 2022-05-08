using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Orders.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddAutofac())
            .ConfigureAppConfiguration(
                (
                    hostingContext,
                    config
                ) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile(
                        "appsettings.json",
                        false,
                        true
                    );
                    config.AddJsonFile(
                        $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                        true,
                        true
                    );
                }
            )
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureLogging(
                (
                    hostingContext,
                    logging
                ) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                }
            )
            .UseIIS()
            .UseStartup<Startup>();
    }
}