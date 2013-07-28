using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace SquidEyes.Generic
{
    public class ValidatedDictionary<K, V> : Dictionary<K, V>
    {
        private Func<K, bool> keyIsValid = null;
        private string keyLambda = null;

        private Func<V, bool> valueIsValid;
        private string valueLambda;

        public ValidatedDictionary(Expression<Func<V, bool>> valueIsValid)
            : this(null, valueIsValid)
        {
        }

        public ValidatedDictionary(Expression<Func<K, bool>> keyIsValid,
            Expression<Func<V, bool>> valueIsValid)
        {
            Contract.Requires(valueIsValid != null);

            if (keyIsValid != null)
            {
                keyLambda = keyIsValid.ToString();

                this.keyIsValid = keyIsValid.Compile();
            }

            valueLambda = valueIsValid.ToString();

            this.valueIsValid = valueIsValid.Compile();
        }

        public new void Add(K key, V value)
        {
            const string CONDITION =
                "must be set to a {0} where the \"{1}\" expression is true";

            Contract.Requires(!key.IsDefault());

            if (keyIsValid != null)
            {
                if (!keyIsValid(key))
                {
                    throw new ArgumentOutOfRangeException(
                        string.Format(CONDITION, typeof(K), keyLambda));
                }
            }

            if (!valueIsValid(value))
            {
                throw new ArgumentOutOfRangeException(
                    string.Format(CONDITION, typeof(V), valueLambda));
            }

            if (!base.ContainsKey(key))
                base.Add(key, value);
        }
    }
}
