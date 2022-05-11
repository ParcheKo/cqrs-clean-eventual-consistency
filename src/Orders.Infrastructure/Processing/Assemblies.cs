using System.Reflection;
using Orders.Application.Orders.RegisterOrder;

namespace Orders.Infrastructure.Processing;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(RegisterOrderCommand).Assembly;
}