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
            CreateNew100Project();
            System.Console.ReadKey();
        }

       
        
        public static void CreateNew100Project()
        {
            string connectionString = "port=3306;server=localhost;user id=Developer;password=holamundo;database=mytasks";
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





    }
}
