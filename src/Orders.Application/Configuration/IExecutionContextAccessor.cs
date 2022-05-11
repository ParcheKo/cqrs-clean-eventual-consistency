using System;

namespace SampleProject.Application.Configuration
{
    public interface IExecutionContextAccessor
    {
        Guid RequestId { get; }

        bool IsAvailable { get; }
    }
}