using Liteon_TestProgram.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class PCHelper
    {
        public static string GetDiskSN()
        {
            try
            {
                // 创建一个ManagementObjectSearcher对象，用于查询WMI信息
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

                // 遍历查询结果
                foreach (ManagementObject obj in searcher.Get())
                {
                    // 尝试获取硬盘的序列号
                    string serialNumber = obj["SerialNumber"]?.ToString();

                    // 如果序列号不为空，则打印出来
                    if (!string.IsNullOrEmpty(serialNumber))
                    {
                        Console.WriteLine("硬盘序列号: " + serialNumber);
                        UIHandleHelper.ShowRunLog("Hard disk sn is " + serialNumber);
                        return serialNumber;
                        // 注意：在某些情况下，Win32_PhysicalMedia 可能需要查询其他WMI类（如 Win32_DiskDrive ）来获取更准确的序列号信息
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕获并打印任何异常信息
                Console.WriteLine("获取硬盘序列号时出错: " + ex.Message);
                MessageBoxEX.Show("获取硬盘序列号时出错: " + ex.Message, true);
            }

            return null;
        }


        public static string GetMainboardSN()
        {
            try
            {
                // 创建一个ManagementObjectSearcher对象，用于查询Win32_BaseBoard类以获取主板信息
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");

                // 获取查询结果中的第一个对象（通常系统中只有一个主板）
                ManagementObject board = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                if (board != null)
                {
                    // 从主板对象中获取型号和序列号
                    //string model = board["Model"]?.ToString();
                    string serialNumber = board["SerialNumber"]?.ToString();

                    // 打印主板型号和序列号
                    //Console.WriteLine("主板型号: " + (model ?? "未知"));
                    Console.WriteLine("主板序列号: " + (serialNumber ?? "unknow"));
                    UIHandleHelper.ShowRunLog("Mainboard sn is " + serialNumber);
                    return serialNumber;
                }
                else
                {
                    Console.WriteLine("未能找到主板信息。");
                    MessageBoxEX.Show("未能找到主板信息。", true);
                }
            }
            catch (Exception ex)
            {
                // 捕获并打印异常信息
                Console.WriteLine("获取主板信息时发生异常: " + ex.Message);
                MessageBoxEX.Show("获取主板信息时发生异常: " + ex.Message, true);
            }

            return null;
        }





    }
}
