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

        public string RootConnectionString
        {
            get { return MyTasks.Infrastructure.Configuration.Application.RootConnectionString; }
        }


        public void InsertUsers()
        {
            Database.Execute(commandText: "INSERT INTO customer (`Name`) VALUES ('Client A');", parameters: null);
            Database.Execute(commandText: "INSERT INTO customer (`Name`) VALUES ('Client B');", parameters: null);
            Database.Execute(commandText: "INSERT INTO customer (`Name`) VALUES ('Client C');", parameters: null);
            Database.Execute(commandText: "INSERT INTO customer (`Name`) VALUES ('Client D');", parameters: null);
            Database.Execute(commandText: "INSERT INTO customer (`Name`) VALUES ('Client E');", parameters: null);
            Database.Execute(commandText: "INSERT INTO customer (`Name`) VALUES ('Client F');", parameters: null);
            Database.Execute(commandText: "INSERT INTO customer (`Name`) VALUES ('Client G');", parameters: null);
        }

       
        //public void DeleteProjects()
        //{
        //    Database.Execute(commandText: "delete from project;", parameters: null);
        //}

        //public void CreateNewTestProject(int customerId)
        //{
        //    Database.Execute(commandText: "INSERT INTO project (`CustomerId`,`ProjectName`,`Description`) VALUES (" +  customerId.ToString() + ",'Test Project','Test Project');", parameters: null);
        //}


        //public void DeleteTickets()
        //{
        //    Database.Execute(commandText: "delete from ticket;", parameters: null);
        //}


    }
}
