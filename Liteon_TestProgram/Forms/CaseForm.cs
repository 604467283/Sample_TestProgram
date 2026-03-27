using Liteon_TestProgram.Base;
using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Liteon_TestProgram.Forms
{
    public partial class CaseForm : Form
    {
        [DllImport("user32.dll")]  //需添加using System.Runtime.InteropServices
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;


        private MainForm _mainForm;

        public static string str_CaseName { get; private set; }

        public CaseForm()
        {
            InitializeComponent();
            this.MouseDown += Form_Base_MouseDown;
        }

        private void Form_Base_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
        }

        /*
        private void LoadData()
        {
            comboBox_Case.Items.Clear();
            string str_Md5File = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\ShowSelectCaseName\\ShowSelectCaseName.ini";
            StringBuilder ValTemp = new StringBuilder();
            IniHelper.GetIniStr("Model", "ProjectName", "null", ValTemp, 2000, str_Md5File);
            string str_CaseNames = ValTemp.ToString()
            string[] arrayCaseName = str_CaseNames.Split(';');


            IniHelper.GetIniStr("MD5_INFO", "MD5_INFO", "null", ValTemp, 100, str_Md5File);
            string str_MD5_INFO = ValTemp.ToString();

            MD5Helper mD5Helper = new MD5Helper();
            if (!mD5Helper.CheckEncryptMd5(str_Md5File, "MD5_INFO", str_MD5_INFO))
            {
                MessageBoxEX.Show("检查机种ShowSelectCaseName.ini加密文件的MD5失败", true);
                Environment.Exit(0);
            }

            bool bLoadcheckBox_LogCollection = checkBox_LogCollection.Checked;

            var oclist = from t in Assembly.GetExecutingAssembly().GetTypes()
                         where t.Namespace == ("Liteon_TestProgram.CaseProject") && t.IsClass && typeof(CaseCodeBase).IsAssignableFrom(t)
                         select t;

            oclist.ToList().ForEach(o =>
            {
                if (o.Name.StartsWith("WCBN")
                || o.Name.StartsWith("WN")
                || o.Name.StartsWith("WB")
                || o.Name.StartsWith("Throughput")
                || o.Name.StartsWith("SP2D"))
                {
                    if (arrayCaseName.Any(name => name.ToLower().Contains("allcase")))
                    {
                        comboBox_Case.Items.Add(o.Name);
                    }
                    else
                    {
                        // element.Replace("(", "_").Replace(")", "").Replace("__", "_")
                        // 将ShowSelectCaseName.ini文件中的Casename去除括号
                        // 使用_替代，如果遇到连续两个_，则保留一个；
                        if (Array.Exists(arrayCaseName, element => element.Replace("(", "_").Replace(")", "").Replace("__", "_").Equals(o.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            comboBox_Case.Items.Add(o.Name);
                        }

                    }
                }

                if (o.Name.StartsWith("ZLogDataCollection") && bLoadcheckBox_LogCollection)
                {
                    comboBox_Case.Items.Add(o.Name);
                }

            });


            try
            {
                // 获取指定目录下的子目录列表
                string[] subdirectories = Directory.GetDirectories(".\\TestDll");

                // 输出子目录的数量
                Console.WriteLine("Number of subdirectories: " + subdirectories.Length);

                foreach (string subdirectory in subdirectories)
                {
                    Console.WriteLine("folder: " + subdirectory);

                    if (subdirectory.Substring(subdirectory.Length - 4).ToUpper() != "_OLD")  //文件夹以_old结尾的，不复制
                    {
                        if (!CopyFilesFromDirectory(subdirectory, ".\\"))
                        {
                            CopyFilesFromDirectory(subdirectory, ".\\");
                            MessageBoxEX.Show("复制测试Dll fail", true);
                            Environment.Exit(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEX.Show("Copy Dll: an error occurred: " + ex.Message, true);
                Environment.Exit(0);
            }




        }

        private void CaseForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        */




        /*
         优化要点说明：

            ​​异步加载​​：使用Task.Run将耗时操作放到后台线程，避免UI卡顿。

            ​​并行处理​​：使用Parallel.Invoke并行执行三个主要任务：加载案例名称、验证MD5、复制DLL。

            ​​批量UI更新​​：使用BeginUpdate/EndUpdate减少UI刷新次数。

            ​​优化反射操作​​：使用AsParallel()并行处理类型查找。

            ​​路径处理​​：使用Path.Combine代替字符串拼接，提高可读性和跨平台兼容性。

            ​​减少重复操作​​：将INI文件读取集中处理，避免多次读取。

            ​​错误处理优化​​：提供更详细的错误信息，特别是复制DLL失败时指出具体目录。

            ​​内存优化​​：预分配StringBuilder容量，减少内存分配。

            ​​字符串比较优化​​：使用StringComparison.OrdinalIgnoreCase代替ToLower()，性能更好。

            ​​提前过滤​​：在获取子目录时就过滤掉_OLD目录，减少不必要的操作。

         */
        private async void CaseForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadData());
            // 加载完成后可以在这里更新UI状态
        }


        private void LoadData()
        {
            // 使用Invoke确保UI操作在主线程执行
            this.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                comboBox_Case.Items.Clear();
            });

            string str_Md5File = Path.Combine(FileProcessHelper.GetCurrentExeDirectory(), "Model_Config", "ShowSelectCaseName", "ShowSelectCaseName.ini");

            // 优化INI文件读取
            var iniValues = new Dictionary<string, string>();
            ReadIniValues(str_Md5File, iniValues);

            if (iniValues["MD5_Switch"] != "0")
            {
                // 并行处理案例加载
                Parallel.Invoke(
                    () => LoadCaseNames(iniValues),
                    () => VerifyMd5(str_Md5File, iniValues),
                    () => CopyTestDlls()
                );
            }
            else
            {
                // 并行处理案例加载
                Parallel.Invoke(
                    () => LoadCaseNames(iniValues),
                    () => CopyTestDlls()
                );
            }

         
        }

        private void ReadIniValues(string iniFile, Dictionary<string, string> iniValues)
        {
            StringBuilder ValTemp = new StringBuilder(2000);
            IniHelper.GetIniStr("Model", "ProjectName", "null", ValTemp, ValTemp.Capacity, iniFile);
            iniValues["ProjectName"] = ValTemp.ToString();

            ValTemp.Clear();
            IniHelper.GetIniStr("MD5_INFO", "MD5_INFO", "null", ValTemp, 100, iniFile);
            iniValues["MD5_INFO"] = ValTemp.ToString();

            ValTemp.Clear();
            IniHelper.GetIniStr("MD5_INFO", "Switch", "null", ValTemp, 100, iniFile);
            iniValues["MD5_Switch"] = ValTemp.ToString();
        }

        private void VerifyMd5(string iniFile, Dictionary<string, string> iniValues)
        {
            MD5Helper mD5Helper = new MD5Helper();
            if (!mD5Helper.CheckEncryptMd5(iniFile, "MD5_INFO", iniValues["MD5_INFO"]))
            {
                this.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                    MessageBoxEX.Show("检查机种ShowSelectCaseName.ini加密文件的MD5失败", true);
                    Environment.Exit(0);
                });
            }
        }

        private void LoadCaseNames(Dictionary<string, string> iniValues)
        {
            bool bLoadcheckBox_LogCollection = false;
            this.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                bLoadcheckBox_LogCollection = checkBox_LogCollection.Checked;
            });

            string[] arrayCaseName = iniValues["ProjectName"].Split(';');
            var caseNames = new List<string>();

            // 使用并行处理反射查找
            var caseTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace == "Liteon_TestProgram.CaseProject" &&
                           t.IsClass &&
                           typeof(CaseCodeBase).IsAssignableFrom(t))
                .AsParallel();

            foreach (var type in caseTypes)
            {
                if (type.Name.StartsWith("WCBN") ||
                    type.Name.StartsWith("WN") ||
                    type.Name.StartsWith("WB") ||
                    type.Name.StartsWith("Throughput") ||
                    type.Name.StartsWith("OQC") ||
                    type.Name.StartsWith("SP2E") ||
                    type.Name.StartsWith("SP2D"))
                {
                    if (arrayCaseName.Any(name => name.Equals("allcase", StringComparison.OrdinalIgnoreCase)))
                    {
                        caseNames.Add(type.Name);
                    }
                    else
                    {
                        string normalizedTypeName = type.Name;
                        if (Array.Exists(arrayCaseName, element =>
                            element.Replace("(", "_")
                                  .Replace(")", "")
                                  .Replace("__", "_")
                                  .Equals(normalizedTypeName, StringComparison.OrdinalIgnoreCase)))
                        {
                            caseNames.Add(type.Name);
                        }
                    }
                }

                if (type.Name.StartsWith("ZLogDataCollection") && bLoadcheckBox_LogCollection)
                {
                    caseNames.Add(type.Name);
                }
            }

            // 批量更新UI
            this.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                comboBox_Case.BeginUpdate();
                foreach (var name in caseNames)
                {
                    comboBox_Case.Items.Add(name);
                }
                comboBox_Case.EndUpdate();
            });
        }

        private void CopyTestDlls()
        {
            try
            {
                string testDllPath = Path.Combine(".", "TestDll");
                if (!Directory.Exists(testDllPath))
                {
                    this.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                        MessageBoxEX.Show("TestDll目录不存在", true);
                        Environment.Exit(0);
                    });
                    return;
                }

                var subdirectories = Directory.GetDirectories(testDllPath)
                    .Where(dir => !dir.EndsWith("_OLD", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (string subdirectory in subdirectories)
                {
                    if (!CopyFilesFromDirectory(subdirectory, ".\\"))
                    {
                        // 重试一次
                        if (!CopyFilesFromDirectory(subdirectory, ".\\"))
                        {
                            this.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                                MessageBoxEX.Show($"复制测试Dll失败: {subdirectory}", true);
                                Environment.Exit(0);
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                    MessageBoxEX.Show("Copy Dll时发生错误: " + ex.Message, true);
                    Environment.Exit(0);
                });
            }
        }










        private void btn_LoadCase_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(comboBox_Case.Text))
            {
                MessageBoxEX.Show("==请先选择机种==", true);
                return;
            }

            str_CaseName = comboBox_Case.Text;

            //进入测试主界面，传递OC
            _mainForm = new MainForm(comboBox_Case.Text);

            this.Hide();
            _mainForm.ShowDialog();

            this.Close();
        }

        private void btn_NGSample_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox_Case.Text))
            {
                MessageBoxEX.Show("==请先选择机种==", true);
                return;
            }


            if (!textBox_NGSample_PWD.Visible)
            {
                label_PWD.Visible = true;
                textBox_NGSample_PWD.Visible = true;
                return;
            }


            if (textBox_NGSample_PWD.Text != "41000306")
            {
                MessageBoxEX.Show("离线测试密码错误", true);
                return;
            }

            str_CaseName = comboBox_Case.Text;

            //进入测试主界面，传递OC
            _mainForm = new MainForm(comboBox_Case.Text);

            this.Hide();
            _mainForm.ShowDialog();
            this.Close();
        }

        public bool CopyFilesFromDirectory(string sourceDir, string destinationDir)
        {
            try
            {
                Directory.CreateDirectory(destinationDir); // 确保目标目录存在

                foreach (string file in Directory.GetFiles(sourceDir))
                {
                    string destFile = Path.Combine(destinationDir, Path.GetFileName(file));

                    if (Path.GetFileName(file).Contains("ClientUtils.dll"))
                    {
                        File.Copy(file, destFile, true); // true 表示如果目标文件已存在，则覆盖它
                    }
                    else
                    {
                        if (!File.Exists(destFile))
                        {
                            File.Copy(file, destFile); // true 表示如果目标文件已存在，则覆盖它
                        }
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button_Max_Click(object sender, EventArgs e)
        {
            // 判断窗口是否处于最大化状态
            if (this.WindowState == FormWindowState.Maximized)
            {
                // 执行取消最大化的操作
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void button_Min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void comboBox_Case_DropDown(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;
            int newWidth;

            foreach (string s in ((ComboBox)sender).Items)
            {
                newWidth = (int)g.MeasureString(s, font).Width
                    + vertScrollBarWidth;

                if (width < newWidth)
                {
                    width = newWidth;
                }
            }

            senderComboBox.DropDownWidth = width;
        }

        private void checkBox_LogCollection_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }


    }
}
