using Orders.Domain.SeedWork;

namespace Orders.Domain.Orders.Rules;

public class OrderNoMustBeUnique : IBusinessRule
{
    private readonly bool _isUnique;

    public OrderNoMustBeUnique(bool isUnique)
    {
        _isUnique = isUnique;
    }

    public bool IsBroken()
    {
        return !_isUnique;
    }

    public string Message => "Order with this order no. already exists.";
}