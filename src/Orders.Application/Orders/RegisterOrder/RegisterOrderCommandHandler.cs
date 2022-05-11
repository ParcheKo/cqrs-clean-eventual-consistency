using System.Threading;
using System.Threading.Tasks;
using SampleProject.Application.Configuration.Commands;
using SampleProject.Application.Customers;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Domain.SeedWork;

namespace SampleProject.Application.Orders.PlaceCustomerOrder;

public class RegisterOrderCommandHandler : ICommandHandler<RegisterOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork
    )
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDto> Handle(
        RegisterOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var orderNoIsUnique = !await _orderRepository.ExistsWithOrderNo(request.OrderNo);
        var order = Order.From(
            request.OrderDate,
            request.CreatedBy,
            request.OrderNo,
            request.ProductName,
            request.Total,
            request.Price,
            orderNoIsUnique
        );

        await this._orderRepository.Add(order);

        await this._unitOfWork.CommitAsync(cancellationToken);

        return new OrderDto() { Id = order.Id.Value };
    }
}