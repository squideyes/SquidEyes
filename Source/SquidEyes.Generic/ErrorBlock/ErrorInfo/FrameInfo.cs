using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public class FrameInfo
    {
        internal FrameInfo(StackFrame stackFrame)
        {
            Contract.Requires(stackFrame != null);

            Interface = stackFrame.GetMethod().GetInterface();
        }

        internal FrameInfo(XElement frame)
        {
            Interface = (string)frame.Attribute("interface");
        }

        public string Interface { get; private set; }

        internal XElement GetElement()
        {
            var element = new XElement("frame",
                new XAttribute("interface", Interface));

            return element;
        }
    }
}