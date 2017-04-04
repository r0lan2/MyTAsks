using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using MyTasks.Data.Contexts;
using MyTasks.Data.Contexts.Interfaces;
using MyTasks.Data.Repositories;
using MyTasks.Data.Validators;
using MyTasks.Domain;
using MyTasks.Domain.DataContracts;
using MyTasks.Domain.Entities;
using MyTasks.Infrastructure;

namespace MyTasks.Data.UnitOfWorks
{
    public class TicketUnitOfWork
    {
        private readonly IWorkinghoursDataContext context;
        private TicketUnitOfWorkValidator _validator;
        public TicketRepository TicketRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext,Customer> CustomerRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext, Area> AreaRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext,Category> CategoryRepository { get; set; }

        public GenericRepository<IWorkinghoursDataContext, Ticketstatus> StatusRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext,Priority> PriorityRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext,Project> ProjectRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext,Users> UserRepository { get; set; }
        public GenericRepository<IWorkinghoursDataContext,WorkingOn> WorkingOnReposotiry { get; set; }

        public GenericRepository<IWorkinghoursDataContext, FileData> FileDataRepository { get; set; }



        public TicketUnitOfWork(IWorkinghoursDataContext dataContext)
        {
            context = dataContext;
            InitRepositories();
        }

        public void InitRepositories()
        {
            TicketRepository = new TicketRepository(context);
            CustomerRepository = new GenericRepository<IWorkinghoursDataContext, Customer>(context);
            CategoryRepository = new GenericRepository<IWorkinghoursDataContext, Category>(context);
            PriorityRepository = new GenericRepository<IWorkinghoursDataContext, Priority>(context);
            ProjectRepository = new GenericRepository<IWorkinghoursDataContext, Project>(context);
            UserRepository = new GenericRepository<IWorkinghoursDataContext, Users>(context);
            WorkingOnReposotiry = new GenericRepository<IWorkinghoursDataContext, WorkingOn>(context);
            FileDataRepository = new GenericRepository<IWorkinghoursDataContext, FileData>(context);
            AreaRepository= new GenericRepository<IWorkinghoursDataContext, Area>(context);
            StatusRepository= new GenericRepository<IWorkinghoursDataContext, Ticketstatus>(context);
        }

        public TicketUnitOfWork()
        {
            context = new WorkinghoursDataContext();
            InitRepositories();
        }

        public ValidationStatus Validate(TicketData proj)
        {
            _validator = new TicketUnitOfWorkValidator(this);
            ValidationResult result = _validator.Validate(proj);
            return new ValidationStatus { IsValid = result.IsValid, ErrorMessage = _validator.ValidationMessage };
        }

        public IEnumerable<TicketSearchResult> TicketSearch(string criteria)
        {
            var criteriaToLower = criteria.ToLower();
            return TicketRepository.Where( t => t.IsLastDetail &&  t.Title.ToLower().Contains(criteriaToLower) || t.TicketNumber.ToString().ToLower().Contains(criteriaToLower) ).Select(
                r => new TicketSearchResult()
                {
                    Title = r.Title,
                    TicketNumber = r.TicketNumber.Value
                }
            );

        }

        public Ticket NewTicket(TicketData ticket)
        {
            var newTicket = new Ticket()
            {
                ProjectId = ticket.ProjectId,
                TicketNumber = null,
                AreaId = ticket.AreaId,
                PriorityId = ticket.PriorityId,
                CategoryId = ticket.CategoryId,
                TicketStatusId = (int)TicketStatus.Open,
                Title = ticket.Title,
                Details = ticket.Content,
                IsHtml = true,
                CreatedBy = ticket.CreatedBy,
                CreatedDate = DateTime.Now,
                AssignedTo = ticket.UserId,
                LastUpdateBy = ticket.CreatedBy,
                LastUpdateDate = DateTime.Now,
                OwnerUserId = ticket.CreatedBy,
                ParentTicketId = null,
                EditionMarkAsDeleted=false,
                IsLastDetail = true,
                IsBillable = ticket.IsBillable,
                HasFiles = ticket.HasFiles,
            };
            TicketRepository.Add(newTicket);
            Save();
            newTicket.TicketNumber = newTicket.TicketDetailId;
            Save();// TODO:Use a stored procedure to insert ?
            return newTicket;
        }

        public Ticket NewTicketEdition(TicketData ticket)
        {
            var ticketNumber = ticket.TicketNumber;
            var ticketDetails = TicketRepository.Where(t => t.TicketNumber == ticketNumber);
            var createdDate = ticketDetails.FirstOrDefault().CreatedDate;
            var createdBy= ticketDetails.FirstOrDefault().CreatedBy;
            foreach (var ticketDetail in ticketDetails)
            {
                ticketDetail.IsLastDetail = false;
            }
            var newTicketEdition = new Ticket()
            {
                ProjectId = ticket.ProjectId,
                AreaId = ticket.AreaId,
                PriorityId = ticket.PriorityId,
                CategoryId = ticket.CategoryId,
                TicketStatusId = ticket.StatusId,
                Title = ticket.Title,
                Details = ticket.Content,
                IsHtml = true,
                CreatedBy = createdBy,
                CreatedDate = createdDate,
                AssignedTo = ticket.UserId,
                LastUpdateBy = ticket.LastUpdateBy,
                LastUpdateDate = DateTime.Now,
                OwnerUserId = ticket.OwnerUserId,
                ParentTicketId = null,
                EditionMarkAsDeleted = false,
                IsLastDetail = true,
                HasFiles = ticket.HasFiles,
                IsBillable = ticket.IsBillable,
                TicketNumber= ticketNumber
            };
            TicketRepository.Add(newTicketEdition);
            Save();
            return newTicketEdition;
        }

        public void AddFileData(Ticket ticket, List<FileData> listOfFiles)
        {
            //ticket.UpdateTicketNumber();
            if (listOfFiles.Count > 0)
            {
                listOfFiles.ForEach(file => FileDataRepository.Add(file));
            }
            Save();
        }

    
        public void NewDailyJob()
        {

        }

        public List<TicketListItem> GetOpenTicketsByUser(string userId)
        {
            var tickets = TicketRepository.GetTicketByUser(userId);

            return GetExtendedTickes(tickets);
        }

        public List<TicketListItem> RunSearch(int? projectId,int? categoryId,string userId, int? priorityId)
        {
            var tickets = TicketRepository.RunSearch(projectId, categoryId, userId, priorityId);

            return GetExtendedTickes(tickets);
        }

        
        public List<TicketListItem> GetExtendedTickes(List<Ticket> tickets)
        {

            var listOfTickets = (from t in tickets
                                join p in ProjectRepository.All() on t.ProjectId equals p.ProjectId
                                join c in CategoryRepository.All() on t.CategoryId equals c.CategoryId
                                join pp in PriorityRepository.All() on t.PriorityId equals pp.PriorityId
                                join aa in AreaRepository.All() on t.AreaId equals aa.AreaId
                                join u in  UserRepository.All() on t.OwnerUserId equals u.Id
                                join uu in UserRepository.All() on t.AssignedTo equals uu.Id
                                join st in StatusRepository.All() on t.TicketStatusId equals  st.TicketStatusId
                                 select new TicketListItem
                                {
                                    TicketDetailId = t.TicketDetailId,
                                    TicketNumber = t.TicketNumber.Value,
                                    ProjectId = p.ProjectId,
                                    ProjectName = p.ProjectName,
                                    CategoryId = c.CategoryId,
                                    Category = c.Name,
                                    PriorityId = pp.PriorityId,
                                    Priority = pp.Name,
                                    AreaId = aa.AreaId,
                                    AreaName = aa.Name,
                                    Title = t.Title,
                                    UserId = t.AssignedTo,
                                    AssignedUserName = uu.UserName,
                                    OwnerUserId = t.OwnerUserId,
                                    OwnerUserName = u.UserName,
                                    LastUpdateDate = t.LastUpdateDate.Value,
                                    Content = t.Details,
                                    IsLastDetail=t.IsLastDetail,
                                    IsBillable = t.IsBillable,
                                    StatusDescription= st.Name,
                                    CreatedDate = t.CreatedDate.Value,
                                    StatusId = st.TicketStatusId,
                                    Files = new List<FileData>()
                                }).ToList();

            foreach (var ticketListItem in listOfTickets)
            {
                ticketListItem.Files = FileDataRepository.Where(f => f.TicketDetailId == ticketListItem.TicketDetailId).ToList();
            }
            
            return listOfTickets;
        }

        public TicketHistoryData GetTicketHistory(int ticketNumber)
        {
            var tickets = TicketRepository.GetTicketDetails(ticketNumber);
            var extendedTickets= GetExtendedTickes(tickets).OrderByDescending(s=>s.TicketDetailId).ToList();
            var lastEditedTicket = extendedTickets.FirstOrDefault(t => t.IsLastDetail);
            return new TicketHistoryData {LastEditedTicket = lastEditedTicket, TicketListItems = extendedTickets};
        }

        public List<ProjectSummary> GetProjectsSummary()
        {
            var projects = ProjectRepository.All().ToList();
            return TicketRepository.GetProjectsSummary(projects);
        }

        public List<UserSummary> GetUserSummary()
        {
            var users = UserRepository.All().ToList();
            return TicketRepository.GetUserSummary(users);
        }

        public DashBoardSummaryDTO GetDashBoardSummary()
        {
            var numberOfUsers = UserRepository.All().Count();
            var numberOfTickets = TicketRepository.GetTickets().Count();
            var numberOfProjects = ProjectRepository.All().Count();
            var numberOfCustomers = CustomerRepository.All().Count();
            return new DashBoardSummaryDTO()
            {
                TotalCustomers = numberOfCustomers,
                TotalProjects = numberOfProjects,
                TotalTickets = numberOfTickets,
                TotalUsers = numberOfUsers
            };
        }

        public List<DashBoardOpenByPriority>  GetDashBoardOpenTicketByPriority(string projects)
        {
            var allProjects = ProjectRepository.All();
            string[] selectedProjects = !string.IsNullOrEmpty(projects) ? projects.Split(',') : allProjects.Select(p => p.ProjectId.ToString()).ToArray();

            var list = new List<DashBoardOpenByPriority>();
            var priorities = PriorityRepository.All();
            var tickets = TicketRepository.GetTickets();
            var totalTicketCount= tickets.Count(t=> selectedProjects.Contains(t.ProjectId.ToString()));
            foreach (var priority in priorities)
            {
                var numberOfTickets = tickets.Count(t => t.PriorityId == priority.PriorityId && selectedProjects.Contains(t.ProjectId.ToString()));
                var item = new DashBoardOpenByPriority()
                {
                    Priority = priority.Name,
                    NumberOfTickets = numberOfTickets,
                    Percentage = totalTicketCount > 0 ? (numberOfTickets * 100 / totalTicketCount)   : 0

                };
                list.Add(item);
            }
            return list;
        }

        public List<DashBoardStatusSummary> GetDashBoardStatusSummary(string projects)
        {
            var allProjects = ProjectRepository.All();
            string[] selectedProjects = !string.IsNullOrEmpty(projects) ? projects.Split(',') : allProjects.Select(p => p.ProjectId.ToString()).ToArray();

            var list = new List<DashBoardStatusSummary>();
            var statusList = StatusRepository.All();
            var tickets = TicketRepository.GetTickets();
            var totalTicketCount = tickets.Count(t => selectedProjects.Contains(t.ProjectId.ToString()));
            foreach (var status in statusList)
            {
                var numberOfTickets = tickets.Count(t => t.TicketStatusId == status.TicketStatusId && selectedProjects.Contains(t.ProjectId.ToString()));
                var item = new DashBoardStatusSummary()
                {
                    Status = status.Name,
                    NumberOfTickets = numberOfTickets,
                    Percentage = totalTicketCount > 0 ? (numberOfTickets * 100 / totalTicketCount)  : 0
                };
                list.Add(item);
            }
            return list;
        }
        

        public List<Project> GetProjects()
        {
            var projectsWithAreas = from p in ProjectRepository.All()
                join a in AreaRepository.All() on p.ProjectId equals a.ProjectId
                select p;
            return projectsWithAreas.ToList();
        }

        public List<Category> GetCategories()
        {
            return CategoryRepository.All().ToList();
        }

        public List<Priority> GetPriorities()
        {
            return PriorityRepository.All().ToList();
        }

        public IEnumerable<AreaDTO> GetAreasByProject(int projectId)
        {
            var areas= AreaRepository.Where(a => a.ProjectId == projectId);
            var query = (from a in areas
                select new AreaDTO() {AreaId = a.AreaId, Name = a.Name}).ToList();
            return query;
        }


        public void Save()
        {   
            context.SaveChanges();
        }

    }
}
