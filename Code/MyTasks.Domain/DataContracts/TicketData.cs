using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Domain.Entities;
using MyTasks.Domain.Entities.Validators;
using MyTasks.Localization;
using MyTasks.Localization.Desktop;

namespace MyTasks.Domain.DataContracts
{
    public class TicketData
    {
        public int TicketDetailId { get; set; } = 0;

        public int TicketNumber { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "TitleIsRequired")]
        [LocalizedDisplayName("Title")]
        public string Title { get; set; }

       
        [LocalizedDisplayName("Content")]
        public string Content { get; set; }

        public int CustomerId { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "ProjectIsRequired")]
        public int ProjectId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "CategoryIdIsRequired")]
        public int CategoryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "PriorityIdIsRequired")]
        public int PriorityId { get; set; }

        public int StatusId { get; set; }

       public string StatusDescription { get; set; }

        public string LastUpdateBy { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "UserIdIsRequired")]
        public string UserId { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public string OwnerUserId { get; set; }

        public string OwnerUserName { get; set; }

        [LocalizedDisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Desktop), ErrorMessageResourceName = "AreaIdIsRequired")]
        public int AreaId { get; set; }

        public bool HasFiles { get; set; }

        public bool IsLastDetail { get; set; }

        public bool IsBillable { get; set; }

        public string AssignedUserName { get; set; }

        public List<FileData> Files { get; set; }

    }
}
