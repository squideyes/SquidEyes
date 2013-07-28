using System;
using System.Xml.Linq;
using Microsoft.VisualBasic.Devices;

namespace SquidEyes.Generic
{
    public class SoftwareInfo
    {
        internal SoftwareInfo()
        {
            Platform = Environment.OSVersion.Platform;
            OsName = new Computer().Info.OSFullName;
            OsVersion = Environment.OSVersion.Version;
            ServicePack = Environment.OSVersion.ServicePack;
            RuntimeVersion = Environment.Version;
            CultureInfo = new CultureInfo();
        }

        internal SoftwareInfo(XElement system)
        {
            var software = system.Element("software");

            Platform = ((string)software.Element("platform")).ToEnum<PlatformID>();
            OsName = ((string)software.Element("osName"));
            OsVersion = new Version((string)software.Element("osVersion"));
            ServicePack = (string)software.Element("servicePack");
            RuntimeVersion = new Version((string)software.Element("runtimeVersion"));
            CultureInfo = new CultureInfo(software);
        }

        public string OsName { get; private set; }
        public Version RuntimeVersion { get; private set; }
        public string ServicePack { get; private set; }
        public PlatformID Platform { get; private set; }
        public Version OsVersion { get; private set; }
        public CultureInfo CultureInfo { get; private set; }

        internal XElement GetElement()
        {
            return new XElement("software",
                new XElement("platform", Platform),
                new XElement("osName", OsName),
                new XElement("osVersion", OsVersion),
                new XElement("servicePack", ServicePack),
                new XElement("runtimeVersion", RuntimeVersion),
                CultureInfo.GetElement());
        }
    }
}