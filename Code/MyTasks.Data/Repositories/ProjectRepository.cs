using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Domain;
using MyTasks.Data.Contexts;
using MyTasks.Domain.DataContracts;
using MyTasks.Infrastructure;
using MyTasks.Localization.Desktop;

namespace MyTasks.Data.Repositories
{
    public class ProjectRepository : GenericRepository<IWorkinghoursDataContext, Project>
    {

        public ProjectRepository(IWorkinghoursDataContext context): base(context)
        {
        }

        public ProjectRepository(): base(new WorkinghoursDataContext())
        {
            
        }

        public Project GetProject(int projectId)
        {
            var project = All().FirstOrDefault(p => p.ProjectId == projectId);
            project.IsUsed = Context.Ticket.FirstOrDefault(t => t.ProjectId == project.ProjectId)!=null;
            return project;
        }

        public List<Project> GetProjects()
        {
            var projects = (from c in All() select c).ToList();
            foreach (var project in projects)
            {
                var exist = Context.Ticket.FirstOrDefault(t => t.ProjectId == project.ProjectId);
                project.IsUsed = exist != null;
                project.UsedCaption = project.IsUsed ? Desktop.Yes : Desktop.No;
            }
            return projects;
        }

        public List<Area> GetAreasByProject(Project project)
        {
            var exist = Context.Ticket.FirstOrDefault(t => t.ProjectId == project.ProjectId);
            project.IsUsed= exist != null;
            var areas = Context.Area.Where(a => a.ProjectId == project.ProjectId).ToList();
            if (!project.IsUsed)
            {
                foreach (var area in areas)
                {
                    area.IsUsed = false;
                }
            }
            else
            {
                foreach (var area in areas)
                {
                    area.IsUsed = Context.Ticket.FirstOrDefault(t=>t.AreaId==area.AreaId)!=null;
                }
            }
            return areas;
        }



    }
}
