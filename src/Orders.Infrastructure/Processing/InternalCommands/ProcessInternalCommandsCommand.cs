using MediatR;
using Orders.Application.Configuration.Commands;
using Orders.Infrastructure.Processing.Outbox;

namespace Orders.Infrastructure.Processing.InternalCommands;

internal class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
{
}