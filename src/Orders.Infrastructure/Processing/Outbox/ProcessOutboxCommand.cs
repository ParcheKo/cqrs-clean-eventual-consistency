using MediatR;
using Orders.Application.Configuration.Commands;

namespace Orders.Infrastructure.Processing.Outbox;

public class ProcessOutboxCommand : CommandBase<Unit>, IRecurringCommand
{
}