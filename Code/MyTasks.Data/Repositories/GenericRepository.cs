using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyTasks.Data.Base;
using MyTasks.Data.Contexts.Interfaces;

namespace MyTasks.Data.Repositories
{
    public class GenericRepository<TContext, TEntity> : IGenericRepository<TEntity>
        where TContext : IUnitOfWork
        where TEntity : class
    {
        protected TContext Context;
        /// <summary>
        /// Constructor that takes a context
        /// </summary>
        /// <param name="context">An established data context</param>
        public GenericRepository(TContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> Select()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public IEnumerable<TEntity> All()
        {
            return Context.Set<TEntity>().AsEnumerable();
        }
        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Single(predicate);
        }
       public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().First(predicate);
        }
       public TEntity FindById(int id)
       {
           return Context.Set<TEntity>().Find(id);
       }
       
        public void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity");

            Context.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot delete a null entity");

            Context.Set<TEntity>().Remove(entity);
        }

        public void Attach(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot attach a null entity");

            Context.Set<TEntity>().Attach(entity);
        }
        public void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot attach a null entity");

            Context.Set<TEntity>().Attach(entity);
            Context.SetAdd(entity);
            Context.ApplyStateChanges();
        }
        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot update a null entity");
            
            Context.Set<TEntity>().Attach(entity);
            Context.SetModified(entity);
            Context.SaveChanges() ;
        }
        #region IDisposable implementation
        private bool _disposedValue;

        public void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state here if required
                }
                // dispose unmanaged objects and set large fields to null
            }
            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
