using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Domain.DataContracts
{
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string key)
            : base(FormatMessage(key))
        {
        }

        //TODO: Re add call to localization when project is ready
        private static string FormatMessage(string key)
        {
            //var value=  MyTasks.Localization.Desktop.Desktop.ResourceManager.GetString(key);
            //return value;
            return key;
        }
    }
}
