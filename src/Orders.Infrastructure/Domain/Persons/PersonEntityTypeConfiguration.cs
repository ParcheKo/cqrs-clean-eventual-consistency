using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Persons;
using Orders.Infrastructure.WriteDatabase;

namespace Orders.Infrastructure.Domain.Persons;

public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    // todo: ef migration fails to run this config when it has a ctor with injected service
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable(
            nameof(OrdersContext.Persons).ToLower(),
            SchemaNames.Orders
        );

        builder.HasKey(b => b.Id);
        builder.Property(p => p.Name).HasColumnType(nameof(SqlDbType.NVarChar)).HasMaxLength(50);
        builder.Property(p => p.Email).HasColumnType(nameof(SqlDbType.NVarChar)).HasMaxLength(150);
        builder.HasIndex(p => p.Email).IsUnique();

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