using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orders.Query.Extensions;
using SampleProject.Domain.Customers;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Domain.SharedKernel;
using SampleProject.Infrastructure.Database;

namespace SampleProject.Infrastructure.Domain.Customers
{
    internal sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable(
                "Persons".ToSnakeCase(),
                SchemaNames.Orders
            );

            // todo : test if these simple configs are done by conventions
            builder.HasKey(b => b.Id);
            builder.Property(p => p.Name);
            builder.Property(p => p.Email);

            // builder.OwnsMany<Order>(OrdersList, x =>
            // {
            //     x.WithOwner().HasForeignKey("CustomerId");
            //
            //     x.ToTable("Orders", SchemaNames.Orders);
            //     
            //     x.Property<bool>("_isRemoved").HasColumnName("IsRemoved");
            //     x.Property<DateTime>("_orderDate").HasColumnName("OrderDate");
            //     x.Property<DateTime?>("_orderChangeDate").HasColumnName("OrderChangeDate");
            //     x.Property<OrderId>("Id");
            //     x.HasKey("Id");
            //
            //     x.Property("_status").HasColumnName("StatusId").HasConversion(new EnumToNumberConverter<OrderStatus, byte>());

            // x.OwnsMany<OrderProduct>(OrderProducts, y =>
            // {
            //     y.WithOwner().HasForeignKey("OrderId");
            //
            //     y.ToTable("OrderProducts", SchemaNames.Orders);
            //     y.Property<OrderId>("OrderId");
            //     y.Property<ProductId>("ProductId");
            //     
            //     y.HasKey("OrderId", "ProductId");
            //
            //     y.OwnsOne<MoneyValue>("Value", mv =>
            //     {
            //         mv.Property(p => p.Currency).HasColumnName("Currency");
            //         mv.Property(p => p.Value).HasColumnName("Value");
            //     });
            //
            //     y.OwnsOne<MoneyValue>("ValueInEUR", mv =>
            //     {
            //         mv.Property(p => p.Currency).HasColumnName("CurrencyEUR");
            //         mv.Property(p => p.Value).HasColumnName("ValueInEUR");
            //     });
            // });

            //     x.OwnsOne<MoneyValue>("_value", y =>
            //     {
            //         y.Property(p => p.Currency).HasColumnName("Currency");
            //         y.Property(p => p.Value).HasColumnName("Value");
            //     });
            // });
        }
    }
}