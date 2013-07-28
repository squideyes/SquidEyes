using System;
using System.Diagnostics.Contracts;
using System.Net.Mail;

namespace SquidEyes.Generic
{
    [Serializable]
    public struct Address : IEquatable<Address>, IComparable<Address>
    {
        public static class Limits
        {
            public static readonly MinMax<int> Length = new MinMax<int>(8, 64);
        }

        private string value;

        public Address(string value)
        {
            Contract.Requires((value == null) || value.IsEmail());

            this.value = value;
        }

        public string LocalPart
        {
            get
            {
                return value.Split('@')[0];
            }
        }

        public string HostName
        {
            get
            {
                return value.Split('@')[1];
            }
        }

        public bool Equals(Address email)
        {
            return value.Equals(email.value);
        }

        public int CompareTo(Address email)
        {
            return value.CompareTo(email.value);
        }

        public override string ToString()
        {
            return value;
        }

        public static readonly Address Empty;

        public static implicit operator Address(string email)
        {
            return new Address(email);
        }

        public static implicit operator string(Address email)
        {
            return email.value;
        }

        public static implicit operator MailAddress(Address email)
        {
            return new MailAddress(email.value);
        }
    }
}
