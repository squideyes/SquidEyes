using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public class ErrorInfo
    {
        public ErrorInfo(Exception error, ErrorKind errorKind)
        {
            Contract.Requires(error != null);

            ErrorKind = errorKind;
            Message = error.Message;
            ErrorType = error.GetType().ToString();
            Source = error.Source;

            FrameInfos = new List<FrameInfo>();

            var stackTrace = new StackTrace(error, true);

            foreach (var stackFrame in stackTrace.GetFrames())
                FrameInfos.Add(new FrameInfo(stackFrame));
        }

        internal ErrorInfo(XElement error)
        {
            ErrorKind = ((string)error.Attribute("kind")).ToEnum<ErrorKind>();
            Message = (string)error.Element("message");
            Source = (string)error.Element("source");
            ErrorType = (string)error.Element("type");

            FrameInfos = new List<FrameInfo>();

            foreach (var frame in error.Element("stack").Elements())
                FrameInfos.Add(new FrameInfo(frame));
        }

        public ErrorKind ErrorKind { get; private set; }
        public string Message { get; private set; }
        public string ErrorType { get; private set; }
        public string Source { get; private set; }
        public List<FrameInfo> FrameInfos { get; private set; }

        public XElement GetElement()
        {
            XElement element = new XElement("error",
                new XAttribute("kind", ErrorKind),
                new XElement("message", Message),
                new XElement("source", Source),
                new XElement("type", ErrorType),
                new XElement("stack",
                    from i in FrameInfos
                    select i.GetElement()));

            return element;
        }
    }
}
