using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTasks.Data.UnitOfWorks;
using MyTasks.Domain;
using MyTasks.Domain.DataContracts;
using MyTasks.Infrastructure;

namespace MyTasks.Web.Models
{
    public class DemoViewModel
    {

        public void RunDemoData()
        {
            AddNormalUsers();
            AddProjectManagerUsers();
            AddDemoCustomers();
            AddDemoProjects();
            AddDemoTickets();
        }

        private void CreateUser(string userName, string roleName)
        {
            var unitOfwork = new ProjectUnitOfWork();
            var userRole = unitOfwork.RoleRepository.Where(r => r.Name == roleName).FirstOrDefault();
            var newUser = new Users()
            {
                Id = Guid.NewGuid().ToString(),
                Email = userName + "@mytasks.cl",
                EmailConfirmed = true,
                PasswordHash = "ADggy6naWtPgOdgAWhwGKQj1EUtBWuRkpYXnUpYXN5+ac2D+2AieripppwYT4jiKIA==",
                SecurityStamp = "78b9c51e-a6ff-47d3-b54b-84ab7223b157",
                PhoneNumber = "32323111",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = new DateTime(2016, 03, 18),
                LockoutEnabled = false,
                AccessFailedCount = 0,
                UserName = userName,
                FullName = userName,
                Address = null,
                DNI = "",
                Language = "en-us",

            };
            var newUserRole = new UserRoles
            {
                UserId = newUser.Id,
                RoleId = userRole.Id
            };
            unitOfwork.UserRepository.Add(newUser);
            unitOfwork.SaveChanges();
            unitOfwork.UserRoleRepository.Add(newUserRole);
            unitOfwork.SaveChanges();
        }

        private void InsertNewProject(string projectName)
        {
            Random r = new Random();
            var unitOfwork = new ProjectUnitOfWork();
            var userUnitOfWork = new UserUnitOfWork();
            var projectManagers = userUnitOfWork.GetProjectManagers();
            var projectManagersCount = projectManagers.Count;
            var customers = unitOfwork.CustomerRepository.All().ToList();
            var customerCount = customers.Count;
            var customerIndex = r.Next(0, customerCount);
            var projectManagerIndex = r.Next(0, projectManagersCount);
            var newProject = new Project
            {
                CustomerId = customers[customerIndex].CustomerId,
                ProjectManagerId = projectManagers[projectManagerIndex].Id,
                Description = projectName,
                ProjectName = projectName,
                Areas = new List<Area> { new Area
                    {
                        Name = "Area A"
                    }
                }
            };
            unitOfwork.ProjectRepository.Add(newProject);
            unitOfwork.SaveChanges();
        }

        private void AddDemoCustomer(string customerName)
        {
            var unitOfwork = new ProjectUnitOfWork();
            var newCustomer = new Customer
            {
                Name = customerName,
            };
            unitOfwork.CustomerRepository.Add(newCustomer);
            unitOfwork.SaveChanges();
        }

        private void AddNormalUsers()
        {
            var names = GetNames();

            var numberOfUsers = 10;
            for (int i = 0; i < numberOfUsers; i++)
            {
                var userName = names.Pop();
                CreateUser(userName, "User");
            }
        }

        private void AddProjectManagerUsers()
        {
            var names = GetProjectManagerNames();

            var numberOfUsers = 5;
            for (int i = 0; i < numberOfUsers; i++)
            {
                var userName = names.Pop();
                CreateUser(userName, "ProjectManager");
            }
        }

        private void AddDemoCustomers()
        {
            var numberOfCustomers = 10;
            var customerNames = GetCustomers();
            for (int i = 0; i < numberOfCustomers; i++)
            {
                var customerName = customerNames.Pop();
                AddDemoCustomer(customerName);
            }
        }

        private void AddDemoProjects()
        {
            var projectNames = GetProjects();
            var numberOfProjects = 10;
            for (int i = 0; i < numberOfProjects; i++)
            {
                var projectName = projectNames.Pop();
                InsertNewProject(projectName);
            }
        }

        private void AddDemoTickets()
        {
            var NumberOfTickets = 500;
            for (int i = 0; i < NumberOfTickets; i++)
            {
                AddDemoTicket();
            }
        }

        private void AddDemoTicket()
        {
            var ticketUnitOfwork = new TicketUnitOfWork();
            var userUnitOfwork = new UserUnitOfWork();
            var random = new Random();

            var projects = ticketUnitOfwork.ProjectRepository.All().ToList();
            var defaultProjectId = projects[random.Next(0, projects.Count)].ProjectId;
            var areas = ticketUnitOfwork.AreaRepository.Where(a => a.ProjectId == defaultProjectId).ToList();
            var defaultAreaId = areas[random.Next(0, areas.Count)].AreaId;
            var categories = ticketUnitOfwork.CategoryRepository.All().ToList();
            var defaultCategoryId = categories[random.Next(0, categories.Count)].CategoryId;
            var priorities = ticketUnitOfwork.PriorityRepository.All().ToList();
            var defaultPriorityId = priorities[random.Next(0, priorities.Count)].PriorityId;
            var normalUsers = userUnitOfwork.GetNormalUsers().ToList();
            var projectManagers = userUnitOfwork.GetProjectManagers().ToList();
            var statusId = (int)TicketStatus.Open;
            var defaultUserId = normalUsers[random.Next(0, normalUsers.Count)].Id;
            var ownerId = projectManagers[random.Next(0, projectManagers.Count)].Id;

            var data = new TicketData
            {
                Title = "FirstIssue",
                Content = "this is a content",
                ProjectId = defaultProjectId,
                CategoryId = defaultCategoryId,
                PriorityId = defaultPriorityId,
                UserId = defaultUserId,
                CreatedBy = ownerId,
                AreaId = defaultAreaId,
                IsBillable = false

            };
            ticketUnitOfwork.NewTicket(data);


        }

        private static Stack<string> GetNames()
        {
            Stack<string> stack = new Stack<string>();
            stack.Push("juan"); stack.Push("felipe"); stack.Push("hugo"); stack.Push("jose");
            stack.Push("francisco"); stack.Push("nery"); stack.Push("anita"); stack.Push("carlos");
            stack.Push("luis"); stack.Push("pedro");
            return stack;
        }

        private static Stack<string> GetProjectManagerNames()
        {
            Stack<string> stack = new Stack<string>();
            stack.Push("santiago"); stack.Push("gaspar"); stack.Push("gabriel"); stack.Push("franco"); stack.Push("ignacio");

            return stack;
        }

        private static Stack<string> GetCustomers()
        {
            Stack<string> stack = new Stack<string>();
            stack.Push("Amazon"); stack.Push("Cocacola"); stack.Push("Biglamp"); stack.Push("SoftLand");
            stack.Push("Microsoft"); stack.Push("Google"); stack.Push("Trello"); stack.Push("Evernote");
            stack.Push("Dropbox"); stack.Push("JetBrains");
            return stack;
        }

        private static Stack<string> GetProjects()
        {
            Stack<string> stack = new Stack<string>();
            stack.Push("MyTasks"); stack.Push("MyCarStore"); stack.Push("Move Servers"); stack.Push("Project X");
            stack.Push("Project Denver"); stack.Push("Project Auth "); stack.Push("Project Testing"); stack.Push("Project Misc");
            stack.Push("Project A"); stack.Push("Project B");
            return stack;
        }

    }
}