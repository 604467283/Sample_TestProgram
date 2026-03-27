using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SFCHelpers
{
    internal static class Program
    {

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);



            // 创建主窗体
            SFCHelpers sFCHelpers = new SFCHelpers();

            // 解析命令行参数
            var parsedArgs = sFCHelpers.ParseCommandLineArgs(args);

            // 如果有命令行参数，处理它们
            if (parsedArgs.Count > 0)
            {
                sFCHelpers.ProcessCommandLineArgs(parsedArgs);
            }
            else
            {
                Application.Run(new SFCHelpers());
            }




        }
    }
}
