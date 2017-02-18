using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading;


namespace MyTasks.Data.Base
{
    public class BaseContext<TContext>: DbContext where TContext : DbContext
    {
        //public IDbSet<AuditType> AuditTypes { get; set; }
        //public IDbSet<Audit> Audits { get; set; }
        //public IDbSet<User> Users { get; set; }
        static BaseContext()
        {
            Database.SetInitializer<TContext>(null);
        }
        protected BaseContext()
            : base("name=DefaultConnection")
        { }

        protected BaseContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection) 
        {

        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void ApplyStateChanges()
        {
            this.ApplyStateChangesOnDbContext();
        }

        public void SetAdd(object entity)
        {
            Entry(entity).State = EntityState.Added;
        }
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
   
        //public T SaveChangesWithAuditing<T>(Func<T> saveChangesFunction)
        //{
        //    Func<object, bool> isTypeToAudit =
        //                       entity =>    entity.GetType() == typeof(Assessment) || 
        //                                    entity.GetType() == typeof(Fishgroup) || 
        //                                    entity.GetType() == typeof(Event) ||
        //                                    entity.GetType() == typeof(TemplateRegistration);
          
        //    var changedElements = new List<Tuple<Audit, DbEntityEntry>>();

        //    foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted).OrderBy(t => t, new OrderEntities()))
        //    {
        //        if (isTypeToAudit(entry.Entity))
        //        {
        //            if (entry.Entity.GetType() == typeof (TemplateRegistration) && changedElements.All(t => t.Item1.TableName != typeof (Assessment).Name))
        //            {
        //                changedElements.Add(new Tuple<Audit, DbEntityEntry>(new Audit { AuditDate = DateTime.Now, TableName = typeof(Assessment).Name, AuditingTypeId = 2 }, entry));
        //            }
        //            else if (entry.Entity.GetType() != typeof(TemplateRegistration))
        //            {
        //                changedElements.Add(new Tuple<Audit, DbEntityEntry>(AuditAdd(entry), entry));   
        //            }
        //        }
        //    }

        //    Func<object, bool> isTypeToRefreshTreeView =
        //        entity => entity.GetType() == typeof(Site) ||
        //                     entity.GetType() == typeof(Area) ||
        //                     entity.GetType() == typeof(Operation) ||
        //                     entity.GetType() == typeof(CustomerBU) ||
        //                     entity.GetType() == typeof(Customer) ||
        //                     entity.GetType() == typeof(CompanyBU) ||
        //                     entity.GetType() == typeof(Company);

        //    foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted).OrderBy(t => t, new OrderEntities()))
        //    {
        //        if (isTypeToRefreshTreeView(entry.Entity))
        //        {
        //            changedElements.Add(new Tuple<Audit, DbEntityEntry>(new Audit{AuditDate = DateTime.Now,TableName = "TreeViewData", AuditingTypeId = 2}, entry));
        //            break;
        //        }
        //    }

        //    saveChangesFunction();

        //    foreach (var changedElement in changedElements)
        //    {
        //        var primaryKeyProperty = changedElement.Item2.Entity.GetType().GetProperty(changedElement.Item1.TableName + "Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        //        var user =
        //               Users.FirstOrDefault(u => u.UserName == Thread.CurrentPrincipal.Identity.Name);
        //        var userId = user != null ? user.UserId : null;

        //        if (primaryKeyProperty != null)
        //        {
        //            var primaryKeyValue = (int) primaryKeyProperty.GetValue(changedElement.Item2.Entity, null);
        //            if (!string.IsNullOrEmpty(userId))
        //            {
        //                changedElement.Item1.Key = primaryKeyValue;
        //                changedElement.Item1.UserId = userId;
        //                Audits.Add(changedElement.Item1);
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(userId))
        //            {
        //                changedElement.Item1.Key = -1;
        //                changedElement.Item1.UserId = userId;
        //                Audits.Add(changedElement.Item1);
        //            }
        //        }

        //    }        

        //    return saveChangesFunction();
        //}

        //public Audit AuditAdd(DbEntityEntry entity)
        //{
        //    var auditTypeId = 0;
        //    switch (entity.State)
        //    {
        //        case EntityState.Added:
        //            auditTypeId = 1;
        //            break;

        //        case EntityState.Modified:
        //            auditTypeId = 2;
        //            break;

        //        case EntityState.Deleted:
        //            auditTypeId = 3;
        //            break;
        //    }
        //    var tableName = entity.Entity.GetType().Name;
        //    return new Audit
        //    {
        //        AuditDate = DateTime.Now,
        //        TableName = tableName,
        //        AuditingTypeId = auditTypeId
        //    };
        //}

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Configurations.Add(new AuditTypeMap());
        //    modelBuilder.Configurations.Add(new AuditMap());
        //    modelBuilder.Configurations.Add(new UserMap());
          
        //}
    }

    //public class OrderEntities : IComparer<DbEntityEntry>
    //{
    //    public int Compare(DbEntityEntry entry1, DbEntityEntry entry2)
    //    {
    //        if (entry1.Entity.GetType() == typeof(Assessment) && entry2.Entity.GetType() == typeof(TemplateRegistration))
    //            return -1;
    //        if (entry2.Entity.GetType() == typeof(Assessment) && entry1.Entity.GetType() == typeof(TemplateRegistration))
    //            return 1;
    //        return 0;
    //    }
    //}
}
