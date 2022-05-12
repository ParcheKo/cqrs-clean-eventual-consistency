using System.Threading;
using System.Threading.Tasks;
using Orders.Application.Configuration.Commands;
using Orders.Domain.Orders;
using Orders.Domain.SeedWork;
using Orders.Domain.SharedKernel.Email;

namespace Orders.Application.Orders.RegisterOrder;

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
            Email.Of(request.PersonEmail),
            request.OrderNo,
            request.ProductName,
            request.Total,
            request.Price,
            orderNoIsUnique
        );

        await _orderRepository.Add(order);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new OrderDto { Id = order.Id.Value };
    }
}