using System;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Processing;

public interface ICommandsDispatcher
{
    Task DispatchCommandAsync(Guid id);
}