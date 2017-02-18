using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain
{
    public class Area
    {
        public Area()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        public int AreaId { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }

        [NotMapped]
        public bool IsUsed { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
