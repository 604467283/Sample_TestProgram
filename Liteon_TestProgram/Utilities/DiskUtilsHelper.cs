using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class DiskUtilsHelper
    {
        // 判断文件是否位于系统盘
        public static bool IsFileOnSystemDrive(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.");
            }

            // 获取系统盘符
            string systemDrive = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string systemDriveLetter = Path.GetPathRoot(systemDrive).Substring(0, 1);

            // 获取文件所在的盘符
            string fileDriveLetter = Path.GetPathRoot(filePath).Substring(0, 1);

            return fileDriveLetter.Equals(systemDriveLetter, StringComparison.OrdinalIgnoreCase);
        }


        // 获取指定盘符的剩余空间（以G为单位）
        public static long GetDriveFreeSpace(string driveLetter)
        {
            if (string.IsNullOrEmpty(driveLetter) || !driveLetter.EndsWith(":"))
            {
                throw new ArgumentException("必须指定驱动器号并以冒号结尾.");
            }

            DriveInfo driveInfo = new DriveInfo(driveLetter);
            if (!driveInfo.IsReady)
            {
                throw new InvalidOperationException("驱动器未准备就绪.");
            }

            return driveInfo.AvailableFreeSpace / (1024 * 1024*1024);
        }

        public static long CheckDrive(string str_ExePath)
        {
            try
            {
                //if (IsFileOnSystemDrive(str_ExePath))
                //{
                //    Console.WriteLine("该log文件路径为C盘，请设定至其他盘");
                //    UIHandleHelper.ShowRunLog("该log文件路径为C盘，请设定至其他盘", true);
                //}
                //else
                {

                    try
                    {
                        string systemDriveLetter = str_ExePath.Substring(0, 1);
                        long freeSpace = GetDriveFreeSpace(systemDriveLetter + ":");
                        Console.WriteLine($"The log drive ({systemDriveLetter}:) has {freeSpace} GB of free space.");
                        UIHandleHelper.ShowRunLog($"The log drive ({systemDriveLetter}:) has {freeSpace} GB of free space.");
                        return freeSpace;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"发生错误: {ex.Message}");
                        UIHandleHelper.ShowRunLog($"获取当前盘剩余空间发生错误: {ex.Message}", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"判断是否当前是否是系统盘发生错误: {ex.Message}");
                UIHandleHelper.ShowRunLog($"判断是否当前是否是系统盘发生错误: {ex.Message}", true);
            }

            return -1;
        }



    }
}
