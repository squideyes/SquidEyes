using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public class ErrorBlock
    {
        private class AssemblyInfoComparer : IComparer<AssemblyData>
        {
            public int Compare(AssemblyData ai1, AssemblyData ai2)
            {
                return ai1.FullName.CompareTo(ai2.FullName);
            }
        }

        private const string ERRORBLOCK = "errorBlock";

        public ErrorBlock(string xml)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(xml));

            var document = XDocument.Parse(xml);

            var errorBlock = document.Element(ERRORBLOCK);

            SetContext(errorBlock);

            SetErrorInfos(errorBlock);

            SetAssemblyInfos(errorBlock);

            SystemInfo = new SystemInfo(errorBlock.Element("system"));
        }

        public ErrorBlock(Exception error, AppInfo appInfo)
            : this(error, appInfo, null)
        {
        }

        public ErrorBlock(Exception error, AppInfo appInfo,
            ValidatedDictionary<string, string> context)
        {
            Contract.Requires(error != null);
            Contract.Requires(appInfo != null);

            DateTimeUtc = DateTime.UtcNow;

            if (context != null)
                Context = context;
            else
                Context = GetNewContext();

            AlertCode = AlertCode.Next();

            AssemblyInfos = new List<AssemblyData>();

            ErrorInfos = GetErrorInfos(error);

            ErrorInfos.Reverse();

            AssemblyInfos = GetAssemblyInfos(
                GetAssemblyFromName(error.Source));

            SystemInfo = new SystemInfo();

            Company = appInfo.Company;
            Product = appInfo.Product;
            Version = appInfo.Version;
        }

        public DateTime DateTimeUtc { get; private set; }
        public AlertCode AlertCode { get; private set; }
        public List<ErrorInfo> ErrorInfos { get; private set; }
        public List<AssemblyData> AssemblyInfos { get; private set; }
        public SystemInfo SystemInfo { get; private set; }
        public string Company { get; private set; }
        public string Product { get; private set; }
        public Version Version { get; private set; }
        public ValidatedDictionary<string, string> Context { get; private set; }

        public string FileName
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append("ErrorBlock_");
                sb.AppendFormat("{0:yyyyMMdd_HHmmss}", DateTimeUtc);
                sb.Append('_');
                sb.Append(AlertCode.ToString().Replace("-", ""));
                sb.Append(".xml");

                return sb.ToString();
            }
        }

        public static List<ErrorInfo> GetErrorInfos(Exception error)
        {
            var errorInfos = new List<ErrorInfo>();

            Exception temp = error;

            while (temp != null)
            {
                ErrorKind errorKind;

                if (temp.InnerException == null)
                    errorKind = ErrorKind.Primary;
                else
                    errorKind = ErrorKind.Secondary;

                errorInfos.Add(new ErrorInfo(temp, errorKind));

                temp = temp.InnerException;
            }

            return errorInfos;
        }

        private void SetContext(XElement errorBlock)
        {
            var context = errorBlock.Element("context");

            DateTimeUtc = (DateTime)context.Element("dateTimeUtc");
            AlertCode = context.Element("alertCode").Value;
            Company = context.Element("company").Value;
            Product = context.Element("product").Value;
            Version = Version.Parse(context.Element("version").Value);

            var keyValues = context.Element("keyValues");

            if (keyValues != null)
            {
                var q = from c in keyValues.Elements("keyValue")
                        select new
                        {
                            Key = c.Attribute("key").Value,
                            Value = XmlHelper.ValueOrNullPhrase(c.Attribute("value"))
                        };

                Context = GetNewContext();

                foreach (var keyValue in q)
                    Context.Add(keyValue.Key, keyValue.Value);
            }
        }

        private void SetErrorInfos(XElement errorBlock)
        {
            ErrorInfos = new List<ErrorInfo>();

            foreach (var error in errorBlock.Element("errors").Elements())
                ErrorInfos.Add(new ErrorInfo(error));
        }

        private void SetAssemblyInfos(XElement errorBlock)
        {
            AssemblyInfos = new List<AssemblyData>();

            foreach (var assembly in errorBlock.Element("assemblies").Elements())
                AssemblyInfos.Add(new AssemblyData(assembly));
        }

        private static Assembly GetAssemblyFromName(string name)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (string.Compare(assembly.GetName().Name, name, true) == 0)
                    return assembly;

            return null;
        }

        private static bool CanIgnore(string value)
        {
            if (value.StartsWith("System"))
                return true;

            if (value.StartsWith("Microsoft.VisualStudio"))
                return true;

            if (value.EndsWith(".vshost.exe"))
                return true;

            switch (value)
            {
                case "mscorlib":
                case "WindowsBase":
                case "PresentationCore":
                case "PresentationFramework":
                    return true;
                default:
                    return false;
            }
        }

        private static List<AssemblyData> GetAssemblyInfos(
            Assembly sourceAssembly)
        {
            var assemblyInfos = new List<AssemblyData>();

            if (sourceAssembly != null)
            {
                assemblyInfos.Add(new AssemblyData(sourceAssembly));

                var assemblies = System.AppDomain.
                    CurrentDomain.GetAssemblies().OrderBy(a => a.FullName);

                foreach (var assembly in assemblies)
                {
                    if (assembly == sourceAssembly)
                        continue;

                    if (assembly is AssemblyBuilder)
                        continue;

                    if (assembly.IsDynamic)
                        continue;

                    var assemblyInfo = new AssemblyData(assembly);

                    if (CanIgnore(assemblyInfo.ShortName))
                        continue;

                    if (assemblyInfo.Version == "0.0.0.0")
                        continue;

                    assemblyInfos.Add(new AssemblyData(assembly));
                }

                assemblyInfos.Sort(new AssemblyInfoComparer());
            }

            return assemblyInfos;
        }

        public XDocument ToDocument()
        {
            return new XDocument(ToElement());
        }

        public XElement ToElement()
        {
            var errorBlock = new XElement(ERRORBLOCK);

            var context = new XElement("context");

            errorBlock.Add(context);

            context.Add(
                new XElement("dateTimeUtc", DateTimeUtc),
                new XElement("alertCode", AlertCode),
                new XElement("company", Company),
                new XElement("product", Product),
                new XElement("version", Version.ToVersionString()));

            if ((Context != null) && Context.HasElements())
            {
                context.Add(new XElement("keyValues",
                    from c in Context
                    select new XElement("keyValue",
                        new XAttribute("key", c.Key),
                        new XAttribute("value", c.Value))));
            }

            errorBlock.Add(new XElement("errors",
                from ei in ErrorInfos select ei.GetElement()));

            if (AssemblyInfos.Count > 0)
            {
                errorBlock.Add(new XElement("assemblies",
                    from ai in AssemblyInfos
                    select ai.GetElement()));
            }

            errorBlock.Add(SystemInfo.GetElement());

            return errorBlock;
        }

        public static ValidatedDictionary<string, string> GetNewContext()
        {
            return new ValidatedDictionary<string, string>(
                key => key.IsTrimmed(false), value => true);
        }
    }
}