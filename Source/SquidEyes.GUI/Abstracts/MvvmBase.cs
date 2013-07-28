using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace SquidEyes.GUI
{
    public abstract class MvvmBase 
    {
        protected readonly IDispatcher Dispatcher;

        public MvvmBase()
        {
            Dispatcher = UIDispatcher.Current;
        }

        protected void Notify(EventHandler handler)
        {
            if (handler != null)
                InternalNotify(() => handler(this, new NotifyArgs()));
        }

        protected void Notify(EventHandler handler, NotifyArgs e)
        {
            if (handler != null)
                InternalNotify(() => handler(this, e));
        }

        protected void Notify(EventHandler<NotifyArgs> handler)
        {
            if (handler != null)
                InternalNotify(() => handler(this, new NotifyArgs()));
        }

        protected void Notify(EventHandler<NotifyArgs> handler, NotifyArgs e)
        {
            if (handler != null)
                InternalNotify(() => handler(this, e));
        }

        protected void Notify<O>(EventHandler<NotifyArgs<O>> handler, NotifyArgs<O> e)
        {
            if (handler != null)
                InternalNotify(() => handler(this, e));
        }

        private void InternalNotify(Action method)
        {
            Contract.Requires(method != null);

            if (UIDispatcher.Current.CheckAccess())
                method();
            else
                UIDispatcher.Current.BeginInvoke(method);
        }
    }
}
