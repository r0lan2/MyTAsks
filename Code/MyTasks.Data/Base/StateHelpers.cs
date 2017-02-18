using System.Data;
using System.Data.Entity;
using MyTasks.Domain.Base;

namespace MyTasks.Data.Base
{
  class StateHelpers
  {
    public static EntityState ConvertState(State state)
    {
      switch (state)
      {
        case State.Added:
          return EntityState.Added;
        case State.Modified:
          return EntityState.Modified;
        case State.Deleted:
          return EntityState.Deleted;
        default:
          return EntityState.Unchanged;
      }
    }
  }
}
