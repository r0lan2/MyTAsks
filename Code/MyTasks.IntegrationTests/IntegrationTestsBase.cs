using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Data;
using NUnit.Framework;

namespace MyTasks.IntegrationTests
{
    [TestFixture]
    public class IntegrationTestsBase
    {
        private MySQLDatabase _database;
        public MySQLDatabase Database
        {
            get
            {
                if (_database == null) 
                    _database=new MySQLDatabase();
                return _database;
            }
        }

        public string ConnectionString
        {
            get { return MyTasks.Infrastructure.Configuration.Application.ConnectionString; }
        }


        public void InsertCustomers()
        {
            Database.Execute(commandText: "INSERT INTO customer (`Name`) VALUES ('AquaChile');", parameters: null);
            Database.Execute(commandText: "INSERT INTO customer (`Name`) VALUES ('Marine Harvest');", parameters: null);
        }

        public void InsertPriorities()
        {
            Database.Execute(commandText: "INSERT INTO priority (`Name`) VALUES ('Alta');", parameters: null);
            Database.Execute(commandText: "INSERT INTO priority (`Name`) VALUES ('Media');", parameters: null);
            Database.Execute(commandText: "INSERT INTO priority (`Name`) VALUES ('Baja');", parameters: null);
        }

        public void DeletePriorities()
        {
            Database.Execute(commandText: "delete from priority;", parameters: null);
        }

        public void DeleteCustomers()
        {
            Database.Execute(commandText: "delete from customer;", parameters: null);
        }


        public void DeleteProjects()
        {
            Database.Execute(commandText: "delete from project;", parameters: null);
        }

        public void CreateNewTestProject(int customerId)
        {
            Database.Execute(commandText: "INSERT INTO project (`CustomerId`,`ProjectName`,`Description`) VALUES (" +  customerId.ToString() + ",'Test Project','Test Project');", parameters: null);
        }


        public void DeleteTickets()
        {
            Database.Execute(commandText: "delete from ticket;", parameters: null);
        }


    }
}
