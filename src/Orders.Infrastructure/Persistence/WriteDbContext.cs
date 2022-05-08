using Microsoft.EntityFrameworkCore;
using Orders.Core.Cards;
using Orders.Core.Transactions;

namespace Orders.Infrastructure.Persistence
{
    public class WriteDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public WriteDbContext(DbContextOptions<WriteDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
            .ToTable("cards");

            modelBuilder.Entity<Card>()
                .Ignore(x => x.Valid);

            modelBuilder.Entity<Transaction>()
            .ToTable("transactions");

            modelBuilder.Entity<Transaction>()
                .Ignore(x => x.Valid);

            modelBuilder
                .Entity<Transaction>()
                .OwnsOne(p => p.Charge)
                .Property(p => p.CurrencyCode).HasColumnName("currency_code");

            modelBuilder
                .Entity<Transaction>()
                .OwnsOne(p => p.Charge)
                .Property(p => p.Amount).HasColumnName("amount");
        }
    }
}