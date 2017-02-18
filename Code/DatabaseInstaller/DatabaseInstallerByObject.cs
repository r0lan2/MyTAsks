using System.Linq;
using BigLamp.DatabaseInstaller.Configuration;
using BigLamp.DatabaseInstaller.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using BigLamp.Extensions.Assembly;

namespace BigLamp.DatabaseInstaller
{
    public class DatabaseInstallerByObject
    {

        private DatabaseInstallerByObject()
        {
            //Empty constructor to avoid CA1053:StaticHolderTypesShouldNotHaveConstructor violation from FxCop. 
        }

        #region "Private variables"
      
        public enum ReadKeyFromPosition
        {
            Prefix,
            Suffix,
            None
        }
        public enum DbScriptType
        {
            CreateDatabase,
            Programmability,
            ChangeScript
        }
        private const string InstallScriptsAssemblyNamespace = "DatabaseInstaller.Resources";
        private static Assembly InstallScriptsAssembly
        {
            get { return Assembly.Load(InstallScriptsAssemblyNamespace); }
        }

        #endregion

        #region "Db Version"


        public static bool IsDatabasseInstalled(string rootConnectionString, string connectionstring)
        {
            //return (GetInstalledDbVersion(connectionString) != null);
            var databaseName = GetDatabaseName(connectionstring);
            bool databaseIsFound = false;
            using (var conn = new MySqlConnection(rootConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SHOW DATABASES LIKE '"+ databaseName + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            databaseIsFound = reader.HasRows;
                        }
                    }
                }
            }

            return databaseIsFound;

        }

        public static long? GetInstalledDbVersion(string connectionstring)
        {
            object result = null;
            using (var sqlConn = new MySqlConnection(connectionstring))
            {
                sqlConn.Open();
                var sqlCmd = new MySqlCommand("select max(version) from DbVersion;", sqlConn);

                try
                {
                       result = sqlCmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    result = null;
                }
            }

            return (long?) result;
        }

        public static long? CheckInstalledDBVersion(string connectionstring)
        {
            using (var sqlConn = new MySqlConnection(connectionstring))
            {
                
                sqlConn.Open();
                var sqlCmd = new MySqlCommand("select max(version) from DbVersion;", sqlConn);
                object result = null;

                try
                {
                    result = sqlCmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    CreateDbVersionTable(connectionstring);
                    result = null;
                }

                if (result == null)
                {
                    return null; //dbVersion does not exist - run all buildscripts (new database)
                }

                if (Convert.IsDBNull(result))
                {
                    return 0; //dbVersion exists but is empty - run all changescripts (upgrade old database)
                }

                if (result is long | result is int)
                {
                    return Convert.ToInt64(result);
                }
                throw new DatabaseInstallerException("Return value for version is not integer");
            }
        }

        public  static void CreateDbVersionTable(string connectionstring)
        {
            string sqlQuery =
                "create table DbVersion (Version bigint not null,ScriptFileName text, primary key (Version));";
            try
            {
                ExecuteSql(sqlQuery, connectionstring);
            }
            catch (Exception)
            {
                throw new DatabaseInstallerException("Failed to create table DbVersion");
            }
        }

        public static void InsertDbVersion(long dbversion, string scriptFileName, string connectionstring)
        {
            using (var sqlConn = new MySqlConnection(connectionstring))
            {
                //INSERT INTO dbversion (`Version`, `ScriptFileName`, `ScripExecutedOn`) VALUES ('3', 'rola.sql', '20150101');
                sqlConn.Open();
                MySqlCommand sqlCmd = null;
                string cmd = @"INSERT INTO dbversion VALUES (" + dbversion + ",'" + scriptFileName + "');";
                sqlCmd = new MySqlCommand(cmd, sqlConn);
              
                int result = 0;
                try
                {
                    result = sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    result = 0;
                }
                if (result == 0)
                {
                    throw new DatabaseInstallerException("No version number inserted into DbVersion");
                }
            }
        }
        #endregion

        /// <summary>
        /// Method to be used by all other apps.
        /// </summary>
        /// <param name="changeScriptsReadKeyFromPosition">
        /// Determins if dbversion/orderkey should be
        /// read from start or end of filename</param>
        /// <param name="connectionString">
        /// Use this to run old installer with views,proc and functions in changescripts
        /// </param>
        /// <remarks></remarks>
        public static void BuildDatabase(ReadKeyFromPosition changeScriptsReadKeyFromPosition, string connectionString)
        {
            long? dbVersion = CheckInstalledDBVersion(connectionString);

            // Run new change scripts
            ExecuteSqlScripts(dbVersion, DbScriptType.ChangeScript, ReadKeyFromPosition.Prefix, true, connectionString, "ChangeScripts.Main");

            ExecuteSqlScriptsByObject(connectionString);

        }

        public static void CreateDatabaseIfDoesntExists(string rootConnectionString, string databaseName)
        {
            ExecuteSql("CREATE DATABASE IF NOT EXISTS `" + databaseName + "`;", rootConnectionString);
        }

        public static void DeleteDatabaseIfExists(string rootConnectionString, string databaseName)
        {
            ExecuteSql("DROP DATABASE IF EXISTS `" + databaseName + "`;", rootConnectionString);
        }

        public static void DeleteAndCreateDatabase(string rootConnectionString, string connectionString)
        {
            var databaseName = GetDatabaseName(connectionString);
            DeleteDatabaseIfExists(rootConnectionString, databaseName);
            CreateDatabaseIfDoesntExists(rootConnectionString, databaseName);
            DatabaseInstallerByObject.BuildDatabase(DatabaseInstallerByObject.ReadKeyFromPosition.Prefix, connectionString);
        }

        private static string GetDatabaseName(string connectionString)
        {
            var sqlConn = new MySqlConnection(connectionString);
            var databaseName = sqlConn.Database;
            return databaseName;
        }

        public static void CreateAndBuildDatabase(string rootConnectionString,string connectionString)
        {
            var databaseName = GetDatabaseName(connectionString);
            CreateDatabaseIfDoesntExists(rootConnectionString, databaseName);
            DatabaseInstallerByObject.BuildDatabase(DatabaseInstallerByObject.ReadKeyFromPosition.Prefix, connectionString);
        }

        public static void BuildDatabase(string connectionString)
        {
           ExecuteSqlScripts(null, DbScriptType.CreateDatabase, ReadKeyFromPosition.Prefix, false, connectionString, "CreateDatabaseIfDoesntExists");
        }
        
        /// <summary>
        /// Used by TestBase to avoid running dbinstaller when changescript are at latest version
        /// </summary>
        /// <param name="currentReadKeyFromPosition"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool HasDbVersionChanged(ReadKeyFromPosition currentReadKeyFromPosition, string connectionString)
        {
            long? dbVersion = CheckInstalledDBVersion(connectionString);
            var scriptFilesToRun = GetScriptFilesToRun(dbVersion, DbScriptType.ChangeScript, currentReadKeyFromPosition, "ChangeScripts.Main");
            return scriptFilesToRun.Any();
        }
        public static void ExecuteSqlScripts(long? dbVersion, DbScriptType scriptType, ReadKeyFromPosition currentReadKeyFromPosition, bool useFileNameAsChangeVersion, string connectionstring, params string[] includeFolders)
        {
            ExecuteSqlScripts(dbVersion, scriptType, currentReadKeyFromPosition, useFileNameAsChangeVersion, connectionstring, string.Empty, string.Empty, includeFolders);
        }

        public static void ExecuteSqlScripts(long? dbVersion, DbScriptType scriptType, ReadKeyFromPosition currentReadKeyFromPosition, bool useFileNameAsChangeVersion, string connectionstring, string dbPrefixToReplace, string dbPrefixReplacement, params string[] includeFolders)
        {
            dynamic scriptFilesToRun = GetScriptFilesToRun(dbVersion, scriptType, currentReadKeyFromPosition, includeFolders);
            ExecuteSqlScripts(scriptFilesToRun, scriptType, useFileNameAsChangeVersion, connectionstring,  true, dbPrefixToReplace, dbPrefixReplacement);
        }

        private static SortedList<long, string> ExecuteSqlScripts(SortedList<long, string> scriptFilesToRun,
                                                                  DbScriptType scriptType,
                                                                  bool useFileNameAsChangeVersion,
                                                                  string connectionstring, bool failOnException = true,
                                                                  string dbPrefixToReplace = "",
                                                                  string dbPrefixReplacement = "")
        {
            //Execute scripts
            var failedScripts = new SortedList<long, string>(scriptFilesToRun); //clone list
            Assembly asm = null;
            asm = InstallScriptsAssembly;

  
            foreach (long key in scriptFilesToRun.Keys)
            {
                try
                {
                    var filename = scriptFilesToRun[key];
                    Debug.WriteLine("Executing sql file " + filename + ".");
                    string sqlScript = null;
                    var sr = new StreamReader(asm.GetManifestResourceStream(filename));
                    sqlScript = sr.ReadToEnd();
                    if (!string.IsNullOrEmpty(dbPrefixReplacement))
                    {
                        sqlScript = sqlScript.Replace(dbPrefixToReplace, dbPrefixReplacement);
                    }
                    //if (!ExludeItem(filename))
                    //{
                        ExecuteSql(sqlScript, connectionstring, filename);
                        if (useFileNameAsChangeVersion)
                        {
                            InsertDbVersion(key,
                                            new FileInfo(filename).Name.Replace(
                                                asm.InferDefaultNamespace() + "." + "ChangeScripts" + ".", string.Empty),
                                            connectionstring);
                        }
                    //}
                    failedScripts.Remove(key);
                }
                catch 
                {
                    if (failOnException)
                    {
                        throw;
                    }
                }

            }

            return failedScripts;
        }
        private static bool ExludeItem(string filename)
        {
            var excludeFile = false;
            foreach (ObjectName regE in DatabaseObjectsListConfiguration.GetConfig().Exclude)
            {
                if (Regex.IsMatch(filename, regE.FieldName, RegexOptions.IgnoreCase))
                {
                    excludeFile = true;
                }
            }
            return excludeFile;
        }

        private static void ExecuteSqlScriptsByObject(string connectionstring)
        {
            //DropAllObjects(connectionstring);

            var scriptFilesToRun = GetScriptFilesToRun(null, DbScriptType.Programmability,
                                                       ReadKeyFromPosition.None, "Functions",
                                                       "Procedures", "Views", "Triggers");

            var previousCount = scriptFilesToRun.Count;
            var i = 1;
            while (scriptFilesToRun.Count > 0)
            {
                scriptFilesToRun = ExecuteSqlScripts(scriptFilesToRun, DbScriptType.Programmability, false,
                                                     connectionstring, false);
                if (scriptFilesToRun.Count == previousCount)
                {
                    // Run with exception enabled to show error for first failing script.
                    scriptFilesToRun = ExecuteSqlScripts(scriptFilesToRun, DbScriptType.Programmability, false,
                                                         connectionstring, true);
                }
                previousCount = scriptFilesToRun.Count;
                if (scriptFilesToRun.Count > 0)
                {
                    Debug.WriteLine(string.Format("Dependency retry {0}. Retrying {1} script(s). ", i,
                                                  scriptFilesToRun.Count));
                }
                i += 1;
            }

        }

        private static SortedList<long, string> GetScriptFilesToRun(long? dbVersion, DbScriptType scriptType,
                                                                    ReadKeyFromPosition currentReadKeyFromPosition,
                                                                    params string[] includeFolders)
        {
            var scriptFilesToRun = new SortedList<long, string>();
            Assembly asm = null;
            asm = InstallScriptsAssembly;

            string[] files = asm.GetManifestResourceNames();

            int index = 1;

            foreach (string scriptFile in files)
            {
                var currentIncludeFolder = string.Empty;

                foreach (string includeFolder in includeFolders)
                {
                    if (scriptFile.Contains(string.Format(".{0}.", includeFolder)))
                    {
                        currentIncludeFolder = includeFolder;
                    }
                }

                if (!string.IsNullOrEmpty(currentIncludeFolder))
                {
                    var keyAsString = string.Empty;
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(scriptFile);
                    var indexOfVersionUnderscore = 0;
                    var tempFileNameForVersionSearch = fileNameWithoutExtension.Replace(" ", "_");

                    // Remove namespace from filename
                    //tempFileNameForVersionSearch = tempFileNameForVersionSearch.Replace(String.Format("{0}.{1}.", asm.InferDefaultNamespace(), currentIncludeFolder), string.Empty);
                    tempFileNameForVersionSearch = tempFileNameForVersionSearch.Replace(String.Format("{0}.{1}.", InstallScriptsAssemblyNamespace, currentIncludeFolder), string.Empty);

                    if (currentReadKeyFromPosition == ReadKeyFromPosition.Suffix & (scriptType == DbScriptType.CreateDatabase | scriptType == DbScriptType.ChangeScript))
                    {
                        indexOfVersionUnderscore = tempFileNameForVersionSearch.LastIndexOf('_');
                        keyAsString = tempFileNameForVersionSearch.Substring(indexOfVersionUnderscore + 1);
                    }
                    else if (currentReadKeyFromPosition == ReadKeyFromPosition.Prefix & (scriptType == DbScriptType.CreateDatabase | scriptType == DbScriptType.ChangeScript))
                    {
                        indexOfVersionUnderscore = tempFileNameForVersionSearch.IndexOf('_');
                        keyAsString = tempFileNameForVersionSearch.Substring(0, indexOfVersionUnderscore);
                    }
                    else
                    {
                        keyAsString = index.ToString();
                    }

                    long key = 0;
                    bool parseResult = long.TryParse(keyAsString, out key);
                    if (!parseResult)
                    {
                        throw new DatabaseInstallerException(string.Format(CultureInfo.InvariantCulture,
                                                                           "Failed to extract version number from file {0} ",
                                                                           scriptFile));
                    }
                    if (dbVersion.HasValue == false || (key > dbVersion.Value))
                    {
                        scriptFilesToRun.Add(key, scriptFile);
                    }

                }

                index += 1;

            }

            return scriptFilesToRun;
        }



        public static void ExecuteSql(string sqlScript, string connectionstring, string filename = "")
        {
            //var delimiter = string.Format("{0}GO", Environment.NewLine);
            //var arrSql =    Strings.Split(sqlScript, delimiter, -1, CompareMethod.Text);
            var arrSql =  new string[]{sqlScript};

            using (var sqlConn = new MySqlConnection(connectionstring))
            {
                sqlConn.Open();
                var transaction = sqlConn.BeginTransaction();
                
                foreach (string sqlScriptSplitted in arrSql)
                {
                    if (!(string.IsNullOrEmpty(sqlScriptSplitted)))
                    {
                        try
                        {
                            var command = new MySqlCommand(sqlScriptSplitted, sqlConn, transaction) {CommandTimeout = 0};
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            if (!(sqlScriptSplitted.Contains("[IgnoreError]")))
                            {
                                transaction.Rollback();
                                if ((string.IsNullOrEmpty(filename)))
                                {
                                    throw new DatabaseInstallerException(string.Format(CultureInfo.InvariantCulture, "Failed to execute SQL. Error message was: {0}. Failed SQL: {1}", ex.Message, sqlScriptSplitted), ex);
                                }
                                throw new DatabaseInstallerException(string.Format(CultureInfo.InvariantCulture, "Failed to execute file {0}. Error message was: {1}. Failed SQL: {2}", Strings.Replace(filename, InstallScriptsAssemblyNamespace, string.Empty), sqlScriptSplitted, ex.Message), ex);
                            }
                        }
                    }
                }
                transaction.Commit();
            }
        }
    }
}

