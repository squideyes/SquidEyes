using System;
using System.Diagnostics.Contracts;

namespace SquidEyes.Generic
{
    public class ErrorArgs
    {
        public ErrorArgs(Exception error)
        {
            Contract.Requires(error != null);

            Error = error;
        }

        public Exception Error { get; private set; }
    }

    public class ErrorArgs<T>
    {
        public ErrorArgs(Exception error, T context)
        {
            Contract.Requires(error != null);
            Contract.Requires(!context.IsDefault());

            Error = error;
            Context = context;
        }

        public Exception Error { get; private set; }
        public T Context { get; private set; }
    }
}
