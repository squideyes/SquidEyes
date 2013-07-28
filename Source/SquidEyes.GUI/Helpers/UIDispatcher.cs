using System;
using System.Diagnostics.Contracts;
using System.Windows;
using WindowsDispatcher = System.Windows.Threading.Dispatcher;

namespace SquidEyes.GUI
{
    public sealed class UIDispatcher : IDispatcher
    {
        private static volatile IDispatcher dispatcher;

        private static readonly object SyncRoot = new Object();
        
        private readonly WindowsDispatcher windowsDispatcher;

        private UIDispatcher(WindowsDispatcher windowsDispatcher)
        {
            this.windowsDispatcher = windowsDispatcher;
        }

        public bool CheckAccess()
        {
            return windowsDispatcher.CheckAccess();
        }

        public void BeginInvoke(Action action)
        {
            Contract.Requires(action != null);

            windowsDispatcher.BeginInvoke(action);
        }

        public static void Initialize()
        {
            dispatcher = new UIDispatcher(WindowsDispatcher.CurrentDispatcher);
        }

        public static IDispatcher Current
        {
            get
            {
                if (dispatcher == null)
                {
                    lock (SyncRoot)
                        dispatcher = new UIDispatcher(WindowsDispatcher.CurrentDispatcher);
                }

                return dispatcher;
            }
        }

        public static void Execute(Action action)
        {
            Contract.Requires(action != null);

            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.BeginInvoke(action);
        }
    }
}
