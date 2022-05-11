using System;

namespace Orders.Domain.SeedWork;

public interface IIntegrationEvent
{
    // toto: dont forget to put JsonConstructor attribute for implementations
    Guid Id { get; }
    DateTime OccurredOn { get; }
}