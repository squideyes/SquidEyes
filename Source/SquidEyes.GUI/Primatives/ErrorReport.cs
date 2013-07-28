using SquidEyes.Generic;
using SquidEyes.Shared;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace SquidEyes.GUI
{
    [Serializable]
    public class ErrorReport : IModelData<ErrorReport>
    {
        public const int MaxAttachmentSize = 1024 * 1024 * 2;

        [DataMember(IsRequired = true)]
        public ErrorBlock ErrorBlock { get; set; }

        [DataMember(IsRequired = false)]
        public string Message { get; set; }

        [DataMember(IsRequired = false)]
        public string FileName { get; set; }

        [DataMember(IsRequired = false)]
        public Guid Guid { get; set; }

        public ErrorReport Clone()
        {
            return new ErrorReport();
        }

        public XElement ToElement()
        {
            return new XElement("errorReport");
        }

        public bool IsValid
        {
            get
            {
                return true;
            }
        }

        public string NameOnly
        {
            get
            {
                return Path.GetFileName(FileName);
            }
        }

        public string BlobName
        {
            get
            {
                return Guid + Path.GetExtension(FileName);
            }
        }

        public XDocument ToDocument()
        {
            var root = 
                new XElement("errorReport", ErrorBlock.ToElement());

            var doc = new XDocument(root);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                var message = String.Join(
                    Environment.NewLine, Message.ToLines().ToArray());

                if (!string.IsNullOrWhiteSpace(message))
                    root.Add(new XElement("message", message));
            }

            if (!string.IsNullOrWhiteSpace(FileName))
            {
                root.Add(new XElement("fileName", NameOnly));
                root.Add(new XElement("blobName", BlobName));
            }

            return doc;
        }

        public void Validate()
        {
            //ErrorBlock.Property("ErrorBlock").IsNotNull();

            //Message.Property("Message").IsValid(
            //    (Message == null) || !string.IsNullOrWhiteSpace(Message),
            //    "must be set to NULL or non-whitespace");

            //if (FileName != null)
            //{
            //    Guid.Property("Guid").IsNotDefault();

            //    FileName.Property("FileName").IsValid(
            //        FileName.IsTrimmed() && Path.HasExtension(FileName),
            //        "must be set to a valid filename with an extension");
            //}
        }
    }
}
