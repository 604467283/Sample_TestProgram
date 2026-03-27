using System;
using System.IO;
using System.Management;
using System.Net.Mail;
using System.Threading.Tasks;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Liteon_TestProgram.Forms;
using NAudio.Wave;

namespace Liteon_TestProgram.Utilities.BTHelper
{
    /// <summary>
    /// 传输进度事件参数
    /// </summary>
    public class TransferProgressEventArgs : EventArgs
    {
        public int Percentage { get; set; }  // 进度百分比（-1表示未知）
        public long BytesTransferred { get; set; }  // 已传输字节数
        public long? TotalBytes { get; set; }  // 总字节数（可能为null）
    }

    /// <summary>
    /// 蓝牙传输结果
    /// </summary>
    public class BluetoothTransferResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Exception Error { get; set; }
        public string FilePath { get; set; }

        public static BluetoothTransferResult Success(string message = null, string filePath = null)
        {
            return new BluetoothTransferResult
            {
                IsSuccess = true,
                Message = message,
                FilePath = filePath
            };
        }

        public static BluetoothTransferResult Fail(string message, Exception error = null)
        {
            return new BluetoothTransferResult
            {
                IsSuccess = false,
                Message = message,
                Error = error
            };
        }
    }

    /// <summary>
    /// 蓝牙图片传输服务
    /// </summary>
    public class BluetoothImageTransferService
    {
        private BluetoothClient _bluetoothClient;
        private BluetoothDeviceInfo _device;
        private readonly Guid _serviceClassId = BluetoothService.ObexObjectPush;

        private WaveOutEvent _waveOut;

        public event EventHandler<TransferProgressEventArgs> TransferProgress;

        /// <summary>
        /// 搜索附近的蓝牙设备
        /// </summary>
        public BluetoothDeviceInfo[] DiscoverDevices()
        {
            _bluetoothClient = new BluetoothClient();
            return _bluetoothClient.DiscoverDevices();
        }

        /// <summary>
        /// 选择要连接的设备
        /// </summary>
        public void SelectDevice(BluetoothAddress deviceAddress)
        {
            _device = new BluetoothDeviceInfo(deviceAddress);
        }




        /// <summary>
        /// 发送图片文件
        /// </summary>
        public async Task<BluetoothTransferResult> SendImageAsync(string filePath)
        {
            if (_device == null)
                return BluetoothTransferResult.Fail("未选择蓝牙设备");

            if (!File.Exists(filePath))
                return BluetoothTransferResult.Fail("图片文件不存在");

            ObexWebRequest request = null;
            Stream fileStream = null;

            try
            {
                var fileInfo = new FileInfo(filePath);
                var uri = new Uri($"obex://{_device.DeviceAddress}/{Path.GetFileName(filePath)}");

                fileStream = File.OpenRead(filePath);
                request = new ObexWebRequest(uri);
                request.ContentLength = fileInfo.Length;

                var requestStream = request.GetRequestStream();
                var buffer = new byte[8192];
                int bytesRead;
                long totalBytesRead = 0;

                while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await requestStream.WriteAsync(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;

                    // 报告进度
                    var percent = (int)((totalBytesRead * 100) / fileInfo.Length);
                    OnTransferProgress(percent, totalBytesRead, fileInfo.Length);
                }

                var response = (ObexWebResponse)await request.GetResponseAsync();
                var statusCode = response.StatusCode;
                response.Close();

                //return statusCode == ObexStatusCode.OK | InTheHand.Net.ObexStatusCode.Final
                //    ? BluetoothTransferResult.Success("图片发送成功", filePath)
                //    : BluetoothTransferResult.Fail($"发送失败，状态码: {statusCode}");


                // 正确的状态码检查方式
                bool isSuccess = response.StatusCode == (ObexStatusCode.OK | ObexStatusCode.Final);

                return isSuccess
                    ? BluetoothTransferResult.Success("图片发送成功", filePath)
                    : BluetoothTransferResult.Fail($"发送失败，状态码: {response.StatusCode} (0x{((int)response.StatusCode):X2})");
            }
            catch (Exception ex)
            {
                return BluetoothTransferResult.Fail("发送图片失败", ex);
            }
            finally
            {
                fileStream?.Dispose();
                // ObexWebRequest 没有 Dispose 方法，但可以关闭相关资源
                try { request?.GetResponse()?.Close(); } catch { }
            }
        }

        /// <summary>
        /// 接收图片文件
        /// </summary>
        public async Task<BluetoothTransferResult> ReceiveImageAsync(string saveDirectory, string fileName = null)
        {
            if (_device == null)
                return BluetoothTransferResult.Fail("未选择蓝牙设备");

            if (!Directory.Exists(saveDirectory))
                Directory.CreateDirectory(saveDirectory);

            BluetoothListener listener = null;
            BluetoothClient client = null;

            try
            {
                listener = new BluetoothListener(_serviceClassId);
                listener.Start();

                client = await Task.Run(() => listener.AcceptBluetoothClient());
                var stream = client.GetStream();

                fileName = fileName ?? $"{Guid.NewGuid()}.jpg";
                var fullPath = Path.Combine(saveDirectory, fileName);

                using (var fileStream = File.Create(fullPath))
                {
                    var buffer = new byte[8192];
                    int bytesRead;
                    long totalBytesRead = 0;
                    long? totalBytes = null;

                    try { totalBytes = stream.Length; } catch { }

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        totalBytesRead += bytesRead;

                        // 报告进度
                        int percent = -1;
                        if (totalBytes.HasValue && totalBytes.Value > 0)
                            percent = (int)((totalBytesRead * 100) / totalBytes.Value);

                        OnTransferProgress(percent, totalBytesRead, totalBytes);
                    }
                }

                return BluetoothTransferResult.Success("图片接收成功", fullPath);
            }
            catch (Exception ex)
            {
                return BluetoothTransferResult.Fail("接收图片失败", ex);
            }
            finally
            {
                client?.Close();
                listener?.Stop();
            }
        }

        private void OnTransferProgress(int percentage, long bytesTransferred, long? totalBytes)
        {
            TransferProgress?.Invoke(this, new TransferProgressEventArgs
            {
                Percentage = percentage,
                BytesTransferred = bytesTransferred,
                TotalBytes = totalBytes
            });
        }


        public (bool, string) FindCheckBluetoothMac(string str_BTMac)
        {
            string macAddress = "";
            // 获取所有蓝牙无线电设备
            BluetoothRadio[] radios = BluetoothRadio.AllRadios;

            UIHandleHelper.ShowRunLog($"发现本机 {radios.Length} 个蓝牙设备");

            if (radios.Length != 1 )
            {
                return (false, $"发现本机 {radios.Length} 个蓝牙设备");
            }

            foreach (BluetoothRadio radio in radios)
            {
                UIHandleHelper.ShowRunLog($"设备名称: {radio.Name}");
                UIHandleHelper.ShowRunLog($"蓝牙地址: {radio.LocalAddress}");
                UIHandleHelper.ShowRunLog($"模式: {radio.Mode}");

                // 获取MAC地址字符串形式
                macAddress = radio.LocalAddress.ToString();
                UIHandleHelper.ShowRunLog($"本机蓝牙MAC地址: {macAddress}");

                // 判断是否启用
                bool isEnabled = radio.Mode != RadioMode.PowerOff;
                UIHandleHelper.ShowRunLog($"状态: {(isEnabled ? "已启用" : "已禁用")}");

                if (str_BTMac == macAddress)
                {
                    return (true, macAddress);
                }
            }

            return (false, macAddress);
        }

        public bool FindCheckBluetoothDeviceIDs(string str_BT_IDs)
        {

            UIHandleHelper.ShowRunLog("查询所有蓝牙设备:");

            // 定义多种可能的蓝牙设备标识
            string[] bluetoothPatterns = {
            //"BTH",          // 标准蓝牙设备
            "Bluetooth",    // 名称包含蓝牙
            //-----"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}", // 蓝牙类GUID
            //"MS_BTH",       // 微软蓝牙
            //------"BTHPORT",      // 蓝牙端口
            //"BTHUSB",       // USB蓝牙
            //----"BTHENUM",      // 蓝牙枚举设备
            //"BTHPAN",       // 蓝牙个人区域网络
            //"BTHLE",        // 蓝牙低功耗
            //"BTHMINI"       // 蓝牙迷你端口
             };

            // 构建WQL查询条件
            string condition = "";
            foreach (var pattern in bluetoothPatterns)
            {
                if (!string.IsNullOrEmpty(condition))
                    condition += " OR ";
                condition += $"DeviceID LIKE '%ID%' AND (Name LIKE '%{pattern}%' OR PNPClass LIKE '%{pattern}%')";
            }

            string query = $"SELECT * FROM Win32_PnPEntity WHERE {condition}";
            UIHandleHelper.ShowRunLog($"使用查询: {query}");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            var devices = searcher.Get();

            if (devices.Count == 0)
            {
                UIHandleHelper.ShowRunLog("未找到任何蓝牙设备");
                return false;
            }


            foreach (ManagementObject device in devices)
            {
                string deviceId = device["DeviceID"]?.ToString() ?? "";
                string name = device["Name"]?.ToString() ?? "";
                string pnpClass = device["PNPClass"]?.ToString() ?? "";

                UIHandleHelper.ShowRunLog($"设备名称: {name}");
                UIHandleHelper.ShowRunLog($"设备ID: {deviceId}");
                UIHandleHelper.ShowRunLog($"PNP类: {pnpClass}");

                if (deviceId.Contains(str_BT_IDs))
                {
                    return true;
                }

            }

            return false ;
        }


        public bool PlayMusic(string audioFilePath)
        {
            try
            {
                _waveOut = new WaveOutEvent();
                var audioFile = new AudioFileReader(audioFilePath);
                _waveOut.Init(audioFile);

                Thread.Sleep(1000);

                _waveOut.Play();


                var ret = MessageBoxEX.Show("请确认耳机是否播放音乐", false);

                if (ret == DialogResult.Yes)
                {
                    UIHandleHelper.ShowRunLog("测试人员选择了YES，播放音乐正常");
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog("测试人员选择了NO，播放音乐不正常", true);
                }

                return false;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"播放音乐出错{ex}", true);
                return false;
            }
            finally
            {
                StopAndDisconnect();
            }
          
        }


        private void StopAndDisconnect()
        {
            try
            {
                if (_waveOut != null)
                {
                    _waveOut?.Stop();
                    _waveOut?.Dispose();
                }
                if (_bluetoothClient != null)
                {
                    _bluetoothClient?.Close();
                }

                Console.WriteLine("已断开连接");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"断开连接时发生错误: {ex.Message}");
            }
        }


    }
}
