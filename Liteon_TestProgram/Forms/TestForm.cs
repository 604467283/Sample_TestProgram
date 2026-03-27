using Liteon_TestProgram.Base;
using Liteon_TestProgram.CaseProject;
using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Timers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;
using Liteon_TestProgram.Save_LogFile;
using static System.Windows.Forms.DataFormats;
using System.IO.Ports;
using Button = System.Windows.Forms.Button;
using NationalInstruments;
using static Liteon_TestProgram.Base.Class_Variable;
using NationalInstruments.Restricted;
using Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V1;
using Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V2;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using Liteon_TestProgram.Utilities.PEM;

namespace Liteon_TestProgram.Forms
{
    public partial class TestForm : Form
    {
        #region 引用dll

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体

        #endregion

        Color normalColor = Color.FromArgb(64, 158, 255);
        Color testColor = Color.FromArgb(0, 191, 255);
        Color failColor = Color.FromArgb(178, 34, 34);
        Color succeedColor = Color.FromArgb(103, 194, 58);

        public Class_Variable.struct_NormalINI_Variable struct_NormalINI;
        public Class_Variable.struct_EncryptINI_Variable struct_EncryptINI;
        public Class_Variable.MacBD_Relation macBD_Relation;
        public Class_Variable.SN_Relation sn_Relation;
        private HttpHelper httpHelper = new HttpHelper();
        MacHelper macHelper = new MacHelper();
        private ConfigForm configForm;
        private readonly SynchronizationContext? _synchronizationContext;

        PEMController pEMController;
        PEMHelper pEMHelper;
        ShieldingHelper shieldingHelper;
        TestCountHelper testCountHelper;
        public MainForm _MainForm;
        public ProjectChangeForm _projectChangeForm;
        public static TestForm testForm_Obj;
        CaseCodeBase wop; //反射机种对象

        private List<Thread> TestFormThreadList = new List<Thread>();
        public bool bIsConnected = false;
        Socket Robotsocket;
        Socket Hostsocket;
        Socket MFCSocket;
        bool bFinishTest;
        bool bFinishTestStu;
        bool bTestStatus;
        bool bFinishTestMac;
        bool bFinishTestCom;
        //用于取消手臂线程
        CancellationTokenSource tokenSource;
        CancellationToken cancellationToken;
        bool bUseRobotBefore = false;

        static AutoResetEvent myResetEvent = new AutoResetEvent(false);
        public bool MulState;
        public bool testAccept = true;
        public string ClientIP;

        string sRevDUTMac;
        string sRevDUTSN;
        string sRevDUTBD;
        string ErrorCode;
        bool bSendSFC;

        private System.Timers.Timer timer_Test;
        private int elapsedSeconds;

        public string str_CaseName { get; private set; }

        public delegate void TestButtonCallBack_Void();
        public TestButtonCallBack_Void CallBack_TestMac;
        public TestButtonCallBack_Void CallBack_ResourceRelease;

        public delegate bool TestButtonCallBack_Bool();
        public TestButtonCallBack_Bool CallBack_TestPre; // 测试预处理
        public TestButtonCallBack_Bool CallBack_TestInit;
        public TestButtonCallBack_Bool CallBack_TestFlow;


        public delegate bool TestButtonCallBack_Bool_Parameter(bool bTestResult);
        public TestButtonCallBack_Bool_Parameter CallBack_TestEnd;


        public TestForm(MainForm mainForm, ProjectChangeForm pc, string str_CaseName)
        {
            this._MainForm = mainForm;
            this.str_CaseName = str_CaseName;
            this._projectChangeForm = pc;
            InitializeComponent();

            _synchronizationContext = SynchronizationContext.Current;

            // 初始化Timer
            timer_Test = new System.Timers.Timer(1000); // 设置Timer间隔为1秒
            timer_Test.Elapsed += OnTimerElapsed; // 订阅Elapsed事件
        }




        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Timer事件处理程序，在后台线程中运行
            elapsedSeconds++;

            // 安全地更新UI
            _synchronizationContext?.Post(_ =>
            {
                label_TestTime.Text = $"测试时间: {elapsedSeconds} s";
            }, null);

            //this.Invoke((MethodInvoker)delegate
            //{
            //    // 这里的代码在UI线程上执行
            //    timeLabel.Text = $"Elapsed Time: {elapsedSeconds} seconds";
            //});
        }


        private void TestForm_Load(object sender, EventArgs e)
        {
            testForm_Obj = this;
            bFinishTest = true;     //Test是否結束標誌

            #region 控件传递和初始设定

            UIHandleHelper.LogBox = richTextBox_log;
            UIHandleHelper.dataGridView = dataGridView_item;

            UIHandleHelper.ControlHandle(label_Status, () => label_Status.BackColor = normalColor);
            UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = false);
            UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
            UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);

            #endregion

            #region 只enable配置按钮

            Button myExceptionButton = this.Controls.Find("btn_Config", true).FirstOrDefault() as Button;
            if (myExceptionButton != null)
            {
                DisableAllButtonsExcept(myExceptionButton);
            }
            else
            {
                // 如果没有找到指定的按钮，你可能想禁用所有按钮或做其他处理
                foreach (Control control in this.Controls)
                {
                    if (control is Button)
                    {
                        ((Button)control).Enabled = false;
                    }
                }
            }
            btn_log.Enabled = true;

            #endregion

            #region 反射case对象

            /*var*/
            wop = CaseCodeBase.Create_Case_Object(_MainForm, this, _projectChangeForm, str_CaseName);
            if (wop != null)
            {
                //nothing
            }
            else
            {
                MessageBoxEX.Show($"TestFormLoad-不支持此机种的测试，测试工具没有该机种{str_CaseName}类", true);
                Environment.Exit(0);
            }

            #endregion

            #region 测试工具置顶

            try
            {
                SetForegroundWindow(this.Handle);
            }
            catch (Exception ex)
            {
                MessageBoxEX.Show($"测试工具置顶失败，{ex}", true);
                Environment.Exit(0);
            }

            #endregion

            #region 读取普通配置档

            bool bResult = false;

            bResult = wop.ReadNormalIni();
            struct_NormalINI = CaseCodeBase.struct_NormalINI;

            #endregion

            #region 检查硬盘空间

            if (bResult)
            {
                long lSpace = DiskUtilsHelper.CheckDrive(struct_NormalINI.stru_str_LogFilePath);
                if (lSpace != -1)
                {
                    bResult = (lSpace >= struct_NormalINI.stru_i_FreeSpaceLimit);
                }
                else
                {
                    bResult = false;
                }
            }

            #endregion

            #region 读取加密配置档

            if (bResult)
            {
                bResult = wop.ReadEncryptIni();
                struct_EncryptINI = CaseCodeBase.struct_EncryptINI;
            }
            #endregion


            Task.Run(() =>
            {
                #region 获取硬盘和主板SN

                if (bResult)
                {
                    CaseCodeBase.struct_TestVariable.PC_HardDeskSN = PCHelper.GetDiskSN().Trim();
                    CaseCodeBase.struct_TestVariable.PC_MainboardSN = PCHelper.GetMainboardSN().Trim();
                }

                #endregion

                #region 初始化串口

                try
                {
                    wop.ConfigComPort();
                }
                catch (Exception ex)
                {
                    MessageBoxEX.Show($"初始化串口错误{ex}", true);
                    Environment.Exit(0);
                }

                #endregion

                #region 检查CallBack_TestPre

                Directory.CreateDirectory(".\\SFCFile");

                if (bResult)
                {
                    if (!CallBack_TestPre())
                    {
                        MessageBoxEX.Show("请检查TestPre-2错误...", true);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    MessageBoxEX.Show("请检查TestPre-1错误...", true);
                    Environment.Exit(0);
                }

                #endregion

                #region mac和Bt的关系: CallBack_TestMac

                if (bResult)
                {
                    CallBack_TestMac();
                    this.macBD_Relation = CaseCodeBase.macBD_Relation;
                    this.sn_Relation = CaseCodeBase.sn_Relation;
                }

                #endregion

                #region 显示测试计数

                _synchronizationContext?.Post(_ =>
                {
                    label_PassNum.Text = struct_NormalINI.stru_i_TestTimes_Pass.ToString();
                    label_FailNum.Text = struct_NormalINI.stru_i_TestTimes_Fail.ToString();
                    double result = ((double)struct_NormalINI.stru_i_TestTimes_Pass / (struct_NormalINI.stru_i_TestTimes_Pass + struct_NormalINI.stru_i_TestTimes_Fail)) * 100;
                    label_FPYNum.Text = result.ToString("F2") + " %";
                }, null);

                #endregion

                #region 手动测试模式


                if (struct_NormalINI.stru_b_DebugMode == false)
                {
                    tokenSource = new CancellationTokenSource();
                    cancellationToken = tokenSource.Token;
                    Thread thC = new Thread(() => ClientReceive(cancellationToken));
                    TestFormThreadList.Add(thC);
                    thC.IsBackground = true;
                    thC.Start();
                }
                else
                {
                    UIHandleHelper.ShowToolStripStatus_Robot("手动测试模式");
                }

                #endregion

                #region Multi_DUT

                if (struct_NormalINI.stru_b_Multi_Switch)
                {
                    if (struct_NormalINI.stru_i_SelectMode == 1)
                    {
                        ServerInit();
                    }
                    else
                    {
                        Thread thC = new Thread(MFCClientR);
                        TestFormThreadList.Add(thC);
                        thC.IsBackground = true;
                        thC.Start();
                    }
                }

                #endregion

                #region  PEM开卡

                #region 旧方法

                /*
                if (struct_NormalINI.stru_b_IsOpenPEM)
                {
                    pEMHelper = new PEMHelper();

                    if (pEMHelper.Init_PEM() != 0)
                    {
                        MessageBoxEX.Show("初始化PEM卡失败!", true);
                        Environment.Exit(0);
                    }
                    else
                    {
                        OpenPEMDUT(false);
                    }
                }
                */

                #endregion

                #region 新方法

                if (struct_NormalINI.stru_b_IsOpenPEM)
                {
                    if (pEMController == null)
                    {
                        pEMController = new PEMController();
                    }

                    pEMController.UILogMessage = UIHandleHelper.ShowRunLog; // 绑定日志输出方法

                    if (pEMController.Initialize() != true)
                    {
                        MessageBoxEX.Show("初始化PEM卡失败!", true);
                        Environment.Exit(0);
                    }

                    if (PEMPowerOff() != true)
                    {
                        MessageBoxEX.Show("PEM卡关闭电源失败!", true);
                        Environment.Exit(0);
                    }

                    DevconHelper.Rescan();

                }

                #endregion

                #endregion

                #region 开关屏蔽箱

                if (struct_NormalINI.stru_i_ShieldingBoxCOM != 0)
                {
                    shieldingHelper = new ShieldingHelper();
                    if (!shieldingHelper.InitShieldingCom(struct_NormalINI.stru_i_ShieldingBoxCOM.ToString()))
                    {
                        MessageBoxEX.Show("初始化屏蔽箱串口失败", true);
                        System.Environment.Exit(System.Environment.ExitCode);
                        this.Dispose();
                        this.Close();
                    }
                    else
                    {
                        OpenCloseShieldingBox(true);
                    }
                }

                #endregion

                #region 测试顶针使用次数

                testCountHelper = new TestCountHelper();
                testCountHelper.CheckTestCount(false);

                #endregion








                #region Barcode控件设定

                if (struct_NormalINI.stru_b_DebugMode)
                {

                    if (this.sn_Relation == SN_Relation.SN)
                    {
                        UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = true);
                        UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Focus());
                    }

                    if (this.macBD_Relation == MacBD_Relation.Separate)
                    {
                        UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = true);
                        UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
                        UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);
                        UIHandleHelper.ShowRunLog("窗口加载完毕，可以开始测试");
                        UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Focus());
                    }

                    if (this.macBD_Relation == MacBD_Relation.PlusOne || this.macBD_Relation == MacBD_Relation.OnlyMac)
                    {
                        UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = true);
                        UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
                        UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);
                        UIHandleHelper.ShowRunLog("窗口加载完毕，可以开始测试");
                        UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Focus());
                    }

                  

                }
                else
                {
                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = false);
                    UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
                    UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);
                    UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = false);
                    UIHandleHelper.ShowRunLog("窗口加载完毕，可以开始测试");
                }



                #endregion

            });


        }

        #region 找控件并禁用，只保留一个空间可用

        private void DisableAllButtonsExcept(Button exceptionButton)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button && control != exceptionButton)
                {
                    ((Button)control).Enabled = false;
                }

                // 如果你的按钮在容器控件（如Panel, GroupBox等）中，你可能需要递归检查这些容器
                DisableButtonsInContainer(control, exceptionButton);
            }
        }

        private void DisableButtonsInContainer(Control container, Button exceptionButton)
        {
            foreach (Control control in container.Controls)
            {
                if (control is Button && control != exceptionButton)
                {
                    ((Button)control).Enabled = false;
                }

                if (control.HasChildren)
                {
                    DisableButtonsInContainer(control, exceptionButton);
                }
            }
        }

        public void EnableAllButtons(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl is Button)
                {
                    ((Button)ctrl).Enabled = true;
                }

                // 递归检查子控件
                EnableAllButtons(ctrl);
            }
        }


        #endregion

        #region PEM有关方法

        public bool OpenPEMDUT(bool OnOff)
        {
            int iOPenDUTTestAgain = 0;

            if (File.Exists(".\\temp.txt"))
                File.Delete(".\\temp.txt");
            if (File.Exists(".\\bttemp.txt"))
                File.Delete(".\\bttemp.txt");
            if (File.Exists(".\\scanLog.txt"))
                File.Delete(".\\scanLog.txt");
            if (File.Exists(".\\IDLog.txt"))
                File.Delete(".\\IDLog.txt");
            if (File.Exists(".\\DUTIP.txt"))
                File.Delete(".\\DUTIP.txt");
            if (File.Exists(".\\FindDevice.txt"))
                File.Delete(".\\FindDevice.txt");
            if (OnOff)
            {

                //開PEM卡
                if (struct_NormalINI.stru_b_IsOpenPEM)
                {
                    if (pEMHelper.OpenPEM(OnOff))
                    {
                        UIHandleHelper.ShowRunLog("Open PEM Card OK!");
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog("Open PEM Card Fail!");
                        return false;
                    }
                }

                //開DUT
                if (struct_NormalINI.stru_b_IsOpenDUT)
                {
                    if (!pEMHelper.OpenBat(".\\enable.bat"))
                    {
                        UIHandleHelper.ShowRunLog("Open DUT Fail at Enable!", true);
                        return false;   //開卡有問題
                    }
                    if (!pEMHelper.OpenBat(".\\FindID.bat"))
                    {
                        UIHandleHelper.ShowRunLog("Open DUT Fail at FindID!", true);
                        return false;   //開卡有問題
                    }
                    if (!CheckIDLog())
                    {
                        UIHandleHelper.ShowRunLog("DUT Not Found!", true);
                        if (iOPenDUTTestAgain < 1)
                        {
                            iOPenDUTTestAgain = 1;
                            UIHandleHelper.ShowRunLog("Test Again...\n");
                            OpenPEMDUT(false);
                            if (struct_NormalINI.stru_i_ShieldingBoxCOM != 0)
                            {
                                if (!shieldingHelper.OpenShieldingBox(struct_NormalINI.stru_i_SleepCycleWhenOpen))
                                {
                                    UIHandleHelper.ShowRunLog("Open ShieldingBox error...", true);
                                    return false;
                                }
                                else
                                {
                                    UIHandleHelper.ShowRunLog("Open ShieldingBox Successful");
                                }

                                if (!shieldingHelper.CloseShieldingBox(struct_NormalINI.stru_i_SleepCycleWhenClose))
                                {
                                    UIHandleHelper.ShowRunLog("Close ShieldingBox error...", true);
                                    Hostsocket.Close();
                                    Multi_releaseStatus();
                                    MessageBoxEX.Show("Close ShieldingBox error, Please call PE to check!!!", true);
                                    this.Close();
                                }
                                else
                                {
                                    UIHandleHelper.ShowRunLog("Close ShieldingBox Successful");
                                }
                            }
                            if (OpenPEMDUT(true))
                            {
                                return true;
                            }
                            else
                                return false;
                        }
                        else
                            return false;   //不找卡
                    }
                }

            }
            else
            {
                if (struct_NormalINI.stru_b_IsOpenDUT)
                {
                    if (!pEMHelper.OpenBat(".\\disable.bat"))
                    {
                        UIHandleHelper.ShowRunLog("Open DUT Fail at Disable!", true);
                        return false;   //關卡有問題
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog("Close DUT Card OK!");
                    }
                }

                if (struct_NormalINI.stru_b_IsOpenPEM)
                {
                    pEMHelper.OpenPEM(OnOff);
                    UIHandleHelper.ShowRunLog("Close PEM Card OK!");
                }

            }

            return true;
        }

        public bool CheckIDLog()
        {
            bool WiFiCard = false;
            bool BTCard = false;
            try
            {
                StreamReader sr = File.OpenText(".\\IDLog.txt");
                String nextLine;
                while ((nextLine = sr.ReadLine()) != null)
                {
                    if (nextLine.IndexOf(struct_NormalINI.stru_str_DeviceName1) >= 0)
                    {
                        WiFiCard = true;
                    }
                    else if (nextLine.IndexOf(struct_NormalINI.stru_str_DeviceName2) >= 0)
                    {
                        BTCard = true;
                    }
                }
                sr.Close();
                if (WiFiCard && BTCard)
                {
                    return true;
                }
                else
                {
                    if (!WiFiCard)
                    {
                        ErrorCode = "W04MQ";
                    }
                    if (!BTCard)
                    {
                        ErrorCode = "BT00I";
                    }
                    return false;
                }

            }
            catch
            {
                UIHandleHelper.ShowRunLog("Don't Open File IDLog!", true);
                return false;
            }

        }

        public (bool, string) PEMPowerOn()
        {
            if (pEMController != null)
            {
                var bResult = pEMController.PowerOn();
                if (bResult.Item1)
                {
                    return (true, bResult.Item2);
                }
                else
                {
                    return (false, bResult.Item2);
                }
            }
            return (false, "null");
        }

        public bool PEMPowerOff()
        {
            if (pEMController != null)
            {
                if (pEMController.PowerOff())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }


        #endregion

        #region 屏蔽箱有关方法

        public bool OpenCloseShieldingBox(bool OpCL)
        {
            if (struct_NormalINI.stru_i_ShieldingBoxCOM != 0)
            {

                if (OpCL)
                {
                    if (!shieldingHelper.OpenShieldingBox(struct_NormalINI.stru_i_SleepCycleWhenOpen))
                    {
                        UIHandleHelper.ShowRunLog("Open ShieldingBox error...", true);
                        return false;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog("Open ShieldingBox Successful");
                    }
                }
                else
                {
                    if (!shieldingHelper.CloseShieldingBox(struct_NormalINI.stru_i_SleepCycleWhenClose))
                    {
                        UIHandleHelper.ShowRunLog("Close ShieldingBox error...", true);
                        Hostsocket.Close();
                        Multi_releaseStatus();
                        MessageBoxEX.Show("关闭屏蔽箱错误，请联系PTE检查", true);
                        Environment.Exit(0);
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog("Close ShieldingBox Successful");
                    }

                }
            }
            return true;
        }

        public void Multi_releaseStatus()
        {
            testAccept = true;
            if (struct_NormalINI.stru_b_Multi_Switch)
            {
                try
                {
                    if (struct_NormalINI.stru_i_SelectMode == 1)
                    {
                        if (MulState)
                        {
                            httpHelper.SendData(dic[ClientIP], "TestStart");
                        }
                    }
                    else
                    {
                        httpHelper.SendData(MFCSocket, "TestStart");
                    }
                }
                catch
                {
                    return;
                }

            }
        }

        public void Multi_lockStatus()
        {
            testAccept = false;
            myResetEvent = new AutoResetEvent(false);
            if (struct_NormalINI.stru_b_Multi_Switch)
            {
                try
                {
                    if (struct_NormalINI.stru_i_SelectMode == 1)
                    {
                        if (MulState)
                        {
                            if (httpHelper.SendData(dic[ClientIP], "TestAccept"))
                            {
                                UIHandleHelper.ShowRunLog("Waiting for the other program finished the test...");
                                myResetEvent.WaitOne();
                                Thread.Sleep(0);
                            }
                        }
                    }
                    else
                    {
                        if (httpHelper.SendData(MFCSocket, "TestAccept"))
                        {
                            UIHandleHelper.ShowRunLog("Waiting for the other program finished the test...");
                            myResetEvent.WaitOne();
                            Thread.Sleep(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowRunLog($"Multi_lockStatus error: {ex}");
                }

            }
        }

        #endregion

        #region Http Robot Multi

        public string StartReceivingAsync(Socket socketClient, byte[] buffer)
        {
            string str = "";
            socketClient.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, (asyncResult) =>
            {
                int bytesRead = socketClient.EndReceive(asyncResult);

                str = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                // 继续异步接收
                StartReceivingAsync(socketClient, buffer);

            }, null);

            return str;
        }


        void ClientReceive(CancellationToken cancellationToken)
        {
            while (true)
            {
                bIsConnected = false;
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //連接到的目標IP
                IPAddress ip = IPAddress.Parse(struct_NormalINI.stru_str_RobotClientHostIP);
                //連接目標端口
                IPEndPoint point = new IPEndPoint(ip, int.Parse(struct_NormalINI.stru_str_RobotClientHostPort));
                try
                {
                    client.Connect(point);
                    Hostsocket = client;
                    Robotsocket = client;
                    bIsConnected = true;
                    UIHandleHelper.ShowToolStripStatus_Robot("连接手臂成功,自动测试模式");
                    String StrsCompNum = String.Format("{0}{1}", struct_NormalINI.stru_i_TestCompNum, "#Start");
                    String sRevBarMac = String.Format("{0}{1}", struct_NormalINI.stru_i_TestCompNum, "#Barcode");
                    String sTestStatus = String.Format("{0}{1}", struct_NormalINI.stru_i_TestCompNum, "#Status");
                    String sTestErrcode = String.Format("{0}{1}", struct_NormalINI.stru_i_TestCompNum, "#TESTERR");
                    String sTestCheck = String.Format("{0}{1}", struct_NormalINI.stru_i_TestCompNum, "#CHECK");
                    String strMsg = "";


                    bUseRobotBefore = true;
                    if (cancellationToken.IsCancellationRequested)
                    {
                        UIHandleHelper.ShowRunLog("Cancel robot auto test0;");
                        UIHandleHelper.ShowToolStripStatus_Robot("手动测试");
                        bUseRobotBefore = false;
                        //Robotsocket.Disconnect(true);
                        Robotsocket.Close(1000);
                        return;
                    }

                    while (true)
                    {
                        try
                        {
                            bUseRobotBefore = true;
                            if (cancellationToken.IsCancellationRequested)
                            {
                                UIHandleHelper.ShowRunLog("Cancel robot auto test1;");
                                UIHandleHelper.ShowToolStripStatus_Robot("手动测试");
                                bUseRobotBefore = false;
                                //Robotsocket.Disconnect(true);
                                Robotsocket.Close(1000);
                                return;
                            }

                            byte[] buffer = new byte[1024 * 1024];

                            int n = client.Receive(buffer); //会阻塞一直等待有数据

                            string s = Encoding.UTF8.GetString(buffer, 0, n);


                            if (bFinishTest)    //判定是否在測試中
                            {
                                //發送測試狀態
                                if (s.IndexOf(sTestStatus) >= 0)
                                {
                                    if (bFinishTestStu == false)
                                    {
                                        bFinishTestStu = true;
                                        if (bTestStatus)
                                            strMsg = String.Format("{0}{1}", struct_NormalINI.stru_i_TestCompNum, "#Pass");
                                        else
                                            strMsg = String.Format("{0}{1}", struct_NormalINI.stru_i_TestCompNum, "#Fail");
                                        httpHelper.SendData(client, strMsg);
                                    }
                                }


                                //接收MAC ID
                                if (s.IndexOf(sRevBarMac) >= 0)
                                {
                                    if (bFinishTestMac == false)
                                    {
                                        bFinishTestMac = true;

                                        UIHandleHelper.ShowRunLog($"sRevBarMac -> {s}");

                                        #region 条码BT和Mac获取

                                        if (this.macBD_Relation != MacBD_Relation.None)
                                        {
                                            CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac = sRevDUTMac = s.Substring(s.IndexOf("#") + 8, CaseCodeBase.struct_TestVariable.iMACLength);
                                        }

                                        //if (struct_NormalINI.stru_i_MACLength == 15)
                                        //{
                                        //    CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac = sRevDUTMac = "23S" + sRevDUTMac;
                                        //}

                                        if (this.macBD_Relation == MacBD_Relation.PlusOne)
                                        {
                                            CaseCodeBase.struct_Barcode.stru_str_sRevDUTBD = sRevDUTBD = macHelper.BTMacSet(sRevDUTMac);
                                        }
                                        else if (this.macBD_Relation == MacBD_Relation.Separate)
                                        {
                                            CaseCodeBase.struct_Barcode.stru_str_sRevDUTBD = sRevDUTBD = s.Substring(s.IndexOf("+") + 1, CaseCodeBase.struct_TestVariable.iBDLength);
                                        }

                                        #endregion



                                        #region 条码SN获取

                                        if (this.macBD_Relation == MacBD_Relation.None && this.sn_Relation == SN_Relation.SN) //条码只有SN
                                        {
                                            CaseCodeBase.struct_Barcode.stru_str_sRevDUTSN = sRevDUTSN = s.Substring(s.IndexOf("#") + 8, CaseCodeBase.struct_TestVariable.iSNLength);
                                        }

                                        if (this.macBD_Relation == MacBD_Relation.PlusOne && this.sn_Relation == SN_Relation.SN) //条码只有mac和sn
                                        {
                                            CaseCodeBase.struct_Barcode.stru_str_sRevDUTSN = sRevDUTSN = s.Substring(s.IndexOf("+") + 1, CaseCodeBase.struct_TestVariable.iSNLength);
                                        }

                                        if (this.macBD_Relation == MacBD_Relation.Separate && this.sn_Relation == SN_Relation.SN) //条码有mac bt和sn
                                        {
                                            int firstPlusIndex = s.IndexOf("+");
                                            int secondPlusIndex = s.IndexOf("+", firstPlusIndex + 1);
                                            CaseCodeBase.struct_Barcode.stru_str_sRevDUTSN = sRevDUTSN = s.Substring(secondPlusIndex + 1, CaseCodeBase.struct_TestVariable.iSNLength);
                                        }

                                        #endregion



                                    }
                                }

                                if (s.IndexOf(StrsCompNum) >= 0)
                                {
                                    if (bFinishTestCom == false && bFinishTestMac == true /*&& sRevDUTMac.Length == struct_NormalINI.stru_i_MACLength*/)
                                    {
                                        bFinishTestCom = true;
                                        StartTest();
                                    }

                                }

                                //收到Test Error指示
                                if (s.IndexOf(sTestErrcode) >= 0)
                                {
                                    UIHandleHelper.ShowRunLog("Msg: 请检查扫描错误");
                                    Hostsocket.Close();
                                    MessageBoxEX.Show("Msg: 请联系PE检查扫描错误", true);

                                }

                                //Check流水碼
                                if (s.IndexOf(sTestCheck) >= 0)
                                {
                                    strMsg = String.Format("{0}#{1}{2}", struct_NormalINI.stru_i_TestCompNum, struct_EncryptINI.stru_str_SFCNumber, "*CHECK");
                                    httpHelper.SendData(client, strMsg);
                                }

                                //顯示接收內容
                                strMsg = String.Format("Receive----->{0}\n", s);
                                UIHandleHelper.ShowRunLog(strMsg);
                                //
                            }

                        }
                        catch
                        {
                            UIHandleHelper.ShowToolStripStatus_Robot("等待连接手臂");
                            bIsConnected = false;
                            break;
                        }
                    }


                }

                catch
                {
                    UIHandleHelper.ShowToolStripStatus_Robot("等待连接手臂");
                    Thread.Sleep(2000);
                    //break;
                }

            }
        }

        public void ServerInit()
        {
            IPAddress ip = IPAddress.Parse(struct_NormalINI.stru_str_Multi_Server_IP);//IP地址
            IPEndPoint point = new IPEndPoint(ip, int.Parse(struct_NormalINI.stru_str_Multi_Server_Port));//端口號
            //創建監聽用的socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //監聽哪個端口
                socket.Bind(point);
                //同一時間點過來10個客戶端,排隊
                socket.Listen(10);
                UIHandleHelper.ShowToolStripStatus_Multi("Multi Server: 等待连接客户端");  //MFG Server to Listening!
                Thread thread = new Thread(AcceptInfo);
                thread.IsBackground = true;
                TestFormThreadList.Add(thread);
                thread.Start(socket);
            }
            catch (System.Exception ex)
            {
                UIHandleHelper.ShowToolStripStatus_Multi($"Multi Server: 连接客户端错误{ex.Message}");
            }
        }

        Dictionary<string, Socket> dic = new Dictionary<string, Socket>();
        void AcceptInfo(object o)
        {
            Socket socket = o as Socket;
            while (true)
            {
                try
                {
                    //創建通信用的Socket

                    Socket tSocket = socket.Accept();
                    String point = tSocket.RemoteEndPoint.ToString();
                    UIHandleHelper.ShowToolStripStatus_Multi("Multi Server: 连接客户端成功"); //Listening successful!
                    //cboIpPort.Items.Add(point);
                    ClientIP = point;
                    dic.Add(point, tSocket);
                    MulState = true;
                    //創建接收線程
                    Thread th = new Thread(ReceiveMsg);
                    TestFormThreadList.Add(th);
                    th.IsBackground = true;
                    th.Start(tSocket);

                }
                catch (System.Exception ex)
                {
                    UIHandleHelper.ShowToolStripStatus_Multi($"Multi Server: 连接客户端错误{ex.Message}");
                    break;
                }
            }
        }
        void ReceiveMsg(object o)
        {
            Socket Serovers = o as Socket;
            while (true)
            {
                //接收客户端发送过来的数据
                try
                {

                    //定义byte数组存放从客户端接收过来的数据
                    byte[] buffer = new byte[1024 * 1024];
                    //将接收过来的数据放到buffer中，并返回实际接受数据的长度
                    int n = Serovers.Receive(buffer);
                    //将字节转换成字符串
                    string words = Encoding.UTF8.GetString(buffer, 0, n);
                    string ip = Serovers.RemoteEndPoint.ToString();

                    if (words.IndexOf("TestAccept") >= 0)
                    {
                        UIHandleHelper.ShowRunLog("receive->TestAccept");
                        if (testAccept)
                        {
                            httpHelper.SendData(dic[ip], "TestStart");
                        }
                    }
                    if (words.IndexOf("TestStart") >= 0)
                    {
                        UIHandleHelper.ShowRunLog("receive->TestStart");
                        myResetEvent.Set();
                        Thread.Sleep(0);
                    }
                }
                catch
                {
                    UIHandleHelper.ShowToolStripStatus_Multi("Multi Server: 正在等待客户端连接...");
                    MulState = false;
                    break;
                }
            }

        }

        void MFCClientR()
        {
            while (true)
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //連接到的目標IP
                IPAddress ip = IPAddress.Parse(struct_NormalINI.stru_str_Multi_Server_IP);
                //連接目標端口
                IPEndPoint point = new IPEndPoint(ip, int.Parse(struct_NormalINI.stru_str_Multi_Server_Port));
                try
                {
                    client.Connect(point);
                    MFCSocket = client;
                    UIHandleHelper.ShowToolStripStatus_Multi("Multi Client: 连接服务端成功");
                    while (true)
                    {
                        try
                        {
                            byte[] buffer = new byte[1024 * 1024];

                            int n = client.Receive(buffer);

                            string s = Encoding.UTF8.GetString(buffer, 0, n);
                            if (s.IndexOf("TestAccept") >= 0)
                            {
                                UIHandleHelper.ShowRunLog("receive->TestAccept");
                                if (testAccept)
                                {
                                    httpHelper.SendData(client, "TestStart");
                                }
                            }
                            if (s.IndexOf("TestStart") >= 0)
                            {
                                UIHandleHelper.ShowRunLog("receive->TestStart");
                                myResetEvent.Set();
                                Thread.Sleep(0);
                            }
                        }
                        catch
                        {
                            //ShowMsg(ex.Message);
                            UIHandleHelper.ShowToolStripStatus_Multi("Multi Client: 正在等待服务端连接...");
                            break;
                        }
                    }
                }

                catch
                {
                    //ShowMsg(ex.Message);
                    UIHandleHelper.ShowToolStripStatus_Multi("Multi Client: 正在等待服务端连接...");
                    Thread.Sleep(2000);
                    //break;
                }
            }
        }

        #endregion


        private async void btn_Test_Click(object sender, EventArgs e)
        {
            await OnOK();

            #region 测试工具置顶

            this.TopMost = true;
            this.Activate();

            #endregion
        }

        private async void StartTest()
        {

            await OnOK();

            #region 测试工具置顶

            this.TopMost = true;
            this.Activate();

            #endregion
        }

        public bool TestInit()
        {
            //重置计时器
            StopAndResetTimer(1000);

            UIHandleHelper.ControlHandle(richTextBox_log, () => richTextBox_log.Text = "");

            #region 测试文件处理

            File.Delete(".\\UI_Log.txt");

            FileProcessHelper.DeleteFile(@$"{PathHelper.GetCurrentExeDirPath()}\SFCFile\", "*.txt", 100);

            #endregion

            UIHandleHelper.ShowRunLog("=============Init===============");

            #region 显示BD和Mac的关系

            if (this.macBD_Relation == MacBD_Relation.OnlyMac)
            {
                UIHandleHelper.ShowRunLog("The relationship between BD and Mac is only mac.");
                UIHandleHelper.ShowRunLog($"The length of the mac address is {CaseCodeBase.struct_TestVariable.iMACLength}.");
            }

            if (this.macBD_Relation == MacBD_Relation.PlusOne)
            {
                UIHandleHelper.ShowRunLog("The relationship between BD and Mac is plus one.");
                UIHandleHelper.ShowRunLog($"The length of the mac address is {CaseCodeBase.struct_TestVariable.iMACLength}.");
            }

            if (this.macBD_Relation == MacBD_Relation.Separate)
            {
                UIHandleHelper.ShowRunLog("The relationship between BD and Mac is separate.");
                UIHandleHelper.ShowRunLog($"The length of the wifi mac address is {CaseCodeBase.struct_TestVariable.iMACLength}.");
                UIHandleHelper.ShowRunLog($"The length of the bt mac address is {CaseCodeBase.struct_TestVariable.iBDLength}.");
            }

            if (this.sn_Relation == SN_Relation.SN)
            {
                UIHandleHelper.ShowRunLog($"The length of the sn is {CaseCodeBase.struct_TestVariable.iSNLength}.");
            }

            #endregion

            #region 设备资源初始化

            try
            {
                wop.ResourceInitialization();
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Instrument init error: {ex}");
                return false;
            }


            #endregion

            #region 配置串口

            try
            {
                wop.ConfigComPort();
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Instrument init error: {ex}");
                return false;
            }

            #endregion

            #region 检查耗材

            if (!testCountHelper.CheckTestCount(false))
            {
                UIHandleHelper.ShowRunLog("治具耗件已达到测试次数，请更换!", true);
                return false;
            }

            #endregion

            #region 处理控件

            UIHandleHelper.ControlHandle(label_Status, () => label_Status.Text = "TEST...");
            UIHandleHelper.ControlHandle(label_Status, () => label_Status.BackColor = testColor);
            UIHandleHelper.ControlHandle(dataGridView_item, () => dataGridView_item.Rows.Clear());

            if (struct_NormalINI.stru_b_DebugMode)
            {
                sRevDUTSN = "";
                sRevDUTMac = "";
                sRevDUTBD = "";
                CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac = "";
                CaseCodeBase.struct_Barcode.stru_str_sRevDUTSN = "";
                CaseCodeBase.struct_Barcode.stru_str_sRevDUTBD = "";

                if (this.macBD_Relation == MacBD_Relation.OnlyMac)
                {
                    CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac = sRevDUTMac = UIHandleHelper.GetControlText(textBox_Mac);
                }
                else if (this.macBD_Relation == MacBD_Relation.PlusOne)
                {
                    CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac = sRevDUTMac = UIHandleHelper.GetControlText(textBox_Mac);
                    CaseCodeBase.struct_Barcode.stru_str_sRevDUTBD = sRevDUTBD = UIHandleHelper.GetControlText(textBox_Mac);
                    UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Text = sRevDUTBD);
                }
                else if (this.macBD_Relation == MacBD_Relation.Separate)
                {
                    CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac = sRevDUTMac = UIHandleHelper.GetControlText(textBox_Mac);
                    CaseCodeBase.struct_Barcode.stru_str_sRevDUTBD = sRevDUTBD = UIHandleHelper.GetControlText(textBox_BT);
                }
                
                if (this.sn_Relation == SN_Relation.SN)
                {
                    CaseCodeBase.struct_Barcode.stru_str_sRevDUTSN = sRevDUTSN = UIHandleHelper.GetControlText(textBox_SN);
                }


                UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
                UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = false);
                UIHandleHelper.ControlHandle(btn_Config, () => btn_Config.Enabled = false);


            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
                UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = false);
                UIHandleHelper.ControlHandle(btn_Config, () => btn_Config.Enabled = false);

                UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Text = sRevDUTSN);
                UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Text = sRevDUTMac);
                UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Text = sRevDUTBD);
            }


            if (this.macBD_Relation == MacBD_Relation.PlusOne || this.macBD_Relation == MacBD_Relation.Separate)
            {
                UIHandleHelper.ShowRunLog($"Mac barcode ---> {sRevDUTMac}");
                UIHandleHelper.ShowRunLog($"BD barcode ---> {sRevDUTBD}");

                if (struct_NormalINI.stru_b_DebugMode)
                {
                    if (this.macBD_Relation == MacBD_Relation.PlusOne)
                    {
                        CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac =
                            sRevDUTMac = sRevDUTMac.Substring(CaseCodeBase.struct_TestVariable.iMAC_ExtractStartPosition, CaseCodeBase.struct_TestVariable.iMACLength);
                        CaseCodeBase.struct_Barcode.stru_str_sRevDUTBD = sRevDUTBD = macHelper.BTMacSet(sRevDUTMac);
                    }

                    if (this.macBD_Relation == MacBD_Relation.Separate)
                    {
                        CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac =
                            sRevDUTMac = sRevDUTMac.Substring(CaseCodeBase.struct_TestVariable.iMAC_ExtractStartPosition, CaseCodeBase.struct_TestVariable.iMACLength);
                        CaseCodeBase.struct_Barcode.stru_str_sRevDUTBD =
                            sRevDUTBD = sRevDUTBD.Substring(CaseCodeBase.struct_TestVariable.iBD_ExtractStartPosition, CaseCodeBase.struct_TestVariable.iBDLength);
                    }

                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Text = sRevDUTMac);
                    UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Text = sRevDUTBD);
                    UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = false);

                    UIHandleHelper.ShowRunLog($"Mac ---> {sRevDUTMac}");
                    UIHandleHelper.ShowRunLog($"BD ---> {sRevDUTBD}");
                }

                if (string.IsNullOrEmpty(sRevDUTMac) == true
               || sRevDUTMac.Length != CaseCodeBase.struct_TestVariable.iMACLength)
                {
                    UIHandleHelper.ShowRunLog("The Mac length is incorrect.", true);
                    return false;
                }

                if (string.IsNullOrEmpty(sRevDUTBD) == true
                   || sRevDUTBD.Length != CaseCodeBase.struct_TestVariable.iBDLength)
                {
                    UIHandleHelper.ShowRunLog("The BD length is incorrect.", true);
                    return false;
                }

            }

            if (this.macBD_Relation == MacBD_Relation.OnlyMac)
            {
                UIHandleHelper.ShowRunLog($"Mac barcode ---> {sRevDUTMac}");

                if (struct_NormalINI.stru_b_DebugMode)
                {
                    CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac =
                            sRevDUTMac = sRevDUTMac.Substring(CaseCodeBase.struct_TestVariable.iMAC_ExtractStartPosition, CaseCodeBase.struct_TestVariable.iMACLength);

                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Text = sRevDUTMac);
                    UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = false);

                    UIHandleHelper.ShowRunLog($"Mac ---> {sRevDUTMac}");
                }

                if (string.IsNullOrEmpty(sRevDUTMac) == true
                    || sRevDUTMac.Length != CaseCodeBase.struct_TestVariable.iMACLength)
                {
                    UIHandleHelper.ShowRunLog("The Mac length is incorrect.", true);
                    return false;
                }
            }



            if (this.sn_Relation == SN_Relation.SN)
            {
                UIHandleHelper.ShowRunLog($"SN barcode ---> {sRevDUTSN}");

                if (struct_NormalINI.stru_b_DebugMode)
                {
                    CaseCodeBase.struct_Barcode.stru_str_sRevDUTSN =
                            sRevDUTSN = sRevDUTSN.Substring(CaseCodeBase.struct_TestVariable.iSN_ExtractStartPosition, CaseCodeBase.struct_TestVariable.iSNLength);

                    UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Text = sRevDUTSN);
                    UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = false);

                    UIHandleHelper.ShowRunLog($"SN ---> {sRevDUTSN}");
                }

                if (string.IsNullOrEmpty(sRevDUTSN) == true
                    || sRevDUTSN.Length != CaseCodeBase.struct_TestVariable.iSNLength)
                {
                    UIHandleHelper.ShowRunLog("The SN length is incorrect.", true);
                    return false;
                }
            }

            #endregion

            #region 检查前六码

            if (struct_NormalINI.stru_b_CheckMacSix_Switch)
            {
                try
                {
                    string SixMac = "";

                    if (this.macBD_Relation == MacBD_Relation.OnlyMac
                        || this.macBD_Relation == MacBD_Relation.Separate
                        || this.macBD_Relation == MacBD_Relation.PlusOne)
                    {
                        SixMac = sRevDUTMac.Substring(0, 6);
                    }

                    if (SixMac != struct_NormalINI.stru_str_CheckMacID1
                        && SixMac != struct_NormalINI.stru_str_CheckMacID2
                        && SixMac != struct_NormalINI.stru_str_CheckMacID3)
                    {
                        UIHandleHelper.ShowRunLog("Check MAC Prefix fail!", true);
                        bSendSFC = false;
                        return false;
                    }
                }
                catch
                {
                    UIHandleHelper.ShowRunLog("Mac ID input Error!", true);
                    bSendSFC = false;
                    return false;
                }

            }

            #endregion

            #region 关闭屏蔽箱

            if (!OpenCloseShieldingBox(false))
            {
                return false;
            }

            #endregion

            #region 开卡

            #region 旧方法
            //if (struct_NormalINI.stru_b_IsOpenPEM)
            //{
            //    if (!OpenPEMDUT(true))
            //    {
            //        return false;
            //    }
            //}
            #endregion

            #region 新方法
            if (struct_NormalINI.stru_b_IsOpenPEM)
            {
                if (pEMController == null)
                {
                    pEMController = new PEMController();
                    pEMController.UILogMessage = UIHandleHelper.ShowRunLog; // 绑定日志输出方法

                    if (pEMController.Initialize() != true)
                    {
                        UIHandleHelper.ShowRunLog("初始化PEM卡失败!", true);
                        return false;
                    }
                }

                if (PEMPowerOn().Item1 == false)
                {
                    return false;
                }

                DevconHelper.Rescan();

                if (!String.IsNullOrEmpty(struct_NormalINI.stru_str_DeviceName1))
                {
                    DevconHelper.EnableByName(struct_NormalINI.stru_str_DeviceName1);
                }

                if (!String.IsNullOrEmpty(struct_NormalINI.stru_str_DeviceName2))
                {
                    DevconHelper.EnableByName(struct_NormalINI.stru_str_DeviceName2);
                }



            }
            #endregion

            #endregion


            return true;
        }




        public void StopAndResetTimer(double newInterval)
        {
            if (timer_Test != null)
            {
                elapsedSeconds = 0;
                timer_Test.Stop();
                // 设我们想要将时间间隔重置 毫秒
                timer_Test.Interval = newInterval;
                // 重新启动计时器
                timer_Test.Start();
            }
        }

        private void btn_Config_Click(object sender, EventArgs e)
        {
            //configForm = new();
            if (_MainForm._configForm != null)
            {
                configForm = _MainForm._configForm;
                configForm.VariableChanged += ConfigForm_VariableChanged;
                configForm.ShowDialog(testForm_Obj);
            }

        }


        /// <summary>
        /// Config UI点击确定后干的事
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigForm_VariableChanged(object sender, EventArgs e)
        {
            #region 记录之前某些变量的值, 用于比对是否需要作出响应

            bool Before_b_DebugMode = struct_NormalINI.stru_b_DebugMode;
            bool Before_b_OpenPEM = struct_NormalINI.stru_b_IsOpenPEM;
            int Before_i_ShieldingBoxCOM = struct_NormalINI.stru_i_ShieldingBoxCOM;

            #endregion

            #region 写INI和去除事件和隐藏Config界面

            wop.WriteNormalIni();
            configForm.VariableChanged -= ConfigForm_VariableChanged;
            configForm.Hide();

            #endregion

            #region 更新变量, <这个位置不能移动，否则后面会乱>

            struct_NormalINI = CaseCodeBase.struct_NormalINI;
            struct_EncryptINI = CaseCodeBase.struct_EncryptINI;

            #endregion

            #region 控件处理

            EnableAllButtons(this);

            UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = false);

            #endregion

            #region 屏蔽箱设定

            if (Before_i_ShieldingBoxCOM != struct_NormalINI.stru_i_ShieldingBoxCOM)
            {
                if (struct_NormalINI.stru_i_ShieldingBoxCOM != 0)
                {
                    if (shieldingHelper != null)
                    {
                        OpenCloseShieldingBox(true);
                    }
                    else
                    {
                        shieldingHelper = new ShieldingHelper();
                        if (!shieldingHelper.InitShieldingCom(struct_NormalINI.stru_i_ShieldingBoxCOM.ToString()))
                        {
                            MessageBoxEX.Show("初始化屏蔽箱串口失败", true);
                            System.Environment.Exit(System.Environment.ExitCode);
                            this.Dispose();
                            this.Close();

                        }
                        else
                        {
                            OpenCloseShieldingBox(true);
                        }
                    }

                }
            }

            #endregion

            #region mac和BD控件的处理

            if (struct_NormalINI.stru_b_DebugMode)
            {
 
                if (this.macBD_Relation == MacBD_Relation.OnlyMac || this.macBD_Relation == MacBD_Relation.PlusOne)
                {
                    //UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);
                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = true);
                    UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Focus());
                }

                if (this.macBD_Relation == MacBD_Relation.Separate)
                {
                    //UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);
                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = true);
                    UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = true);
                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Focus());
                }

                if (this.sn_Relation == SN_Relation.SN)
                {
                    UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = true);
                }
            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);
            }

            #endregion

            #region debug模式处理

            if (Before_b_DebugMode != struct_NormalINI.stru_b_DebugMode) //比对之前和现在的值是否有变化
            {
                if (struct_NormalINI.stru_b_DebugMode == false)  //需求使用手臂
                {
                    //先取消再运行
                    if (bUseRobotBefore)  //之前使用手臂
                    {
                        if (tokenSource != null)
                        {
                            tokenSource.Cancel();
                            Thread.Sleep(200);
                            tokenSource = null;
                            cancellationToken = CancellationToken.None;
                            //Robotsocket.Close();
                            Thread.Sleep(200);
                        }
                    }


                    tokenSource = new CancellationTokenSource();
                    cancellationToken = tokenSource.Token;
                    Thread thC = new Thread(() => ClientReceive(cancellationToken));
                    thC.IsBackground = true;
                    thC.Start();


                }
                else  //需求不使用手臂
                {
                    if (bUseRobotBefore) //之前使用手臂
                    {
                        if (tokenSource != null)
                        {
                            tokenSource.Cancel(); // 取消使用
                            tokenSource = null;
                            cancellationToken = CancellationToken.None;
                            //Robotsocket.Close();
                        }
                    }
                    else
                    {
                        UIHandleHelper.ShowToolStripStatus_Robot("手动测试模式");
                    }
                }
            }

            #endregion

            #region PEM处理

            if (Before_b_OpenPEM != struct_NormalINI.stru_b_IsOpenPEM)
            {
                if (struct_NormalINI.stru_b_IsOpenPEM)
                {
                    if (pEMController == null)
                    {
                        pEMController = new PEMController();
                        pEMController.UILogMessage = UIHandleHelper.ShowRunLog; // 绑定日志输出方法
                    }

                    if (pEMController.Initialize() != true)
                    {
                        MessageBoxEX.Show("初始化PEM卡失败!", true);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    if (pEMController == null)
                    {
                        pEMController.Exit();
                    }
                }
            }


            #endregion

        }



        //正式的测试流程
        public async Task OnOK()
        {
            await Task.Run(() =>
            {

                //调用 UIHandleHelper.InitializeTiming() 来启动全局计时器
                UIHandleHelper.InitializeTiming();

                this.TopMost = false;

                bFinishTest = false;
                bool bResult = false;

                try
                {
                    bResult = TestInit();

                    //TestInit的补充处理，正式测试前的处理
                    if (bResult && CallBack_TestInit is not null)
                    {
                        bResult = CallBack_TestInit();
                    }

                    if (bResult)
                    {
                        UIHandleHelper.ShowRunLog("Init",
                                                                  FailColor: false,
                                                                  ShowGridView: true,
                                                                  TestItemName: "Init",
                                                                  TestItemContent: "===",
                                                                  TestItemResult: true);
                    }
                    else
                    {
                        //UIHandleHelper.DataGridViewShow("Init", "===", false);
                        //初始化失败的不用存SFC
                        CaseCodeBase.struct_TestVariable.bNeedCreateSFCFile = false;
                    }

                    //测试内容的实现，包含 1：测试前的Check   2：测试主体    3：测试后的Check等
                    if (bResult && CallBack_TestFlow is not null)
                    {
                        bResult = CallBack_TestFlow();
                    }

                    //1：一些额外的操作，比如测试断电   2:保存除外挂运行的log以外的log   3:制作SFC Log
                    if (CallBack_TestEnd is not null)
                    {
                        bool bEndResult = false;
                        bEndResult = CallBack_TestEnd(bResult);
                        bResult = bResult && bEndResult;
                    }

                }
                catch (Exception ex)
                {
                    bResult = false;
                    UIHandleHelper.ShowRunLog($"Test process termination:\r\n{ex}", true);
                }
                finally
                {
                    if (CallBack_ResourceRelease is not null)
                    {
                        try
                        {
                            CallBack_ResourceRelease();
                        }
                        catch (Exception ex)
                        {
                            UIHandleHelper.ShowRunLog($"ResourceRelease err \r\n: {ex}");
                        }
                    }
                }


                //关卡
                #region PEM关卡

                //旧方法
                //if (struct_NormalINI.stru_b_IsOpenPEM)
                //{
                //    OpenPEMDUT(false);
                //}

                //新方法
                if (struct_NormalINI.stru_b_IsOpenPEM)
                {
                    if (!String.IsNullOrEmpty(struct_NormalINI.stru_str_DeviceName1))
                    {
                        DevconHelper.DisableByName(struct_NormalINI.stru_str_DeviceName1);
                    }

                    if (!String.IsNullOrEmpty(struct_NormalINI.stru_str_DeviceName2))
                    {
                        DevconHelper.DisableByName(struct_NormalINI.stru_str_DeviceName2);
                    }

                    PEMPowerOff();

                    DevconHelper.Rescan();
                }

                #endregion




                //打开屏蔽箱
                OpenCloseShieldingBox(true);

                //发送测试结果给Robot
                SendRobotResult(bResult);

                //保存测试外挂的log
                SaveLog(bResult);

                //测试完的控件处理
                TestEndControl(bResult);

                //测试完的变量复位
                VariableReset(bResult);



            });



        }


        public void SaveLog(bool bResult)
        {
            //记录测试时长
            string str_TestTime = UIHandleHelper.GetControlText(label_TestTime);
            UIHandleHelper.ShowRunLog(str_TestTime);
            double dblTestTime = double.Parse(str_TestTime.Substring(str_TestTime.IndexOf(":") + 1).Replace("s", ""));

            SaveLogProcess.SaveLog_Local(bResult, struct_NormalINI.stru_str_LogFilePath, struct_EncryptINI.stru_str_ProjectName, CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac, ".\\UI_Log.txt", "_UI.txt");
            if (struct_EncryptINI.stru_str_ProjectName.Contains("Sample") == false) //如果是Sample程式就不需要保存html文件
            {
                SaveLogProcess.SaveLog_Local(bResult, struct_NormalINI.stru_str_LogFilePath, struct_EncryptINI.stru_str_ProjectName, CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac, dataGridView_item, dblTestTime, "_UI.html");
            }
        }

        public void TestEndControl(bool bResult)
        {
            #region 测试结果显示

            if (bResult)
            {
                UIHandleHelper.ControlHandle(label_Status, () => label_Status.Text = "PASS");
                UIHandleHelper.ControlHandle(label_Status, () => label_Status.BackColor = succeedColor);
            }
            else
            {
                UIHandleHelper.ControlHandle(label_Status, () => label_Status.Text = "FAIL");
                UIHandleHelper.ControlHandle(label_Status, () => label_Status.BackColor = failColor);
            }

            #endregion

            #region Barcode控件显示

            if (struct_NormalINI.stru_b_DebugMode)
            {
                if (this.sn_Relation == SN_Relation.SN)
                {
                    UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = true);
                    UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Text = "");
                    UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Focus());
                }

                if (this.macBD_Relation == MacBD_Relation.OnlyMac || this.macBD_Relation == MacBD_Relation.PlusOne)
                {
                    UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
                    UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Text = "");

                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = true);
                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Text = "");
                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Focus());
                }
                else if (this.macBD_Relation == MacBD_Relation.Separate)
                {
                    UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = true);
                    UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Text = "");

                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = true);
                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Text = "");
                    UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Focus());
                }

            



                UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = false);
                UIHandleHelper.ControlHandle(btn_Config, () => btn_Config.Enabled = true);

            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = false);
                UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = false);
                UIHandleHelper.ControlHandle(btn_Config, () => btn_Config.Enabled = true);

                UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Text = "");
                UIHandleHelper.ControlHandle(textBox_Mac, () => textBox_Mac.Text = "");
                UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Text = "");
            }

            #endregion

            #region 测试统计显示更新和回写INI

            int iTestCount = 0;
            StringBuilder ValTemp = new StringBuilder();
            string str_CaseFolder = new string(this.str_CaseName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{this.str_CaseName}\\{this.str_CaseName}_Normal.ini";

            IniHelper.GetIniStr("TestTimes", "Fail", "null", ValTemp, 10, str_IniPath);
            int iTestTimes_Fail = int.Parse(ValTemp.ToString());

            IniHelper.GetIniStr("TestTimes", "Pass", "null", ValTemp, 10, str_IniPath);
            int iTestTimes_Pass = int.Parse(ValTemp.ToString());

            if (bResult)
            {
                iTestCount = iTestTimes_Pass + 1;
                UIHandleHelper.ControlHandle(label_PassNum, () => label_PassNum.Text = iTestCount.ToString());
                double result = ((double)iTestCount / (iTestCount + iTestTimes_Fail)) * 100;
                UIHandleHelper.ControlHandle(label_FPYNum, () => label_FPYNum.Text = result.ToString("F2") + " %");
                IniHelper.WriteIniStr("TestTimes", "Pass", iTestCount.ToString(), str_IniPath);
            }
            else
            {
                iTestCount = iTestTimes_Fail + 1;
                UIHandleHelper.ControlHandle(label_FailNum, () => label_FailNum.Text = iTestCount.ToString());
                double result = ((double)iTestTimes_Pass / (iTestCount + iTestTimes_Pass)) * 100;
                UIHandleHelper.ControlHandle(label_FPYNum, () => label_FPYNum.Text = result.ToString("F2") + " %");
                IniHelper.WriteIniStr("TestTimes", "Fail", iTestCount.ToString(), str_IniPath);
            }

            #endregion
        }

        public bool SendRobotResult(bool bResult)
        {
            String strMsg = "";

            if (bResult)
            {
                strMsg = struct_NormalINI.stru_i_TestCompNum.ToString() + "#Pass";
            }
            else
            {
                strMsg = struct_NormalINI.stru_i_TestCompNum.ToString() + "#Fail";
            }

            if (bIsConnected)
            {
                if (!httpHelper.SendData(Hostsocket, strMsg))
                {
                    UIHandleHelper.ShowRunLog("Send winsock test result data to robot fail.");
                    return false;
                }
            }

            return true;
        }

        public void VariableReset(bool bResult)
        {
            if (bResult)
            {
                bTestStatus = true;
            }
            else
            {
                bTestStatus = false;
            }

            bFinishTest = true;
            bFinishTestCom = false;
            bFinishTestMac = false;
            bFinishTestStu = false;

            sRevDUTSN = "";
            sRevDUTMac = "";
            sRevDUTBD = "";
            CaseCodeBase.struct_Barcode.stru_str_sRevDUTMac = "";
            CaseCodeBase.struct_Barcode.stru_str_sRevDUTSN = "";
            CaseCodeBase.struct_Barcode.stru_str_sRevDUTBD = "";

            if (timer_Test != null)
            {
                timer_Test.Stop();
            }

        }

        private void btn_log_Click(object sender, EventArgs e)
        {
            //通过 Windows Shell API 直接调用，比启动 explorer.exe 更快：
            string logPath = Path.Combine(struct_NormalINI.stru_str_LogFilePath, struct_EncryptINI.stru_str_ProjectName);

            if (Directory.Exists(logPath))
            {
                Type shellType = Type.GetTypeFromProgID("Shell.Application");
                dynamic shell = Activator.CreateInstance(shellType);
                shell.Open(logPath); // 直接通过Shell打开
            }
            else
            {
                MessageBoxEX.Show($"文件夹不存在：{logPath}", true);
            }
        }

        private async Task OpenLogFolder()
        {
            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(struct_NormalINI.stru_str_LogFilePath + "\\" + struct_EncryptINI.stru_str_ProjectName) && Directory.Exists(struct_NormalINI.stru_str_LogFilePath + "\\" + struct_EncryptINI.stru_str_ProjectName))
                {
                    Process.Start("explorer.exe", struct_NormalINI.stru_str_LogFilePath + "\\" + struct_EncryptINI.stru_str_ProjectName);
                }
                else
                {
                    MessageBoxEX.Show($"指定的文件夹路径无效或不存在！{struct_NormalINI.stru_str_LogFilePath + "\\" + struct_EncryptINI.stru_str_ProjectName}", true);
                }
            });
        }


        /// <summary>
        /// 用于关闭时执行的代码
        /// </summary>
        public void TestForm_FormClose()
        {

            if (CallBack_ResourceRelease is not null)
            {
                try
                {
                    CallBack_ResourceRelease();
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowRunLog($"ResourceRelease err \r\n: {ex}");
                }
            }


            if (pEMController != null)
            {
                pEMController.PowerOff();
                pEMController.Exit();
            }

            Multi_releaseStatus();

            ProcessHelper.KillProcessByName("zhiDE_CmdLine");
        }

        private void textBox_Mac_TextChanged(object sender, EventArgs e)
        {
            if (struct_NormalINI.stru_b_DebugMode)
            {
                if (textBox_Mac.Text.Length == CaseCodeBase.struct_TestVariable.iMAC_OriginalLength)
                {
                    if ( (this.macBD_Relation == MacBD_Relation.PlusOne || this.macBD_Relation == MacBD_Relation.OnlyMac) && this.sn_Relation == SN_Relation.None)
                    {
                        UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = true);
                        UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Focus());
                    }

                    if (this.macBD_Relation == MacBD_Relation.Separate)
                    {
                        UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Enabled = true);
                        UIHandleHelper.ControlHandle(textBox_BT, () => textBox_BT.Focus());
                    }

                    if (this.macBD_Relation != MacBD_Relation.Separate && this.sn_Relation == SN_Relation.SN)
                    {
                        UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = true);
                        UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Focus());
                    }
                }

           

            }
        }

        private void textBox_BT_TextChanged(object sender, EventArgs e)
        {
            if (struct_NormalINI.stru_b_DebugMode)
            {
                if (textBox_BT.Text.Length == CaseCodeBase.struct_TestVariable.iBD_OriginalLength)
                {
                    if ( (this.macBD_Relation == MacBD_Relation.PlusOne || this.macBD_Relation == MacBD_Relation.Separate) && this.sn_Relation != SN_Relation.SN)
                    {
                        UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = true);
                        UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Focus());
                    }

                    if ( (this.macBD_Relation == MacBD_Relation.PlusOne || this.macBD_Relation == MacBD_Relation.Separate) && this.sn_Relation == SN_Relation.SN)
                    {
                        UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Enabled = true);
                        UIHandleHelper.ControlHandle(textBox_SN, () => textBox_SN.Focus());
                    }

                }
            }
        }

        private void textBox_SN_TextChanged(object sender, EventArgs e)
        {
            if (struct_NormalINI.stru_b_DebugMode)
            {
                if (textBox_SN.Text.Length == CaseCodeBase.struct_TestVariable.iSN_OriginalLength)
                {
                    if (this.sn_Relation == SN_Relation.SN)
                    {
                        UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Enabled = true);
                        UIHandleHelper.ControlHandle(btn_Test, () => btn_Test.Focus());
                    }
                }
            }
        }



    }
}
