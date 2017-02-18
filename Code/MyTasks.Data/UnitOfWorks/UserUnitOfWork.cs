using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Data.Contexts;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Data.Repositories;
using MyTasks.Domain;
using MyTasks.Domain.DataContracts;
using MyTasks.Domain.Entities;
using MyTasks.Infrastructure;


namespace MyTasks.Data.UnitOfWorks
{
    public class UserUnitOfWork
    {
        private readonly IWorkinghoursDataContext context;
        public GenericRepository<IWorkinghoursDataContext, Users> UserRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext, UserRoles> UserRoleRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext, Roles> RoleRepository { get; set; }

        public UserUnitOfWork(IWorkinghoursDataContext dataContext)
        {
            context = dataContext;
            InitRepositories();
        }


        public UserUnitOfWork()
        {
            context = new WorkinghoursDataContext();
            InitRepositories();

        }

        public void InitRepositories()
        {
            UserRepository = new GenericRepository<IWorkinghoursDataContext, Users>(context);
            UserRoleRepository= new GenericRepository<IWorkinghoursDataContext, UserRoles>(context);
            RoleRepository = new GenericRepository<IWorkinghoursDataContext, Roles>(context);
        }


        public Roles GetProjectRole()
        {
            return RoleRepository.All().FirstOrDefault(r => r.Name == "ProjectManager");
        }

        public List<Users> GetProjectManagers()
        {
            var role = GetProjectRole();
            return GetUsersByRole(role.Id);
        }

        public List<Users> GetUsersByRole(string roleId)
        {
            var users = (from u in UserRepository.All()
                         join r in UserRoleRepository.All() on u.Id equals r.UserId
                         where r.RoleId == roleId
                         select u).ToList();
            return users;   
        }



    }
}

