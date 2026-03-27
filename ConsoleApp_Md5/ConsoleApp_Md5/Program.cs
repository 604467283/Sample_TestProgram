using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp_Md5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========ShowSelectCaseName.ini Md5============");
            Goon:

            string password = PromptForPasswordWithEchoOff("Please Enter Password: ", 20);
         
            if (password == "liteon" + DateTime.Now.ToString("MMddHHmm"))
            {
                try
                {
                    string filePath = $"{System.Environment.CurrentDirectory}\\ShowSelectCaseName.ini";

                    string str_FileContent = ReadFileToStringSkipLines(filePath, "MD5_INFO");
                    Console.WriteLine($"MD5: {ComputeMD5Hash(str_FileContent)}");
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Check ShowSelectCaseName.ini Md5 Error: \r\n {ex}");
                }
              
            }
            else
            {
                Console.WriteLine("wrong password, please login again");
                goto Goon;
            }

            Console.ReadKey();
        }

        public static string ReadFileToStringSkipLines(string filePath, string skipString)
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


        public static string ComputeMD5Hash(string input)
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

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        private const uint ENABLE_PROCESSED_INPUT = 0x0001;
        private const uint ENABLE_LINE_INPUT = 0x0002;
        private const uint ENABLE_ECHO_INPUT = 0x0004;
        private const uint ENABLE_WINDOW_INPUT = 0x0008;
        private const uint ENABLE_MOUSE_INPUT = 0x0010;
        private const uint ENABLE_INSERT_MODE = 0x0020;
        private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        private const uint ENABLE_EXTENDED_FLAGS = 0x0080;
        private const uint ENABLE_PROCESSED_OUTPUT = 0x0100;
        private const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0200;

        public static string PromptForPasswordWithEchoOff(string promptMessage, int maxLength)
        {
            Console.Write(promptMessage);
            StringBuilder password = new StringBuilder(maxLength);
            IntPtr stdin = GetStdHandle(-10); // STD_INPUT_HANDLE
            uint originalMode;
            GetConsoleMode(stdin, out originalMode);
            SetConsoleMode(stdin, originalMode & ~ENABLE_ECHO_INPUT);

            try
            {
                ConsoleKeyInfo key;
                while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        Console.Write("\b \b");
                        password.Length--;
                    }
                    else if (key.Key != ConsoleKey.Escape && password.Length < maxLength)
                    {
                        password.Append(key.KeyChar);
                        Console.Write("*");
                    }
                }

                Console.WriteLine();
            }
            finally
            {
                SetConsoleMode(stdin, originalMode);
            }

            return password.ToString();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);









    }
}
