namespace SimpleTrader.Domain.Services
{
    public interface IDataService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(int key);
        Task<TEntity> Create(TEntity entity);
        Task<TEntity> Update(int key, TEntity entity);
        Task<bool> Delete(int key);
    }
}
