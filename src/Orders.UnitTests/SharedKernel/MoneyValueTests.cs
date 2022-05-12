using System.Collections.Generic;
using NUnit.Framework;
using Orders.Domain.SharedKernel;
using Orders.Domain.SharedKernel.Money;
using Orders.UnitTests.SeedWork;

namespace Orders.UnitTests.SharedKernel;

[TestFixture]
public class MoneyValueTests : TestBase
{
    [Test]
    public void MoneyValueOf_WhenCurrencyIsProvided_IsSuccessful()
    {
        var value = Money.Of(
            120,
            "EUR"
        );

        Assert.That(
            value.Value,
            Is.EqualTo(120)
        );
        Assert.That(
            value.Currency,
            Is.EqualTo("EUR")
        );
    }

    [Test]
    public void MoneyValueOf_WhenCurrencyIsNotProvided_ThrowsMoneyValueMustHaveCurrencyRuleBroken()
    {
        AssertBrokenRule<MoneyMustHaveCurrency>(
            () =>
            {
                Money.Of(
                    120,
                    ""
                );
            }
        );
    }

    [Test]
    public void GivenTwoMoneyValuesWithTheSameCurrencies_WhenAddThem_IsSuccessful()
    {
        var valueInEuros = Money.Of(
            100,
            "EUR"
        );
        var valueInEuros2 = Money.Of(
            50,
            "EUR"
        );

        var add = valueInEuros + valueInEuros2;

        Assert.That(
            add.Value,
            Is.EqualTo(150)
        );
        Assert.That(
            add.Currency,
            Is.EqualTo("EUR")
        );
    }

    [Test]
    public void GivenTwoMoneyValuesWithTheSameCurrencies_SumThem_IsSuccessful()
    {
        var valueInEuros = Money.Of(
            100,
            "EUR"
        );
        var valueInEuros2 = Money.Of(
            50,
            "EUR"
        );

        IList<Money> values = new List<Money>
        {
            valueInEuros, valueInEuros2
        };

        var add = values.Sum();

        Assert.That(
            add.Value,
            Is.EqualTo(150)
        );
        Assert.That(
            add.Currency,
            Is.EqualTo("EUR")
        );
    }

    [Test]
    public void
        GivenTwoMoneyValuesWithDifferentCurrencies_WhenAddThem_ThrowsMoneyValueOperationMustBePerformedOnTheSameCurrencyRule()
    {
        var valueInEuros = Money.Of(
            100,
            "EUR"
        );
        var valueInDollars = Money.Of(
            50,
            "USD"
        );
        AssertBrokenRule<MoneyOperationMustBePerformedOnTheSameCurrency>(
            () =>
            {
                var add = valueInEuros + valueInDollars;
            }
        );
    }
}