using Liteon_TestProgram.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class MD5Helper
    {

        /// <summary>
        /// 计算文件的MD5哈希值
        /// </summary>
        /// <param name="filePath">文件的完整路径</param>
        /// <returns>文件的MD5哈希值，以十六进制字符串形式表示</returns>
        public string ComputeFileMD5Hash(string filePath)
        {
            try
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] hashBytes = md5.ComputeHash(stream);

                        // 将字节数组转换为十六进制字符串
                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in hashBytes)
                        {
                            sb.Append(b.ToString("x2"));
                        }

                        Console.WriteLine($"{filePath} Md5: {sb.ToString()}");
                        return sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // 处理可能的异常，例如文件不存在或读取错误
                Console.WriteLine($"An error occurred while calculating MD5: {ex.Message}");
                UIHandleHelper.ShowRunLog($"An error occurred while calculating MD5: {ex.Message}", true);
                return null;
            }
        }


        public string ComputeMD5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public string ReadFileToStringSkipLines(string filePath, string skipString)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                // 使用StreamReader逐行读取文件内容
                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 如果行中不包含要跳过的字符串，则添加到StringBuilder中
                        if (!line.Contains(skipString))
                        {
                            sb.AppendLine(line);
                        }
                    }
                }

                // 返回处理后的字符串
                return sb.ToString() + "===LITEON===";
            }
            catch (IOException ex)
            {
                // 处理文件读取时可能发生的异常
                Console.WriteLine($"Error reading file: {ex.Message}");
                throw new ArgumentException($"Error reading file: {ex.Message}");
            }
        }

        public bool CheckEncryptMd5(string filePath, string skipString, string str_ReadIniMd5)
        {
            try
            {
                string str_FileContent = ReadFileToStringSkipLines(filePath, skipString);
                string str_NewMd5 = ComputeMD5Hash(str_FileContent);
                if (str_NewMd5 == str_ReadIniMd5)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"The encrypted file is actually Md5: {str_NewMd5}", true);
                    MessageBoxEX.Show("加密文件实际Md5和加密文件里的MD5不一致", true);
                }
            }
            catch (Exception ex)
            {
                MessageBoxEX.Show($"Error Check Encrypt Md5: {ex.Message}", true);
                return false;
            }
            
            return false;
        }



        public string GetStringMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }




        //=============整文件MD5计算====================


        public static string CalculateMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static bool VerifyMD5(string filePath, string expectedMD5)
        {
            if (!File.Exists(filePath))
            {
                MessageBoxEX.Show($"文件不存在-{filePath}", true);
                return false;
            }

            string actualMD5 = CalculateMD5(filePath);
            return string.Equals(actualMD5, expectedMD5, StringComparison.OrdinalIgnoreCase);
        }


    }
}
