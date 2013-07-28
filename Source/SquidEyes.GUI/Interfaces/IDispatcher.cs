using System;

namespace SquidEyes.GUI
{
    public interface IDispatcher
    {
        bool CheckAccess();
        void BeginInvoke(Action action);
    }
}
