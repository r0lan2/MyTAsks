//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Data;

namespace BigLamp.Extensions.Dataset
{
    public static class DataSetExtensions
    {
        /// <summary>
        ///     Checks if any table in a dataset has rows and returns true or false.
        /// </summary>
        public static bool HasRows(this DataSet data)
        {
            return HasRows(data, -1);
        }
        /// <summary>
        ///     Checks if specific table in a dataset has rows and returns true or false.
        /// </summary>
        public static bool HasRows(this DataSet data, int tableIndex)
        {
            bool datasetHasRows = false;
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (tableIndex == -1)
            {
                foreach (DataTable tableWithinLoop in data.Tables)
                {
                    datasetHasRows = tableWithinLoop.HasRows();
                }
            }
            else
            {
                datasetHasRows = data.Tables[tableIndex].HasRows();
            }

            return datasetHasRows;

        }

        /// <summary>
        /// Gets the original value (before update) for a a column in a row.    
        /// </summary>
        public static string GetOriginalValueForColumnInDataRow(this DataSet data, string tableName, int rowIndex, string columnName)
        {
            return data.Tables[tableName].GetOriginalValueForColumnInDataRow(rowIndex, columnName);
        }
        /// <summary>
        /// Gets the original value (before update) for a a column in a row.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetOriginalValueForColumnInDataRow(this DataSet data, int tableIndex, int rowIndex, string columnName)
        {

            return data.Tables[tableIndex].GetOriginalValueForColumnInDataRow(rowIndex, columnName);

        }

        public static DataSet SelectDistinct(this DataSet ds, string sourceTableName, string returnTableName, string columnName)
        {
            var distinctDataset = new DataSet();
            distinctDataset.Tables.Add(ds.Tables[sourceTableName].SelectDistinct(returnTableName, columnName));
            return distinctDataset;
        }
    }
}
