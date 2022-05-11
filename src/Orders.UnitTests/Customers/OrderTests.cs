using System;
using NUnit.Framework;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Domain.Customers.Orders.Rules;
using SampleProject.Domain.Customers.Rules;
using SampleProject.UnitTests.SeedWork;

namespace SampleProject.UnitTests.Customers
{
    [TestFixture]
    public class OrderTests : TestBase
    {
        class OrderCreation : OrderTests
        {
            [Test]
            public void should_fail_when_order_date_is_after_today()
            {
                const string personEmail = "test@test.com";
                const string productName = "test product";
                const string orderNo = "order-1";
                const int total = 1;
                const decimal price = 2.99m;
                const bool orderNoIsUnique = true;

                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                AssertBrokenRule<OrderDateMustBeTodayOrBefore>(
                    () =>
                    {
                        Order.From(
                            tomorrow,
                            personEmail,
                            orderNo,
                            productName,
                            total,
                            price,
                            orderNoIsUnique
                        );
                    }
                );
            }

            [Test]
            [TestCase(0)]
            [TestCase(1)]
            public void should_succeed_when_order_date_is_up_to_today(int daysBeforeToday)
            {
                const string personEmail = "test@test.com";
                const string productName = "test product";
                const string orderNo = "order-1";
                const int total = 1;
                const decimal price = 2.99m;
                const bool orderNoIsUnique = true;

                var today = DateTime.Today;
                var orderDate = today.AddDays(-daysBeforeToday);

                Order.From(
                    orderDate,
                    personEmail,
                    orderNo,
                    productName,
                    total,
                    price,
                    orderNoIsUnique
                );
            }

            [Test]
            public void should_fail_when_order_no_is_duplicate()
            {
                const string personEmail = "test@test.com";
                const string productName = "test product";
                const string orderNo = "order-1";
                const int total = 1;
                const decimal price = 2.99m;
                const bool orderNoIsUnique = false;

                var today = DateTime.Today;

                AssertBrokenRule<OrderNoMustBeUnique>(
                    () =>
                    {
                        Order.From(
                            today,
                            personEmail,
                            orderNo,
                            productName,
                            total,
                            price,
                            orderNoIsUnique
                        );
                    }
                );
            }
        }
    }
}