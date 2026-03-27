using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Properties;
using Liteon_TestProgram.Utilities;
using ScottPlot.Rendering.RenderActions;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Liteon_TestProgram
{

    public partial class MainForm : Form
    {

        [DllImport("user32.dll")]  //需添加using System.Runtime.InteropServices
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        bool isMouseDown = false; //表示鼠标当前是否处于按下状态，初始值为否 


        public string str_ShowDefaultRollScreenInfo = "===光寶科技(常州)有限公司欢迎您===";


        private readonly string str_SelectCaseName;
        private readonly SynchronizationContext _synchronizationContext;
        private readonly System.Threading.Timer _timer;
        public ConfigForm _configForm;
        public TestForm testForm;
        public ProjectChangeForm projectChangeForm;


        private void ConfigForm_ValueChanged(object sender, EventArgs e)
        {
            // _configForm.VariableChanged -= ConfigForm_ValueChanged;

            UIHandleHelper.ControlHandle(textBox_Info1, () => textBox_Info1.Text = $"{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}");
            UIHandleHelper.ControlHandle(textBox_Info2, () => textBox_Info2.Text = $"{CaseCodeBase.struct_EncryptINI.stru_str_CaseVersion}");
            UIHandleHelper.ControlHandle(textBox_Info3, () => textBox_Info3.Text = $"{CaseCodeBase.struct_EncryptINI.stru_str_SFCNumber}");
            UIHandleHelper.ControlHandle(textBox_Info4, () => textBox_Info4.Text = $"{CaseCodeBase.struct_NormalINI.stru_i_TestCompNum}#{CaseCodeBase.struct_NormalINI.stru_str_TesterPort}");

        }

        public MainForm(string str_SelectCaseName)
        {
            InitializeComponent();

            //滚动屏幕默认的字符串
            label_info5.Text = str_ShowDefaultRollScreenInfo;
            

            //第一个Panel占总宽度的65%,初始大小
            int totalWidth = splitContainer1.Width;
            int panelWidth = totalWidth * 65 / 100;
            splitContainer1.SplitterDistance = panelWidth;

            //设定可以拖动窗口的事件
            this.MouseDown += Form_Base_MouseDown;

            //MouseMove += Main_MouseMove;
            //MouseUp += Main_MouseUp;


            if (_configForm == null)
            {
                _configForm = new ConfigForm();
                _configForm.VariableChanged += ConfigForm_ValueChanged;
            }

            UIHandleHelper.toolStripStatusLabel_Robot = toolStripStatusLabel_Robot;
            UIHandleHelper.toolStripStatusLabel_Multi = toolStripStatusLabel_Multi;
            UIHandleHelper.statusStrip = statusStrip1;
            this.str_SelectCaseName = str_SelectCaseName;

            //// 眸鳳絞UI盄最腔SynchronizationContext
            _synchronizationContext = SynchronizationContext.Current;

            _timer = new System.Threading.Timer(TimeOut_CallBack, null, 1000, 1000);

            addTestForm();


            #region 局部变量，只是用来判断是否你选的的机种是否支持，不支持则退出程序，没有其他用途

            //MainForm mf,TestForm tf
            ProjectChangeForm projectChangeForm = new ProjectChangeForm(this, str_SelectCaseName);
            TestForm testForm = new TestForm(this, projectChangeForm, str_SelectCaseName);
            

            var wop = CaseCodeBase.Create_Case_Object(this, testForm, projectChangeForm, str_SelectCaseName);
            if (wop != null)
            {

            }
            else
            {
                MessageBoxEX.Show($"Mainform-不支持此机种的测试，测试工具没有该机种{str_SelectCaseName}类", true);
                Environment.Exit(0);
            }


            #endregion

        }





        void TimeOut_CallBack(object obj)
        {
            _synchronizationContext?.Post(_ =>
            {
                toolStripStatusLabel_Time.Text = DateTime.Now.ToString();
            }, null);
        }


        private void Form_Base_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
        }


        #region 处理WM_NCHITTEST消息以调整窗口大小

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTCLIENT = 1;
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;

            if (m.Msg == WM_NCHITTEST)
            {
                int borderWidth = 10; // 边缘敏感区域宽度
                Point pos = new Point(m.LParam.ToInt32() & 0xFFFF, m.LParam.ToInt32() >> 16);
                pos = this.PointToClient(pos);

                // 检查是否在边缘区域
                if (pos.X <= borderWidth && pos.Y <= borderWidth)
                    m.Result = (IntPtr)HTTOPLEFT;
                else if (pos.X >= ClientSize.Width - borderWidth && pos.Y <= borderWidth)
                    m.Result = (IntPtr)HTTOPRIGHT;
                else if (pos.X <= borderWidth && pos.Y >= ClientSize.Height - borderWidth)
                    m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (pos.X >= ClientSize.Width - borderWidth && pos.Y >= ClientSize.Height - borderWidth)
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (pos.X <= borderWidth)
                    m.Result = (IntPtr)HTLEFT;
                else if (pos.X >= ClientSize.Width - borderWidth)
                    m.Result = (IntPtr)HTRIGHT;
                else if (pos.Y <= borderWidth)
                    m.Result = (IntPtr)HTTOP;
                else if (pos.Y >= ClientSize.Height - borderWidth)
                    m.Result = (IntPtr)HTBOTTOM;
                else
                    base.WndProc(ref m); // 其他区域交由默认处理
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        #endregion



        private void addTestForm()
        {

            #region 测试页和变更记录页

            TabPage tabPage_Tool1 = new TabPage();
            tabPage_Tool1.ImageIndex = 2;
            tabPage_Tool1.Text = "变更记录";

          
            projectChangeForm = new ProjectChangeForm(this, this.str_SelectCaseName);
            projectChangeForm.Location = new Point(0, 0);
            projectChangeForm.TopLevel = false;
            projectChangeForm.Dock = DockStyle.Fill;

            projectChangeForm.Show();//必须 20240925
            tabPage_Tool1.Controls.Add(projectChangeForm);



            TabPage tabPage = new TabPage();
            tabPage.ImageIndex = 1;
            tabPage.Text = "测试页";
            tabPage.BackColor = Color.FromArgb(46, 50, 58);
           
            tabControl_test.Controls.Add(tabPage);
            tabControl_test.Controls.Add(tabPage_Tool1);

            testForm = new TestForm(this, projectChangeForm, this.str_SelectCaseName);
            testForm.Location = new Point(0, 0);
            testForm.TopLevel = false;
            testForm.Dock = DockStyle.Fill;

            testForm.Show();//必须 20240925
            tabPage.Controls.Add(testForm);

            #endregion

            #region 工具页

            AddToolTabPage(tabControl_test, typeof(ToolForm_01), "小工具_1", 1, this, this.str_SelectCaseName);
            AddToolTabPage(tabControl_test, typeof(ToolForm_02), "小工具_2", 2, this, this.str_SelectCaseName);

            #endregion

            #region 添加tab选项卡的旧方法


            //#region ToolForm_01

            //TabPage tabPage_Tool1 = new TabPage();
            //tabPage_Tool1.ImageIndex = 1;
            //tabPage_Tool1.Text = "小工具_1";

            //tabControl_test.Controls.Add(tabPage_Tool1);

            //#endregion

            //#region ToolForm_02

            //TabPage tabPage_Tool2 = new TabPage();
            //tabPage_Tool2.ImageIndex = 2;
            //tabPage_Tool2.Text = "小工具_2";

            //tabControl_test.Controls.Add(tabPage_Tool2);

            //#endregion

            //#region ToolForm_02

            //ToolForm_02 toolForm_02 = new ToolForm_02(this, this.str_SelectCaseName);
            //toolForm_02.Location = new Point(0, 0);
            //toolForm_02.TopLevel = false;
            //toolForm_02.Dock = DockStyle.Fill;

            //toolForm_02.Show();//必须 20240925

            //tabPage_Tool2.Controls.Add(toolForm_02);

            //#endregion

            //#region ToolForm_01

            //ToolForm_01 toolForm = new ToolForm_01(this, this.str_SelectCaseName);
            //toolForm.Location = new Point(0, 0);
            //toolForm.TopLevel = false;
            //toolForm.Dock = DockStyle.Fill;

            //toolForm.Show();//必须 20240925
            //tabPage_Tool1.Controls.Add(toolForm);

            //#endregion





            //// 订阅testForm的某个关闭事件
            this.FormClosing += (sender, e) =>
            {
                //testForm.TestForm_FormClose(sender, e);

                // 处理关闭逻辑，如移除TabPage
                //if (tabPage.Controls.Contains(testForm))
                //{
                //    tabPage.Controls.Remove(testForm);
                //}
            };


            #endregion

        }

        /// <summary>
        /// 添加工具标签页和对应的工具窗体
        /// </summary>
        /// <param name="tabControl">要添加标签页的TabControl</param>
        /// <param name="toolFormType">工具窗体的类型</param>
        /// <param name="tabText">标签页显示的文本</param>
        /// <param name="imageIndex">标签页的图标索引</param>
        /// <param name="parentForm">父窗体</param>
        /// <param name="selectCaseName">选择的案例名称</param>
        private void AddToolTabPage(TabControl tabControl, Type toolFormType, string tabText, int imageIndex, Form parentForm, string selectCaseName)
        {
            // 创建并配置标签页
            TabPage tabPage = new TabPage
            {
                ImageIndex = imageIndex,
                Text = tabText
            };

            tabControl.Controls.Add(tabPage);

            // 创建工具窗体实例
            Form toolForm = (Form)Activator.CreateInstance(toolFormType, parentForm, selectCaseName);
            toolForm.Location = new Point(0, 0);
            toolForm.TopLevel = false;
            toolForm.Dock = DockStyle.Fill;
            toolForm.Show(); // 必须显示

            tabPage.Controls.Add(toolForm);
        }

       


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var ret = MessageBoxEX.Show("确定关闭此测试工具？", false);
            if (ret != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            if (testForm != null)
            {
                testForm.TestForm_FormClose();
            }
            
            _timer.Change(-1, -1);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowReleaseTime();

            /*
            StringBuilder ValTemp = new StringBuilder();
            string str_CaseFolder = new string(this.str_SelectCaseName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{str_CaseProjectName}\\{str_CaseProjectName}\\{this.str_SelectCaseName}_Encrypt.ini";

            IniHelper.GetIniStr("Model", "ProjectName", "null", ValTemp, 100, str_IniPath);
            string ProjectName = ValTemp.ToString();

            IniHelper.GetIniStr("Model", "Version", "null", ValTemp, 50, str_IniPath);
            string ProjectVersion = ValTemp.ToString();

            IniHelper.GetIniStr("Model", "SFCNumber", "null", ValTemp, 10, str_IniPath);
            string SFCNumber = ValTemp.ToString();


            //================
            str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{this.str_SelectCaseName}_Normal.ini";

            IniHelper.GetIniStr("TesterPort", "PortName", "null", ValTemp, 10, str_IniPath);
            string PortName = ValTemp.ToString();

            UIHandleHelper.ControlHandle(textBox_Info1, () => textBox_Info1.Text = ProjectName);
            UIHandleHelper.ControlHandle(textBox_Info2, () => textBox_Info2.Text = ProjectVersion);
            UIHandleHelper.ControlHandle(textBox_Info3, () => textBox_Info3.Text = SFCNumber);
            UIHandleHelper.ControlHandle(textBox_Info4, () => textBox_Info4.Text = $"{CaseCodeBase.struct_NormalINI.stru_i_TestCompNum}#{PortName}");
            */

            UIHandleHelper.ControlHandle(textBox_Info1, () => textBox_Info1.Text = $"{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}");
            UIHandleHelper.ControlHandle(textBox_Info2, () => textBox_Info2.Text = $"{CaseCodeBase.struct_EncryptINI.stru_str_CaseVersion}");
            UIHandleHelper.ControlHandle(textBox_Info3, () => textBox_Info3.Text = $"{CaseCodeBase.struct_EncryptINI.stru_str_SFCNumber}");
            UIHandleHelper.ControlHandle(textBox_Info4, () => textBox_Info4.Text = $"{CaseCodeBase.struct_NormalINI.stru_i_TestCompNum}#{CaseCodeBase.struct_NormalINI.stru_str_TesterPort}");
        }

        public void ShowReleaseTime()
        {
            string assemblyPath = typeof(Program).Assembly.Location;
            DateTime lastWriteTime = File.GetLastWriteTime(assemblyPath);
            toolStripStatusLabel_ReleaseTime.Text = "Compile Time (UTC+8): " + lastWriteTime;



            // 获取当前执行的程序集的名称
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyName = assembly.GetName().Name;
            string[] strVersion = assemblyName.Split('_');
            toolStripStatusLabel_ToolVersion.Text = "UI Version: " + strVersion[2];
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            var ret = MessageBoxEX.Show("确定关闭此测试工具？", false);
            if (ret != DialogResult.Yes)
            {
                return;
            }
            else
            {

                if (testForm != null)
                {
                    testForm.TestForm_FormClose();
                    testForm.Dispose();
                }

                _timer.Change(-1, -1);
                System.Environment.Exit(0);
            }
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

        /// <summary>
        /// 滚定屏幕info5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_info5_Tick(object sender, EventArgs e)
        {
            if (label_info5.Left + label_info5.Width < textBox_info5.Left)
            {
                label_info5.Left = textBox_info5.Width + textBox_info5.Left;
            }

            label_info5.Left -= 10;
        }

        private void timer_RollScreenInfoIni_Tick(object sender, EventArgs e)
        {
            Task.Run(() => 
            {
                ReadRollScreenInfoIni();
            });
           
        }


        public void ReadRollScreenInfoIni()
        {

            string str_RollScreenInfoIniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\ShowSelectCaseName\\ShowSelectCaseName.ini";
            StringBuilder ValTemp = new StringBuilder();

            string str_RemoteInfoIniCheckSwitch = "";
            string str_RemoteInfoIniPath = "";
            string str_RemoteInfoIniText = "";
            string str_RemoteInfoIniMd5 = "";

            try
            {

                if (File.Exists(str_RollScreenInfoIniPath) == false)
                {
                    throw new ArgumentException("ShowSelectCaseName.ini文件不存在。");
                }

                //本地的INI
                #region [RollScreenIni_Path]

                IniHelper.GetIniStr("RollScreenIni_Path", "Switch", "0", ValTemp, 200, str_RollScreenInfoIniPath);
                str_RemoteInfoIniCheckSwitch = ValTemp.ToString();

                if (string.IsNullOrEmpty(str_RemoteInfoIniCheckSwitch) || str_RemoteInfoIniCheckSwitch == "0")
                {
                    Console.WriteLine("Check RollScreenIni_Path Ini Switch is off");
                    UIHandleHelper.ControlHandle(label_info5, () => label_info5.Text = str_ShowDefaultRollScreenInfo);
                    return;
                }



                IniHelper.GetIniStr("RollScreenIni_Path", "PathAndName", "", ValTemp, 200, str_RollScreenInfoIniPath);
                str_RemoteInfoIniPath = ValTemp.ToString();

                if (string.IsNullOrEmpty(str_RemoteInfoIniPath))
                {
                    Console.WriteLine("Check RollScreenIni_Path Ini PathAndName is empty");
                    UIHandleHelper.ControlHandle(label_info5, () =>  label_info5.Text = str_ShowDefaultRollScreenInfo);
                    return;
                }

                #endregion




                //远端的INI
                #region [RollScreenInfo]

                str_RemoteInfoIniText = IniHelper.GetIniStr(str_RemoteInfoIniPath, "RollScreenInfo", "Info", Encoding.UTF8);

                if (string.IsNullOrEmpty(str_RemoteInfoIniText))
                {
                    Console.WriteLine("Check Remote Info Ini Text is empty");
                    UIHandleHelper.ControlHandle(label_info5, () => label_info5.Text = str_ShowDefaultRollScreenInfo);
                    return;
                }


                str_RemoteInfoIniMd5 = IniHelper.GetIniStr(str_RemoteInfoIniPath, "RollScreenInfo", "Md5", Encoding.UTF8);

                if (string.IsNullOrEmpty(str_RemoteInfoIniMd5))
                {
                    Console.WriteLine("Check Remote Info Ini Md5 is empty");
                    UIHandleHelper.ControlHandle(label_info5, () => label_info5.Text = str_ShowDefaultRollScreenInfo);
                    return;
                }
                else
                {
                    string stringMd5 = str_RemoteInfoIniText + "RollScreenInfoMD5";

                    MD5Helper mD5Helper = new MD5Helper();
                    string Md5 = mD5Helper.GetStringMd5Hash(stringMd5);
                    if (Md5 != str_RemoteInfoIniMd5)
                    {
                        Console.WriteLine($"<RollScreenInfoMD5> Check Remote Info Ini Md5 Fail: {Md5}");
                        UIHandleHelper.ControlHandle(label_info5, () => label_info5.Text = str_ShowDefaultRollScreenInfo);
                        return;
                    }
                    
                }



                UIHandleHelper.ControlHandle(label_info5, () => label_info5.Text = str_RemoteInfoIniText);

                #endregion



            }
            catch (Exception ex)
            {
                Console.WriteLine($"RollScreenInfo failed:{ex}");
            }

            return;
        }







    }
}