using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.EnumerateWindow
{
    internal class WindowFinder
    {
        // 导入user32.dll中的FindWindow函数
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // 公开方法，根据窗口标题查找窗口句柄
        public IntPtr FindWindowByTitle(string windowTitle)
        {
            // 调用FindWindow函数，并返回结果
            return FindWindow(null, windowTitle);
            // 注意：这里将类名参数设置为null，表示我们根据窗口标题来查找窗口
        }


        // 公开方法，根据窗口类名查找窗口句柄
        public IntPtr FindWindowByClassName(string className)
        {
            // 调用FindWindow函数，并传入类名和null作为窗口标题
            // 因为我们只想根据类名来查找窗口
            return FindWindow(className, null);
        }

        public IntPtr FindWindowByClassNameAndTitle(string className, string windowTitle)
        {
            // 调用FindWindow函数，并传入类名和null作为窗口标题
            // 因为我们只想根据类名来查找窗口
            return FindWindow(className, windowTitle);
        }



    }
}
