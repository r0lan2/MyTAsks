//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace BigLamp.Extensions.Dataset
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// Returns an arraylist of a selected column in a datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ArrayList ArrayListFromDataTable(this DataTable dt, string columnName)
        {

            if (dt == null)
            {
                throw new ArgumentNullException("dt");
            }

            var arr = new ArrayList();

            if (dt.Columns.Contains(columnName))
            {
                foreach (DataRow row in dt.Rows)
                {
                    arr.Add(row[columnName]);
                }
            }
            else
            {
                throw new IndexOutOfRangeException(columnName + " does not exist in " + dt.TableName);
            }

            return arr;

        }

        /// <summary>
        /// Checks if table has rows and returns true or false.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool HasRows(this DataTable table)
        {
            bool currenthasRows = false;
            if (table != null)
            {
                if (table.Rows.Count > 0)
                {
                    currenthasRows = true;
                }
            }
            return currenthasRows;
        }
        /// <summary>
        /// Gets the original value (before update) for a a column in a row.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetOriginalValueForColumnInDataRow(this DataTable table, int rowIndex, string columnName)
        {

            if (table == null)
            {
                throw new ArgumentNullException("table");
            }

            var orgRows = table.Select(null, null, DataViewRowState.OriginalRows);
            var row = orgRows[rowIndex];
            var rowValue = Convert.ToString(row[columnName, DataRowVersion.Original]);

            return rowValue;

        }
        /// <summary>
        /// Returns distinct values from selected column in datatable.
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="returnTableName">Name of table to be returned</param>
        /// <param name="columnName">Name of column to distinct</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable SelectDistinct(this DataTable sourceTable, string returnTableName, string columnName)
        {

            if (sourceTable == null)
            {
                throw new ArgumentNullException("sourceTable");
            }

            var dt = new DataTable(returnTableName) {Locale = CultureInfo.InvariantCulture};

            dt.Columns.Add(columnName, sourceTable.Columns[columnName].DataType);

//INSTANT C# WARNING: Every field in a C# anonymous type initializer is immutable:
//ORIGINAL LINE: Dim query = From row In sourceTable.AsEnumerable Select New With {.columnToDistinct = row.Field(Of Object)(columnName)}
            var query = from row in sourceTable.AsEnumerable()
                        select new {columnToDistinct = row.Field<object>(columnName)};

            foreach (var value in query.Select(u => u.columnToDistinct).Distinct())
            {
                dt.Rows.Add(new[] {value});
            }

            return dt;
        }
        /// <summary>
        /// Returns a pivoted version of the table
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable PivotTable(this DataTable source)
        {
            var dest = new DataTable("Pivoted" + source.TableName);

            dest.Columns.Add(" ");

//INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
//		DataRow r = null;
            foreach (DataRow r in source.Rows)
            {
                dest.Columns.Add(r[0].ToString());
            }
            for (var i = 0; i < source.Columns.Count - 1; i++)
            {
                dest.Rows.Add(dest.NewRow());
            }

            for (var i = 0; i < dest.Rows.Count; i++)
            {
//INSTANT C# TODO TASK: There is no C# equivalent to VB's implicit 'once only' variable initialization within loops:
                for (var c = 0; c < dest.Columns.Count; c++)
                {
                    if (c == 0)
                    {
                        dest.Rows[i][0] = source.Columns[(i + 1)].ColumnName;
                    }
                    else
                    {
                        dest.Rows[i][c] = source.Rows[(c - 1)][(i + 1)];
                    }
                }
            }
            dest.AcceptChanges();
            return dest;
        } //PivotTable

        public static List<T> GetAsList<T>(this DataTable table) where T: class
        {
            var data = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                data.Add(row.GetAs<T>());
            }
            return data;
        }
    }
}
