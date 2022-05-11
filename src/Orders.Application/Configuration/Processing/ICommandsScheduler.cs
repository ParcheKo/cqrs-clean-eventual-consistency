using System.Threading.Tasks;
using Orders.Application.Configuration.Commands;

namespace Orders.Application.Configuration.Processing;

public interface ICommandsScheduler
{
    Task EnqueueAsync<T>(ICommand<T> command);
}