
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonkeyExporter
{
    public class TableHandles
    {
        private static object lockCapture = new object();
        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        private delegate bool EnumedWindow(IntPtr handleWindow, ArrayList handles);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList lParam);

        [DllImport("User32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("User32.dll")]
        private static extern int GetWindowText(IntPtr hwnd, StringBuilder buffer, int len);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        private const int ALT = 0xA4;
        private const int EXTENDEDKEY = 0x1;
        private const int KEYUP = 0x2;
        private const uint Restore = 9;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);


        const int GWL_EXSTYLE = -20;
        const int WS_EX_TOPMOST = 0x0008;

        [Flags]
        public enum SetWindowPosFlags : uint
        {
            SWP_ASYNCWINDOWPOS = 0x4000,

            SWP_DEFERERASE = 0x2000,

            SWP_DRAWFRAME = 0x0020,

            SWP_FRAMECHANGED = 0x0020,

            SWP_HIDEWINDOW = 0x0080,

            SWP_NOACTIVATE = 0x0010,

            SWP_NOCOPYBITS = 0x0100,

            SWP_NOMOVE = 0x0002,

            SWP_NOOWNERZORDER = 0x0200,

            SWP_NOREDRAW = 0x0008,

            SWP_NOREPOSITION = 0x0200,

            SWP_NOSENDCHANGING = 0x0400,

            SWP_NOSIZE = 0x0001,

            SWP_NOZORDER = 0x0004,

            SWP_SHOWWINDOW = 0x0040,
        }

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);


        public static int GetNumChildWindows(string appName)
        {
            Process[] anotherApps = Process.GetProcessesByName(appName);
            if (anotherApps.Length == 0) return 0;
            if (anotherApps[0] != null)
            {
                var allChildWindows = new WindowHandleInfo(anotherApps[0].MainWindowHandle).GetAllChildHandles();
                return allChildWindows.Count;
            }
            else
            {
                return 0;
            }
        }


        public static Size GetWindowSize(IntPtr handle)
        {
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            return new Size(width, height);
        }

        public static Image CaptureWindowOH(IntPtr handle, int tableWidth, int tableHeight, int clientWidth, int clientHeight)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int borderSize = Math.Abs(tableWidth - clientWidth) / 2;
            int titleBarSize = Math.Abs(tableHeight - borderSize - clientHeight);
            int width = windowRect.right - windowRect.left - 2 * borderSize;
            int height = windowRect.bottom - windowRect.top - titleBarSize - borderSize;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            //GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, borderSize, titleBarSize, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }

        //public static Image CaptureWindow(IntPtr handle, EnumCasino casino, bool forceUnity = false)
        //{
        //    try
        //    {
        //        lock (lockCapture)
        //        {
        //            bool isValid = IsWindow(handle);
        //            if (!isValid)
        //            {
        //                Singleton.Log("Handle is not window. ", LogLevel.Error);
        //                return null;
        //            }

        //            Size tableSize = ParseOH.GetTableSize(casino);
        //            Image result;
        //            if (!Properties.Settings.Default.inUnity && !forceUnity)
        //            {
        //                Size clientSize;
        //                if (casino == EnumCasino.Pokerbaazi)
        //                {
        //                    GetClientRect(handle, out var winRect);
        //                    clientSize = new Size(winRect.Right - winRect.Left, winRect.Bottom - winRect.Top);
        //                }
        //                else
        //                {
        //                    clientSize = ParseOH.GetTableClientSize(casino);
        //                }
        //                result = CaptureWindowOH(handle, tableSize.Width, tableSize.Height, clientSize.Width, clientSize.Height);
        //            }
        //            else
        //            {
        //                result = CaptureWindowOH(handle, tableSize.Width, tableSize.Height, tableSize.Width, tableSize.Height);
        //            }

        //            if (result == null)
        //            {
        //                Singleton.Log("null in CaptureWindow ", LogLevel.Error);
        //                return null;
        //            }
        //            else
        //            {
        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Singleton.Log("Exception in CaptureWindow " + ex.ToString(), LogLevel.Error);
        //        return null;
        //    }

        //}

        //public static Bitmap CaptureClientWindow(IntPtr handle, EnumCasino casino)
        //{
        //    try
        //    {
        //        lock (lockCapture)
        //        {
        //            bool isValid = IsWindow(handle);
        //            if (!isValid)
        //            {
        //                Singleton.Log("Handle is not window in CaptureClientWindow. ", LogLevel.Error);
        //                return null;
        //            }
        //            var rect = new RECT();
        //            GetWindowRect(handle, out rect);
        //            Size clientSize = ParseOH.GetTableClientSize(casino);
        //            Size tableSize = ParseOH.GetTableSize(casino);
        //            int border = Convert.ToInt32((tableSize.Width - clientSize.Width) / 2);
        //            int header = tableSize.Height - clientSize.Height - border;

        //            int x = rect.Left + border;
        //            int y = rect.Top + header;
        //            int width = rect.Right - rect.Left - 2 * border;
        //            int height = rect.Bottom - rect.Top - header - border;
        //            var bounds = new Rectangle(x, y, width, height);
        //            var result = new Bitmap(bounds.Width, bounds.Height);

        //            using (var graphics = Graphics.FromImage(result))
        //            {
        //                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
        //            }

        //            if (result == null)
        //            {
        //                Singleton.Log("null in CaptureClientWindow ", LogLevel.Error);
        //                return null;
        //            }
        //            else
        //            {
        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Singleton.Log("Exception in CaptureClientWindow: " + ex.ToString(), LogLevel.Error);
        //        return null;
        //    }

        //}

        private static bool GetWindowHandle(IntPtr windowHandle, ArrayList windowHandles)
        {
            windowHandles.Add(windowHandle);
            return true;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


        public static IntPtr GetHandleWithTitleWords(string word1, string word2)
        {
            // to get all the applications handler opened right now in your windows
            var windowHandles = new ArrayList();
            EnumedWindow callBackPtr = GetWindowHandle;
            EnumWindows(callBackPtr, windowHandles);

            foreach (IntPtr windowHandle in windowHandles.ToArray()) // loop thru each handler
            {
                string text = GetText(windowHandle).ToLower();
                if (text.Contains(word1.ToLower()) && text.Contains(word2.ToLower()))
                {
                    return windowHandle;
                }
            }

            return new IntPtr(0);
        }

        public static IntPtr GetHandleWithTitle(string title)
        {
            // to get all the applications handler opened right now in your windows
            var windowHandles = new ArrayList();
            EnumedWindow callBackPtr = GetWindowHandle;
            EnumWindows(callBackPtr, windowHandles);

            foreach (IntPtr windowHandle in windowHandles.ToArray()) // loop thru each handler
            {
                bool isVisible = IsWindowVisible(windowHandle);
                if (isVisible)  // if handler is visible or you can see it on your taskbar
                {
                    string text = GetText(windowHandle).ToLower();
                    if (text.Contains(title.ToLower()))
                    {
                        return windowHandle;
                    }
                }
            }

            return new IntPtr(0);
        }

        public static string GetText(IntPtr hWnd)
        {
            try
            {
                // Allocate correct GetWindowTextLengthstring length first
                int length = GetWindowTextLength(hWnd);
                StringBuilder sb = new StringBuilder(length + 1);
                GetWindowText(hWnd, sb, sb.Capacity);
                return sb.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}