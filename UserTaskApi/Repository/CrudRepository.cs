using Microsoft.EntityFrameworkCore;

namespace UserTaskApi.Controllers
{
    public class CrudRepository<TEntity, TKey> : ICrudRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public CrudRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public TEntity GetById(TKey id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            return entity;
        }

        public void Remove(TKey id)
        {
            var entity = DbSet.Find(id);
            if (entity != null)
            {
                DbSet.Remove(entity);
                Context.SaveChanges();
            }
        }
    }
}