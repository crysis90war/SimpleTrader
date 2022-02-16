using Microsoft.EntityFrameworkCore;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.EF.Services.Common;

namespace SimpleTrader.EF.Services
{
    public class AccountDataService : IAccountService
    {
        private readonly ApplicationDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Account> _nonQueryDataService;

        public AccountDataService(ApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Account>(contextFactory);
        }

        public async Task<Account> Create(Account entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int key)
        {
            return await _nonQueryDataService.Delete(key);
        }

        public async Task<Account> Get(int key)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                Account entity = await context.Accounts
                    .Include(a => a.AccountHolder)
                    .Include(a => a.AssetTransactions)
                    .FirstOrDefaultAsync((a) => a.Id == key);

                return entity;
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Account> entities = await context.Accounts
                    .Include(a => a.AccountHolder)
                    .Include(a => a.AssetTransactions)
                    .ToListAsync();

                return entities;
            }
        }

        public async Task<Account> GetByEmail(string email)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Accounts
                    .Include(a => a.AccountHolder)
                    .Include(a => a.AssetTransactions)
                    .FirstOrDefaultAsync(a => a.AccountHolder.Email == email);
            }
        }

        public async Task<Account> GetByUsername(string username)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Accounts
                    .Include(a => a.AccountHolder)
                    .Include(a => a.AssetTransactions)
                    .FirstOrDefaultAsync(a => a.AccountHolder.Username == username);
            }
        }

        public async Task<Account> Update(int key, Account entity)
        {
            return await _nonQueryDataService.Update(key, entity);
        }
    }
}
