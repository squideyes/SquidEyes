using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SquidEyes.Generic
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
        AllowMultiple = false, Inherited = true)]
    public class IsMD5Attribute : ValidationAttribute
    {
        private static Regex regex = new Regex(
            @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.Compiled);

        public override bool IsValid(object value)
        {
            var md5 = Convert.ToString(value);

            if (string.IsNullOrWhiteSpace(md5))
                return false;

            if (md5.Length != 24)
                return false;

            return regex.IsMatch(md5);
        }
    }
}
