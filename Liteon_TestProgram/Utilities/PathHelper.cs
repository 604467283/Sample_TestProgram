using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class PathHelper
    {
        // 获取当前EXE文件所在的目录路径
        public static string GetCurrentExeDirPath()
        {
            //E:\AreaX\Liteon_TestProgram\Liteon_TestProgram\bin\Debug\net6.0-windows
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        //获取当前exe所在的上级目录
        public static string GetParentDirectoryPath(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
                return null;

            //] E:\AreaX\Liteon_TestProgram\Liteon_TestProgram\bin\Debug
            // 使用Path.GetDirectoryName获取目录部分
            return Path.GetDirectoryName(fullPath);
        }

        //获取当前exe所在的上级目录
        public static string GetExeParentDirectory()
        {
           //E:\AreaX\ConsoleApp13\ConsoleApp13\bin\Debug
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string exeDirectory = Directory.GetParent(baseDirectory).FullName;
            string parentDirectory = Directory.GetParent(exeDirectory).FullName;
            return parentDirectory;
        }


    }
}
