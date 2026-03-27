using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Liteon_TestProgram.Utilities.TCPIP_Helper
{

    public class TCPIP_ClientHelper : IDisposable
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private readonly string _host;
        private readonly int _port;
        private readonly Encoding _encoding;
        private bool _disposed;

        /// <summary>
        /// 初始化TCP客户端
        /// </summary>
        /// <param name="host">服务器主机名或IP地址</param>
        /// <param name="port">服务器端口</param>
        /// <param name="encoding">通信编码，默认为UTF-8</param>
        public TCPIP_ClientHelper(string host, int port, Encoding encoding = null)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _port = port;
            _encoding = encoding ?? Encoding.UTF8;
            _tcpClient = new TcpClient();
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="timeoutMilliseconds">连接超时时间(毫秒)</param>
        /// <exception cref="TimeoutException">连接超时</exception>
        /// <exception cref="SocketException">网络错误</exception>
        public async Task ConnectAsync(int timeoutMilliseconds = 5000)
        {
            if (_tcpClient == null)
            {
                _tcpClient = new TcpClient();
            }

            var connectTask = _tcpClient.ConnectAsync(_host, _port);
            var timeoutTask = Task.Delay(timeoutMilliseconds);

            if (await Task.WhenAny(connectTask, timeoutTask) == timeoutTask)
            {
                throw new TimeoutException($"连接服务器 {_host}:{_port} 超时");
            }

            await connectTask; // 确保任何异常都被传播
            _networkStream = _tcpClient.GetStream();
        }

        /// <summary>
        /// 发送数据并接收响应
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="receiveTimeoutMilliseconds">接收超时时间(毫秒)</param>
        /// <returns>服务器响应数据</returns>
        public async Task<string> SendAndReceiveAsync(string data, int receiveTimeoutMilliseconds = 5000)
        {
            if (_tcpClient == null || !_tcpClient.Connected)
            {
                throw new InvalidOperationException("客户端未连接");
            }

            // 发送数据
            var sendData = _encoding.GetBytes(data);
            await _networkStream.WriteAsync(sendData, 0, sendData.Length);

            // 接收响应
            var buffer = new byte[4096];
            var receiveTask = _networkStream.ReadAsync(buffer, 0, buffer.Length);
            var timeoutTask = Task.Delay(receiveTimeoutMilliseconds);

            if (await Task.WhenAny(receiveTask, timeoutTask) == timeoutTask)
            {
                throw new TimeoutException("接收服务器响应超时");
            }

            int bytesRead = await receiveTask;
            return _encoding.GetString(buffer, 0, bytesRead);
        }

        /// <summary>
        /// 发送数据(不等待响应)
        /// </summary>
        /// <param name="data">要发送的数据</param>
        public async Task SendAsync(string data)
        {
            if (_tcpClient == null || !_tcpClient.Connected)
            {
                throw new InvalidOperationException("客户端未连接");
            }

            var sendData = _encoding.GetBytes(data);
            await _networkStream.WriteAsync(sendData, 0, sendData.Length);
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="receiveTimeoutMilliseconds">接收超时时间(毫秒)</param>
        /// <returns>接收到的数据</returns>
        public async Task<string> ReceiveAsync(int receiveTimeoutMilliseconds = 5000)
        {
            if (_tcpClient == null || !_tcpClient.Connected)
            {
                throw new InvalidOperationException("客户端未连接");
            }

            var buffer = new byte[4096];
            var receiveTask = _networkStream.ReadAsync(buffer, 0, buffer.Length);
            var timeoutTask = Task.Delay(receiveTimeoutMilliseconds);

            if (await Task.WhenAny(receiveTask, timeoutTask) == timeoutTask)
            {
                throw new TimeoutException("接收服务器响应超时");
            }

            int bytesRead = await receiveTask;
            return _encoding.GetString(buffer, 0, bytesRead);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            _networkStream?.Close();
            _tcpClient?.Close();
        }

        /// <summary>
        /// 检查连接状态
        /// </summary>
        public bool IsConnected => _tcpClient?.Connected ?? false;

        #region IDisposable Implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _networkStream?.Dispose();
                    _tcpClient?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TCPIP_ClientHelper()
        {
            Dispose(false);
        }
        #endregion
    }    

}
