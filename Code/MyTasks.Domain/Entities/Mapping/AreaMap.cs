using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class AreaMap : EntityTypeConfiguration<Area>
    {
        public AreaMap()
        {
            // Primary Key
            this.HasKey(t => t.AreaId);

            // Properties
            // Table & Column Mappings
            this.ToTable("area");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
        }
    }
}
