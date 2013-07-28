using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public class SystemInfo
    {
        internal SystemInfo()
        {
            ProgramInfo = new ProgramInfo();
            MachineInfo = new MachineInfo();
            IdentityInfo = new IdentityInfo();
            MemoryInfo = new MemoryInfo();
            SoftwareInfo = new SoftwareInfo();
        }

        internal SystemInfo(XElement system)
        {
            ProgramInfo = new ProgramInfo(system);
            MachineInfo = new MachineInfo(system);
            IdentityInfo = new IdentityInfo(system);
            MemoryInfo = new MemoryInfo(system);
            SoftwareInfo = new SoftwareInfo(system);
        }

        public ProgramInfo ProgramInfo { get; private set; }
        public MachineInfo MachineInfo { get; private set; }
        public IdentityInfo IdentityInfo { get; private set; }
        public MemoryInfo MemoryInfo { get; private set; }
        public SoftwareInfo SoftwareInfo { get; private set; }

        internal XElement GetElement()
        {
            XElement element = new XElement("system");

            element.Add(ProgramInfo.GetElement());
            element.Add(MachineInfo.GetElement());
            element.Add(IdentityInfo.GetElement());
            element.Add(MemoryInfo.GetElement());
            element.Add(SoftwareInfo.GetElement());

            return element;
        }
    }
}