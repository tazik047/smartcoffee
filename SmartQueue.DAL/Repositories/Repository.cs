using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;

namespace SmartQueue.DAL.Repositories
{
    class Repository<T> : IRepository<T> where T: class, IBaseEntity
    {
        protected readonly EfDbContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(EfDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual void Add(T item)
        {
            DbSet.Add(item);
        }

        public virtual void Edit(T item)
        {
            Context.SetState(item, EntityState.Modified);
        }

        public virtual void Delete(T entity)
        {
            var entityForDelete = DbSet.Find(entity.Id);
            DbSet.Remove(entityForDelete);
        }

        public IEnumerable<T> Get()
        {
            return DbSet;
        }

        public T Get(long id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<T> Get(Func<T, bool> predicat)
        {
            return DbSet.Where(predicat);
        }

        public long Count()
        {
            return DbSet.LongCount();
        }
    }
}