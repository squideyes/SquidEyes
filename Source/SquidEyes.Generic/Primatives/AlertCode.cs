using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace SquidEyes.Generic
{
    [Serializable]
    public struct AlertCode : IEquatable<AlertCode>, IComparable<AlertCode>
    {
        private const string CHARACTERS =
            "123456789ABCDEFGHJKMNPQRSTUVWXYZ";

        private static Random random = new Random();

        private string value;

        public AlertCode(string value)
        {
            Contract.Requires(IsValid(value));

            if (value.Length == 11)
                this.value = value;
            else
                this.value = value.Substring(0, 5) + "-" + value.Substring(5);
        }

        public bool Equals(AlertCode alertCode)
        {
            return value.Equals(alertCode.value);
        }

        public int CompareTo(AlertCode alertCode)
        {
            return value.CompareTo(alertCode.value);
        }

        public override string ToString()
        {
            return value;
        }

        public string WithoutDash
        {
            get
            {
                return value.Replace("-", "");
            }
        }

        public static readonly AlertCode Empty;

        private static string UInt64ToBase32(UInt64 number)
        {
            uint index = 12;
            ulong factor = 0;

            char[] chars = new char[index + 1];

            do
            {
                factor = (number % 32);

                chars[index] = CHARACTERS[(int)factor];

                number = (number - factor) / 32;

                index--;
            }
            while (number > 0);

            var sb = new StringBuilder(11);

            foreach (char c in chars)
            {
                if (c == '\0')
                    continue;

                sb.Append(c);

                if (sb.Length == 5)
                    sb.Append('-');
            }

            return sb.ToString();
        }

        public static bool IsValid(string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(value));

            int length = value.Length;

            if (length == 11)
            {
                if (value[5] != '-')
                    return false;
            }
            else if (length != 10)
            {
                return false;
            }

            for (int i = 0; i < length; i++)
            {
                if ((length == 11) && (i == 5))
                    continue;

                if (CHARACTERS.IndexOf(value[i]) == -1)
                    return false;
            }

            return true;
        }

        public static string Next()
        {
            byte[] bytes = new byte[8];

            while (true)
            {
                random.NextBytes(bytes);

                ulong number = BitConverter.ToUInt64(bytes, 0) >> 16;

                string result = UInt64ToBase32(number);

                if (result.Length == 11)
                    return result;
            }
        }

        public static implicit operator AlertCode(string alertCode)
        {
            return new AlertCode(alertCode);
        }

        public static implicit operator string(AlertCode alertCode)
        {
            return alertCode.value;
        }
    }
}
