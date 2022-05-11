using System;
using System.Threading.Tasks;

namespace SampleProject.Domain.Customers.Orders
{
    public interface IOrderRepository
    {
        Task<Order> GetById(OrderId id);

        Task Add(Order person);
        Task<bool> ExistsWithOrderNo(string orderNo);
    }
}