using System;

namespace Orders.Application.Configuration;

public interface IExecutionContextAccessor
{
    Guid RequestId { get; }

    bool IsAvailable { get; }
}