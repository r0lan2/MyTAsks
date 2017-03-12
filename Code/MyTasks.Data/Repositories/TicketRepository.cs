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

namespace MyTasks.Data.Repositories
{

    public class TicketRepository : GenericRepository<IWorkinghoursDataContext, Ticket>
    {
        public TicketRepository(IWorkinghoursDataContext context): base(context)
        {
        }

        public TicketRepository(): base(new WorkinghoursDataContext())
        {

        }


        public List<Ticket> GetTicketByUser(string userId)
        {
            var ticketsByUser = (from t in All()
                                    where t.AssignedTo == userId && t.IsLastDetail
                                    select t).ToList();
            return ticketsByUser;
        }

        public List<Ticket> GetTicketDetails(int ticketNumber)
        {
            return All().Where(t =>  t.TicketNumber == ticketNumber).ToList();
        }

        public List<ProjectSummary> GetProjectsSummary(List<Project> projects)
        {
            var openTickets = (from t in All()
                               where t.TicketStatusId == (int)TicketStatus.Open && t.IsLastDetail == true
                               group t by t.ProjectId
                into g
                               select new
                               {
                                   ProjectId = g.Key,
                                   OpenTickets = g.Count()
                               }).ToList();

            var closedTickets = (from t in All()
                                 where t.TicketStatusId == (int)TicketStatus.Closed && t.IsLastDetail == true
                                 group t by t.ProjectId
               into g
                                 select new
                                 {
                                     ProjectId = g.Key,
                                     ClosedTickets = g.Count()
                                 }).ToList();

            var resolvedTickets = (from t in All()
                                   where t.TicketStatusId == (int)TicketStatus.Resolved && t.IsLastDetail == true
                                   group t by t.ProjectId
                into g
                                   select new
                                   {
                                       ProjectId = g.Key,
                                       ResolvedTickets = g.Count()
                                   }).ToList();

            var summary = (from p in projects
                           join ot in openTickets on p.ProjectId equals ot.ProjectId into projOt
                           from projectOpenTickets in projOt.DefaultIfEmpty()
                           join ct in closedTickets on p.ProjectId equals ct.ProjectId into projCt
                           from projectClosedTickets in projCt.DefaultIfEmpty()
                           join rt in resolvedTickets on p.ProjectId equals rt.ProjectId into projRt
                           from projectResolvedTickets in projRt.DefaultIfEmpty()
                           select new ProjectSummary
                           {
                               ProjectName = p.ProjectName,
                               ProjectId = p.ProjectId,
                               OpenTickets = projectOpenTickets?.OpenTickets ?? 0,
                               ClosedTickets = projectClosedTickets?.ClosedTickets ?? 0,
                               ResolvedTickets = projectResolvedTickets?.ResolvedTickets ?? 0
                           }).OrderBy(p=> p.ProjectName).ToList();
            return summary;
        }

        public List<Ticket> RunSearch(int? projectId,int? categoryId,string userId, int? priorityId)
        {
            return All().Where(t =>(t.ProjectId == projectId || projectId==0) 
                                    && (t.CategoryId == categoryId || categoryId ==0) 
                                    && (t.PriorityId ==  priorityId || priorityId ==0 ) 
                                    && (t.AssignedTo == userId || string.IsNullOrEmpty(userId)) 
                                    && t.IsLastDetail).ToList();
        }



        //TODO:Add resolvedBy and ClosedBy columns to Ticket Table
        public List<UserSummary> GetUserSummary(List<Users> users)
        {
            var openTickets = (from t in All()
                               where t.TicketStatusId == (int)TicketStatus.Open && t.IsLastDetail == true
                               group t by t.AssignedTo
                into g
                               select new
                               {
                                   UserId = g.Key,
                                   OpenTickets = g.Count()
                               }).ToList();

            var closedTickets = (from t in All()
                                 where t.TicketStatusId == (int)TicketStatus.Closed && t.IsLastDetail == true
                                 group t by t.LastUpdateBy
               into g
                                 select new
                                 {
                                     UserId = g.Key,
                                     ClosedTickets = g.Count()
                                 }).ToList();

            var resolvedTickets = (from t in All()
                                   where t.TicketStatusId == (int)TicketStatus.Resolved && t.IsLastDetail == true
                                   group t by t.LastUpdateBy
                into g
                                   select new
                                   {
                                       UserId = g.Key,
                                       ResolvedTickets = g.Count()
                                   }).ToList();

            var summary = (from p in users
                           join ot in openTickets on p.Id equals ot.UserId into projOt
                           from projectOpenTickets in projOt.DefaultIfEmpty()
                           join ct in closedTickets on p.Id equals ct.UserId into projCt
                           from projectClosedTickets in projCt.DefaultIfEmpty()
                           join rt in resolvedTickets on p.Id equals rt.UserId into projRt
                           from projectResolvedTickets in projRt.DefaultIfEmpty()
                           select new UserSummary()
                           {
                               UserId = p.Id,
                               UserName = p.UserName,
                               FullName = p.FullName,
                               OpenTickets = projectOpenTickets?.OpenTickets ?? 0,
                               ClosedTickets = projectClosedTickets?.ClosedTickets ?? 0,
                               ResolvedTickets = projectResolvedTickets?.ResolvedTickets ?? 0
                           }).OrderBy(p => p.UserName).ToList();
            return summary;
        }

    }
       

}
