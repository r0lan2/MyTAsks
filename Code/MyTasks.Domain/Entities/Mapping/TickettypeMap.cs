using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class TickettypeMap : EntityTypeConfiguration<Tickettype>
    {
        public TickettypeMap()
        {
            // Primary Key
            this.HasKey(t => t.TicketTypeId);

            // Properties
            // Table & Column Mappings
            this.ToTable("tickettype");
            this.Property(t => t.TicketTypeId).HasColumnName("TicketTypeId");
            this.Property(t => t.Name).HasColumnName("Name");
          
        }

    }
}
