using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Infrastructure.WriteDatabase;

namespace Orders.Infrastructure.Processing.Outbox;

internal sealed class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(
            // as long as it is queried using raw sql
            nameof(OrdersContext.OutboxMessages) /*.ToLower()*/,
            SchemaNames.Application
        );

        builder.HasKey(b => b.Id);
        builder.Property(p => p.Id).HasColumnName(nameof(OutboxMessage.Id));
        builder.Property(p => p.OccurredOn).HasColumnName(nameof(OutboxMessage.OccurredOn));
        builder.Property(p => p.Type).HasColumnName(nameof(OutboxMessage.Type));
        builder.Property(p => p.Data).HasColumnName(nameof(OutboxMessage.Data));
        builder.Property(p => p.ProcessedDate).HasColumnName(nameof(OutboxMessage.ProcessedDate));

        builder.Property(b => b.Id).ValueGeneratedNever();
    }
}