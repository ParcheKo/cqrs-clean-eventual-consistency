using System;
using Microsoft.EntityFrameworkCore;
using Orders.Core;

namespace Orders.Api.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static void ConfigureDatabaseNamingConvention(this DbContextOptionsBuilder builder, DatabaseNamingConvention namingConvention)
    {
        switch (namingConvention)
        {
            case DatabaseNamingConvention.AsIs:
                break;
            case DatabaseNamingConvention.CamelCase:
                builder.UseSnakeCaseNamingConvention();
                break;
            case DatabaseNamingConvention.SnakeCase:
                builder.UseSnakeCaseNamingConvention();
                break;
            default:
                throw new InvalidOperationException("Database naming convention not specified.");
        }
    }

}