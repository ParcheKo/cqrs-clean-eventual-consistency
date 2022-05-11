using System;

namespace Orders.Api.Extensions;

public class DatabaseNamingConventionNotSpecifiedException : InvalidOperationException
{
    public DatabaseNamingConventionNotSpecifiedException()
        : base("Database naming convention not specified in configuration.")
    {
    }
}