using System.Data;

namespace Orders.Application.Configuration.Data;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}