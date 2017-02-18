using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class RolesMap : EntityTypeConfiguration<Roles>
    {
        public RolesMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("roles");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");

            //// Relationships
            //this.HasRequired(t => t.Dictionary)
            //    .WithMany(t => t.DeviceTypes)
            //    .HasForeignKey(d => d.DictionaryId);
        }
    }
}
