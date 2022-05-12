using Orders.Domain.SeedWork;

namespace Orders.Domain.SharedKernel.Money;

public class MoneyMustHaveCurrency : IBusinessRule
{
    private readonly string _currency;

    public MoneyMustHaveCurrency(string currency)
    {
        _currency = currency;
    }

    public bool IsBroken()
    {
        return string.IsNullOrEmpty(_currency);
    }

    public string Message => "Money value must have currency";
}