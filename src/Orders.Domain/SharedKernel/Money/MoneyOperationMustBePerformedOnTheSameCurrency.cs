using Orders.Domain.SeedWork;

namespace Orders.Domain.SharedKernel.Money;

public class MoneyOperationMustBePerformedOnTheSameCurrency : IBusinessRule
{
    private readonly Money _left;

    private readonly Money _right;

    public MoneyOperationMustBePerformedOnTheSameCurrency(
        Money left,
        Money right
    )
    {
        _left = left;
        _right = right;
    }

    public bool IsBroken()
    {
        return _left.Currency != _right.Currency;
    }

    public string Message => "Money value currencies must be the same";
}