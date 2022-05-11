using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orders.Api;
using Orders.Core.Shared;
using Orders.Infrastructure.Persistence;
using Orders.UnitTest.Fakes;

namespace Orders.UnitTest.Api;

public class ApiApplicationFactory : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(
            services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<WriteDbContext>)
                );

                services.Remove(descriptor);

                var connection = new SqliteConnection("datasource=:memory:");
                connection.Open();

                services.AddDbContext<WriteDbContext>(options => { options.UseSqlite(connection); });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<WriteDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<ApiApplicationFactory>>();

                    db.Database.EnsureCreated();
                }
            }
        ).ConfigureTestContainer<ContainerBuilder>(
            builder =>
            {
                builder
                    .RegisterType<FakeCache>()
                    .As<ICache>()
                    .SingleInstance();

                builder
                    .RegisterType<FakeEventBus>()
                    .As<IEventBus>()
                    .SingleInstance();
            }
        );
    }
}