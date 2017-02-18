using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MyTasks.Data.Contexts;
using MyTasks.Domain;
using BigLamp.DatabaseInstaller;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Infrastructure;

namespace MyTasks.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            //ExecuteExample();
            //TestRunInstaller();
            //CreateNew100Project();
            GetSummaryOfProjects();
            System.Console.ReadKey();
        }

        public static void TestRunInstaller()
        {
           string connectionString = "port=3306;server=localhost;user id=Developer;password=holamundo;database=codefirst";
           DatabaseInstallerByObject.BuildDatabase(DatabaseInstallerByObject.ReadKeyFromPosition.Prefix, connectionString);
           System.Console.WriteLine("ready!");
        }
        
        public static void CreateNew100Project()
        {
            string connectionString = "port=3306;server=localhost;user id=Developer;password=holamundo;database=codefirst";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (WorkinghoursDataContext context = new WorkinghoursDataContext(connection, false))
                {
                    var unitOfwork = new ProjectUnitOfWork(context);
                    var firstCustomer = unitOfwork.CustomerRepository.All().First();
                    for (int i = 0; i < 1000; i++)
                    {
                        var newProject = new Project
                        {
                            CustomerId = firstCustomer.CustomerId,
                            Description = "project " + i,
                            ProjectName = "project " + i
                        };
                        unitOfwork.ProjectRepository.Add(newProject);
                    }
                    unitOfwork.SaveChanges();
                }
            }
          
        }


        public static void GetSummaryOfProjects()
        {
            List<Project> projects= new List<Project>();
            var proj1 = new Project {ProjectId = 1, ProjectName = "projA"};
            projects.Add(proj1);
            var proj2 = new Project { ProjectId = 2, ProjectName = "projB" };
            projects.Add(proj2);
            var proj3 = new Project { ProjectId = 3, ProjectName = "projC" };
            projects.Add(proj3);
            List<Ticket> allTickets= new List<Ticket>();

            var openTickets = (from t in allTickets
                               where t.TicketStatusId == (int)TicketStatus.Open && t.IsLastDetail == true
                               group t by t.ProjectId
            into g
                               select new
                               {
                                   ProjectId = g.Key,
                                   OpenTickets = g.Count()
                               }).ToList();

            var closedTickets = (from t in allTickets
                                 where t.TicketStatusId == (int)TicketStatus.Closed && t.IsLastDetail == true
                                 group t by t.ProjectId
               into g
                                 select new
                                 {
                                     ProjectId = g.Key,
                                     ClosedTickets = g.Count()
                                 }).ToList();

            var resolvedTickets = (from t in allTickets
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
                select new
                {
                    projectName = p.ProjectName,
                    projectId = p.ProjectId,
                    openTickets = projectOpenTickets?.OpenTickets ?? 0,
                    closedTickets = projectClosedTickets?.ClosedTickets??0,
                    resolvedTickets = projectResolvedTickets?.ResolvedTickets??0
                }).ToList();
            


        }




    }
}
