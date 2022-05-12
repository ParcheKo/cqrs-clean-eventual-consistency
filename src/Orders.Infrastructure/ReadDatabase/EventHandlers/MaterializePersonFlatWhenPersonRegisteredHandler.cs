using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Orders.Domain.Persons;
using Orders.Infrastructure.ReadDatabase.Models;
using Orders.Infrastructure.ReadDatabase.MongoDb;

namespace Orders.Infrastructure.ReadDatabase.EventHandlers;

public class MaterializePersonFlatWhenPersonRegisteredHandler : INotificationHandler<PersonRegistered>
{
    private readonly ReadDbContext _readDbContext;

    public MaterializePersonFlatWhenPersonRegisteredHandler(ReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task Handle(
        PersonRegistered notification,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}