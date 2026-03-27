using Liteon_TestProgram.Utilities.PEM;
using SimpleWifi;
using SimpleWifi.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.WIFIHelper
{
    internal class WifiHelper
    {
        private Wifi wifi = new Wifi();
        private AccessPoint accessPoint;

        public (bool, int )ConnectWifi(string ssid, string password, bool bShowAllWifi = false)
        {
            //https://blog.csdn.net/liupengcaho/article/details/135845688

            int iSignalStrength = 0;
            bool bConnected = false;
            //wifi = new Wifi();

            if (wifi.NoWifiAvailable)
            {
                UIHandleHelper.ShowRunLog("-- NO WIFI CARD WAS FOUND --", true);
                return (false, 0);
            }

            try
            {
                // 获取所有WIFI列表，遍历找到
                wifi.GetAccessPoints().ForEach(item =>
                {
                    if (bShowAllWifi)
                    {
                        // 获取所有的WIFI
                        UIHandleHelper.ShowRunLog($"Nearby WIFI:{item.Name}");
                    }

                    
                    // 根据名称找到想要连接的WIFI
                    if (item.Name == ssid)
                    {
                        accessPoint = item;

                        AuthRequest authRequest = new AuthRequest(item);
                        // WIFI密码
                        authRequest.Password = password;
                        // 同步
                        bool result = bConnected = item.Connect(authRequest);

                        uint uiSignalStrength = item.SignalStrength;

                        Thread.Sleep(1000);
                        iSignalStrength = checked((int)uiSignalStrength);

                        // 打印连接结果
                        UIHandleHelper.ShowRunLog($"{ssid} Wifi Connect:" + result);
                    }
                });
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Wifi connect error: {ex}", true);
                return (false, 0);
            }

            if (!bConnected)
            {
                UIHandleHelper.ShowRunLog($"{ssid} Wifi Connect Fail", true);
            }


            return (bConnected, iSignalStrength);
        }

        /// <summary>
        ///  0->Disconnected, 1->Connected
        /// </summary>
        /// <returns></returns>
        public int GetWifiConnectStatus()
        {
            int currentStatus = -1;

            if (wifi != null )
            {
                currentStatus = (int)wifi.ConnectionStatus;
            }

            return currentStatus;
        }

        public void DisConnectWifi()
        {
            if (wifi!=null && accessPoint!=null)
            {
                accessPoint.DeleteProfile();
                wifi.Disconnect();
            }
        }

        //=================================================================

        /// <summary>
        /// -1: 默认值no wifi adapter  0: Check pass  1: check mac fail  2: check id fail  3: check error   此方法还返回两个string，分别是读出来的mac和id
        /// </summary>
        /// <param name="adapterName"></param>
        /// <param name="mac"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        /*
        public (int, string, string )CheckWifiInfoByName(string adapterName, string mac, string id)  // 替换为你的WLAN适配器实际名称
        {
            int iResult = -1;
            try
            {
                // 搜索WLAN适配器，这里假设你知道适配器的名称或部分名称
                string wlanAdapterName = adapterName; // 请替换为你的WLAN适配器实际名称的一部分  Realtek RTL8192DU Wireless LAN 802.11n USB 2.0 Network Adapter
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE Name LIKE '%" + wlanAdapterName + "%'");

                foreach (ManagementObject obj in searcher.Get())
                {
                    // 输出网络适配器的名称和其他属性
                    Console.WriteLine("WLAN适配器名称: " + obj["Name"]);
                    Console.WriteLine("MAC地址: " + obj["MacAddress"]);
                    Console.WriteLine("PNP设备ID: " + obj["PNPDeviceID"]);

                    string macAddress = obj["MacAddress"]?.ToString().Replace(":", "");
                    string deviceID = obj["PNPDeviceID"]?.ToString();

                    UIHandleHelper.ShowRunLog($"WIFI adapter name: {obj["Name"]}");
                    UIHandleHelper.ShowRunLog($"WIFI mac address: {macAddress}");

                    if (String.IsNullOrEmpty(macAddress))
                    {
                        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                        foreach (NetworkInterface adapter in adapters)
                        {
                            if (adapter.Description.Contains(adapterName))
                            {
                                UIHandleHelper.ShowRunLog("MAC地址: " + adapter.GetPhysicalAddress().ToString());
                                macAddress = adapter.GetPhysicalAddress().ToString();

                                // 获取网络接口的 ID
                                string interfaceId = adapter.Id;
                                UIHandleHelper.ShowRunLog("网络接口ID: " + interfaceId);

                                break;
                            }
                        }
                    }

                    if (macAddress == mac)
                    {
                        iResult =  0;
                    }
                    else
                    {
                        iResult =  1;
                    }



                    if (iResult == 0)
                    {
                        UIHandleHelper.ShowRunLog($"WIFI ID info: {deviceID}");
                        if (deviceID.Contains(id))
                        {
                            iResult = 0;
                        }
                        else
                        {
                            iResult = 2;
                        }
                    }

                    return (iResult, macAddress, id);

                }

            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog("Check wifi ids error: " + ex.Message, true);
                return (3, ex.Message, ex.Message);
            }

            return (iResult, null, null);
        }
        */


        /// <summary>
        /// -1: 默认值no wifi adapter  0: Check pass  1: check mac fail  2: check id fail  3: check error   此方法还返回两个string，分别是读出来的mac和id
        /// </summary>
        /// <param name="adapterName"></param>
        /// <param name="mac"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public (int, string, string) CheckWifiInfoByName(string adapterName, string mac, string id) 
        {
            string macAddress = "";
            string interfaceId = "";



            try
            {
                // 构建完整的命令
                //string command = $@"wmic nic where ""Description LIKE '%{adapterName}%'"" get MACAddress /value";

                //CustomProcess customProcess = new("cmd.exe");
                //bool bRetVal = customProcess.RunCommandLine($"/c {command} | find \"=\"", "MACAddress=", out string msg, 1000, 3);

                //if (!bRetVal)
                //{
                //    UIHandleHelper.ShowRunLog($"警告：未找到匹配的网络适配器 '{adapterName}'", true);
                //    macAddress = "00-00-00-00-00-00"; // 默认 MAC 地址
                //    interfaceId = "N/A"; // 默认接口 ID
                //    return (-1, macAddress, interfaceId);
                //}
                //else
                //{

                //    string[] parts = msg.Split(new[] { "MACAddress=" }, StringSplitOptions.None);

                //    // 从后往前查找包含有效MAC地址的部分
                //    for (int i = parts.Length - 1; i >= 0; i--)
                //    {
                //        if (!string.IsNullOrWhiteSpace(parts[i]))
                //        {
                //            // 提取MAC地址部分（去除前后空白和冒号）
                //            string potentialMac = parts[i].Trim()
                //                                        .Replace("\r", "")
                //                                        .Replace("\n", "")
                //                                        .Replace(":", "")
                //                                        .Replace(" ", "");

                //            // 简单验证MAC地址长度（标准MAC地址去除冒号后应为12位）
                //            if (potentialMac.Length >= 12)
                //            {
                //                macAddress = potentialMac.Substring(0, 12); // 取前12个字符
                //                break;
                //            }
                //        }
                //    }


                //    if (!string.IsNullOrEmpty(macAddress))
                //    {
                //        UIHandleHelper.ShowRunLog($"Wifi Mac: {macAddress}");
                //    }
                //    else
                //    {
                //        UIHandleHelper.ShowRunLog("No MAC Address found.", true);
                //        return (1, macAddress, interfaceId);
                //    }
                //}

                macAddress = NetworkAdapterHelper.GetMacAddressByDescriptionLike(adapterName);

                if (!string.IsNullOrEmpty(macAddress))
                {
                    UIHandleHelper.ShowRunLog($"Wifi Mac: {macAddress}");
                }
                else
                {
                    UIHandleHelper.ShowRunLog("No MAC Address found.", true);
                    return (1, macAddress, interfaceId);
                }

                // 获取网络接口的 ID
                interfaceId = DevconHelper.GetDeviceIdByName(adapterName);
                UIHandleHelper.ShowRunLog("网络接口硬件ID: " + interfaceId);

                if (macAddress != mac)
                {
                    return (1, macAddress, interfaceId);
                }


                if (interfaceId.Contains(id) == false)
                {
                    return (2, macAddress, interfaceId);
                }
                return (0, macAddress, interfaceId);
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog("Check wifi ids error: " + ex.Message, true);
                return  (3, macAddress, interfaceId);
            }
        }


        public void DisableWifiByName(string adapterName)  // 替换为你的WLAN适配器实际名称
        {
            try
            {
                // 搜索WLAN适配器，这里假设你知道适配器的名称或部分名称
                string wlanAdapterName = adapterName; // 请替换为你的WLAN适配器实际名称的一部分  Realtek RTL8192DU Wireless LAN 802.11n USB 2.0 Network Adapter
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE Name LIKE '%" + wlanAdapterName + "%' AND NetConnectionStatus IS NOT NULL");

                foreach (ManagementObject obj in searcher.Get())
                {
                    // 输出网络适配器的名称
                    Console.WriteLine("WIFI 适配器名称: " + obj["Name"]);
                    UIHandleHelper.ShowRunLog($"WIFI adapter Name: {obj["Name"]}");
                }

                foreach (ManagementObject adapter in searcher.Get())
                {
                    // 禁用WLAN适配器
                    DisableWlanAdapter(adapter);

                    // 等待一段时间，这里设置为1秒，你可以根据需要调整
                    System.Threading.Thread.Sleep(1000);

                    // 重新启用WLAN适配器
                    //EnableWlanAdapter(adapter);
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog("Disable wifi by name error: " + ex.Message, true);
                Console.WriteLine("Disable wifi by name error: " + ex);
            }
        }


        public void EnableWifiByName(string adapterName)  // 替换为你的WLAN适配器实际名称
        {
            try
            {
                // 搜索WLAN适配器，这里假设你知道适配器的名称或部分名称
                string wlanAdapterName = adapterName; // 请替换为你的WLAN适配器实际名称的一部分  Realtek RTL8192DU Wireless LAN 802.11n USB 2.0 Network Adapter
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE Name LIKE '%" + wlanAdapterName + "%' AND NetConnectionStatus IS NOT NULL");

                foreach (ManagementObject obj in searcher.Get())
                {
                    // 输出网络适配器的名称
                    Console.WriteLine("WIFI 适配器名称: " + obj["Name"]);
                    UIHandleHelper.ShowRunLog($"WIFI adapter Name: {obj["Name"]}");
                }

                foreach (ManagementObject adapter in searcher.Get())
                {
                    // 重新启用WLAN适配器
                    EnableWlanAdapter(adapter);

                    // 等待一段时间，这里设置为2秒，你可以根据需要调整
                    System.Threading.Thread.Sleep(2000);                
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog("Enable wifi by name error: " + ex.Message, true);
                Console.WriteLine("Enable wifi by name error: " + ex);
            }
        }


        void DisableWlanAdapter(ManagementObject adapter)
        {
            try
            {
                ManagementBaseObject inParams = adapter.GetMethodParameters("Disable");
                ManagementBaseObject outParams = adapter.InvokeMethod("Disable", inParams, null);

                uint returnValue = (uint)outParams["ReturnValue"];
                if (returnValue == 0)
                {
                    UIHandleHelper.ShowRunLog("The WLAN adapter is disabled successfully.");
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Failed to disable the WLAN adapter，return code: " + returnValue, true);
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog("Disable the WLAN adapter error: " + ex.Message, true);
                Console.WriteLine("Disable the WLAN adapter error: " + ex);
            }
        }

        void EnableWlanAdapter(ManagementObject adapter)
        {
            try
            {
                ManagementBaseObject inParams = adapter.GetMethodParameters("Enable");
                ManagementBaseObject outParams = adapter.InvokeMethod("Enable", inParams, null);

                // 检查适配器当前状态
                bool isEnabled = (ushort)adapter["NetConnectionStatus"] == 2; // 2 表示已enable
                if (isEnabled)
                {
                    UIHandleHelper.ShowRunLog("The WLAN adapter is already enabled.");
                    return;
                }

                uint returnValue = (uint)outParams["ReturnValue"];
                if (returnValue == 0)
                {
                    UIHandleHelper.ShowRunLog("The WLAN adapter is enable successfully.");
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Failed to enable the WLAN adapter，return code: " + returnValue, true);
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog("Enable the WLAN adapter error:" + ex.Message, true);
                Console.WriteLine("Enable wifi by name error: " + ex);
            }
        }



        void EnableWlanAdapterUsingNetsh(string adapterName, bool bEnable)
        {
            try
            {
                string str_Arguments = null;
                if (bEnable)
                {
                    str_Arguments = $"interface set interface \"{adapterName}\" enable";
                }
                else 
                {
                    str_Arguments = $"interface set interface \"{adapterName}\" disable";
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = str_Arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Verb = "runas" // 以管理员权限运行
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    Console.WriteLine( $"output: {output}" );
                    Console.WriteLine( $"error: {error}" );

                    if (process.ExitCode == 0)
                    {
                        UIHandleHelper.ShowRunLog("The WLAN adapter is enabled successfully using netsh.");
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"Failed to enable the WLAN adapter using netsh. Error: {error}", true);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog("Enable the WLAN adapter using netsh error: " + ex.Message, true);
            }
        }

        /// <summary>
        /// 使用netsh wlan show interfaces后用来提取获取型号强度
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int GetSignalPercentage(string ssidName)
        {

            CustomProcess customProcess = new CustomProcess("netsh.exe");

            bool bRetVal = customProcess.RunCommandLine(" wlan show interfaces", ssidName, out string msg, 2000, 1);

            if (!bRetVal)
            {
                return -1; // 如果没有找到返回-1
            }

            // 按行分割文本
            string[] lines = msg.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                // 查找包含百分号的行
                if (line.Contains("%"))
                {
                    // 找到百分号的位置
                    int percentIndex = line.IndexOf('%');
                    if (percentIndex > 0)
                    {
                        // 从百分号向前查找数字的开始位置
                        int startIndex = percentIndex - 1;
                        while (startIndex >= 0 && char.IsDigit(line[startIndex]))
                        {
                            startIndex--;
                        }

                        // 提取数字部分
                        string numberStr = line.Substring(startIndex + 1, percentIndex - startIndex - 1);
                        if (int.TryParse(numberStr, out int result))
                        {
                            return result;
                        }
                    }
                }
            }

            return -1; // 如果没有找到返回-1
        }




    }
}
