using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.Entities.Mapping
{
    public class UserRolesMap : EntityTypeConfiguration<UserRoles>
    {
        public UserRolesMap()
        {
            // Properties
            // Table & Column Mappings
            this.ToTable("userroles");
            HasKey(t => new {t.UserId, t.RoleId});
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
        }
    }
}
