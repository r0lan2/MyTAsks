using System;
using System.Data.Entity;

namespace MyTasks.Data.Base
{
  public interface IUnitOfWork:IDisposable 
  {
    int SaveChanges();
    IDbSet<TEntity> Set<TEntity>() where TEntity : class;
    void SetModified(object entity);
    void SetAdd(object entity);
    void ApplyStateChanges();
  }
  
}