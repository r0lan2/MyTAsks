using System.ComponentModel.DataAnnotations.Schema;


namespace MyTasks.Domain.Base
{
    public class BaseObject
    {
        [NotMapped]
        public State State { get; set; }
    }
}
