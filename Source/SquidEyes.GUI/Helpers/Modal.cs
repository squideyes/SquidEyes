using SquidEyes.Generic;
using System.Reflection;
using System.Windows;

namespace SquidEyes.GUI
{
    public static class Modal
    {
        private static AppInfo appInfo;

        static Modal()
        {
            appInfo = new AppInfo(Assembly.GetEntryAssembly());
        }

        public static bool FolderDialog(Window owner, string caption,
            ref string selectedPath)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();

            dialog.Description = caption;
            dialog.ShowNewFolderButton = true;
            dialog.SelectedPath = selectedPath;

            var result = dialog.ShowDialog(owner.GetIWin32Window());

            selectedPath = dialog.SelectedPath;

            return result == System.Windows.Forms.DialogResult.OK;
        }

        public static void InfoDialog(Window owner, string format, 
            params object[] args)
        {
            MessageBox.Show(owner, string.Format(format, args), 
                appInfo.GetTitle(),
                MessageBoxButton.OK, MessageBoxImage.Information,
                MessageBoxResult.OK, MessageBoxOptions.None);
        }

        public static void WarningDialog(Window owner, string format, 
            params object[] args)
        {
            MessageBox.Show(owner, string.Format(format, args), 
                appInfo.GetTitle(), 
                MessageBoxButton.OK, MessageBoxImage.Warning, 
                MessageBoxResult.OK, MessageBoxOptions.None);
        }

        public static void ErrorDialog(Window owner, string format, 
            params object[] args)
        {
            MessageBox.Show(owner, string.Format(format, args),
                appInfo.GetTitle(),
                MessageBoxButton.OK, MessageBoxImage.Error, 
                MessageBoxResult.OK,MessageBoxOptions.None);
        }

        public static void FailureDialog(string format, 
            params object[] args)
        {
            MessageBox.Show(string.Format(format, args),
                appInfo.GetTitle(),
                MessageBoxButton.OK, MessageBoxImage.Exclamation,
                MessageBoxResult.OK, MessageBoxOptions.None);
        }

        public static bool YesNoDialog(Window owner, string format, 
            params object[] args)
        {
            return MessageBox.Show(owner, string.Format(format, args),
                appInfo.GetTitle(),
                MessageBoxButton.YesNo, MessageBoxImage.Question,
                MessageBoxResult.Yes, MessageBoxOptions.None) ==
                MessageBoxResult.Yes;
        }
    }
}
