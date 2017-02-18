//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Collections;
using System.Data;
using System.Reflection;
using BigLamp.Extensions.IDataReader;

namespace BigLamp.Extensions.Dataset
{
    public static class DataRowExtensions
    {

        /// <summary>
        /// Returns a list of T which maps columns in the datarow to properties in T
        /// </summary>
        /// <typeparam name="T">Class to populate</typeparam>
        /// <param name="row">Row to populate from</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T GetAs<T>(this DataRow row) where T: class
        {
            // Create a new Object
            var newObjectToReturn = Activator.CreateInstance<T>();
            // Get all the properties in our Object
            var props = IDataReaderExtensions.GetCachedProperties<T>();
            // For each property get the data from the reader to the object
            var columnList = row.GetColumnList();
            foreach (var t in props)
            {
                if (columnList.Contains(t.Name) && row[t.Name] != DBNull.Value)
                {
                    if (!(t.PropertyType.IsTypeNullable()) & row.Table.Columns[t.Name].AllowDBNull)
                    {
                        throw new InvalidCastException(string.Format("Column '{0}' in datatable is nullable, corresponding property in class is not.", t.Name));
                    }
                    if (t.GetType().IsTypeNullable() & !(row.Table.Columns[t.Name].AllowDBNull))
                    {
                        throw new InvalidCastException(string.Format("Property '{0}' in class is nullable, corresponding column in datatable is not.", t.Name));
                    }
                    typeof(T).InvokeMember(t.Name, BindingFlags.SetProperty, null, newObjectToReturn, new[] {row[t.Name]});
                }
            }
            return newObjectToReturn;
        }

        public static bool ColumnExists(this DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName);
        }

        public static ArrayList GetColumnList(this DataRow row)
        {

            var columnList = new ArrayList();
            foreach (DataColumn column in row.Table.Columns)
            {
                columnList.Add(column.ColumnName);
            }
            return columnList;

        }

        /// <summary>
        /// [ <c>public static bool IsTypeNullable(Type typeToTest)</c> ]
        /// <para></para>
        /// Reports whether a given Type is nullable (Nullable&lt; Type &gt;)
        /// </summary>
        /// <param name="typeToTest">The Type to test</param>
        /// <returns>
        /// true = The given Type is a Nullable&lt; Type &gt;; false = The type is not nullable, or <paramref name="typeToTest"/> 
        /// is null.
        /// </returns>
        /// <remarks>
        /// This method tests <paramref name="typeToTest"/> and reports whether it is nullable (i.e. whether it is either a 
        /// reference type or a form of the generic Nullable&lt; T &gt; type).
        /// </remarks>
        public static bool IsTypeNullable(this Type typeToTest)
        {
            // Abort if no type supplied
            if (typeToTest == null)
            {
                return false;
            }

            // If this is not a value type, it is a reference type, so it is automatically nullable
            //  (NOTE: All forms of Nullable<T> are value types)
            if (!typeToTest.IsValueType)
            {
                return true;
            }

            // Report whether TypeToTest is a form of the Nullable<> type
            return typeToTest.IsGenericType && typeToTest.GetGenericTypeDefinition() == typeof(Nullable<>);
        }


    }
}
