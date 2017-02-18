using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain.DataContracts;
using NUnit.Framework;

namespace MyTasks.IntegrationTests
{
    [TestFixture]
    public class TicketUnitOfWorkTests : IntegrationTestsBase
    {
        private int defaultCustomerId;
        private int defaultCategoryId;
        private int defaultProjectId;
        private int defaultPriorityId;
        private string defaultUserId;
        private int defaultTypeId;

        private TicketUnitOfWork unitOfWork;
        

        [SetUp]
        public void SetupInitData()
        {
            
            unitOfWork=new TicketUnitOfWork();
            defaultCustomerId = unitOfWork.CustomerRepository.All().ToList()[0].CustomerId;
            CreateNewTestProject(defaultCustomerId);
            defaultCategoryId = unitOfWork.CategoryRepository.All().ToList()[0].CategoryId;
            defaultProjectId = unitOfWork.ProjectRepository.All().ToList()[0].ProjectId;
            defaultPriorityId = unitOfWork.PriorityRepository.All().ToList()[0].PriorityId;
            defaultUserId = unitOfWork.UserRepository.All().ToList()[0].Id;
            defaultTypeId = unitOfWork.AreaRepository.All().ToList()[0].AreaId;

        }


        [Test]
        public void TicketCountEqualToZero()
        {
            Assert.AreEqual(unitOfWork.TicketRepository.All().Count(), 0);
        }


        [Test]
        public void AddNewTicket()
        {
            var unitOfWork = new TicketUnitOfWork();
            var data= new TicketData();
            data.Title = "FirstIssue";
            data.Content = "this is a content";
            data.CustomerId = defaultCustomerId;
            data.ProjectId = defaultProjectId;
            data.CategoryId = defaultCategoryId;
            data.PriorityId = defaultPriorityId;
            data.UserId = defaultUserId;
            data.CreatedBy = defaultUserId;
            data.AreaId = defaultTypeId;
            unitOfWork.NewTicket(data);
            Assert.AreEqual(unitOfWork.TicketRepository.All().Count(), 1);
        }



        [TearDown]
        public void DeleteTestData()
        {
            DeleteTickets();
            DeleteProjects();
        }



    }
}
