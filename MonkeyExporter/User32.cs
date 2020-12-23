using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyExporter
{
    /// <summary>
    /// Helper class containing User32 API functions
    /// </summary>
    public class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public struct POINT
        {
            public int x;
            public int y;
        }

        //[DllImport("user32.dll")]
        //public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("USER32.DLL")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_RESTORE = 9;       // Restore the window (not maximized nor minimized). 

        private static IntPtr shellWindow = IntPtr.Zero;
        private static Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

        private static bool _fEnumWindowsCallBack(IntPtr hWnd, int lParam)
        {
            if (hWnd == shellWindow) return true;
            if (!IsWindowVisible(hWnd)) return true;

            string title = getWindowTitle(hWnd);
            if (title.Length == 0) return true;

            windows[hWnd] = title;
            return true;
        }

        public static IDictionary<IntPtr, string> GetOpenWindows()
        {
            shellWindow = GetShellWindow();
            windows.Clear();
            EnumWindowsProc fEnumWindowsCallBack = new EnumWindowsProc(_fEnumWindowsCallBack);
            EnumWindows(fEnumWindowsCallBack, 0);
            return windows;
        }

        public static string getWindowTitle(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            if (length == 0) return "";

            StringBuilder builder = new StringBuilder(length);
            GetWindowText(hWnd, builder, length + 1);

            return builder.ToString();
        }
    }

    /// <summary>
    /// Helper class containing Gdi32 API functions
    /// </summary>
    class GDI32
    {

        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
            int nWidth, int nHeight, IntPtr hObjectSource,
            int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
            int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }
}
