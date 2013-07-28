using System;
using System.Xml.Linq;
using Microsoft.VisualBasic.Devices;

namespace SquidEyes.Generic
{
    public class MemoryInfo
    {
        internal MemoryInfo()
        {
            var computer = new Computer();

            WorkingSet = Environment.WorkingSet;
            TotalVirtual = (long)computer.Info.TotalVirtualMemory;
            TotalPhysical = (long)computer.Info.TotalPhysicalMemory;
            FreeVirtual = (long)computer.Info.AvailableVirtualMemory;
            FreePhysical = (long)computer.Info.AvailablePhysicalMemory;
        }

        internal MemoryInfo(XElement system)
        {
            var memory = system.Element("memory");

            WorkingSet = (long)memory.Element("workingSet");
            TotalVirtual = (long)memory.Element("totalVirtual");
            TotalPhysical = (long)memory.Element("totalPhysical");
            FreeVirtual = (long)memory.Element("freeVirtual");
            FreePhysical = (long)memory.Element("freePhysical");
        }

        public long TotalVirtual { get; private set; }
        public long TotalPhysical { get; private set; }
        public long FreeVirtual { get; private set; }
        public long FreePhysical { get; private set; }
        public long WorkingSet { get; private set; }

        internal XElement GetElement()
        {
            return new XElement("memory",
                new XElement("workingSet", WorkingSet),
                new XElement("totalVirtual", TotalVirtual),
                new XElement("totalPhysical", TotalPhysical),
                new XElement("freeVirtual", FreeVirtual),
                new XElement("freePhysical", FreePhysical));
        }
    }
}