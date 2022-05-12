using System;

namespace Orders.Core;

public class DatabaseNamingConventionNotSpecifiedException : InvalidOperationException
{
    public DatabaseNamingConventionNotSpecifiedException()
        : base("Database naming convention not specified in configuration.")
    {
    }
}