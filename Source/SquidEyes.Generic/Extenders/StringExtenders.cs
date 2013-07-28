using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SquidEyes.Generic
{
    public static partial class StringExtenders
    {
        private static Dictionary<int, Regex> regexes =
            new Dictionary<int, Regex>();


        //do I sue this?????????????????????????????????????????????????????????????????????
        public static bool IsUrl(this string uriString, string baseUri = null)
        {
            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
                return false;

            if (!uriString.StartsWith(baseUri))
                return false;

            return true;
        }

        public static bool IsEmail(this string value)
        {
            const string PATTERN =
                @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$";

            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (!value.Length.InRange(Address.Limits.Length))
                return false;

            return value.IsMatch(PATTERN);
        }

        public static bool IsPhone(this string value,
            PhoneKind phoneKind, bool allowExtension = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            const string EXTENSION_SUFFIX =
                @"(\s+?[xX]\d{1,5})?";

            const string DOMESTIC_PREFIX =
                @"[2-9]\d{2}\-[2-9]\d{2}\-\d{4}";

            const string INTERNATIONAL_PREFIX =
                @"(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*";

            if (value.Trim().Length == 0)
                return false;

            var pattern = new StringBuilder();

            pattern.Append('^');

            switch (phoneKind)
            {
                case PhoneKind.Domestic:
                    pattern.Append(DOMESTIC_PREFIX);
                    break;
                case PhoneKind.International:
                    pattern.Append(INTERNATIONAL_PREFIX);
                    break;
                case PhoneKind.EitherFormat:
                    pattern.Append("((");
                    pattern.Append(DOMESTIC_PREFIX);
                    pattern.Append(")|(");
                    pattern.Append(INTERNATIONAL_PREFIX);
                    pattern.Append("))");
                    break;
                case PhoneKind.NoFormat:
                    return (IsTrimmed(value, false));
            }

            if (allowExtension)
                pattern.Append(EXTENSION_SUFFIX);

            pattern.Append('$');

            return IsMatch(value, pattern.ToString());
        }

        [DebuggerHidden]
        public static string ToAOrAn(this string value,
            bool upperFirst = false, List<string> anExceptions = null)
        {
            Contract.Requires(value.IsTrimmed());

            if ((anExceptions != null) &&
                anExceptions.Contains(value.ToLower()))
            {
                return upperFirst ? "An" : "an";
            }

            switch (value.ToLower())
            {
                case "hour":
                case "honest":
                case "heir":
                case "heirloom":
                    return upperFirst ? "An" : "an";
                case "use":
                case "useless":
                case "user":
                    return upperFirst ? "A" : "a";
            }

            switch (char.ToLower(value[0]))
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    return upperFirst ? "An" : "an";
                default:
                    return upperFirst ? "A" : "a";
            }
        }

        [DebuggerHidden]
        public static List<string> ToLines(this string value,
            bool trimTopAndBottom = true)
        {
            var lines = new List<string>();

            using (var reader = new StringReader(value))
            {
                string line;

                bool canTrim = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (trimTopAndBottom && canTrim)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;
                    }

                    canTrim = false;

                    lines.Add(line);
                }
            }

            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    lines.RemoveAt(i);
                else
                    break;
            }

            return lines;
        }

        [DebuggerHidden]
        public static string ToSingleLine(this string value)
        {
            var sb = new StringBuilder();

            var reader = new StringReader(value);

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (sb.Length > 0)
                    sb.Append(' ');

                sb.Append(line.Trim());
            }

            return sb.ToString();
        }

        [DebuggerHidden]
        public static bool IsIpAddress(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            IPAddress address;

            return IPAddress.TryParse(value, out address);
        }

        [DebuggerHidden]
        public static bool IsDomain(this string value)
        {
            const string PATTERN =
                @"^((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$";

            if (string.IsNullOrWhiteSpace(value))
                return false;

            return value.IsMatch(PATTERN);
        }

        [DebuggerHidden]
        public static string AOrAn(this string value, bool properCase)
        {
            string result = properCase ? "A" : "a";

            if (!string.IsNullOrWhiteSpace(value))
            {
                char c = value[0];

                switch (c)
                {
                    case 'a':
                    case 'e':
                    case 'i':
                    case 'o':
                    case 'u':
                        result = properCase ? "An" : "an";
                        break;
                }
            }

            return result;
        }

        [DebuggerHidden]
        public static bool IsMatch(this string value, string pattern)
        {
            return value.IsMatch(pattern, RegexOptions.None);
        }

        [DebuggerHidden]
        public static bool IsMatch(this string value,
            string pattern, RegexOptions options)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (string.IsNullOrWhiteSpace(pattern))
                return false;

            int hashCode = pattern.GetHashCode();

            Regex regex;

            if (!regexes.TryGetValue(hashCode, out regex))
            {
                options |= RegexOptions.Compiled;

                regex = new Regex(pattern, options);

                regexes.Add(hashCode, regex);
            }

            return regex.IsMatch(value);
        }

        [DebuggerHidden]
        public static bool IsCondition(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return (!char.IsWhiteSpace(value[0]) &&
                (!char.IsWhiteSpace(value[value.Length - 1])));
        }


        [DebuggerHidden]
        public static bool IsTrimmed(this string value, int maxLength)
        {
            return value.IsTrimmed(1, maxLength);
        }

        [DebuggerHidden]
        public static bool IsTrimmed(this string value, int minLength, int maxLength)
        {
            if (value == (string)null)
                return false;

            if (minLength < 1)
                return false;

            if (maxLength < minLength)
                return false;

            if (value.Length < minLength)
                return false;

            if (value.Length > maxLength)
                return false;

            return (!char.IsWhiteSpace(value[0]) &&
                (!char.IsWhiteSpace(value[value.Length - 1])));
        }

        [DebuggerHidden]
        public static bool IsKnownAs(this string value, bool emptyOK = false)
        {
            if (!value.IsTrimmed(emptyOK))
                return false;

            return value.IsMatch("[A-Z][A-Za-z0-9]+");
        }

        [DebuggerHidden]
        public static bool IsTrimmed(this string value, bool emptyOK = false)
        {
            if (value == (string)null)
                return false;

            if (value == string.Empty)
                return emptyOK;

            return (!char.IsWhiteSpace(value[0]) &&
                (!char.IsWhiteSpace(value[value.Length - 1])));
        }

        [DebuggerHidden]
        public static bool IsHostOrIP(this string value)
        {
            return value.IsIP() || value.IsHost();
        }

        [DebuggerHidden]
        public static bool IsIP(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            IPAddress address;

            return IPAddress.TryParse(value, out address);
        }

        [DebuggerHidden]
        public static bool IsHost(this string value)
        {
            const string PATTERN =
                @"^((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$";

            if (string.IsNullOrWhiteSpace(value))
                return false;

            return value.IsMatch(PATTERN);
        }

        [DebuggerHidden]
        public static string ToPrefix(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            switch (char.ToUpper(value[0]))
            {
                case 'A':
                case 'E':
                case 'I':
                case 'O':
                case 'U':
                    return "an";
                default:
                    return "a";
            }
        }

        [DebuggerHidden]
        public static bool IsFileName(this string value, bool mustBeRooted = true)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(value));

            try
            {
                new FileInfo(value);

                if (!mustBeRooted)
                    return true;
                else
                    return Path.IsPathRooted(value);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        [DebuggerHidden]
        public static bool IsDirectory(this string value, bool mustBeRooted = true)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(value));

            try
            {
                new DirectoryInfo(value);

                if (!mustBeRooted)
                    return true;
                else
                    return Path.IsPathRooted(value);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }
}