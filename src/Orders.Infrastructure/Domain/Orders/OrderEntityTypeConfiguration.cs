using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Core.Extensions;
using Orders.Domain.Orders;
using Orders.Infrastructure.WriteDatabase;

namespace Orders.Infrastructure.Domain.Orders;

internal sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    // todo: ef migration fails to run this config when it has a ctor with injected service
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(
            nameof(OrdersContext.Orders).ToLower(),
            SchemaNames.Orders
        );

        builder.HasKey(b => b.Id);
        builder.Property(p => p.OrderDate).HasColumnType(nameof(SqlDbType.DateTime2));
        builder.OwnsOne(p => p.CreatedBy)
            .Property(p => p.Value).HasColumnName(nameof(Order.CreatedBy).ToSnakeCase())
            .HasColumnType(nameof(SqlDbType.NVarChar)).HasMaxLength(150);
        builder.Property(p => p.OrderNo).HasColumnType(nameof(SqlDbType.NVarChar)).HasMaxLength(50);
        builder.Property(p => p.ProductName).HasColumnType(nameof(SqlDbType.NVarChar)).HasMaxLength(100);
        builder.Property(p => p.Total).HasColumnType(nameof(SqlDbType.Int));
        builder.Property(p => p.Price).HasColumnType(nameof(SqlDbType.Decimal)).HasPrecision(
            24,
            2
        );
        builder.Property(p => p.TotalPrice).HasColumnType(nameof(SqlDbType.Decimal)).HasPrecision(
            24,
            2
        ).UsePropertyAccessMode(PropertyAccessMode.Property);
        builder.HasIndex(p => p.OrderNo).IsUnique();
    }
}