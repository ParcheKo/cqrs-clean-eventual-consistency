using System.Threading.Tasks;

namespace Orders.Domain.Orders;

public interface IOrderRepository
{
    Task<Order> GetById(OrderId id);

    Task Add(Order person);
    Task<bool> ExistsWithOrderNo(string orderNo);
}