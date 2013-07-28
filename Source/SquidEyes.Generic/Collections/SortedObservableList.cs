using System;
using System.Collections.ObjectModel;

namespace SquidEyes.Generic
{
    public class SortedObservableList<T> : ObservableCollection<T>
           where T : IComparable<T>
    {
        protected override void InsertItem(int index, T item)
        {
            for (int i = 0; i < Count; i++)
            {
                switch (Math.Sign(this[i].CompareTo(item)))
                {
                    case 0:
                        throw new InvalidOperationException(
                            "A duplicate item may not be inserted!");
                    case 1:
                        base.InsertItem(i, item);
                        return;
                    case -1:
                        break;
                }
            }

            base.InsertItem(this.Count, item);
        }
    }
}
