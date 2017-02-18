using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain
{
    public class Dbversion
    {
        public long Version { get; set; }
        public string ScriptFileName { get; set; }
    }
}
