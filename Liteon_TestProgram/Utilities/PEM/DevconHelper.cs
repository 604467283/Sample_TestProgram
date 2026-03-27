using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.PEM
{
    internal class DevconHelper
    {

        private static readonly string DevConPath = FindDevCon();

        // ================== 核心方法 ================== //
        /// <summary>
        /// 刷新设备管理器（扫描硬件更改）
        /// </summary>
        public static bool Rescan(int iTimeout = 2000) => RunDevConCommand("rescan", iTimeout);

        /// <summary>
        /// 启用指定设备（通过硬件ID或设备实例ID）
        /// </summary>
        public static bool Enable(string deviceId, int iTimeout = 2000) => RunDevConCommand($"enable \"{deviceId}\"", iTimeout);

        /// <summary>
        /// 禁用指定设备（通过硬件ID或设备实例ID）
        /// </summary>
        public static bool Disable(string deviceId, int iTimeout = 2000) => RunDevConCommand($"disable \"{deviceId}\"", iTimeout);

        // ================== 新增功能：按名称操作设备 ================== //
        /// <summary>
        /// 根据设备名称获取硬件ID（支持模糊匹配）
        /// </summary>
        /// <param name="deviceName">设备名称（如"Realtek USB GbE"）</param>
        /// <returns>硬件ID（如"PCI\\VEN_10EC&DEV_8168"），未找到时返回null</returns>
        public static string GetDeviceIdByName(string deviceName)
        {
            try
            {
                // 方法1：优先使用WMI查询（更快）
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT HardwareID, Description FROM Win32_PnPEntity");
                foreach (ManagementObject obj in searcher.Get())
                {
                    string description = obj["Description"]?.ToString();
                    if (description != null && description.Contains(deviceName))
                    {
                        string[] hardwareIds = (string[])obj["HardwareID"];
                        return hardwareIds?.FirstOrDefault(); // 返回第一个硬件ID
                    }
                }

                // 方法2：回退到devcon.exe（更准确）
                string output = RunDevConAndGetOutput("findall");
                using (StringReader reader = new StringReader(output))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(deviceName) && line.Contains(": "))
                        {
                            return line.Split(new[] { ": " }, StringSplitOptions.None)[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取设备ID失败: {ex.Message}");
                return $"获取设备ID失败: {ex.Message}";
            }
            return null;
        }

        /// <summary>
        /// 根据设备名称禁用设备
        /// </summary>
        public static bool DisableByName(string deviceName)
        {
            string deviceId = GetDeviceIdByName(deviceName);
            return !string.IsNullOrEmpty(deviceId) && Disable(deviceId);
        }

        /// <summary>
        /// 根据设备名称启用设备
        /// </summary>
        public static bool EnableByName(string deviceName)
        {
            string deviceId = GetDeviceIdByName(deviceName);
            return !string.IsNullOrEmpty(deviceId) && Enable(deviceId);
        }

        // ================== 私有方法 ================== //
        private static bool RunDevConCommand(string arguments, int iTimeout)
        {
            if (string.IsNullOrEmpty(DevConPath))
            {
                Console.WriteLine("错误: 未找到 devcon.exe");
                return false;
            }

            try
            {
                UIHandleHelper.ShowRunLog(arguments);
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = DevConPath,
                        Arguments = arguments,
                        Verb = "runas",
                        UseShellExecute = true,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    process.Start();
                    process.WaitForExit();
                    Thread.Sleep(iTimeout);
                    return process.ExitCode == 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"执行命令失败: {ex.Message}");
                return false;
            }
        }

        private static string RunDevConAndGetOutput(string arguments)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = DevConPath,
                        Arguments = arguments,
                        Verb = "runas",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true
                    };
                    process.Start();
                    return process.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取命令输出失败: {ex.Message}");
                return string.Empty;
            }
        }

        private static string FindDevCon()
        {
            string[] searchPaths =
            {
            "devcon.exe",
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "devcon.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "devcon.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Windows Kits", "10", "Tools", "x64", "devcon.exe")
        };

            return searchPaths.FirstOrDefault(File.Exists);
        }



    }
}
