using FluentFTP;
using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.NetworkServer
{
    internal class FTPHelper
    {
        private string _ftpHost;
        private int _ftpPort;
        private string _ftpUser;
        private string _ftpPass;


        public FTPHelper(string ftpHost, string ftpUser, string ftpPass, int ftpPort)
        {
            _ftpHost = ftpHost;
            _ftpPort = ftpPort;
            _ftpUser = ftpUser;
            _ftpPass = ftpPass;
        }

        // 登录FTP服务器
        public bool Login()
        {
            using (var client = new FtpClient(_ftpHost, _ftpUser, _ftpPass, _ftpPort))
            {
                try
                {
                    client.Connect();
                    return client.IsConnected;
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowRunLog($"FTP Login Failed: {ex}", true);
                    return false;
                }
            }
        }

        // 上传文件到FTP服务器
        public bool UploadFile(string localFilePath, string remoteFilePath)
        {
            using (var client = new FtpClient(_ftpHost, _ftpUser, _ftpPass, _ftpPort))
            {
                try
                {
                    client.Connect();
                    client.UploadFile(localFilePath, remoteFilePath, FtpRemoteExists.Overwrite, false, FtpVerify.None);
                    return true;
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowRunLog($"FTP Upload Failed: {ex}", true);
                    return false;
                }
            }
        }


        // 从FTP服务器下载文件
        public bool DownloadFile(string remoteFilePath, string localFilePath)
        {
            using (var client = new FtpClient(_ftpHost, _ftpUser, _ftpPass, _ftpPort))
            {
                try
                {
                    client.Connect();
                    client.DownloadFile(localFilePath, remoteFilePath, FtpLocalExists.Overwrite, FtpVerify.None);
                    return true;
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowRunLog($"FTP Download Failed: {ex}", true);
                    return false;
                }
            }
        }


        // 连接到FTP服务器并尝试创建文件夹
        public bool CreateFolder(string folderPath)
        {
            try
            {
                // 初始化FTP客户端
                using (var client = new FtpClient(_ftpHost, _ftpUser, _ftpPass, _ftpPort))
                {
                    // 连接到服务器
                    client.Connect();

                    // 检查文件夹是否存在，如果不存在则创建
                    if (!client.DirectoryExists(folderPath))
                    {
                        // 尝试创建文件夹
                        client.CreateDirectory(folderPath);
                        return true; // 文件夹创建成功
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"Folder {folderPath} exists。");
                        return true; // 文件夹已存在，返回false
                    }
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Create Directory Error: {ex}", true);
                return false; // 如果发生错误，返回false
            }
        }




    }
}
