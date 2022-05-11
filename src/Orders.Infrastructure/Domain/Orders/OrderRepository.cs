using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleProject.Domain.Customers;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Infrastructure.Database;
using SampleProject.Infrastructure.SeedWork;

namespace SampleProject.Infrastructure.Domain.Customers
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersContext _context;

        public OrderRepository(OrdersContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Order person)
        {
            await this._context.Orders.AddAsync(person);
        }

        public async Task<bool> ExistsWithOrderNo(string orderNo)
        {
            return await _context.Orders.AnyAsync(p => p.OrderNo == orderNo);
        }

        public async Task<Order> GetById(OrderId id)
        {
            return await this._context.Orders
                // .IncludePaths(
                //     PersonEntityTypeConfiguration.OrdersList,
                //     PersonEntityTypeConfiguration.OrderProducts
                // )
                .SingleAsync(x => x.Id == id);
        }
    }
}