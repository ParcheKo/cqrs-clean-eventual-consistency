using System;
using FluentAssertions;
using NUnit.Framework;
using Orders.Domain.Orders;
using Orders.Domain.Orders.Events;
using Orders.Domain.Orders.Rules;
using Orders.Domain.SharedKernel.Email;
using Orders.UnitTests.SeedWork;

namespace Orders.UnitTests;

[TestFixture]
public class OrderTests : TestBase
{
    private class OrderRegistration : OrderTests
    {
        [Test]
        public void should_fail_when_order_date_is_after_today()
        {
            const string createdBy = "test@test.com";
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
                        Email.Of(createdBy),
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
            const string createdBy = "test@test.com";
            const string productName = "test product";
            const string orderNo = "order-1";
            const int total = 1;
            const decimal price = 2.99m;
            const bool orderNoIsUnique = true;

            var today = DateTime.Today;
            var orderDate = today.AddDays(-daysBeforeToday);

            var order = Order.From(
                orderDate,
                Email.Of(createdBy),
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
            const string createdBy = "test@test.com";
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
                        Email.Of(createdBy),
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

    class WhenOrderRegisteredSuccessfully : OrderTests
    {
        [Test]
        [TestCase(
            10,
            50,
            500
        )]
        [TestCase(
            5,
            35.99,
            179.95
        )]
        public void total_price_should_be_as_total_times_price(
            int total,
            decimal price,
            decimal totalPrice
        )
        {
            const string createdBy = "test@test.com";
            const string productName = "test product";
            const string orderNo = "order-1";
            const bool orderNoIsUnique = true;

            var today = DateTime.Today;

            var order = Order.From(
                today,
                Email.Of(createdBy),
                orderNo,
                productName,
                total,
                price,
                orderNoIsUnique
            );

            order.TotalPrice.Should().BeApproximately(
                totalPrice,
                0.000_000_1M
            );
        }

        [Test]
        public void order_registration_must_have_been_notified()
        {
            const string createdBy = "test@test.com";
            const string productName = "test product";
            const string orderNo = "order-1";
            const int total = 1;
            const decimal price = 2.99m;
            const bool orderNoIsUnique = true;

            var today = DateTime.Today;

            var order = Order.From(
                today,
                Email.Of(createdBy),
                orderNo,
                productName,
                total,
                price,
                orderNoIsUnique
            );

            var orderRegisteredEvent = AssertPublishedDomainEvent<OrderRegisteredEvent>(order);
            orderRegisteredEvent.CreatedBy.Should().Be(createdBy);
        }

        public void order_properties_must_have_been_set()
        {
            const string createdBy = "test@test.com";
            const string productName = "test product";
            const string orderNo = "order-1";
            const int total = 1;
            const decimal price = 2.99m;
            const bool orderNoIsUnique = true;

            var today = DateTime.Today;

            var order = Order.From(
                today,
                Email.Of(createdBy),
                orderNo,
                productName,
                total,
                price,
                orderNoIsUnique
            );

            order.OrderDate.Should().Be(today);
            order.CreatedBy.Should().Be(createdBy);
            order.OrderNo.Should().Be(orderNo);
            order.ProductName.Should().Be(productName);
            order.Total.Should().Be(total);
            order.Price.Should().Be(price);
        }
    }
}