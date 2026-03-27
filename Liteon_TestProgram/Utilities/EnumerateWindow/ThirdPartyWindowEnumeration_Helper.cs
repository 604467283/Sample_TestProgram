using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Liteon_TestProgram.Utilities.EnumerateWindow
{
    internal class ThirdPartyWindowEnumeration_Helper
    {
        /// <summary>
        /// 枚举找标题的窗口，也可以按className找
        /// </summary>
        /// <param name="targetWindowTitle"></param>
        /// <param name="targetWindowClassName"></param>
        /// <returns></returns>
        public IntPtr FindExpectWindow(string targetWindowTitle, string targetWindowClassName = null)
        {
            // 创建WindowFinder类的实例
            WindowFinder windowFinder = new WindowFinder();

            // 要查找的窗口标题
            IntPtr hwnd = IntPtr.Zero;
            // 查找窗口句柄
            if(string.IsNullOrEmpty(targetWindowTitle) == false && string.IsNullOrEmpty(targetWindowClassName) == false)
            {
                hwnd = windowFinder.FindWindowByClassNameAndTitle(targetWindowClassName, targetWindowTitle);
            }
            else if (string.IsNullOrEmpty(targetWindowTitle) == false)
            {
                hwnd = windowFinder.FindWindowByTitle(targetWindowTitle);
            }
            else if(string.IsNullOrEmpty(targetWindowClassName) == false)
            {
                hwnd = windowFinder.FindWindowByClassName(targetWindowClassName);
            }


            // 检查是否找到了窗口
            if (hwnd != IntPtr.Zero)
            {
                Console.WriteLine("Find the window handle: " + hwnd.ToString("X"));
            }
            else
            {
                Console.WriteLine("Window not found!");
            }

            return hwnd;
        }


        /// <summary>
        /// 使用FindExpectWindow找到的窗口的句柄，按控件名称枚举来确定控件，再修改它的text，一般用于textbox等
        /// </summary>
        /// <param name="windowHwnd"></param>
        /// <param name="targetControlTitle"></param>
        /// <param name="setText"></param>
        public void FindAndSetControlText(IntPtr windowHwnd, string targetControlTitle, string setText)
        {
            //枚举窗口中的所有控件
            List<ControlEnumerator.ControlInfo> controls = ControlEnumerator.GetControls(windowHwnd);

            foreach (var control in controls)
            {
                Console.WriteLine($"Handle: {control.Handle.ToString("X")}, Text: {control.Text}");

                if (control.Text.ToLower() == targetControlTitle.ToLower())
                {
                    ControlManipulator.SetEditText(control.Handle, setText);
                    break;
                }
            }
        }

        /// <summary>
        /// 使用FindExpectWindow找到的窗口的句柄，按枚举的个数来确定控件，再修改它的text，一般用于textbox等
        /// </summary>
        /// <param name="windowHwnd"></param>
        /// <param name="targetEnumeratedNums"></param>
        /// <param name="setText"></param>
        public void FindAndSetControlText(IntPtr windowHwnd, int targetEnumeratedNums, string setText)
        {
            //枚举窗口中的所有控件
            List<ControlEnumerator.ControlInfo> controls = ControlEnumerator.GetControls(windowHwnd);

            int iCount = 0;
            foreach (var control in controls)
            {
                Console.WriteLine($"{iCount} -> Handle: {control.Handle.ToString("X")}, Text: {control.Text}");

                if (iCount == targetEnumeratedNums)
                {
                    ControlManipulator.SetEditText(control.Handle, setText);
                    break;
                }

                iCount++;
            }
        }

        /// <summary>
        /// 使用FindExpectWindow找到的窗口的句柄，按枚举的个数来确定需要控件, 再获取控件Text，没有找到返回null
        /// </summary>
        /// <param name="windowHwnd"></param>
        /// <param name="targetEnumeratedNums"></param>
        /// <returns></returns>
        public string GetControlText(IntPtr windowHwnd, int targetEnumeratedNums)
        {
            //枚举窗口中的所有控件
            List<ControlEnumerator.ControlInfo> controls = ControlEnumerator.GetControls(windowHwnd);

            int iCount = 0;
            foreach (var control in controls)
            {
                Console.WriteLine($"{iCount} -> Handle: {control.Handle.ToString("X")}, Text: {control.Text}");

                if (iCount == targetEnumeratedNums)
                {
                    return control.Text;
                }

                iCount++;
            }

            return null;
        }

        /// <summary>
        /// 使用FindExpectWindow找到的窗口的句柄，按枚举的个数来确定需要控件，再发送回车给它，一般用于按钮等；
        /// </summary>
        /// <param name="windowHwnd"></param>
        /// <param name="targetEnumeratedNums"></param>
        public void SendControlEnter(IntPtr windowHwnd, int targetEnumeratedNums)
        {
            //枚举窗口中的所有控件
            List<ControlEnumerator.ControlInfo> controls = ControlEnumerator.GetControls(windowHwnd);

            int iCount = 0;
            foreach (var control in controls)
            {
                Console.WriteLine($"{iCount} -> Handle: {control.Handle.ToString("X")}, Text: {control.Text}");

                if (iCount == targetEnumeratedNums)
                {
                    // 发送回车键消息
                    ControlManipulator.SendEnterKeystroke(control.Handle);
                }
            }
        }


        /// <summary>
        /// 使用FindExpectWindow找到的窗口的句柄，按控件名称枚举来确定需要控件，再发送回车给它，一般用于按钮等；
        /// </summary>
        /// <param name="windowHwnd"></param>
        /// <param name="targetControlTitle"></param>
        public void SendControlEnter(IntPtr windowHwnd, string targetControlTitle)
        {
            //枚举窗口中的所有控件
            List<ControlEnumerator.ControlInfo> controls = ControlEnumerator.GetControls(windowHwnd);

            foreach (var control in controls)
            {
                Console.WriteLine($"Handle: {control.Handle.ToString("X")}, Text: {control.Text}");

                if (control.Text.ToLower() == targetControlTitle.ToLower())
                {
                    // 发送回车键消息
                    ControlManipulator.SendEnterKeystroke(control.Handle);
                }
            }
        }






    }
}
