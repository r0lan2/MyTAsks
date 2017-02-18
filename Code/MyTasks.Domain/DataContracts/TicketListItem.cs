using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.DataContracts
{
    public class TicketListItem : TicketData
    {
        public string ProjectName { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string AreaName { get; set; }
        public bool IsFirstEdition { 
            get { return TicketDetailId == TicketNumber; }
        }
    }
}
