﻿using System.Threading.Tasks;

namespace Orders.Command.Abstractions
{
    public interface ICommandHandler { }

    public interface ICommandHandler<TCommand, TResult> : ICommandHandler
        where TCommand : ICommand<TResult> where TResult : ICommandResult
    {
        Task<TResult> Handle(TCommand command);
    }

    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : ICommand
    {
        Task HandleNonResult(TCommand command);
    }
}