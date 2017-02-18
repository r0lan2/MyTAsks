using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.DataContracts
{
    public class UserSummary
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        //open tickets assigned cases for this user
        public int OpenTickets { get; set; }
        //closed tickets for this user
        public int ClosedTickets { get; set; }
        //resolved tickets for this user
        public int ResolvedTickets { get; set; }
    }
}
