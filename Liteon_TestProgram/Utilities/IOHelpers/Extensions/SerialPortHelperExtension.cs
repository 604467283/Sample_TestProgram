using NationalInstruments.Restricted;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.IOHelpers.Extensions
{
    internal static class SerialPortHelperExtension
    { /// <summary>
      /// 查看当前串口是否存在于系统中
      /// </summary>
      /// <param name="port"></param>
      /// <returns></returns>
        public static bool Exist(this SerialPortHelper port) => SerialPort.GetPortNames().Any(x => x == port.BaseSerialPort.PortName);

        /// <summary>
        /// 读取到指定的字符串,返回指示是否超时完成
        /// </summary>
        /// <param name="port"></param>
        /// <param name="excepted"></param>
        /// <param name="readTimeout"></param>
        /// <returns></returns>
        public static bool ReadToExcepted(this SerialPortHelper port, string excepted, int readTimeout)
        {
            try
            {
                port.Open();
                _ = port.ReadTo(excepted, readTimeout);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
            finally
            {
                port.Close();
            }
        }

        public static bool ReadToExcepted(this SerialPortHelper portHelper, Regex regex, out Match match, int timeout)
        {
            try
            {
                portHelper.Open();
                string received = portHelper.ReadTo(regex, timeout);
                match = regex.Match(received);
                return true;
            }
            catch (TimeoutException)
            {
                match = Match.Empty;
                return false;
            }
            finally
            {
                portHelper.Close();
            }
        }

        public static bool RunCommandLine(this SerialPortHelper port, string command, string excepted, int readtimeout, int retries)
        {
            for (int i = 0; i < retries; i++)
            {
                if (port.RunCommandLine(command, excepted, readtimeout))
                {
                    return true;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            return false;
        }


        public static bool RunCommandLine(this SerialPortHelper port, string command, Predicate<string> predicate, out string dataReceived, int readTimeout, uint retries, int interval = 200)
        {
            for (int i = 0; i < retries; i++)
            {
                if (port.RunCommandLine(command, predicate, out dataReceived, readTimeout))
                {
                    return true;
                }
                else
                {
                    Thread.Sleep(interval);
                }
            }
            dataReceived = string.Empty;
            return false;
        }

  

        public static bool RunCommandLine(this SerialPortHelper port, byte[] bytes, string excepted, out string dataReceived, int readTimeout)
        {
            try
            {
                string str_temp = "";
                port.Open();
                UIHandleHelper.ShowRunLog($"|TRCE|{port.BaseSerialPort.PortName}| Send Command: {BitConverter.ToString(bytes)}");
                port.BaseSerialPort.Write(bytes, 0, bytes.Length);
                if (excepted.IsEmpty() == false)
                {
                    str_temp = port.ReadTo(excepted, readTimeout);
                }

                dataReceived = str_temp;
                return true;
            }
            catch (TimeoutException)
            {
                dataReceived = string.Empty;
                return false;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|{port.BaseSerialPort.PortName}| Error: {ex.Message}", true);
                dataReceived = string.Empty;
                return false;
            }
            finally
            {
                port.Close();
            }
        }

        public static bool RunCommandLine(this SerialPortHelper port, byte[] bytes, string excepted, out string dataRecevied, int readTimeout, int retires, int interval = 200)
        {
            for (int n = 0; n < retires; n++)
            {
                if (port.RunCommandLine(bytes, excepted, out dataRecevied, readTimeout))
                {
                    return true;
                }
                else
                {
                    Thread.Sleep(interval);
                }
            }
            dataRecevied = string.Empty;
            return false;
        }

        public static bool RunCommandLineByDelegate(this SerialPortHelper port, string command, Predicate<string> prediccate, out string dataRecevied, int readTimeout)
        {
            dataRecevied = string.Empty;
            try
            {
                port.Open();
                UIHandleHelper.ShowRunLog($"|TRCE|{port.BaseSerialPort.PortName}| Send Command: {command}");
                StringBuilder buffer = new();
                //write
                port.BaseSerialPort.Write(command);
                var watcher = Stopwatch.StartNew();
                while (watcher.ElapsedMilliseconds < readTimeout)
                {
                    string content = port.BaseSerialPort.ReadExisting();
                    if (string.IsNullOrEmpty(content))
                    {
                        continue;
                    }
                    UIHandleHelper.ShowRunLog($"|TRCE|{port.BaseSerialPort.PortName}| Recv: {content}");
                    buffer.Append(content);
                    if (prediccate(buffer.ToString()))
                    {
                        dataRecevied = buffer.ToString();
                        return true;
                    }
                }
                return false;
            }
            catch (TimeoutException)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|{port.BaseSerialPort.PortName}| Error: read timeout", true);
                return false;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|{port.BaseSerialPort.PortName}| Error: {ex.Message}", true);
                return false;
            }
            finally
            {
                port.Close();
            }
        }

        /// <summary>
        /// 发送指令并读取到指定的行数
        /// </summary>
        /// <param name="port"></param>
        /// <param name="command"></param>
        /// <param name="readPredicate"></param>
        /// <param name="readTimeout"></param>
        /// <returns></returns>
        public static string[] SendReadLines(this SerialPortHelper port, string command, Predicate<string> readPredicate, int readTimeout)
        {
            try
            {
                port.Open();
                port.BaseSerialPort.Write(command);
                return port.ReadLines(readPredicate, readTimeout);
            }
            finally
            {
                port.Close();
            }
        }

        public static void WaitForArrival(string portName, int waitTimeout = -1)
        {
            //check port exists first
            if (SerialPort.GetPortNames().Contains(portName))
            {
                return;
            }
            bool portArrival = false;



            int timeUsed = 0;
            int timeNow = 0;

            while (true)
            {
                if (waitTimeout == -1)
                {
                    if (portArrival)
                    {
                        break;
                    }
                }
                else if (waitTimeout - timeUsed > 0)
                {
                    timeNow = Environment.TickCount;
                    if (portArrival)
                    {
                        break;
                    }
                    timeUsed += Environment.TickCount - timeNow;
                }
                else
                {
                    throw new TimeoutException();
                }
            }
        }

        public static bool WaitForArrival(this SerialPortHelper portHelper, int waitTimeout = 2000)
        {
            try
            {
                WaitForArrival(portHelper.BaseSerialPort.PortName, waitTimeout);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        public static void WriteBytes(this SerialPortHelper port, byte[] data)
        {
            try
            {
                port.Open();
                UIHandleHelper.ShowRunLog($"|TRCE|{port.BaseSerialPort.PortName}| Send Command: {BitConverter.ToString(data)}");
                port.BaseSerialPort.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|{port.BaseSerialPort.PortName}| Error: {ex.Message}", true);
            }
            finally
            {
                port.Close();
            }
        }

        public static byte[] ReadBytes(this SerialPortHelper port)
        {
          
            byte[] result = new byte[port.BaseSerialPort.BytesToRead];
            try
            {
                port.Open();
                port.BaseSerialPort.Read(result, 0, result.Length);
                UIHandleHelper.ShowRunLog($"|TRCE|{port.BaseSerialPort.PortName}| Recv: {BitConverter.ToString(result)}");
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|{port.BaseSerialPort.PortName}| Error: {ex.Message}", true);
            }
            finally
            {
                port.Close();
            }
            return result;
        }




    }
}
