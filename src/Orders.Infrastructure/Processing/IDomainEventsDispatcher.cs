using System.Threading.Tasks;

namespace Orders.Infrastructure.Processing;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}