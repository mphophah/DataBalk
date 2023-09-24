namespace UserTaskApi.Controllers
{
    public interface ICrudRepository<TEntity, TKey> where TEntity : class
    {
        TEntity GetById(TKey id);
        IEnumerable<TEntity> GetAll();
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Remove(TKey id);
    }
}