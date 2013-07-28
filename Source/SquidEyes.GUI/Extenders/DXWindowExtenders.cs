using System.Windows;
using DevExpress.Xpf.Core;

namespace SquidEyes.GUI
{
    public static class DXWindowExtenders
    {
        public static void SetOwner(this DXWindow view, DXWindow owner)
        {
            if (owner == null)
            {
                view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                view.Owner = owner;

                view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }
    }
}
