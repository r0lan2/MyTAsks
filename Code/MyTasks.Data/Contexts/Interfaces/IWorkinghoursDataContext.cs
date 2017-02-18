using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Data.Base;
using MyTasks.Domain;
using MyTasks.Domain.Entities;

namespace MyTasks.Data.Contexts.Interfaces
{
    public interface IWorkinghoursDataContext : IUnitOfWork
    {
        IDbSet<Category> Category { get; set; }
        IDbSet<Area> Area { get; set; }
        IDbSet<Customer> Customer { get; set; }
        IDbSet<Priority> Priority { get; set; }
        IDbSet<Project> Project { get; set; }
        IDbSet<Roles> Roles { get; set; }
        IDbSet<Ticket> Ticket { get; set; }
        IDbSet<Ticketstatus> Ticketstatus { get; set; }
        IDbSet<Users> Users { get; set; }
        IDbSet<WorkingOn> Workingon { get; set; }
        IDbSet<ApplicationSettings> ApplicationSettings { get; set; }
        IDbSet<Dbversion> DbVersions { get; set; }
        IDbSet<UserRoles> UserRoles { get; set; }
    }


}
