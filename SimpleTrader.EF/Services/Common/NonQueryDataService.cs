using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SimpleTrader.Domain.Models;

namespace SimpleTrader.EF.Services.Common
{
    public class NonQueryDataService<TEntity> where TEntity : DomainObject
    {
        private readonly ApplicationDbContextFactory _contextFactory;

        public NonQueryDataService(ApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<TEntity> createdResult = await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();
                return createdResult.Entity;
            }
        }

        public async Task<TEntity> Update(int key, TEntity entity)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                entity.Id = key;
                context.Set<TEntity>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> Delete(int key)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                TEntity entity = await context.Set<TEntity>().FirstOrDefaultAsync((e) => e.Id.GetType() == key.GetType());
                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
