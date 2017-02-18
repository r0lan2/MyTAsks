using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Data;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;
using NUnit.Framework;

namespace MyTasks.IntegrationTests
{
    [TestFixture]
    public class ProjectUnitOfWorkTests : IntegrationTestsBase
    {
        [SetUp]
        public void InsertInitProjectData()
        {
           
        }

        [TearDown]
        public void DeleteTestData()
        {
            DeleteProjects();
        }

        
        [Test]
        public void ProjectGetListEqualsZero()
        {
            var unitOfWork= new ProjectUnitOfWork();
            Assert.AreEqual(unitOfWork.ProjectRepository.All().Count(),0);
        }

         [Test]
        public void InsertNewProject()
        {
            var unitOfwork= new ProjectUnitOfWork();
            var firstCustomer = unitOfwork.CustomerRepository.All().First();
            var newProject = new Project
            {
                CustomerId = firstCustomer.CustomerId,
                Description = "project a",
                ProjectName = "projecta A"
            };
            unitOfwork.ProjectRepository.Add(newProject);
            unitOfwork.SaveChanges();
            var savedProject =unitOfwork.ProjectRepository.GetSingle(s => s.ProjectName == "projecta A");
            Assert.AreEqual(savedProject.Description, "project a");

        }
        

    }
}
