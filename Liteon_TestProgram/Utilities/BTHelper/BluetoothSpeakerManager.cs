using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using Liteon_TestProgram.Forms;

namespace Liteon_TestProgram.Utilities.BTHelper
{
    public class BluetoothSpeakerManager : IDisposable
    {
        private BluetoothClient _bluetoothClient;
        private BluetoothDeviceInfo _connectedDevice;
        private bool _disposed = false;

        /// <summary>
        /// 发现附近的蓝牙设备
        /// </summary>
        /// <returns>发现的蓝牙设备数组</returns>
        public BluetoothDeviceInfo[] DiscoverDevices()
        {
            _bluetoothClient = new BluetoothClient();
            return _bluetoothClient.DiscoverDevices();
        }

        /// <summary>
        /// 通过设备名称获取蓝牙地址
        /// </summary>
        /// <param name="deviceName">蓝牙设备名称</param>
        /// <returns>蓝牙地址，如果未找到则返回null</returns>
        public BluetoothAddress GetDeviceAddressByName(string deviceName)
        {
            var devices = DiscoverDevices();
            var device = devices.FirstOrDefault(d => d.DeviceName.Equals(deviceName, StringComparison.OrdinalIgnoreCase));
            return device?.DeviceAddress;
        }

        /// <summary>
        /// 配对蓝牙设备
        /// </summary>
        /// <param name="deviceAddress">蓝牙设备地址</param>
        /// <param name="pinCode">配对码（可选）</param>
        /// <returns>配对是否成功</returns>
        public bool PairDevice(BluetoothAddress deviceAddress, string pinCode = null)
        {
            try
            {
                return BluetoothSecurity.PairRequest(deviceAddress, pinCode);
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"配对失败: {ex.Message}", true);
                return false;
            }
        }

        /// <summary>
        /// 通过设备名称配对蓝牙设备
        /// </summary>
        /// <param name="deviceName">蓝牙设备名称</param>
        /// <param name="pinCode">配对码（可选）</param>
        /// <returns>配对是否成功</returns>
        public bool PairDeviceByName(string deviceName, string pinCode = null)
        {
            var address = GetDeviceAddressByName(deviceName);
            if (address == null)
            {
                UIHandleHelper.ShowRunLog($"未找到名为'{deviceName}'的蓝牙设备", true);
                return false;
            }
            return PairDevice(address, pinCode);
        }

        /// <summary>
        /// 连接到蓝牙音箱
        /// </summary>
        /// <param name="deviceAddress">蓝牙设备地址</param>
        /// <param name="serviceGuid">服务GUID</param>
        /// <returns>连接是否成功</returns>
        public bool ConnectToSpeaker(BluetoothAddress deviceAddress, string serviceGuid)
        {
            try
            {
                if (deviceAddress == null)
                {
                    throw new ArgumentNullException(nameof(deviceAddress), "蓝牙设备地址不能为空");
                }

                var device = new BluetoothDeviceInfo(deviceAddress);

                if (device.InstalledServices != null && device.InstalledServices.Length > 0)
                {
                    UIHandleHelper.ShowRunLog("Installed Services (UUIDs):");
                    foreach (Guid serviceGuid1 in device.InstalledServices)
                    {
                        UIHandleHelper.ShowRunLog(serviceGuid1.ToString("B")); // "B" 格式为 {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}

                        // 可选：获取已知服务的名称（如 SPP、A2DP 等）
                        string serviceName = BluetoothService.GetName(serviceGuid1);
                        if (!string.IsNullOrEmpty(serviceName))
                        {
                            UIHandleHelper.ShowRunLog($"  - Service Name: {serviceName}");
                        }
                    }
                }

                if (!device.Authenticated && !PairDevice(deviceAddress))
                {
                    UIHandleHelper.ShowRunLog("BT设备认证失败", true);
                    return false;
                }

                _bluetoothClient = new BluetoothClient();
                var audioServiceUuid = new Guid(serviceGuid);
                _bluetoothClient.Connect(device.DeviceAddress, audioServiceUuid);

                _connectedDevice = device;
                UIHandleHelper.ShowRunLog($"已连接到蓝牙音响: {device.DeviceName}");
                return true;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"蓝牙连接失败: {ex.Message}", true);
                return false;
            }
        }

        /// <summary>
        /// 通过设备名称连接到蓝牙音箱
        /// </summary>
        /// <param name="deviceName">蓝牙设备名称</param>
        /// <param name="serviceGuid">服务GUID</param>
        /// <returns>连接是否成功</returns>
        public bool ConnectToSpeakerByName(string deviceName, string serviceGuid)
        {
            var address = GetDeviceAddressByName(deviceName);
            if (address == null)
            {
                UIHandleHelper.ShowRunLog($"未找到名为'{deviceName}'的蓝牙设备", true);
                return false;
            }
            else
            {
                MessageBoxEX.Show("<关掉此窗口>，若新配对设备，请准备~点击电脑系统的蓝牙连接请求", true);
            }
            return ConnectToSpeaker(address, serviceGuid);
        }

        /// <summary>
        /// 断开当前蓝牙连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (_bluetoothClient != null)
                {
                    _bluetoothClient.Close();
                    _bluetoothClient.Dispose();
                    _bluetoothClient = null;
                    _connectedDevice = null;
                    UIHandleHelper.ShowRunLog("已断开蓝牙音响连接！");
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"BT断开连接失败: {ex.Message}", true);
            }
        }

        /// <summary>
        /// 检查当前是否已连接
        /// </summary>
        public bool IsConnected => _bluetoothClient != null && _connectedDevice != null;

        /// <summary>
        /// 获取当前连接的设备信息
        /// </summary>
        public BluetoothDeviceInfo ConnectedDevice => _connectedDevice;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Disconnect();
                }
                _disposed = true;
            }
        }

        ~BluetoothSpeakerManager()
        {
            Dispose(false);
        }
    }
}
