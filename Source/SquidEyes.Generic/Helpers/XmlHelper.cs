using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public static class XmlHelper
    {
        public const string NullPhrase = "{NULL}";

        public static string ValueOrNullPhrase(XAttribute attribute)
        {
            if (attribute == null)
                return NullPhrase;
            else
                return attribute.Value ?? NullPhrase;
        }

        public static string ValueOrNull(XAttribute attribute)
        {
            var value = attribute.Value;

            if (value == NullPhrase)
                return null;
            else
                return value;
        }

        private static string ToXml(Action<XmlWriter> save, bool omitDeclaration)
        {
            var settings = new XmlWriterSettings();

            settings.OmitXmlDeclaration = omitDeclaration;

            settings.Indent = true;

            string xml;

            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                    save(writer);

                stream.Position = 0;

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                    xml = reader.ReadToEnd();
            }

            return xml;
        }

        public static string ToXml(this XElement element)
        {
            return ToXml(writer => element.Save(writer), true);
        }

        public static string ToXml(this XDocument document)
        {
            return ToXml(writer => document.Save(writer), false);
        }
    }
}
