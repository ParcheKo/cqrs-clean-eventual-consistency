using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Query.Extensions;
using SampleProject.Infrastructure.Database;

namespace SampleProject.Infrastructure.Processing.InternalCommands
{
    internal sealed class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
    {
        public void Configure(EntityTypeBuilder<InternalCommand> builder)
        {
            builder.ToTable(
                nameof(OrdersContext.InternalCommands).ToSnakeCase(),
                SchemaNames.Application
            );

            // builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();
        }
    }
}