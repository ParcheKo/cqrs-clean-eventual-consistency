using Microsoft.EntityFrameworkCore;
using Orders.Domain.Orders;
using Orders.Domain.Persons;
using Orders.Infrastructure.Processing.InternalCommands;
using Orders.Infrastructure.Processing.Outbox;

namespace Orders.Infrastructure.WriteDatabase;

public class OrdersContext : DbContext
{
    public OrdersContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Person> Persons { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<InternalCommand> InternalCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersContext).Assembly);
    }
}