using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Configuration.Commands;
using Orders.Domain.SeedWork;
using Orders.Infrastructure.WriteDatabase;

namespace Orders.Infrastructure.Processing;

public class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;

    private readonly OrdersContext _ordersContext;

    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerWithResultDecorator(
        ICommandHandler<T, TResult> decorated,
        IUnitOfWork unitOfWork,
        OrdersContext ordersContext
    )
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _ordersContext = ordersContext;
    }

    public async Task<TResult> Handle(
        T command,
        CancellationToken cancellationToken
    )
    {
        var result = await _decorated.Handle(
            command,
            cancellationToken
        );

        if (command is InternalCommandBase<TResult>)
        {
            var internalCommand = await _ordersContext.InternalCommands.FirstOrDefaultAsync(
                x => x.Id == command.Id,
                cancellationToken
            );

            if (internalCommand != null) internalCommand.ProcessedDate = DateTime.UtcNow;
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return result;
    }
}