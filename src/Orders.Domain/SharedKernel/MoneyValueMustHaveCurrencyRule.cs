using Orders.Domain.SeedWork;

namespace Orders.Domain.SharedKernel;

public class MoneyValueMustHaveCurrencyRule : IBusinessRule
{
    private readonly string _currency;

    public MoneyValueMustHaveCurrencyRule(string currency)
    {
        _currency = currency;
    }

    public bool IsBroken()
    {
        return string.IsNullOrEmpty(_currency);
    }

    public string Message => "Money value must have currency";
}