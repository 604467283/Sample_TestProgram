using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class ProcessHelper
    {

        public static void KillProcessByName(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit(); // 可选，等待进程真正结束
                    Console.WriteLine($"{process.ProcessName} has been killed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to kill {process.ProcessName}: {ex.Message}");
                }
            }
        }




    }
}
