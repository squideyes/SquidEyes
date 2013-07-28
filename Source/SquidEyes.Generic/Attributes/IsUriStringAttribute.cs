using System;
using System.ComponentModel.DataAnnotations;

namespace SquidEyes.Generic
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
        AllowMultiple = false, Inherited = true)]
    public sealed class IsUriStringAttribute : ValidationAttribute
    {
        private UriKind uriKind;

        public IsUriStringAttribute(UriKind uriKind)
        {
            this.uriKind = uriKind;
        }

        public override bool IsValid(object value)
        {
            var uriString = Convert.ToString(value);

            if (uriString.Length > 2048)
                return false;

            return Uri.IsWellFormedUriString(uriString, uriKind);
        }
    }
}
