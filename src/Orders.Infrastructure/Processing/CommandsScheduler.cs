using System;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Orders.Application.Configuration.Commands;
using Orders.Application.Configuration.Data;
using Orders.Application.Configuration.Processing;

namespace Orders.Infrastructure.Processing;

public class CommandsScheduler : ICommandsScheduler
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sqlInsert = "INSERT INTO [app].[InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
                                 "(@Id, @EnqueueDate, @Type, @Data)";

        await connection.ExecuteAsync(
            sqlInsert,
            new
            {
                command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = command.GetType().FullName,
                Data = JsonConvert.SerializeObject(command)
            }
        );
    }
}