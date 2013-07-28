using System;

namespace SquidEyes.Generic
{
    public class ComboItem<T> : IComparable<ComboItem<T>>
        where T : struct
    {
        public ComboItem(T enumeration)
        {
            Enumeration = enumeration;
        }
            
        public T Enumeration { get; private set; }

        public string Description
        {
            get
            {
                return EnumHelper.GetDescription<T>(Enumeration);
            }
        }

        public int CompareTo(ComboItem<T> other)
        {
            return Description.CompareTo(other.Description);
        }
    }
}
