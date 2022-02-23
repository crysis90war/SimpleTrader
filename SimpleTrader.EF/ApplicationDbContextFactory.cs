using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SimpleTrader.EF
{
    public class ApplicationDbContextFactory
    {
        private readonly string _connectionString;

        public ApplicationDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            options.UseSqlServer(_connectionString);
            return new ApplicationDbContext(options.Options);
        }
    }
}
