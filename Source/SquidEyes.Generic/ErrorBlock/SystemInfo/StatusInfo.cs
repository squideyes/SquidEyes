using System;
using System.Xml.Linq;
using Microsoft.VisualBasic.Devices;

namespace SquidEyes.Generic
{
    public class StatusInfo
    {
        internal StatusInfo()
        {
            ShuttingDown = Environment.HasShutdownStarted;
            IsInteractive = Environment.UserInteractive;
            NetworkOnline = new Computer().Network.IsAvailable;
            OsOnlineFor = new TimeSpan(Environment.TickCount * 10000L);
        }

        internal StatusInfo(XElement machine)
        {
            var status = machine.Element("status");

            NetworkOnline = (bool)status.Element("networkOnline");
            ShuttingDown = (bool)status.Element("shuttingDown");
            IsInteractive = (bool)status.Element("isInteractive");
            OsOnlineFor = TimeSpan.Parse((string)status.Element("osOnlineFor"));
        }

        public bool IsInteractive { get; private set; }
        public bool NetworkOnline { get; private set; }
        public bool IsSecure { get; private set; }
        public TimeSpan OsOnlineFor { get; private set; }
        public bool ShuttingDown { get; private set; }

        internal XElement GetElement()
        {
            return new XElement(new XElement("status",
                new XElement("networkOnline", NetworkOnline),
                new XElement("shuttingDown", ShuttingDown),
                new XElement("isInteractive", IsInteractive),
                new XElement("osOnlineFor", OsOnlineFor.ToString())));
        }
    }
}