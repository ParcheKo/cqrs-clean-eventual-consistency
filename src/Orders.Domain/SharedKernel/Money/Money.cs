using System;
using System.Collections.Generic;
using System.Linq;
using Orders.Domain.SeedWork;

namespace Orders.Domain.SharedKernel.Money;

public class Money : ValueObject
{
    private Money(
        decimal value,
        string currency
    )
    {
        Value = value;
        Currency = currency;
    }

    public decimal Value { get; }

    public string Currency { get; }

    public static Money Of(
        decimal value,
        string currency
    )
    {
        CheckRule(new MoneyMustHaveCurrency(currency));

        return new Money(
            value,
            currency
        );
    }

    public static Money Of(Money value)
    {
        return new Money(
            value.Value,
            value.Currency
        );
    }

    public static Money operator +(
        Money moneyLeft,
        Money moneyRight
    )
    {
        CheckRule(
            new MoneyOperationMustBePerformedOnTheSameCurrency(
                moneyLeft,
                moneyRight
            )
        );

        return new Money(
            moneyLeft.Value + moneyRight.Value,
            moneyLeft.Currency
        );
    }

    public static Money operator *(
        int number,
        Money moneyRight
    )
    {
        return new Money(
            number * moneyRight.Value,
            moneyRight.Currency
        );
    }

    public static Money operator *(
        decimal number,
        Money moneyRight
    )
    {
        return new Money(
            number * moneyRight.Value,
            moneyRight.Currency
        );
    }
}

public static class SumExtensions
{
    public static Money Sum<T>(
        this IEnumerable<T> source,
        Func<T, Money> selector
    )
    {
        return Money.Of(
            source.Select(selector).Aggregate(
                (
                    x,
                    y
                ) => x + y
            )
        );
    }

    public static Money Sum(this IEnumerable<Money> source)
    {
        return source.Aggregate(
            (
                x,
                y
            ) => x + y
        );
    }
}