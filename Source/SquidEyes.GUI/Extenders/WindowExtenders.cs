using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SquidEyes.GUI
{
    public static class WindowExtenders
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
            int x, int y, int width, int height, uint flags);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;
        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x00010000;
        private const int WS_MINIMIZEBOX = 0x00020000;
        private const int WS_SYSMENU = 0x00080000;
        private const int WS_EX_CONTEXTHELP = 0x00000400;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_CONTEXTHELP = 0xF180;

        public static void HideSysMenu(this Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;

            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }

        public static void HideMinimizeBox(this Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;

            SetWindowLong(hwnd, GWL_STYLE,
                GetWindowLong(hwnd, GWL_STYLE) & ~(WS_MINIMIZEBOX));
        }

        public static void HideMaximizeBox(this Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;

            SetWindowLong(hwnd, GWL_STYLE,
                GetWindowLong(hwnd, GWL_STYLE) & ~(WS_MAXIMIZEBOX));
        }

        public static void HideMinimizeAndMaximizeBoxes(this Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;

            SetWindowLong(hwnd, GWL_STYLE,
                GetWindowLong(hwnd, GWL_STYLE) & ~(WS_MAXIMIZEBOX | WS_MINIMIZEBOX));
        }

        private static Action ShowHelp;

        public static void ShowHelpIcon(this Window sessionInfo, Action showHelp)
        {
            ShowHelp = showHelp;

            var hwnd = new WindowInteropHelper(sessionInfo).Handle;

            var styles = GetWindowLong(hwnd, GWL_STYLE);

            styles &= Int32.MaxValue ^ (WS_MINIMIZEBOX | WS_MAXIMIZEBOX);

            SetWindowLong(hwnd, GWL_STYLE, styles);

            styles = GetWindowLong(hwnd, GWL_EXSTYLE);

            styles |= WS_EX_CONTEXTHELP;

            SetWindowLong(hwnd, GWL_EXSTYLE, styles);

            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
                SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);

            ((HwndSource)PresentationSource.FromVisual(sessionInfo)).AddHook(HelpHook);
        }

        private static IntPtr HelpHook(IntPtr hwnd, int msg, IntPtr wParam,
            IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SYSCOMMAND && ((int)wParam & 0xFFF0) == SC_CONTEXTHELP)
            {
                ShowHelp();

                handled = true;
            }

            return IntPtr.Zero;
        }

        public static System.Windows.Forms.IWin32Window GetIWin32Window(
            this System.Windows.Media.Visual visual)
        {
            var source = System.Windows.PresentationSource.
                FromVisual(visual) as System.Windows.Interop.HwndSource;

            return new OldWindow(source.Handle);
        }

        private class OldWindow : System.Windows.Forms.IWin32Window
        {
            private readonly IntPtr handle;

            public OldWindow(IntPtr handle)
            {
                this.handle = handle;
            }

            IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get
                {
                    return handle;
                }
            }
        }
    }
}
