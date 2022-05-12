using Microsoft.EntityFrameworkCore;
using Orders.Core;

namespace Orders.Infrastructure.WriteDatabase;

public static class DbContextOptionsBuilderExtensions
{
    public static void ConfigureDatabaseNamingConvention(
        this DbContextOptionsBuilder builder,
        DatabaseNamingConvention namingConvention
    )
    {
        switch (namingConvention)
        {
            case DatabaseNamingConvention.Normal:
                break;
            case DatabaseNamingConvention.CamelCase:
                builder.UseCamelCaseNamingConvention();
                break;
            case DatabaseNamingConvention.SnakeCase:
                builder.UseSnakeCaseNamingConvention();
                break;
            default:
                throw new DatabaseNamingConventionNotSpecifiedException();
        }
    }
}