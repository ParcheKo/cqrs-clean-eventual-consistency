using System;

namespace Orders.Command.Abstractions;

public interface ICommandResult
{
    bool Success { get; }
    DateTime Executed { get; }
}