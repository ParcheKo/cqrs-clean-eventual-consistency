using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Core.Extensions;
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
        builder.OwnsOne(
            p => p.Email,
            o =>
            {
                o.Property(e => e.Value).HasColumnName(nameof(Person.Email).ToSnakeCase())
                    .HasColumnType(nameof(SqlDbType.NVarChar)).HasMaxLength(150);
                o.HasIndex(e => e.Value);
            }
        );
    }
}