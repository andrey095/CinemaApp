using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Cinema.DAL.Commons
{
    public abstract class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class, new()
    {
        DbContext context;
        DbSet<T> dbSet;
        public GenericRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
        public virtual IQueryable<T> GetAll()
        {
            return dbSet;
        }
        public virtual IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }
        public virtual T Get(TKey id)
        {
            return dbSet.Find(id);
        }
        public void Create(T obj)
        {
            dbSet.AddOrUpdate(obj);
        }
        public void Update(T obj)
        {
            dbSet.AddOrUpdate(obj);
        }
        public void Delete(TKey id)
        {
            T obj = Get(id);
            dbSet.Remove(obj);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
