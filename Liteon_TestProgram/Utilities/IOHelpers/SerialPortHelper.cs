using Liteon_TestProgram.Utilities.IOHelpers.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.IOHelpers
{
    internal class SerialPortHelper:IDisposable
    {
        private readonly SerialPort _port;
        private bool _manualControl = false;
        private string _nickName;
        private Stream _serialPortBaseStream;
        private bool disposedValue;

        // 静态计数器
        //private static int _instanceCount = 0;

        // 获取当前实例数量
        //public static int GetInstanceCount() => _instanceCount;

        public SerialPortHelper(SerialPortConfig config)
        {
            _port = new();
            _port.ReadBufferSize = 32768;
            SetConfig(config);

            //Interlocked.Increment(ref _instanceCount);
        }


        /// <summary>
        /// 设置串口配置
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetConfig(SerialPortConfig config)
        {
            if (_port is null || _port.IsOpen)
            {
                throw new InvalidOperationException("port is opened or port is invalid");
            }
            _port.PortName = config.PortName;
            _port.BaudRate = config.BaudRate;
            _port.DataBits = config.DataBits;
            _port.StopBits = config.StopBits;
            _port.Parity = config.Parity;
            _port.RtsEnable = config.RtsEnable;
            _port.DtrEnable = config.DtrEnable;
        }

        public SerialPort BaseSerialPort { get => _port; }
        public bool CtsHolding => _port.CtsHolding;
        public bool DtrEnable { get => _port.DtrEnable; set => _port.DtrEnable = value; }
        public bool IsOpen { get => _port.IsOpen; }
        public string NickName { get => _nickName ?? _port.PortName; set => _nickName = $"{_port.PortName} [{value}] "; }
        public int ReceiveInterval { get; set; } = 200;
        public bool RtsEnable { get => _port.RtsEnable; set => _port.RtsEnable = value; }


        public void Close(bool closing = false)
        {
            //设置立即关闭
            _manualControl = !closing;
            Close();
        }


        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
            Console.WriteLine("SerialPortHelper Dispose.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _port?.Dispose();
                    _serialPortBaseStream?.Dispose();

                    //Interlocked.Decrement(ref _instanceCount);
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

       

        public bool RunCommandLine(string command, string expected, int readTimeout = 2000) => RunCommandLine(command, expected, out _, readTimeout);

        public bool RunCommandLine(string command, Predicate<string> predicate, int readTimeout = 2000) => RunCommandLine(command, predicate, out _, readTimeout);

        public bool RunCommandLine(string commandLine, string excepted, out string recevied, int timeout, uint retryNumbers, int intervel = 2000, string explain = null)
        {
            recevied = string.Empty;
            for (int i = 0; i < retryNumbers; i++)
            {
                if (RunCommandLine(commandLine, excepted, out recevied, timeout))
                {
                    return true;
                }
                Thread.Sleep(intervel);
            }
            return false;
        }

        public bool RunCommandLine(string command, string expected, out string exceptedValue, int readTimeout = 2000)
        {
            if (readTimeout == 0)
            {
                throw new ArgumentException("read timeout can not set to zero");
            }
            try
            {
                Open();
                UIHandleHelper.ShowRunLog($"|TRCE|{ _port.PortName}| Send Command: {command}");
                exceptedValue = string.Empty;

                _port.DiscardInBuffer();
                Thread.Sleep(10);

                _port.Write(command);
                if (expected != string.Empty)
                {
                    exceptedValue = ReadTo(expected, readTimeout);
                }
                return true;
            }
            catch (TimeoutException)
            {
                exceptedValue = string.Empty;
                UIHandleHelper.ShowRunLog($"|FAIL|{_port.PortName}| Error: read timeout", true);
                return false;
            }
            catch (Exception ex)
            {
                exceptedValue = string.Empty;
                UIHandleHelper.ShowRunLog($"|FAIL|{_port.PortName}| Error: {ex.Message}", true);
                //throw new ArgumentException($"|FAIL|{_port.PortName}| Error: {ex.Message}");
                Console.WriteLine($"|FAIL|{_port.PortName}| Error: {ex}");
                return false;
            }
            finally
            {
                Close();
            }

        }







        public void Close()
        {
            //如果为手动则不关闭
            if (_manualControl)
            {
                return;
            }

            try
            {
                _port.Close();
                _serialPortBaseStream?.Dispose();
                _serialPortBaseStream = null;
            }
            catch (Exception)
            {
            }
            finally
            {
                Dispose();  //Daniel 20250530添加
                _manualControl = false;
            }
        }

        public void Open()
        {
            //端口是否开启
            if (_port.IsOpen && _manualControl)
            {
                return;
            }
            else if (_port.IsOpen && !_manualControl)
            {
                throw new InvalidOperationException("port is already opened, and current state is not manual control");
            }

            //释放上次打开端口遗留的BaseStream
            _serialPortBaseStream?.Dispose();
            //开启端口
            _port.Open();
            //保存打开端口时的BaseSteam
            _serialPortBaseStream = _port.BaseStream;
        }


        public string ReadTo(string exceptedValue, int readTimeout = -1)
        {
            StatusCheck();

            char lastValueChar = exceptedValue.Last();
            StringBuilder currentLine = new();
            StringBuilder newLine = new();
            ANSIHandler handler = new();

            int timeNow;
            int timeUsed = 0;
            try
            {
                while (true)
                {
                    char? singleChar = null;
                    if (readTimeout == -1)
                    {
                        if (_port.BytesToRead > 0)
                        {
                            singleChar = (char)_port.ReadChar();
                        }

                    }
                    else if (readTimeout - timeUsed > 0)
                    {
                        timeNow = Environment.TickCount;
                        if (_port.BytesToRead > 0)
                        {
                            singleChar = (char)_port.ReadChar();
                        }
                        timeUsed += Environment.TickCount - timeNow;
                    }
                    else
                    {
                        throw new TimeoutException();
                    }

                    if (singleChar is null)
                    {
                        continue;
                    }

                    var stripChar = handler.StripANSI((char)singleChar);

                    if (stripChar is null)
                    {
                        continue;
                    }


                    currentLine.Append(singleChar);
                    newLine.Append(singleChar);
                    if (singleChar == '\n')
                    {
                        UIHandleHelper.ShowRunLog($"|TRCE|{_port.PortName}| Recv: {newLine.ToString().TrimEnd()}");
                        newLine.Clear();
                    }

                    if (lastValueChar == singleChar && currentLine.Length >= exceptedValue.Length)
                    {
                        bool found = true;

                        for (int i = 2; i <= exceptedValue.Length; i++)
                        {
                            //^i 是C# 8.0引入的索引运算符，表示从字符串的末尾开始计数。例如，^1 表示最后一个字符，^2 表示倒数第二个字符
                            if (exceptedValue[^i] != currentLine[^i])
                            {
                                found = false;
                                break;
                            }
                        }
                        if (found)
                        {
                            return currentLine.ToString();
                        }
                    }


                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (newLine.Length > 0)
                {
                    UIHandleHelper.ShowRunLog($"|TRCE|{_port.PortName}| Recv: {newLine.ToString()}");
                }
            }
        }
        


        private void StatusCheck()
        {
            if (!_port.IsOpen)
            {
                throw new InvalidOperationException(nameof(_port));
            }
        }


        public bool RunCommandLine(string command, Predicate<string> predicate, out string dataReceived, int readTimeout = 2000)
        {
            if (readTimeout == 0)
            {
                throw new ArgumentException("read timeout can not set to zero");
            }
            try
            {
                Open();
                UIHandleHelper.ShowRunLog($"|TRCE|{_port.PortName}| Send Command: {command}");
                if (!string.IsNullOrEmpty(command))
                    _port.Write(command);

                _port.DiscardInBuffer();
                Thread.Sleep(10);

                dataReceived = ReadAll(readTimeout);
                return predicate(dataReceived);
            }
            catch (TimeoutException)
            {
                dataReceived = string.Empty;
                UIHandleHelper.ShowRunLog($"|FAIL|{_port.PortName}| Error: read timeout", true);
                return false;
            }
            catch (Exception ex)
            {
                dataReceived = string.Empty;
                UIHandleHelper.ShowRunLog($"|FAIL|{_port.PortName}| Error: {ex.Message}", true);
                return false;
            }
            finally
            {
                Close();
            }
        }

        public string ReadAll(int readTimeout = -1)
        {
            StatusCheck();

            StringBuilder currentLine = new();
            StringBuilder newLine = new();
            Stopwatch stopwatch = new();
            stopwatch.Start();
            char[] buffer = new char[1024];

            while (true)
            {
                if (_port.BytesToRead > 0)
                {
                    break;
                }
                if (readTimeout != -1 && stopwatch.ElapsedMilliseconds > readTimeout)
                {
                    throw new TimeoutException();
                }
                //减缓cpu压力
                Thread.Sleep(10);
            }
            int originReadTimeout = _port.ReadTimeout;
            try
            {
                _port.ReadTimeout = ReceiveInterval;
                while (true)
                {
                    int count = _port.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < count; i++)
                    {
                        currentLine.Append(buffer[i]);
                        newLine.Append(buffer[i]);
                        if (buffer[i] == '\n')
                        {
                            UIHandleHelper.ShowRunLog($"|TRCE|{_port.PortName}| Recv: {newLine.ToString().TrimEnd()}");
                            newLine.Clear();
                        }
                    }
                }
            }
            catch
            {
                _port.ReadTimeout = originReadTimeout;
                return currentLine.ToString();
            }
            finally
            {
                if (newLine.Length > 0)
                {
                    UIHandleHelper.ShowRunLog($"|TRCE|{_port.PortName}| Recv: {newLine.ToString()}");
                }
            }
        }


        public string ReadTo(Regex regex, int readTimeout)
        {
            StatusCheck();

            StringBuilder currentLine = new();
            StringBuilder newLine = new();
            ANSIHandler handler = new();

            int timeNow;
            int timeUsed = 0;
            try
            {
                while (true)
                {
                    char? singleChar = null;
                    if (readTimeout == -1)
                    {
                        if (_port.BytesToRead > 0)
                        {
                            singleChar = (char)_port.ReadChar();
                        }

                    }
                    else if (readTimeout - timeUsed > 0)
                    {
                        timeNow = Environment.TickCount;
                        if (_port.BytesToRead > 0)
                        {
                            singleChar = (char)_port.ReadChar();
                        }
                        timeUsed += Environment.TickCount - timeNow;
                    }
                    else
                    {
                        throw new TimeoutException();
                    }

                    if (singleChar is null)
                    {
                        continue;
                    }

                    var stripChar = handler.StripANSI((char)singleChar);

                    if (stripChar is null)
                    {
                        continue;
                    }


                    currentLine.Append(singleChar);
                    newLine.Append(singleChar);
                    if (singleChar == '\n')
                    {
                        UIHandleHelper.ShowRunLog($"|TRCE|{_port.PortName}| Recv: {newLine.ToString().TrimEnd()}");
                        newLine.Clear();
                    }

                    string currentLineValue = currentLine.ToString();
                    if (regex.IsMatch(currentLineValue))
                    {
                        return currentLineValue;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (newLine.Length > 0)
                {
                    UIHandleHelper.ShowRunLog($"|TRCE|{_port.PortName}| Recv: {newLine.ToString()}");
                }
            }
        }


        /// <summary>
        /// 从串口读取指定的行数,使用predicate作为读取结束判别
        /// </summary>
        /// <param name="readPredicate">读取完成的predicate</param>
        /// <param name="readTimeout">读取行数超时时间,-1为永不超时</param>
        /// <returns></returns>
        public string[] ReadLines(Predicate<string> readPredicate, int readTimeout)
        {
            StatusCheck();

            var stopwatch = Stopwatch.StartNew();
            var lines = new List<string>();
            var buffer = new StringBuilder();

            try
            {
                while (true)
                {
                    if (readTimeout == -1 || stopwatch.ElapsedMilliseconds < readTimeout)
                    {
                        if (_port.BytesToRead > 0)
                        {
                            buffer.Append((char)_port.ReadChar());
                            if (buffer[^1] == '\n')
                            {
                                string line = buffer.ToString().TrimEnd();
                                lines.Add(line);
                                UIHandleHelper.ShowRunLog($"|TRCE|{_port.PortName}| Recv: {line}");
                                if (readPredicate(line))
                                {
                                    break;
                                }
                                buffer.Clear();
                            }
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
            catch (TimeoutException)
            {
                // Handle timeout if needed
            }

            return lines.ToArray();
        }

        public bool Open(bool manualControl = false)
        {
            _manualControl = manualControl;
            try
            {
                Open();
            }
            catch (Exception e)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|{_port.PortName}| Error: manual control open failed, {e.Message}", true);
            }
            return _port.IsOpen;
        }


    }
}
