using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class IniHelper
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        //参数说明：section：INI文件中的段落；key：INI文件中的关键字；val：INI文件中关键字的数值；filePath：INI文件的完整的路径和名称。
        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        //参数说明：section：INI文件中的段落名称；key：INI文件中的关键字；def：无法读取时候时候的缺省数值；retVal：读取数值；size：数值的大小；filePath：INI文件的完整路径和名称。


        public static long GetIniStr(string section, string key, string def, StringBuilder retVal, int size, string filePath)
        {
            return GetPrivateProfileString(section, key, def, retVal, size, filePath);
        }
        //寫.ini文件
        public static long WriteIniStr(string section, string key, string val, string filepath)
        {
            return WritePrivateProfileString(section, key, val, filepath);
        }

        public static string GetIniStr(string filePath, string section, string key, Encoding encoding)
        {
            string returnValue = string.Empty;
            try
            {

                string[] allLines = File.ReadAllLines(filePath, encoding);

                foreach (string line in allLines)
                {
                    // 检查行是否以'['开始并以']'结束，且内容是否与指定节匹配
                    if (line.StartsWith("[") && line.EndsWith("]") && line.Substring(1, line.Length - 2).Equals(section, StringComparison.OrdinalIgnoreCase))
                    {
                        // 从下一行开始查找键值对
                        for (int i = Array.IndexOf(allLines, line) + 1; i < allLines.Length; i++)
                        {
                            string subLine = allLines[i];
                            // 忽略空行和以'['开始的行（表示新节的开始）
                            if (string.IsNullOrWhiteSpace(subLine) || subLine.StartsWith("["))
                            {
                                break;
                            }

                            // 查找与指定键匹配的键值对
                            if (subLine.Replace(" ", "").Contains(key + "="))
                            {
                                // 提取并返回键值对中的值部分
                                returnValue = subLine.Substring(subLine.IndexOf("=") + 1).Trim();
                                break;
                            }
                        }

                        // 找到匹配项后退出循环
                        if (returnValue != null)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine(ex.Message);
                return returnValue;
            }
            return returnValue;
        }




    }
}
