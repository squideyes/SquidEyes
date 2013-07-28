using System;
using System.Text;

namespace SquidEyes.Generic
{
    public static partial class VersionExtenders
    {
        public static string ToVersionString(this Version version)
        {
            var sb = new StringBuilder();

            sb.Append(version.Major);
            sb.Append('.');
            sb.Append(version.Minor);

            if ((version.Build != 0) || (version.Revision != 0))
            {
                sb.Append('.');
                sb.Append(version.Build);
            }

            if (version.Revision != 0)
            {
                sb.Append('.');
                sb.Append(version.Revision);
            }

            return sb.ToString();
        }
    }
}
