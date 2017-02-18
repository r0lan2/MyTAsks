using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class TicketstatusMap : EntityTypeConfiguration<Ticketstatus>
    {
        public  TicketstatusMap()
        {
            // Primary Key
            this.HasKey(t => t.TicketStatusId);

            // Properties
            // Table & Column Mappings
            this.ToTable("ticketstatus");
            this.Property(t => t.TicketStatusId).HasColumnName("TicketStatusId");
            this.Property(t => t.Name).HasColumnName("Name");

            //// Relationships
            //this.HasRequired(t => t.Dictionary)
            //    .WithMany(t => t.DeviceTypes)
            //    .HasForeignKey(d => d.DictionaryId);
        }
    }
}
