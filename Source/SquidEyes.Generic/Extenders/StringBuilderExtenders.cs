using System.Text;

namespace SquidEyes.Generic
{
    public static class StringBuilderExtenders
    {
        public static void AddField(this StringBuilder sb,
            object value, char delimiter = ',')
        {
            if (sb.Length > 0)
                sb.Append(delimiter);

            sb.Append(value);
        }
    }
}
