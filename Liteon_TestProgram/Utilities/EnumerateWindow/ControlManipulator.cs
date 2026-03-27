using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.EnumerateWindow
{
    internal class ControlManipulator
    {
        #region 设定控件Text

        // 导入必要的Windows API函数
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, StringBuilder lParam);

        private const int EM_SETTEXT = 0x00C2;

        // 设置编辑框控件的文本
        public static void SetEditText(IntPtr hWndEdit, string text)
        {
            StringBuilder sb = new StringBuilder(text, text.Length + 1); // +1以包含null终止符
            SendMessage(hWndEdit, EM_SETTEXT, IntPtr.Zero, sb);
        }

        #endregion


        #region 给控件发送回车

        // Import SendMessage function from user32.dll
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // Constants for SendMessage function
        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const uint VK_RETURN = 0x0D; // Virtual-Key Code for ENTER key

        // Method to send an ENTER keystroke to a specified window handle
        public static void SendEnterKeystroke(IntPtr hWnd)
        {
            // Send key down event for ENTER key
            PostMessage(hWnd, WM_KEYDOWN, (IntPtr)VK_RETURN, IntPtr.Zero);
            // Send key up event for ENTER key
            PostMessage(hWnd, WM_KEYUP, (IntPtr)VK_RETURN, IntPtr.Zero);
        }

        #endregion

    }
}
