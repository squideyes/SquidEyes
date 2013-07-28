using System;
using System.Security.Policy;
using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public class MachineInfo
    {
        internal MachineInfo()
        {
            Domain = Environment.UserDomainName;

            MachineName = Environment.MachineName;

            if (System.TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
                TimeZone = System.TimeZone.CurrentTimeZone.DaylightName;
            else
                TimeZone = System.TimeZone.CurrentTimeZone.StandardName;

            string basePath =
                AppDomain.CurrentDomain.GetData("APPBASE").ToString();

            SecurityZone = Zone.CreateFromUrl(basePath).SecurityZone.ToString();

            StatusInfo = new StatusInfo();
        }

        internal MachineInfo(XElement system)
        {
            var machine = system.Element("machine");

            Domain = (string)machine.Element("domain");
            MachineName = (string)machine.Element("machineName");
            TimeZone = (string)machine.Element("timeZone");
            SecurityZone = (string)machine.Element("securityZone");
            StatusInfo = new StatusInfo(machine);
        }

        public StatusInfo StatusInfo { get; private set; }
        public string Domain { get; private set; }
        public string MachineName { get; private set; }
        public string TimeZone { get; private set; }
        public string SecurityZone { get; private set; }

        internal XElement GetElement()
        {
            return new XElement(new XElement("machine", 
                new XElement("domain", Domain),
                new XElement("machineName", MachineName),
                new XElement("timeZone", TimeZone),
                new XElement("securityZone", SecurityZone),
                StatusInfo.GetElement()));
        }
    }
}