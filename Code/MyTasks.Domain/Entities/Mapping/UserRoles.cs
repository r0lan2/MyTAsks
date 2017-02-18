
using MyTasks.Domain.Base;

namespace MyTasks.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class UserRoles : BaseObject
    {
        public UserRoles()
        {
            //this.Users = new HashSet<Users>();
            //this.Roles = new HashSet<Roles>();
        }

        public string UserId { get; set; }
        public string RoleId { get; set; }

        //public virtual ICollection<Users> Users { get; set; }
        //public virtual ICollection<Roles> Roles { get; set; }


    }
}
