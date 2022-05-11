using System;

namespace Orders.Infrastructure.WriteDatabase;

public class DatabaseNamingConventionNotSpecifiedException : InvalidOperationException
{
    public DatabaseNamingConventionNotSpecifiedException()
        : base("Database naming convention not specified in configuration.")
    {
    }
}