using System;

namespace SquidEyes.GUI
{
    public class NotifyArgs : EventArgs
    {
    }

    public class NotifyArgs<D> : NotifyArgs
    {
        public NotifyArgs(D data)
        {
            Data = data;
        }

        public D Data { get; protected set; }
    }
}
