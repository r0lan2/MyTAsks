using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTasks.Domain;
using MyTasks.Domain.DataContracts;

namespace MyTasks.Web.Models
{
    public class SearchViewModel
    {
        public int SelectedProjectId { get; set; }
        public List<Project> Projects { get; set; }
        //public int SelectedAreaId { get; set; }
        //public int SelectedCategoryId { get; set; }
        //public int SelectedAssignToUserdId { get; set; }
        //public int SelectedPriorityId { get; set; }

        public List<TicketListItem> SearchResult { get; set; }

    }
}