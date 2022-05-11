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
        builder.Property(p => p.Id).HasColumnName(nameof(InternalCommand.Id));
        builder.Property(p => p.Type).HasColumnName(nameof(InternalCommand.Type));
        builder.Property(p => p.Data).HasColumnName(nameof(InternalCommand.Data));
        builder.Property(p => p.ProcessedDate).HasColumnName(nameof(InternalCommand.ProcessedDate));
        builder.Property(b => b.Id).ValueGeneratedNever();
    }
}