using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using MyTasks.Data.Contexts;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Data.Repositories;
using MyTasks.Domain;
using MyTasks.Data.Validators;

namespace MyTasks.Data.UnitOfWorks
{
    public class ProjectUnitOfWork
    {
        public IWorkinghoursDataContext Context { get; }
        public ProjectRepository ProjectRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext, Area> AreasRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext, Customer> CustomerRepository { get; set; }

        public GenericRepository<IWorkinghoursDataContext, Users> UserRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext, UserRoles> UserRoleRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext, Roles> RoleRepository { get; set; }

        private ProjectUnitOfWorkValidator _validator;

        public ProjectUnitOfWork(IWorkinghoursDataContext dataContext)
        {
            Context = dataContext;
            ProjectRepository = new ProjectRepository(Context);
            CustomerRepository = new GenericRepository<IWorkinghoursDataContext, Customer>(Context);
            AreasRepository= new GenericRepository<IWorkinghoursDataContext, Area>(Context);
            UserRepository = new GenericRepository<IWorkinghoursDataContext, Users>(Context);
            UserRoleRepository= new GenericRepository<IWorkinghoursDataContext, UserRoles>(Context);
            RoleRepository= new GenericRepository<IWorkinghoursDataContext, Roles>(Context);
        }

        public ProjectUnitOfWork()
        {
            Context = new WorkinghoursDataContext();
            ProjectRepository = new ProjectRepository(Context);
            CustomerRepository = new GenericRepository<IWorkinghoursDataContext, Customer>(Context);
            AreasRepository = new GenericRepository<IWorkinghoursDataContext, Area>(Context);
            UserRepository = new GenericRepository<IWorkinghoursDataContext, Users>(Context);
            UserRoleRepository = new GenericRepository<IWorkinghoursDataContext, UserRoles>(Context);
            RoleRepository = new GenericRepository<IWorkinghoursDataContext, Roles>(Context);
        }

        public void DeleteProject(int projectId)
        {
            var project= ProjectRepository.FindById(projectId);
            var areas = AreasRepository.Where(a => a.ProjectId == projectId).ToList();
            foreach (var area in areas)
            {
                Context.Area.Remove(area);
            }
           
            Context.Project.Remove(project);
            Context.SaveChanges();
        }

        public Project GetProject(int projectId)
        {
            return ProjectRepository.GetProject(projectId);
        }

        public IQueryable<Project> GetProjectsByCustomer(int? selectCustomerId)
        {
           var projects = ProjectRepository.GetProjects();
            var projectList = projects.OrderBy(d => d.ProjectId).ToList();

            return projectList.AsQueryable();
        }

        public Project GetProjectAndAreas(int projectId)
        {
            var project= ProjectRepository.FindById(projectId);
            project.Areas = ProjectRepository.GetAreasByProject(project);
            return project;
        }


        public ValidationStatus Validate(Project proj)
        {
            _validator = new ProjectUnitOfWorkValidator(this);
            ValidationResult result= _validator.Validate(proj);
            return new ValidationStatus {IsValid = result.IsValid, ErrorMessage = _validator.ValidationMessage};
        }
        

        public void AddProject(Project proj)
        {
            ProjectRepository.Add(proj);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

      

    }
}
