using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.NetworkServer
{
    internal class MapDriveHelper
    {
        public bool CheckNetworkConnection(string serverPath)
        {
            try
            {
                // 尝试访问共享文件夹的根目录
                DirectoryInfo dirInfo = new DirectoryInfo(serverPath);
                if (dirInfo.Exists)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Error accessing network share: {ex}", true);
            }
            return false;
        }


        public void DeleteExistingConnections_MapDrive()
        {
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c net use * /delete /y";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();

                // 可以选择读取输出或忽略
                string output = process.StandardOutput.ReadToEnd();
            }
        }

        public bool LoginServer_MapDrive(string sNetPath, string sUserName, string sPassword)
        {
            try
            {
                using (var process = new System.Diagnostics.Process())
                {
                    string passwordArg = $"/user:{sUserName} {sPassword}";
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c net use \"{sNetPath}\" {passwordArg} /persistent:yes";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    if (output.Contains("The command completed successfully."))
                    {
                        UIHandleHelper.ShowRunLog($"Login server {sNetPath} OK!");
                        return true;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"Error connecting to server {sNetPath}. Output: {output}", true);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Error: {ex.Message}", true);
                return false;
            }
        }


    }
}
