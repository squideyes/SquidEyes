using System;
using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public class ProgramInfo
    {
        internal ProgramInfo()
        {
            var currentDomain = AppDomain.CurrentDomain;

            Name = currentDomain.FriendlyName;
            BasePath = currentDomain.GetData("APPBASE").ToString();
            ConfigFile = currentDomain.GetData("APP_CONFIG_FILE").ToString();
            CommandLine = Environment.CommandLine;
        }

        internal ProgramInfo(XElement system)
        {
            var application = system.Element("program");

            Name = (string)application.Element("name");
            BasePath = (string)application.Element("basePath");
            ConfigFile = (string)application.Element("configFile");
            CommandLine = (string)application.Element("commandLine");
        }

        public string Name { get; private set; }
        public string BasePath { get; private set; }
        public string ConfigFile { get; private set; }
        public string CommandLine { get; private set; }

        internal XElement GetElement()
        {
            return new XElement("program",
                new XElement("name", Name),
                new XElement("basePath", BasePath),
                new XElement("configFile", ConfigFile),
                new XElement("commandLine", CommandLine));
        }
    }
}