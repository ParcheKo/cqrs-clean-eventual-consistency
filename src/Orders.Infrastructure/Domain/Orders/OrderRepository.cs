using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Orders;
using Orders.Infrastructure.WriteDatabase;

namespace Orders.Infrastructure.Domain.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersContext _context;

    public OrderRepository(OrdersContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Add(Order person)
    {
        await _context.Orders.AddAsync(person);
    }

    public async Task<bool> ExistsWithOrderNo(string orderNo)
    {
        return await _context.Orders.AnyAsync(p => p.OrderNo == orderNo);
    }

    public async Task<Order> GetById(OrderId id)
    {
        return await _context.Orders
            // .IncludePaths(
            //     PersonEntityTypeConfiguration.OrdersList,
            //     PersonEntityTypeConfiguration.OrderProducts
            // )
            .SingleAsync(x => x.Id == id);
    }
}