using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTasks.Domain.Base;

namespace MyTasks.Data.Base
{
    public static class ContextHelpers
    {
        //Only use with short lived contexts 
        public static void ApplyStateChangesOnDbContext(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<BaseObject>())
            {
                var stateInfo = entry.Entity;
                entry.State = StateHelpers.ConvertState(stateInfo.State);
            }
        }
    }
}
