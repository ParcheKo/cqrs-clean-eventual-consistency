using System;
using MediatR;

namespace SampleProject.Domain.SeedWork
{
    public interface IIntegrationEvent
    {
        // toto: dont forget to put JsonConstructor attribute for implementations
        Guid Id { get;  }
        DateTime OccurredOn { get; }
    }
}