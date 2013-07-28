using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public class AssemblyData
    {
        internal AssemblyData(Assembly assembly)
        {
            var name = assembly.GetName();

            if (assembly is AssemblyBuilder)
                Location = "(Emitted Assembly)";
            else if (assembly.IsDynamic)
                Location = "(Dynamic Assembly)";
            else
                Location = assembly.Location;

            BuildDate = AssemblyBuildDate(assembly);
            FullName = assembly.FullName;
            ShortName = name.Name;

            GetAttributes(assembly);
        }

        internal AssemblyData(XElement assembly)
        {
            Location = (string)assembly.Element("location");
            BuildDate = (DateTime)assembly.Element("buildDate");
            FullName = (string)assembly.Element("fullName");
            Product = (string)assembly.Element("product");
            Company = (string)assembly.Element("company");
            Title = (string)assembly.Element("title");
        }

        public string ShortName { get; private set; }
        public string Location { get; private set; }
        public string Version { get; private set; }
        public DateTime BuildDate { get; private set; }
        public string FullName { get; private set; }
        public string Product { get; private set; }
        public string Company { get; private set; }
        public string Title { get; private set; }

        private DateTime AssemblyBuildDate(Assembly assembly)
        {
            DateTime dateTime;

            Version version = assembly.GetName().Version;

            dateTime = new DateTime(2000, 1, 1);
            dateTime.AddDays(version.Build);
            dateTime.AddSeconds(version.Revision * 2);

            if (TimeZone.IsDaylightSavingTime(dateTime,
                TimeZone.CurrentTimeZone.GetDaylightChanges(dateTime.Year)))
            {
                dateTime.AddHours(1);
            }

            if ((dateTime > DateTime.Now) || (version.Build < 730) ||
                (version.Revision == 0))
            {
                try
                {
                    return File.GetLastWriteTime(assembly.Location);
                }
                catch
                {
                    return DateTime.MaxValue;
                }
            }

            return dateTime;
        }

        private void GetAttributes(Assembly assembly)
        {
            foreach (object attribute in assembly.GetCustomAttributes(false))
                GetAttributeValue(attribute);
        }

        private void GetAttributeValue(object attribute)
        {
            switch (attribute.GetType().ToString())
            {
                case "System.Reflection.AssemblyProductAttribute":
                    Product = ((AssemblyProductAttribute)attribute).Product;
                    break;
                case "System.Reflection.AssemblyCompanyAttribute":
                    Company = ((AssemblyCompanyAttribute)attribute).Company;
                    break;
                case "System.Reflection.AssemblyTitleAttribute":
                    Title = ((AssemblyTitleAttribute)attribute).Title;
                    break;
            }
        }

        private void AddElement(XElement element, XName name, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
                element.Add(new XElement(name, content));
        }

        internal XElement GetElement()
        {
            XElement element = new XElement("assembly");

            element.Add(new XElement("fullName", FullName));
            element.Add(new XElement("location", Location));
            element.Add(new XElement("buildDate", BuildDate));
            AddElement(element, "product", Product);
            AddElement(element, "company", Company);
            AddElement(element, "title", Title);

            return element;
        }
    }
}