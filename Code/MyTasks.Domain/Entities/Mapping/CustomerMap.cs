using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class CustomerMap:  EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.CustomerId);

            // Properties
            // Table & Column Mappings
            this.ToTable("customer");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.MainContactName).HasColumnName("MainContactName");
            this.Property(t => t.MainContactEmail).HasColumnName("MainContactEmail");


        }
    }
}
