using System;
using System.Threading;
using System.Windows;
using DevExpress.Xpf.Core;

namespace SquidEyes.GUI
{
    public partial class WaitAndDoDialog : DXWindow
    {
        private Action action;

        public WaitAndDoDialog(string caption, Action action)
        {
            InitializeComponent();

            Caption = caption;

            this.action = action;

            bool canClose = false;

            DataContext = this;

            Closing += (s, e) =>
            {
                e.Cancel = !canClose;
            };

            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    action();

                    canClose = true;

                    Dispatch(() => Close());
                }
                catch (Exception error)
                {
                    Error = error;

                    canClose = true;

                    Dispatch(() => Close());
                }
            });
        }

        public string Caption { get; private set; }
        public Exception Error { get; private set; }

        private void Dispatch(Action action)
        {
            Dispatcher.BeginInvoke((Action)(() => { action(); }));
        }

        public static bool WaitAndDo(DXWindow owner, string caption,
            Action action, out Exception error)
        {
            error = null;

            var dialog = new WaitAndDoDialog(caption, action);

            dialog.Owner = owner;

            if (owner != null)
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dialog.ShowDialog();

            if (dialog.Error == null)
            {
                return true;
            }
            else
            {
                error = dialog.Error;

                return false;
            }
        }
    }
}
