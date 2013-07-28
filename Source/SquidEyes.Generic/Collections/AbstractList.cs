using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SquidEyes.Generic
{
    public abstract class AbstractList<T> : IEnumerable<T>
    {
        protected List<T> items = new List<T>();

        public T First
        {
            get
            {
                return items[0];
            }
        }

        public T FirstOrDefault
        {
            get
            {
                if (Count == 0)
                    return default(T);
                else
                    return items[0];
            }
        }

        public T LastOrDefault
        {
            get
            {
                if (Count == 0)
                    return default(T);
                else
                    return items[Count - 1];
            }
        }

        public T Last
        {
            get
            {
                return items[Count - 1];
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public T this[int index]
        {
            get
            {
                return items[index];
            }
        }

        public void ForEach(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            items.ForEach(action);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
