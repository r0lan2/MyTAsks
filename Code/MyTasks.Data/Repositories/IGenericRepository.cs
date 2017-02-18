using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyTasks.Data.Repositories
{
    public interface IGenericRepository<TEntity>
  where TEntity : class
    {
        IQueryable<TEntity> Select();

        IEnumerable<TEntity> All();

        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);

        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate);

        TEntity FindById(int id);

        void Add(TEntity entity);

        void Delete(TEntity entity);

        void Attach(TEntity entity);

        void Dispose(bool disposing);

        void Dispose();
    }
}
