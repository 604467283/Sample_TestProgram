using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class SshHelper
    {
        public readonly SshClient Client;

        public SshHelper(string host, int iPort, string userName, string password)
        {
            Client = new SshClient(host, iPort, userName, password);
        }

        public SshHelper(ConnectionInfo connection)
        {
            Client = new SshClient(connection);
        }

        public void Dispose()
        {
            ((IDisposable)Client).Dispose();
        }

        public bool IsConnected => Client.IsConnected;

        public void Connect()
        {
            UIHandleHelper.ShowRunLog($"try connecting to host: {Client.ConnectionInfo.Host}");
            try
            {
                Client.Connect();
                UIHandleHelper.ShowRunLog("connected!");
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|SSH| Error: fail to connect host: {ex.Message}", true);
            }
        }

        public void Disconnect()
        {
            try
            {
                UIHandleHelper.ShowRunLog("|TRCE|SSH| try disconnect");
                Client.Disconnect();
                UIHandleHelper.ShowRunLog("|TRCE|SSH| diconnected!");
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|SSH| Error: disconnect fail!: {ex.Message}", true);
            }
        }

        /// <summary>
        /// 运行指令,不使用终端仿真
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public string RunCommandLine(string commandLine, int timeout = -1)
        {
            UIHandleHelper.ShowRunLog($"|INFO|SSH| Send: {commandLine}");

            CancellationTokenSource tokenSource = new();
            StringBuilder sb = new();
            try
            {
                SshCommand cmd = Client.CreateCommand(commandLine.TrimEnd());
                if (timeout > 0)
                {
                    tokenSource.CancelAfter(timeout);
                }
                IAsyncResult begin = cmd.BeginExecute();
                using StreamReader reader = new(cmd.OutputStream, Encoding.UTF8, true, 1024, true);
                while (!tokenSource.IsCancellationRequested)
                {
                    if (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line is not null)
                        {
                            sb.AppendLine(line);
                            UIHandleHelper.ShowRunLog($"|TRCE|SSH| Recv: {StringTool.RemoveColorEscapeSequences(line.TrimEnd())}");
                        }
                        continue;
                    }

                    if (begin.IsCompleted)
                    {
                        tokenSource.Cancel();
                    }
                }
                if (!begin.IsCompleted)
                {
                    cmd.CancelAsync();
                }
                UIHandleHelper.ShowRunLog($"|FAIL|SSH| Error: {cmd.Error}");
                cmd.EndExecute(begin);
                return StringTool.RemoveColorEscapeSequences(sb.ToString());
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|SSH| Error: exception: {ex.Message}", true);
                return string.Empty;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<string> RunCommandLineAsync(string commandLine, int timeout = -1)
        {
            UIHandleHelper.ShowRunLog($"|INFO|SSH| Send: {commandLine}");

            CancellationTokenSource tokenSource = new();
            StringBuilder sb = new();
            try
            {
                SshCommand cmd = Client.CreateCommand(commandLine);
                if (timeout > 0)
                {
                    tokenSource.CancelAfter(timeout);
                }
                IAsyncResult begin = cmd.BeginExecute();
                using StreamReader reader = new(cmd.OutputStream, Encoding.UTF8, true, 1024, true);
                while (!tokenSource.IsCancellationRequested)
                {
                    if (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        if (line is not null)
                        {
                            sb.AppendLine(line);
                            UIHandleHelper.ShowRunLog($"|TRCE|SSH| Recv: {line}");
                        }
                        continue;
                    }

                    if (begin.IsCompleted)
                    {
                        tokenSource.Cancel();
                    }
                }
                if (!begin.IsCompleted)
                {
                    cmd.CancelAsync();
                }
                UIHandleHelper.ShowRunLog($"|FAIL|SSH| Error: {cmd.Error}");
                cmd.EndExecute(begin);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|SSH| Error: exception: {ex.Message}", true);
                return string.Empty;
            }
        }

        public async Task<string> RunCommandLineAsync(string commandLine, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            CancellationTokenSource tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            StringBuilder sb = new();
            try
            {
                if (!Client.IsConnected)
                    Client.Connect();
                SshCommand cmd = Client.CreateCommand(commandLine);

                IAsyncResult begin = cmd.BeginExecute();
                using StreamReader reader = new(cmd.OutputStream, Encoding.UTF8, true, 1024, true);
                while (!tokenSource.IsCancellationRequested)
                {
                    if (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        if (line is not null)
                        {
                            sb.AppendLine(line);
                            UIHandleHelper.ShowRunLog($"|TRCE|SSH| Recv: {line}");
                        }
                        Thread.Sleep(100);
                        continue;
                    }

                    if (begin.IsCompleted)
                    {
                        tokenSource.Cancel();
                    }
                }
                if (!begin.IsCompleted)
                {
                    cmd.CancelAsync();
                }
                UIHandleHelper.ShowRunLog($"|FAIL|SSH| Error: {cmd.Error}");
                cmd.EndExecute(begin);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|SSH| Error: exception: {ex.Message}", true);
                return string.Empty;
            }
        }
        public async Task<string> GnssTest064(List<string> cmds, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            CancellationTokenSource tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            StringBuilder sb = new();
            try
            {
                if (!Client.IsConnected)
                    Client.Connect();
                if (Client.IsConnected)
                {
                    using (ShellStream shell = Client.CreateShellStream("", 0, 0, 0, 0, 0))
                    {
                        string data = string.Empty;
                        shell.DataReceived += (a, b) =>
                        {
                            sb.Append(System.Text.Encoding.Default.GetString(b.Data));
                        };
                        foreach (var cmd in cmds)
                        {
                            shell.WriteLine(cmd);
                            Thread.Sleep(100);
                        }

                        while (!tokenSource.IsCancellationRequested)
                        {
                            Thread.Sleep(1000);
                            if (sb.ToString().Contains("Update Location Info"))
                            {
                                Thread.Sleep(3000);
                                break;
                            }
                        }
                        shell.WriteLine("\x03");
                        shell.Close();
                        return sb.ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|SSH| Error: [SSH Fail]: {ex.Message}", true);
            }
            return string.Empty;
        }

        public bool RunCommandLine(string commandLine, string exceptedValue, out string result, int timeout = 3000, string explain = null)
        {
            UIHandleHelper.ShowRunLog($"|INFO|SSH| Send: {commandLine}, excepted value: {exceptedValue}");
            if (explain is not null)
            {
                UIHandleHelper.ShowRunLog($"|TRCE|SSH| explain: {explain}");
            }

            string ret = RunCommandLine(commandLine, timeout);
            result = ret;
            if (ret.Contains(exceptedValue))
            {
                UIHandleHelper.ShowRunLog($"|TRCE|SSH| wait excepted value success");
                return true;
            }

            UIHandleHelper.ShowRunLog($"|TRCE|SSH| wait excepted value failed!retry!");
            return false;
        }

        public bool RunCommandLine(string commandLine, string exceptedValue, out string result, int timeout = 3000, uint retryNumber = 3, int interval = 2000, string explain = null)
        {
            result = string.Empty;
            for (int i = 0; i < retryNumber; i++)
            {
                UIHandleHelper.ShowRunLog($"|TRCE|SSH| Send: {commandLine}, excepted value: {exceptedValue}");
                if (explain is not null)
                {
                    UIHandleHelper.ShowRunLog($"|TRCE|SSH| explain: {explain}");
                }

                string ret = RunCommandLine(commandLine, timeout);
                result = ret;
                if (ret.Contains(exceptedValue))
                {
                    UIHandleHelper.ShowRunLog($"|TRCE|SSH| wait excepted value success");
                    return true;
                }
                UIHandleHelper.ShowRunLog($"|TRCE|SSH| wait excepted value failed!retry!");
                Thread.Sleep(interval);
            }
            return false;
        }



    }
}
