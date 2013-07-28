using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SquidEyes.Generic
{
    public static partial class TokensExtenders
    {
        private static void AppendToken(StringBuilder sb,
            string name, bool toUpper)
        {
            if (toUpper)
                sb.Append(name.ToUpper());
            else
                sb.Append(name);
        }
    }
}
