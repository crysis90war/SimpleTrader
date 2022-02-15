using Microsoft.EntityFrameworkCore;
using SimpleTrader.EF.Configurations;
using SimpleTrader.Domain.Models;

namespace SimpleTrader.EF
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AssetTransaction> AssetTransactions { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AssetTransactionConfiguration());
        }
    }
}
