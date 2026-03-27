using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.IOHelpers
{
    internal class ExeProcessOnceHelper : IDisposable
    {
        protected readonly Process Instance;
        protected readonly string ProgramPath;
        private readonly StringBuilder ErrorDataReceived = new StringBuilder();
        private readonly StringBuilder OutputDataReceived = new StringBuilder();
        private bool disposedValue;
        public ExeProcessOnceHelper(string programPath, string workingDirectory)
        {

            ProgramPath = programPath;
            ProgramName = Path.GetFileNameWithoutExtension(ProgramPath);
            Instance = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = programPath,
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                }
            };  
            Instance.OutputDataReceived += CustomProcess_OutputDataReceived;
            Instance.ErrorDataReceived += CustomProcess_ErrorDataReceived;
        }

        public int ExitCode => Instance.ExitCode;
        public string ProgramName { get; }
        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool RunCommandLine(string command, out string outputData, out string errorData, int timeout)
        {
            try
            {
                Instance.StartInfo.Arguments = command;
                Instance.Start();
                Instance.BeginOutputReadLine();
                Instance.BeginErrorReadLine();
                bool waitResult = WaitForExit(timeout);
                outputData = OutputDataReceived.ToString();
                errorData = ErrorDataReceived.ToString();
                return waitResult;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Run cmd error: \r\n{ex}", true);
            }

            outputData = OutputDataReceived.ToString();
            errorData = ErrorDataReceived.ToString();
            return true;

        }

        public bool RunCommandLine(string command, out string outputData, int timeout) => RunCommandLine(command, out outputData, out _, timeout);

        public string RunCommandLine(string command, int timeout)
        {
            RunCommandLine(command, out string outputData, timeout);
            return outputData;
        }

        public string RunCommandLine(string command) => RunCommandLine(command, -1);

        public bool RunCommandLine(string cmd, string excepted, out string result, int timeout = 5000, int retires = 3)
        {
            result = string.Empty;
            for (int i = 0; i < retires; i++)
            {
                result = RunCommandLine(cmd, timeout);
                if (result.Contains(excepted))
                {
                    return true;
                }
                Thread.Sleep(1000);
            }
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Instance.ErrorDataReceived -= CustomProcess_ErrorDataReceived;
                    Instance.ErrorDataReceived -= CustomProcess_OutputDataReceived;
                    Instance.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        private void CustomProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data is null)
            {
                return;
            }
            Console.WriteLine($"process error message: {e.Data}");
            ErrorDataReceived.AppendLine(e.Data);
        }

        private void CustomProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data is null)
            {
                return;
            }
            Console.WriteLine($"process output message: {e.Data}");
            OutputDataReceived.AppendLine(e.Data);
        }

        private bool WaitForExit(int timeout)
        {
            bool waitResult;

            if (timeout > 0)
            {
                waitResult = Instance.WaitForExit(timeout);
                if (!waitResult)
                {
                    Console.WriteLine($"execute timeout! force kill");
                    Instance.Kill();
                    Instance.WaitForExit(); // 等待被杀死的进程真正退出
                }
            }
            else
            {
                Instance.WaitForExit();
                waitResult = true;
            }

            Instance.CancelOutputRead();
            Instance.CancelErrorRead();

            return waitResult;
        }
        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~CustomProcess()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }
    }
}
