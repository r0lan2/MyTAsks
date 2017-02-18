using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



namespace MyTasks.Infrastructure.Configuration
{
    public class ConfigurationSectionBase: ConfigurationSection
    {
        protected PropertyInformation get_PropertyInformation(string name)
        {
            System.Configuration.
            PropertyInformation propertyInformation = this.ElementInformation.Properties[name];
            if (propertyInformation != null)
                return propertyInformation;
            throw new ConfigurationErrorsException( string.Format("No Property with name {0} in configuration section {0}", (object)name, (object)this.SectionInformation.Name));
        }


        protected object get_PropertyValue(string name)
        {
            return this.get_PropertyInformation(name).Value;
        }

        protected void set_PropertyValue(string name, object value)
        {
            this.get_PropertyInformation(name).Value = RuntimeHelpers.GetObjectValue(value);
        }

        protected string get_PropertyValueAsString(string name)
        {
            object objectValue = RuntimeHelpers.GetObjectValue(this.get_PropertyValue(name));
            if (objectValue == null)
                return (string)null;
            string str = objectValue as string;
            if (str != null)
                return str;
            else
                throw new Exceptions.InvalidCastException(typeof(string), RuntimeHelpers.GetObjectValue(objectValue), name);
        }

        protected void set_PropertyValueAsString(string name, string value)
        {
            this.set_PropertyValue(name, (object)value);
        }

        protected Uri get_PropertyValueAsUri(string name)
        {
            object objectValue = RuntimeHelpers.GetObjectValue(this.get_PropertyValue(name));
            if (objectValue == null)
                return (Uri)null;
            Uri uri = objectValue as Uri;
            if (uri != null)
                return uri;
            else
                throw new Exceptions.InvalidCastException(typeof(Uri), RuntimeHelpers.GetObjectValue(objectValue), name);
        }

        protected void set_PropertyValueAsUri(string name, Uri value)
        {
            this.set_PropertyValue(name, (object)value);
        }


    }
}
