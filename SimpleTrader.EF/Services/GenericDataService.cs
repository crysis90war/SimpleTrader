using Microsoft.EntityFrameworkCore;
using SimpleTrader.EF.Services.Common;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;

namespace SimpleTrader.EF.Services
{
    public class GenericDataService<TEntity> : IDataService<TEntity> where TEntity : DomainObject
    {
        private readonly ApplicationDbContextFactory _contextFactory;
        private readonly NonQueryDataService<TEntity> _nonQueryDataService;

        public GenericDataService(ApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<TEntity>(contextFactory);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int key)
        {
            return await _nonQueryDataService.Delete(key);
        }

        public async Task<TEntity> Get(int key)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                TEntity entity = await context.Set<TEntity>().FirstOrDefaultAsync((e) => e.Id == key);
                return entity;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<TEntity> entities = await context.Set<TEntity>().ToListAsync();
                return entities;
            }
        }

        public async Task<TEntity> Update(int key, TEntity entity)
        {
            return await _nonQueryDataService.Update(key, entity);
        }
    }
}
