using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.EnumerateWindow
{
    internal class ControlEnumerator
    {

        // 导入必要的Windows API函数
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        // 用于存储控件信息的类
        public class ControlInfo
        {
            public IntPtr Handle { get; set; }
            public string Text { get; set; }
        }

        // 递归枚举控件
        private static List<ControlInfo> EnumerateControls(IntPtr hWndParent)
        {
            List<ControlInfo> controls = new List<ControlInfo>();
            IntPtr hWndChild = FindWindowEx(hWndParent, IntPtr.Zero, null, null);

            while (hWndChild != IntPtr.Zero)
            {
                ControlInfo controlInfo = new ControlInfo
                {
                    Handle = hWndChild
                };

                StringBuilder sb = new StringBuilder(256);
                GetWindowText(hWndChild, sb, sb.Capacity);
                controlInfo.Text = sb.ToString();

                controls.Add(controlInfo);

                // 递归枚举子控件
                controls.AddRange(EnumerateControls(hWndChild));

                hWndChild = FindWindowEx(hWndParent, hWndChild, null, null);
            }

            return controls;
        }

        // 公开方法，用于枚举给定窗口句柄的所有控件
        public static List<ControlInfo> GetControls(IntPtr hWnd)
        {
            return EnumerateControls(hWnd);
        }

    }
}
