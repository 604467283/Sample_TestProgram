using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.WIFIHelper
{
    internal  class NetworkAdapterHelper
    {
        /// <summary>
        /// 获取所有网络接口的信息（名称、描述、类型、MAC地址）
        /// </summary>
        public static List<NetworkAdapterInfo> GetAllAdapters()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Select(adapter => new NetworkAdapterInfo
                {
                    Name = adapter.Name,
                    Description = adapter.Description,
                    Type = adapter.NetworkInterfaceType,
                    MacAddress = adapter.GetPhysicalAddress().ToString()
                })
                .ToList();
        }

        /// <summary>
        /// 获取所有 Wi-Fi 网卡的信息
        /// </summary>
        public static List<NetworkAdapterInfo> GetWifiAdapters()
        {
            return GetAllAdapters()
                .Where(adapter => adapter.Type == NetworkInterfaceType.Wireless80211)
                .ToList();
        }

        /// <summary>
        /// 根据描述名称（Description）查找网卡的 MAC 地址
        /// </summary>
        /// <param name="description">网卡描述（如 "Intel(R) Wi-Fi 6 AX201"）</param>
        /// <param name="ignoreCase">是否忽略大小写（默认true）</param>
        public static string GetMacAddressByDescription(string description, bool ignoreCase = true)
        {
            var adapter = GetAllAdapters()
                .FirstOrDefault(a => a.Description.Equals(description,
                    ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));

            return adapter?.MacAddress ?? "未找到匹配的网卡";
        }

        /// <summary>
        /// 根据描述名称（模糊匹配）查找网卡的 MAC 地址
        /// </summary>
        /// <param name="partialDescription">网卡描述的部分名称（如 "Intel" 或 "Wi-Fi 6"）</param>
        /// <param name="ignoreCase">是否忽略大小写（默认true）</param>
        public static string GetMacAddressByDescriptionLike(string partialDescription, bool ignoreCase = true)
        {
            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            var adapter = GetAllAdapters()
                .FirstOrDefault(a => a.Description.Contains(partialDescription, comparison));

            return adapter?.MacAddress ?? "未找到匹配的网卡";
        }


    }


    /// <summary>
    /// 网卡信息模型
    /// </summary>
    public class NetworkAdapterInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public NetworkInterfaceType Type { get; set; }
        public string MacAddress { get; set; }
    }
}
