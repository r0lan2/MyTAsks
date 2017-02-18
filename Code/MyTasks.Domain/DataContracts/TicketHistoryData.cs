using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.DataContracts
{
    public class TicketHistoryData
    {
        public List<TicketListItem> TicketListItems { get; set; }
        public TicketListItem LastEditedTicket { get; set; }
    }
}
