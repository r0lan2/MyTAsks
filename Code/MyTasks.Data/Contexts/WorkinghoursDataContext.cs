using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Domain;
using MyTasks.Domain.Entities.Mapping;
using MyTasks.Data.Base;

namespace MyTasks.Data.Contexts
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class WorkinghoursDataContext : BaseContext<WorkinghoursDataContext>, IWorkinghoursDataContext // DbContext
    {
        public WorkinghoursDataContext(): base()
        {
        }

        public WorkinghoursDataContext(DbConnection existingConnection, bool contextOwnsConnection): base(existingConnection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new AreaMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new PriorityMap());
            modelBuilder.Configurations.Add(new ProjectMap());
            modelBuilder.Configurations.Add(new RolesMap());
            modelBuilder.Configurations.Add(new TicketMap());
            modelBuilder.Configurations.Add(new TicketstatusMap());
            modelBuilder.Configurations.Add(new UsersMap());
            modelBuilder.Configurations.Add(new WorkingOnMap());
            modelBuilder.Configurations.Add(new ApplicationSettingsMap());
            modelBuilder.Configurations.Add(new DbVersionMap());
            modelBuilder.Configurations.Add(new FileDataMap());
            modelBuilder.Configurations.Add(new UserRolesMap());


        }

        public IDbSet<Area> Area { get; set; }
        public IDbSet<Category> Category { get; set; }
        public IDbSet<Customer> Customer { get; set; }
        public IDbSet<Priority> Priority { get; set; }
        public IDbSet<Project> Project { get; set; }
        public IDbSet<Roles> Roles { get; set; }
        public IDbSet<Ticket> Ticket { get; set; }
        public IDbSet<Ticketstatus> Ticketstatus { get; set; }
        public IDbSet<Users> Users { get; set; }
        public IDbSet<WorkingOn> Workingon { get; set; }
        public IDbSet<ApplicationSettings> ApplicationSettings { get; set; }
        public IDbSet<Dbversion> DbVersions { get; set; }
        public IDbSet<UserRoles> UserRoles { get; set; }
    }
}
