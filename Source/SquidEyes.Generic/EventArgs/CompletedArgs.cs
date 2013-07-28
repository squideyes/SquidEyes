using System;
using System.Diagnostics.Contracts;

namespace SquidEyes.Generic
{
    public class CompletedArgs<T> : EventArgs
    {
        public CompletedArgs(Exception error)
        {
            Contract.Requires(error != null);

            Error = error;
        }

        public CompletedArgs()
        {
            Cancelled = true;
        }

        public CompletedArgs(T result)
        {
            Contract.Requires(!result.IsDefault());

            Finished = true;

            Result = result;
        }

        public Exception Error { get; private set; }
        public bool Finished { get; private set; }
        public T Result { get; private set; }
        public bool Cancelled { get; private set; }
    }
}
