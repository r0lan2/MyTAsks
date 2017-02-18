//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace BigLamp.Extensions.IDataReader
{
    /// <summary>
    /// Converted from C# to VB:  http://galratner.com/blogs/net/archive/2009/11/08/move-a-datareader-to-an-object-with-reflection-revisited.aspx
    /// </summary>
    /// <remarks></remarks>
    public static class IDataReaderExtensions
    {

        #region No caching
        /// <summary>
        /// Return the current row in the reader as an object
        /// </summary>
        /// <param name="reader">The Reader</param>
        /// <param name="objectToReturnType">The type of object to return</param>
        /// <returns>Object</returns>
        public static object GetNoCachingAs(this System.Data.IDataReader reader, Type objectToReturnType)
        {
            // Create a new Object
            var newObjectToReturn = Activator.CreateInstance(objectToReturnType);
            // Get all the properties in our Object
            var props = objectToReturnType.GetProperties();
            // For each property get the data from the reader to the object
            foreach (var t in props)
            {
                if (ColumnExists(reader, t.Name) && reader[t.Name] != DBNull.Value)
                {
                    objectToReturnType.InvokeMember(t.Name, BindingFlags.SetProperty, null, newObjectToReturn, new[] {reader[t.Name]});
                }
            }
            return newObjectToReturn;
        }
        public static T GetNoCachingAs<T>(this System.Data.IDataReader reader)
        {
            // Create a new Object
            var newObjectToReturn = Activator.CreateInstance<T>();
            // Get all the properties in our Object
            var props = typeof(T).GetProperties();
            // For each property get the data from the reader to the object
            foreach (var t in props)
            {
                if (reader.ColumnExists(t.Name) && reader[t.Name] != DBNull.Value)
                {
                    typeof(T).InvokeMember(t.Name, BindingFlags.SetProperty, null, newObjectToReturn, new[] {reader[t.Name]});
                }
            }
            return newObjectToReturn;
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Class to convert to</typeparam>
        /// <param name="reader">Datareader to convert</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T GetAs<T>(this System.Data.IDataReader reader) where T: class
        {
            // Create a new Object
            var newObjectToReturn = Activator.CreateInstance<T>();
            var propertyName = string.Empty;
            var propertyTypeName = string.Empty;
            var tableColumnType = string.Empty;
            try
            {
                // Get all the properties in our Object
                var props = GetCachedProperties<T>();
                // For each property get the data from the reader to the object
                var columnList = reader.GetColumnList();
                foreach (var t in props)
                {
                    propertyName = t.Name;
                    propertyTypeName = t.PropertyType.Name;
                    if (columnList.Contains(propertyName.ToLowerInvariant()) && reader[t.Name] != DBNull.Value)
                    {
                        tableColumnType = reader[propertyName].GetType().ToString();
                        typeof(T).InvokeMember(propertyName, BindingFlags.SetProperty, null, newObjectToReturn, new[] {reader[propertyName]});
                    }
                }
            }
            catch (MissingMethodException ex)
            {
                if (ex.Message.Contains("Method") && ex.Message.Contains("not found"))
                {
                    throw new MissingMemberException(string.Format("Type '{0}' of column in table matches type '{1}' of property '{2}' in class.", tableColumnType, propertyTypeName, propertyName), ex);
                }
            }
            return newObjectToReturn;
        }
        /// <summary>
        /// Returns a column in datareader to specified type
        /// Use Nullable(Of ) for columns that can be DbNull 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T GetItemAs<T>(this System.Data.IDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return reader.GetItemAsNullable<T>(columnName);
                }
                if (typeof(T) == typeof(string))
                {
                    return default(T);
                }
                throw new InvalidCastException(string.Format("Column {0} is DbNull, use a Nullable(Of {1}) type", columnName, typeof(T)));
            }
            return (T)reader[columnName];
        }

        public static T GetItemAs<T>(this System.Data.IDataReader reader, int columnIndex)
        {
            return reader.GetItemAs<T>(reader.GetName(columnIndex));
        }

        /// <summary>
        /// Check if an SqlDataReader contains a field
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="columnName">The column name</param>
        /// <returns></returns>
        public static bool ColumnExists(this System.Data.IDataReader reader, string columnName)
        {
            var schemaTable = reader.GetSchemaTable();
            if (schemaTable != null)
                schemaTable.DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            var dataTable = reader.GetSchemaTable();
            return dataTable != null && (dataTable.DefaultView.Count > 0);
        }

        public static ArrayList GetColumnList(this System.Data.IDataReader reader)
        {

            var columnList = new ArrayList();
            var readerSchema = reader.GetSchemaTable();
            if (readerSchema == null) throw new NullReferenceException("GetSchemaTable returned null");
            for (var i = 0; i < readerSchema.Rows.Count; i++)
            {
                columnList.Add(readerSchema.Rows[i]["ColumnName"].ToString().ToLowerInvariant());
            }
            return columnList;

        }

        private static T GetItemAsNullable<T>(this System.Data.IDataReader reader, string columnName)
        {
            var columnIndex = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(columnIndex))
            {
                return default(T);
            }
            return (T)reader[columnIndex];
        }
        #region Caching
        // Dictionary to store cached properites

        private static readonly IDictionary<string, PropertyInfo[]> PropertiesCache = new Dictionary<string, PropertyInfo[]>();

        // Help with locking

        private static readonly ReaderWriterLockSlim PropertiesCacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Get an array of PropertyInfo for this type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>PropertyInfo[] for this type</returns>
        public static PropertyInfo[] GetCachedProperties<T>()
        {
            if (PropertiesCacheLock.TryEnterUpgradeableReadLock(100))
            {
                PropertyInfo[] props;
                try
                {
                    if (!(PropertiesCache.TryGetValue(typeof(T).FullName, out props)))
                    {
                        props = typeof(T).GetProperties();
                        if (PropertiesCacheLock.TryEnterWriteLock(100))
                        {
                            try
                            {
                                PropertiesCache.Add(typeof(T).FullName, props);
                            }
                            finally
                            {
                                PropertiesCacheLock.ExitWriteLock();
                            }
                        }
                    }
                }
                finally
                {
                    PropertiesCacheLock.ExitUpgradeableReadLock();
                }
                return props;
            }
            return typeof(T).GetProperties();
        }
        #endregion
    }
}
