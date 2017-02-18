using System.Configuration;

namespace BigLamp.DatabaseInstaller.Configuration
{

    public class ObjectNamesCollection : ConfigurationElementCollection
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ObjectName)element).FieldName;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ObjectName();
        }

        //ORIGINAL LINE: Public Property HiddenDimensionCollection(ByVal index As Integer) As ObjectName
        //INSTANT C# NOTE: C# does not support parameterized properties - the following property has been divided into two methods:
        public ObjectName HiddenDimensionCollection(int index)
        {
            return (ObjectName)(BaseGet(index));
        }

        public void set_HiddenDimensionCollection(int index, ObjectName value)
        {
            if (base.BaseGet(index) != null)
            {
                base.BaseRemoveAt(index);
            }
            base.BaseAdd(index, value);
        }
    }
}


