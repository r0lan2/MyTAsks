using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using  MyTasks.Infrastructure.Configuration;

namespace MyTasks.Web.Infrastructure.Configuration
{
    public static class ApplicationInfo
    {
        public static string CompanyName()
        {
            return AssemblyInformation.CompanyName;
        }

        public static string BuildVersion()
        {
            var version = AssemblyInformation.Version;

            return string.Format("{0}.{1}.{2}.{3}",
                                    version.Major.ToString(CultureInfo.InvariantCulture),
                                    version.Minor.ToString(CultureInfo.InvariantCulture),
                                    version.Build.ToString(CultureInfo.InvariantCulture),
                                    version.Revision.ToString(CultureInfo.InvariantCulture));
        }

        public static string MajorVersion()
        {
            var version = AssemblyInformation.Version;

            return string.Format("{0}.{1}",
                                    version.Major.ToString(CultureInfo.InvariantCulture),
                                    version.Minor.ToString(CultureInfo.InvariantCulture));
        }

        public static string DbVersion()
        {
            var version = BigLamp.DatabaseInstaller.DatabaseInstallerByObject.GetInstalledDbVersion(
                MyTasks.Infrastructure.Configuration.Application.ConnectionString);

            return version.HasValue ? version.ToString() : string.Empty;
        }
    }
}