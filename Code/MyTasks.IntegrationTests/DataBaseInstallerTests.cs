using BigLamp.DatabaseInstaller;
using NUnit.Framework;

namespace MyTasks.IntegrationTests
{
    public class DataBaseInstallerTests : IntegrationTestsBase
    {
       
        [Test]
        public void UpgradeDataBase()
        {
            DatabaseInstallerByObject.BuildDatabase(DatabaseInstallerByObject.ReadKeyFromPosition.Prefix, ConnectionString);
        }
        [Test]
        public void CleanInstallationFromScrath()
        {
            var rootConnectionString = "server=localhost;user=Developer;port=3306;password=holamundo;"; ;
            DatabaseInstallerByObject.CreateAndBuildDatabase(rootConnectionString, ConnectionString);
        }

        [Test]
        public void DeleteAndCreateDatabase()
        {
            var rootConnectionString = "server=localhost;user=Developer;port=3306;password=holamundo;"; ;
            DatabaseInstallerByObject.DeleteAndCreateDatabase(rootConnectionString, ConnectionString);
        }

    }
}
