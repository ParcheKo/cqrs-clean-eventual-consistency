using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Infrastructure.WriteDatabase;

namespace Orders.Infrastructure.Processing.InternalCommands;

internal sealed class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.ToTable(
            // as long as it is queried using raw sql
            nameof(OrdersContext.InternalCommands) /*.ToLower()*/,
            SchemaNames.Application
        );

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedNever();
    }
}