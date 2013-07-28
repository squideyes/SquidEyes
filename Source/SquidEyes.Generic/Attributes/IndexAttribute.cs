namespace SquidEyes.Generic
{
    using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class IndexAttribute : Attribute
    {
        public IndexAttribute(string name, bool unique = false)
        {
            Name = name;
            IsUnique = unique;
        }

        public string Name { get; private set; }
        public bool IsUnique { get; private set; }
    }
}
