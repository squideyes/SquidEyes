using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SquidEyes.Generic
{
    public class AppInfo
    {
        public AppInfo()
            : this(Assembly.GetEntryAssembly())
        {
        }

        public AppInfo(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            Company = GetEntryCompany(assembly);
            Product = GetEntryProduct(assembly);
            Version = assembly.GetName().Version;
        }

        public Version Version { get; private set; }
        public string Company { get; private set; }
        public string Product { get; private set; }

        public string GetTitle()
        {
            var sb = new StringBuilder();

            sb.Append(Product);
            sb.Append(" v");
            sb.Append(Version.Major);
            sb.Append('.');
            sb.Append(Version.Minor);

            if ((Version.Build != 0) || (Version.Revision != 0))
            {
                sb.Append('.');
                sb.Append(Version.Build);
            }

            if (Version.Revision != 0)
            {
                sb.Append('.');
                sb.Append(Version.Revision);
            }

            return sb.ToNewString();
        }

        private string GetString<T>(Assembly assembly, Func<T, string> getString)
        {
            var customAttributes = assembly.GetCustomAttributes(typeof(T), false);

            if ((customAttributes != null) && (customAttributes.Length > 0))
                return getString(((T)customAttributes[0]));

            return string.Empty;
        }

        private string CleanUp(string value)
        {
            return Path.GetInvalidFileNameChars().Aggregate(value,
                (current, c) => current.Replace(c.ToString(), " ")).Trim();
        }

        public string GetLocalAppDataPath(params string[] subFolders)
        {
            Contract.Requires(subFolders.Length > 0);
            Contract.Requires(subFolders.All(folder => folder.IsTrimmed()));

            var items = new List<string>(subFolders);

            var path = Path.Combine(Environment.GetFolderPath(
               Environment.SpecialFolder.LocalApplicationData),
               CleanUp(Company), CleanUp(Product.Replace("™", "")));

            foreach (var subFolder in subFolders)
                path = Path.Combine(path, CleanUp(subFolder));

            return path;
        }

        public string GetMyDocumentsPath(params string[] subFolders)
        {
            Contract.Requires(subFolders.Length > 0);
            Contract.Requires(subFolders.All(folder => folder.IsTrimmed()));
            Contract.Requires(subFolders.All(folder =>
                folder.IndexOfAny(Path.GetInvalidPathChars()) == -1));

            var items = new List<string>(subFolders);

            var path = Path.Combine(Environment.GetFolderPath(
               Environment.SpecialFolder.MyDocuments),
               CleanUp(Company), CleanUp(Product.Replace("™", "")));

            foreach (var subFolder in subFolders)
                path = Path.Combine(path, CleanUp(subFolder));

            return path;
        }

        private static string GetEntryCompany(Assembly assembly)
        {
            return assembly.GetAttribute<AssemblyCompanyAttribute>().Company;
        }

        private static string GetEntryProduct(Assembly assembly)
        {
            return assembly.GetAttribute<AssemblyProductAttribute>().Product;
        }
    }
}
