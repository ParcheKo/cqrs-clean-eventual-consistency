using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Configuration.Commands;
using Orders.Domain.SeedWork;
using Orders.Infrastructure.WriteDatabase;

namespace Orders.Infrastructure.Processing;

public class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _decorated;

    private readonly OrdersContext _ordersContext;

    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        IUnitOfWork unitOfWork,
        OrdersContext ordersContext
    )
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _ordersContext = ordersContext;
    }

    public async Task<Unit> Handle(
        T command,
        CancellationToken cancellationToken
    )
    {
        await _decorated.Handle(
            command,
            cancellationToken
        );

        if (command is InternalCommandBase)
        {
            var internalCommand =
                await _ordersContext.InternalCommands.FirstOrDefaultAsync(
                    x => x.Id == command.Id,
                    cancellationToken
                );

            if (internalCommand != null) internalCommand.ProcessedDate = DateTime.UtcNow;
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}