using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class CustomProcess
    {
        protected readonly string ProgramPath;
        protected readonly Process Instance;

        public string ProgramName { get; }

        public CustomProcess(string programPath, string workingDirectory = null)
        {
          
            ProgramPath = programPath;
            ProgramName = Path.GetFileNameWithoutExtension(ProgramPath);
            Instance = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = programPath,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    //UseShellExecute = false,
                    WorkingDirectory = workingDirectory
                }
            };

            Instance.OutputDataReceived += CustomProcess_OutputDataReceived;
            Instance.ErrorDataReceived += CustomProcess_ErrorDataReceived;
        }

        private readonly StringBuilder OutputDataReceived = new();
        private readonly StringBuilder ErrorDataReceived = new();
        private bool disposedValue;

        private void CustomProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data is null)
            {
                return;
            }
            OutputDataReceived.AppendLine(e.Data);
            UIHandleHelper.ShowRunLog($"|NRM|Process| Received: {e.Data}");
        }

        private void CustomProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data is null)
            {
                return;
            }
            ErrorDataReceived.AppendLine(e.Data);
            UIHandleHelper.ShowRunLog($"|ERR|Process| Received: {e.Data}", true);
        }

      

        public bool RunCommandLine(string command, out string outputData, out string errorData, int timeout)
        {
            try
            {
                UIHandleHelper.ShowRunLog($"|TRCE|Process| Send: arg: + {command.Trim()}");

                OutputDataReceived.Clear();
                ErrorDataReceived.Clear();
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

            return false;
        }

        public bool RunCommandLine(string command, out string outputData, int timeout) => RunCommandLine(command, out outputData, out _, timeout);

        public string RunCommandLine(string command, int timeout)
        {
            RunCommandLine(command, out string outputData, timeout);
            return outputData;
        }

        public string RunCommandLine(string command) => RunCommandLine(command, -1);

        private bool WaitForExit(int timeout)
        {
            bool waitResult;

            if (timeout > 1)
            {
                waitResult = Instance.WaitForExit(timeout);
            }
            else
            {
                Instance.WaitForExit();
                waitResult = true;
            }
            if (!waitResult)
            {
                UIHandleHelper.ShowRunLog($"|FAIL|Process| Error: execute timeout! force kill", true);
                Instance.Kill();
            }
            Instance.WaitForExit();

            Instance.CancelOutputRead();
            Instance.CancelErrorRead();

            return waitResult;
        }

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
                    Instance.OutputDataReceived -= CustomProcess_OutputDataReceived;
                    Instance.ErrorDataReceived -= CustomProcess_ErrorDataReceived;
                    Instance.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~CustomProcess()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }



    }
}
