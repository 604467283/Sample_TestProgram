using Liteon_TestProgram.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class FileProcessHelper
    {
        /// <summary>
        /// 运行的exe文件所在的目录，不包含exe
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentExeDirectory()
        {
            return System.Environment.CurrentDirectory;
        }

        /// <summary>
        /// /运行的exe文件所在的目录，包含exe
        /// </summary>
        /// <returns></returns>
        public static string GetExePathFromDirectory()
        {
            string exeDirectory = GetCurrentExeDirectory();
            string exeFileName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(exeDirectory, exeFileName);
        }


        /// <summary>
        /// 删除指定文件夹中的某些文件； eg: DeleteFile(".\\Bin\\Log\\", "*.txt");
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Filestr"></param>
        public static void DeleteFile(string filePath, string Filestr)
        {
            try
            {
                String[] MyFile = System.IO.Directory.GetFiles(filePath, Filestr, System.IO.SearchOption.TopDirectoryOnly);
                int i = 0;

                while (true)
                {
                    try
                    {
                        if (File.Exists(MyFile[i]))
                        {
                            File.Delete(MyFile[i]);
                        }
                        i++;
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBoxEX.Show(ex.Message, true);
            }
        }


        /// <summary>
        /// 达到最低文件的数量要求就删除全部文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Filestr"></param>
        /// <param name="iReachDelMinNums"></param>
        public static void DeleteFile(string filePath, string Filestr, int iReachDelMinNums)
        {
            try
            {
                String[] MyFile = System.IO.Directory.GetFiles(filePath, Filestr, System.IO.SearchOption.TopDirectoryOnly);
                int i = 0;

                if (MyFile.Length > iReachDelMinNums)
                {
                    while (true)
                    {
                        try
                        {
                            if (File.Exists(MyFile[i]))
                            {
                                File.Delete(MyFile[i]);
                            }
                            i++;
                        }
                        catch
                        {
                            break;
                        }
                    }
                }


            }
            catch (System.Exception ex)
            {
                MessageBoxEX.Show(ex.Message, true);
            }
        }


        //这个函数接收一个文件路径作为参数，并返回文件名（包括扩展名）。
        /// <summary>
        /// 例如，如果 filePath 是 "C:\\example\\myfile.txt"，那么返回值将是 "myfile.txt"。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            return System.IO.Path.GetFileName(filePath);
        }

        /// <summary>
        /// 这个函数同样接收一个文件路径作为参数，但返回的文件名不包括扩展名。
        /// 例如，如果 filePath 是 "C:\\example\\myfile.txt"，那么返回值将是 "myfile"。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string filePath)
        {
            return System.IO.Path.GetFileNameWithoutExtension(filePath);
        }





    }
}
