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
            DatabaseInstallerByObject.CreateAndBuildDatabase(RootConnectionString, ConnectionString);
        }

        [Test]
        public void DeleteAndCreateDatabase()
        {
            DatabaseInstallerByObject.DeleteAndCreateDatabase(RootConnectionString, ConnectionString);
        }

    }
}
