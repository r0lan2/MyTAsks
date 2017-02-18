using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Web.Models;

namespace MyTasks.Web.Extensions
{
    public static class UserExtensions
    {
        public static async Task<List<ApplicationUser>> AsyncToList(this List<ApplicationUser> users)
        {
            return await Task.Run(() => users.ToList());
        }
    }
}
