using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class PriorityMap : EntityTypeConfiguration<Priority>
    {
        public PriorityMap()
        {
            // Primary Key
            this.HasKey(t => t.PriorityId);

            // Properties
            // Table & Column Mappings
            this.ToTable("priority");
            this.Property(t => t.PriorityId).HasColumnName("PriorityId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");

            //// Relationships
            //this.HasRequired(t => t.Dictionary)
            //    .WithMany(t => t.DeviceTypes)
            //    .HasForeignKey(d => d.DictionaryId);
        }
    }
}
