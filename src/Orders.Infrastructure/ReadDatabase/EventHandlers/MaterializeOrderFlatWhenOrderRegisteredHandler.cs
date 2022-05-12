using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using Orders.Domain.Orders.Events;
using Orders.Infrastructure.ReadDatabase.Models;
using Orders.Infrastructure.ReadDatabase.MongoDb;

namespace Orders.Infrastructure.ReadDatabase.EventHandlers;

public class MaterializeOrderFlatWhenOrderRegisteredHandler : INotificationHandler<OrderRegisteredEvent>
{
    private readonly ReadDbContext _readDbContext;

    public MaterializeOrderFlatWhenOrderRegisteredHandler(ReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task Handle(
        OrderRegisteredEvent notification,
        CancellationToken cancellationToken
    )
    {
        // todo : read model has some design problems now.
        // return;
        var person = await _readDbContext.PersonFlatMaterializedView
            .Find(p => p.Email == notification.CreatedBy)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (person is null)
        {
            throw new InvalidOperationException("Person has not yet persisted in database.");
        }
        
        var order = new OrderFlatQueryModel
        {
            Id = Guid.NewGuid(),
            OrderId = notification.OrderId.Value,
            OrderDate = notification.OrderDate,
            CreatedBy = notification.CreatedBy,
            OrderNo = notification.OrderNo,
            ProductName = notification.ProductName,
            Total = notification.Total,
            Price = notification.Price,
            TotalPrice = notification.TotalPrice,
            PersonName = person.Name,
        };

        await _readDbContext.OrderFlatMaterializedView.InsertOneAsync(
            order,
            cancellationToken: cancellationToken
        );
    }
}