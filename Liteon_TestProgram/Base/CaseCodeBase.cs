using Liteon_TestProgram.Forms;
using Liteon_TestProgram.InstrumentControl;
using Liteon_TestProgram.Utilities;
using Liteon_TestProgram.Utilities.IOHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Liteon_TestProgram.Base
{
    internal class CaseCodeBase
    {
        public static Class_Variable.struct_NormalINI_Variable struct_NormalINI = new ();
        public static Class_Variable.struct_EncryptINI_Variable struct_EncryptINI = new();
        public static Class_Variable.struct_Barcode_Variable struct_Barcode = new();
        public static Class_Variable.MacBD_Relation macBD_Relation = new();
        public static Class_Variable.SN_Relation sn_Relation = new();
        public static Class_Variable.struct_Test_Variable struct_TestVariable = new();
        public TestForm testForm;
        public MainForm mianForm;
        public ProjectChangeForm projectChangeForm;
        public TextBox Tips_Title_TextBox;
        public TextBox Tips_TextBox;
        public Label ChangeList_Title_Label;
        public RichTextBox ChangeList_RichTextBox;
        public string str_CaseProjectName;

        [AllowNull]
        protected SerialPortHelper COM1;

        [AllowNull]
        protected SerialPortHelper COM2;

        [AllowNull]
        protected SerialPortHelper COM3;

        [AllowNull]
        protected SerialPortHelper COM4;

        [AllowNull]
        protected SerialPortHelper COM5;

        [AllowNull]
        protected SerialPortHelper COM6;

        [AllowNull]
        protected ITFMultimeter Multimeter1;

        [AllowNull]
        protected ITFMultimeter Multimeter2;

        [AllowNull]
        protected ITFMultimeter Multimeter3;

        [AllowNull]
        protected ITFPowerSupply PSU1;

        [AllowNull]
        protected ITFPowerSupply PSU2;

        [AllowNull]
        protected ITFPowerSupply PSU3;

        [AllowNull]
        protected SshHelper SshClient;

        [AllowNull]
        protected AdbProcess Adb;


        public SerialPortConfig serialPortConfig_1 = new();
        public SerialPortConfig serialPortConfig_2 = new();
        public SerialPortConfig serialPortConfig_3 = new();
        public SerialPortConfig serialPortConfig_4 = new();
        public SerialPortConfig serialPortConfig_5 = new();
        public SerialPortConfig serialPortConfig_6 = new();


        public CaseCodeBase wop;

        public CaseCodeBase(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
        {
            str_CaseProjectName = str_CaseProject;
            mianForm = mf;
            testForm = tf;
            projectChangeForm = pc;
            Tips_Title_TextBox = ((TextBox)mianForm.Controls.Find("textBox_Tips_Title", true)[0]);
            Tips_TextBox = ((TextBox)mianForm.Controls.Find("textBox_Tips_Content", true)[0]);

            ChangeList_Title_Label = ((Label)projectChangeForm.Controls.Find("label_ProjectName", true)[0]);
            ChangeList_RichTextBox = ((RichTextBox)projectChangeForm.Controls.Find("richTextBox_ChangeList", true)[0]);

            if (Tips_Title_TextBox.InvokeRequired)
            {
                Tips_Title_TextBox.Invoke(new Action(() => { Tips_Title_TextBox.Text = ""; }));
            }

            if (Tips_TextBox.InvokeRequired)
            {
                Tips_TextBox.Invoke(new Action(() => { Tips_TextBox.Text = ""; }));
            }

            if (ChangeList_Title_Label.InvokeRequired)
            {
                ChangeList_Title_Label.Invoke(new Action(() => { ChangeList_Title_Label.Text = ""; }));
            }

            if (ChangeList_RichTextBox.InvokeRequired)
            {
                ChangeList_RichTextBox.Invoke(new Action(() => { ChangeList_RichTextBox.Text = ""; }));
            }

            // ConfigComPort();

            #region 方法分配

            if (testForm.CallBack_TestPre != null)
            {
                testForm.CallBack_TestPre = null;
            }

            if (testForm.CallBack_TestInit != null)
            {
                testForm.CallBack_TestInit = null;
            }

            if (testForm.CallBack_TestFlow != null)
            {
                testForm.CallBack_TestFlow = null;
            }

            if (testForm.CallBack_TestEnd != null)
            {
                testForm.CallBack_TestEnd = null;
            }

            if (testForm.CallBack_TestMac != null)
            {
                testForm.CallBack_TestMac = null;
            }

            if (testForm.CallBack_ResourceRelease != null)
            {
                testForm.CallBack_ResourceRelease = null;
            }

            

            testForm.CallBack_TestMac = Func_TestMac;
            testForm.CallBack_TestPre = Func_TestPre;
            testForm.CallBack_TestInit = Func_TestInit;
            testForm.CallBack_TestFlow = Func_TestFlow;
            testForm.CallBack_TestEnd = Func_TestEnd;
            testForm.CallBack_ResourceRelease = Func_ResourceRelease;

            #endregion

        }

        public void ResourceInitialization()
        {
            #region 仪器1

            if (struct_NormalINI.stru_str_InstrumentName_1.Contains("Manual_Control") == false)
            {
                var obj = Create_Device_Object(struct_NormalINI.stru_str_InstrumentName_1, struct_NormalINI.stru_str_InstrumentAddr_1);
                Type[] interfaces = obj.GetType().GetInterfaces(); // 获取所有实现的接口
                foreach (Type interfaceType in interfaces)
                {
                    string interfaceName = interfaceType.Name; // 获取接口的名称
                    if (interfaceName == "ITFMultimeter")
                    {
                        Multimeter1 = (ITFMultimeter)obj;
                        break;
                    }

                    if (interfaceName == "ITFPowerSupply")
                    {
                        PSU1 = (ITFPowerSupply)obj;
                        break;
                    }
                }
            }

            #endregion

            #region 仪器2

            if (struct_NormalINI.stru_str_InstrumentName_2.Contains("Manual_Control") == false)
            {
                var obj = Create_Device_Object(struct_NormalINI.stru_str_InstrumentName_2, struct_NormalINI.stru_str_InstrumentAddr_2);
                Type[] interfaces = obj.GetType().GetInterfaces(); // 获取所有实现的接口
                foreach (Type interfaceType in interfaces)
                {
                    string interfaceName = interfaceType.Name; // 获取接口的名称
                    if (interfaceName == "ITFMultimeter")
                    {
                        Multimeter2 = (ITFMultimeter)obj;
                        break;
                    }

                    if (interfaceName == "ITFPowerSupply")
                    {
                        PSU2 = (ITFPowerSupply)obj;
                        break;
                    }
                }
            }

            #endregion

            #region 仪器3

            if (struct_NormalINI.stru_str_InstrumentName_3.Contains("Manual_Control") == false)
            {
                var obj = Create_Device_Object(struct_NormalINI.stru_str_InstrumentName_3, struct_NormalINI.stru_str_InstrumentAddr_3);
                Type[] interfaces = obj.GetType().GetInterfaces(); // 获取所有实现的接口
                foreach (Type interfaceType in interfaces)
                {
                    string interfaceName = interfaceType.Name; // 获取接口的名称
                    if (interfaceName == "ITFMultimeter")
                    {
                        Multimeter3 = (ITFMultimeter)obj;
                        break;
                    }

                    if (interfaceName == "ITFPowerSupply")
                    {
                        PSU3 = (ITFPowerSupply)obj;
                        break;
                    }
                }
            }

            #endregion

        }


        public void ConfigComPort()
        {
            serialPortConfig_1.PortName = struct_NormalINI.stru_str_PortName_1;
            serialPortConfig_1.BaudRate = struct_NormalINI.stru_i_BaudRate_1;
            serialPortConfig_1.DataBits = struct_NormalINI.stru_i_DataBits_1;
            serialPortConfig_1.StopBits = struct_NormalINI.stru_StopBits_1;
            serialPortConfig_1.Parity = struct_NormalINI.stru_Parity_1;
            if (serialPortConfig_1.PortName!= "COM0")
            {
                COM1 = new(serialPortConfig_1);
            }
          

            serialPortConfig_2.PortName = struct_NormalINI.stru_str_PortName_2;
            serialPortConfig_2.BaudRate = struct_NormalINI.stru_i_BaudRate_2;
            serialPortConfig_2.DataBits = struct_NormalINI.stru_i_DataBits_2;
            serialPortConfig_2.StopBits = struct_NormalINI.stru_StopBits_2;
            serialPortConfig_2.Parity = struct_NormalINI.stru_Parity_2;
            if (serialPortConfig_2.PortName != "COM0")
            {
                COM2 = new(serialPortConfig_2);
            }

            serialPortConfig_3.PortName = struct_NormalINI.stru_str_PortName_3;
            serialPortConfig_3.BaudRate = struct_NormalINI.stru_i_BaudRate_3;
            serialPortConfig_3.DataBits = struct_NormalINI.stru_i_DataBits_3;
            serialPortConfig_3.StopBits = struct_NormalINI.stru_StopBits_3;
            serialPortConfig_3.Parity = struct_NormalINI.stru_Parity_3;
            if (serialPortConfig_3.PortName != "COM0")
            {
                COM3 = new(serialPortConfig_3);
            }

            serialPortConfig_4.PortName = struct_NormalINI.stru_str_PortName_4;
            serialPortConfig_4.BaudRate = struct_NormalINI.stru_i_BaudRate_4;
            serialPortConfig_4.DataBits = struct_NormalINI.stru_i_DataBits_4;
            serialPortConfig_4.StopBits = struct_NormalINI.stru_StopBits_4;
            serialPortConfig_4.Parity = struct_NormalINI.stru_Parity_4;
            if (serialPortConfig_4.PortName != "COM0")
            {
                COM4 = new(serialPortConfig_4);
            }

            serialPortConfig_5.PortName = struct_NormalINI.stru_str_PortName_5;
            serialPortConfig_5.BaudRate = struct_NormalINI.stru_i_BaudRate_5;
            serialPortConfig_5.DataBits = struct_NormalINI.stru_i_DataBits_5;
            serialPortConfig_5.StopBits = struct_NormalINI.stru_StopBits_5;
            serialPortConfig_5.Parity = struct_NormalINI.stru_Parity_5;
            if (serialPortConfig_5.PortName != "COM0")
            {
                COM5 = new(serialPortConfig_5);
            }

            serialPortConfig_6.PortName = struct_NormalINI.stru_str_PortName_6;
            serialPortConfig_6.BaudRate = struct_NormalINI.stru_i_BaudRate_6;
            serialPortConfig_6.DataBits = struct_NormalINI.stru_i_DataBits_6;
            serialPortConfig_6.StopBits = struct_NormalINI.stru_StopBits_6;
            serialPortConfig_6.Parity = struct_NormalINI.stru_Parity_6;
            if (serialPortConfig_6.PortName != "COM0")
            {
                COM6 = new(serialPortConfig_6);
            }
        }

        public void ShowChangeListMessage(string Title, string[] Message)
        {
            // 更新标题
            if (ChangeList_Title_Label.InvokeRequired)
            {
                ChangeList_Title_Label.Invoke(new Action(() => { ChangeList_Title_Label.Text = Title; }));
            }
            else
            {
                ChangeList_Title_Label.Text = Title;
            }

            // 清空文本框
            if (ChangeList_RichTextBox.InvokeRequired)
            {
                ChangeList_RichTextBox.Invoke(new Action(() => { ChangeList_RichTextBox.Text = ""; }));
            }
            else
            {
                ChangeList_RichTextBox.Text = "";
            }

            // 添加内容并设置样式
            if (Message != null && Message.Length > 0)
            {
                for (int i = 0; i < Message.Length; i++)
                {
                    if (!string.IsNullOrEmpty(Message[i]))
                    {
                        string line = Message[i].Trim();

                        if (ChangeList_RichTextBox.InvokeRequired)
                        {
                            ChangeList_RichTextBox.Invoke(new Action(() =>
                            {
                                // 设置字体样式：第一行或V开头的行加大加粗
                                bool isImportantLine = (i == 0) || line.StartsWith("V", StringComparison.OrdinalIgnoreCase);

                                ChangeList_RichTextBox.SelectionFont = new Font(
                                    ChangeList_RichTextBox.Font.FontFamily,
                                    isImportantLine ? 14 : 9,  // 重要行14pt，普通行9pt
                                    isImportantLine ? FontStyle.Bold : FontStyle.Regular);

                                ChangeList_RichTextBox.AppendText(line + "\r\n");
                            }));
                        }
                        else
                        {
                            // 设置字体样式：第一行或V开头的行加大加粗
                            bool isImportantLine = (i == 0) || line.StartsWith("V", StringComparison.OrdinalIgnoreCase);

                            ChangeList_RichTextBox.SelectionFont = new Font(
                                ChangeList_RichTextBox.Font.FontFamily,
                                isImportantLine ? 14 : 9,  // 重要行14pt，普通行9pt
                                isImportantLine ? FontStyle.Bold : FontStyle.Regular);

                            ChangeList_RichTextBox.AppendText(line + "\r\n");
                        }
                    }
                }
            }
        }

        public void ShowTipsMessage(string Title, string[] Message)
        {
            if (Tips_Title_TextBox.InvokeRequired)
            {
                Tips_Title_TextBox.Invoke(new Action(() => { Tips_Title_TextBox.Text = Title; }));
            }
            else
            { 
                Tips_Title_TextBox.Text = Title;
            }

            if (Tips_TextBox.InvokeRequired)
            {
                Tips_TextBox.Invoke(new Action(() => { Tips_TextBox.Text = ""; }));
            }
            else
            {
                Tips_TextBox.Text = "";
            }

            for (int i = 0; i < Message.Length; i++)
            {
                if (Message[i] != null)
                {
                    if (Tips_TextBox.InvokeRequired)
                    {
                        Tips_TextBox.Invoke(new Action(() => { Tips_TextBox.AppendText(Message[i].Trim() + "\r\n"); }));
                    }
                    else
                    {
                        Tips_TextBox.AppendText(Message[i].Trim() + "\r\n");
                    }
                    
                }
            }
        }

        public static CaseCodeBase Create_Case_Object(MainForm mf, TestForm tf, ProjectChangeForm pc,string ClassName = "")
        {
            try
            {
                object obj = null;
                string TypeName = "Liteon_TestProgram.CaseProject." + ClassName;

                Type objType = Type.GetType(TypeName, false);//生成对应的类型
                if (objType != null)
                {
                    obj = Activator.CreateInstance(objType, new object[4] { mf, tf, pc, ClassName});//生成对应的对象  
                    return (CaseCodeBase)obj;//返回对象
                }
                return null;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog(ex.ToString());
                return null;
            }
        }

        public static Object Create_Device_Object(string DeviceName, string str_Addr)
        {
            try
            {
                object obj = null;
                string TypeName = "Liteon_TestProgram.InstrumentControl." + DeviceName;

                Type objType = Type.GetType(TypeName, false);//生成对应的类型
                if (objType != null)
                {
                    obj = Activator.CreateInstance(objType, new object[1] { str_Addr});//生成对应的对象  
                    return obj;//返回对象
                }
                return null;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog(ex.ToString());
                return null;
            }
        }




        /// <summary>
        /// 用于释放资源，比如仪器控制等
        /// </summary>
        public virtual void Func_ResourceRelease()
        {

        }


        /// <summary>
        /// 用于设定Mac和BT的关系，是否加一, 以及长度
        /// </summary>
        public virtual void Func_TestMac()
        {
            MessageBoxEX.Show("请在派生类中重写Func_TestMac方法,\r\n 设置Mac和BD的长度和关系.", true);
            Environment.Exit(0);
        }


        /// <summary>
        /// 用于增加在打开Form的时候初始化加载一些设定和检查的补充；
        /// 以下为不需要再增加的方法：读取普通和加密的INI和检测硬盘剩余空间；
        /// </summary>
        /// <returns></returns>
        public virtual bool Func_TestPre()
        {
            return false;
        }

        /// <summary>
        /// 用于对点击测试开始后的测试的初始化的补充
        /// </summary>
        /// <returns></returns>
        public virtual bool Func_TestInit()
        {
            return false;
        }

        /// <summary>
        /// 用于测试主体，主要包含测试前的Check，测试RF和测试后的Check
        /// </summary>
        /// <returns></returns>
        public virtual bool Func_TestFlow()
        {
            return false;
        }

        /// <summary>
        /// 用于需要补充的实现： 1:保存除外挂运行的log以外的log  2:制作SFC Log 3: 增加一些断电的操作；
        /// 以下为不需要再增加的方法：保存外挂运行的log的log；开关屏蔽箱；
        /// 参数：传递的是测试结果
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Func_TestEnd(bool bTestResult)
        {
            return false;
        }


        public virtual List<string> CreateTestStringList()
        {
            List<string> list_str = new List<string>();
            list_str.Add("null\r\n");
            return list_str;
        }


        public virtual List<string> CreateChangeStringList()
        {
            List<string> list_str = new List<string>();
            list_str.Add("null\r\n");
            return list_str;
        }






        #region 读取INI

        public bool ReadNormalIni()
        {
            StringBuilder ValTemp = new StringBuilder();
            string str_CaseFolder = new string(str_CaseProjectName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{str_CaseProjectName}\\{str_CaseProjectName}_Normal.ini";

            try
            {

                if (File.Exists(str_IniPath) == false)
                {
                    MessageBoxEX.Show($"{str_CaseProjectName}_Normal.ini文件不存在。", true);
                    throw new ArgumentException($"{str_CaseProjectName}_Normal.ini文件不存在。");
                }

                #region [Barcode]

                IniHelper.GetIniStr("Barcode", "CheckMacSix_Switch", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_CheckMacSix_Switch = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("Barcode", "CheckMacID1", "null", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_CheckMacID1 = ValTemp.ToString();

                IniHelper.GetIniStr("Barcode", "CheckMacID2", "null", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_CheckMacID2 = ValTemp.ToString();

                IniHelper.GetIniStr("Barcode", "CheckMacID3", "null", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_CheckMacID3 = ValTemp.ToString();


                #endregion

                #region [ShieldingBox]

                IniHelper.GetIniStr("ShieldingBox", "ShieldingBoxCOM", "0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_ShieldingBoxCOM = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ShieldingBox", "SleepCycleWhenOpen", "3000", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_SleepCycleWhenOpen = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ShieldingBox", "SleepCycleWhenClose", "3000", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_SleepCycleWhenClose = int.Parse(ValTemp.ToString());

                #endregion

                #region [PEM]

                IniHelper.GetIniStr("PEM", "IsOpenPEM", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_IsOpenPEM = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("PEM", "DeviceName1", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_DeviceName1 = ValTemp.ToString();

                IniHelper.GetIniStr("PEM", "DeviceName2", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_DeviceName2 = ValTemp.ToString();

                #endregion

                #region [Path]

                IniHelper.GetIniStr("Path", "SFCFilePath", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_SFCFilePath = ValTemp.ToString();

                IniHelper.GetIniStr("Path", "LogFilePath", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_LogFilePath = ValTemp.ToString();

                IniHelper.GetIniStr("Path", "FreeSpaceLimit", "10", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_FreeSpaceLimit = int.Parse(ValTemp.ToString());


                #endregion

                #region [TCP_IP]

                IniHelper.GetIniStr("TCP_IP", "HostIP", "null", ValTemp, 50, str_IniPath);
                struct_NormalINI.stru_str_RobotClientHostIP = ValTemp.ToString();

                IniHelper.GetIniStr("TCP_IP", "HostPort", "null", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_RobotClientHostPort = ValTemp.ToString();

                IniHelper.GetIniStr("TCP_IP", "TestCompNum", "0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_TestCompNum = int.Parse(ValTemp.ToString());

                #endregion

                #region [Multi_DUT]

                IniHelper.GetIniStr("Multi_DUT", "Multi_Switch", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_Multi_Switch = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("Multi_DUT", "SelectMode", "0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_SelectMode = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("Multi_DUT", "Multi_Server_IP", "null", ValTemp, 50, str_IniPath);
                struct_NormalINI.stru_str_Multi_Server_IP = ValTemp.ToString();

                IniHelper.GetIniStr("Multi_DUT", "Multi_Server_Port", "null", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_Multi_Server_Port = ValTemp.ToString();

                #endregion

                #region [TesterPort]

                IniHelper.GetIniStr("TesterPort", "PortName", "null", ValTemp, 50, str_IniPath);
                struct_NormalINI.stru_str_TesterPort = ValTemp.ToString();

                #endregion

                #region [ComPort_1]

                IniHelper.GetIniStr("ComPort_1", "PortName", "COM0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_PortName_1 = ValTemp.ToString();

                IniHelper.GetIniStr("ComPort_1", "BaudRate", "9600", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_BaudRate_1 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_1", "DataBits", "8", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_DataBits_1 = int.Parse(ValTemp.ToString());


                IniHelper.GetIniStr("ComPort_1", "StopBits", "One", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out StopBits stopBitsValue))
                {
                    struct_NormalINI.stru_StopBits_1 = stopBitsValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_1 StopBits设置。");
                }

                IniHelper.GetIniStr("ComPort_1", "Parity", "None", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out Parity parityValue))
                {
                    struct_NormalINI.stru_Parity_1 = parityValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_1 Parity设置。");
                }

                IniHelper.GetIniStr("ComPort_1", "RtsEnable", "0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_RtsEnable_1 = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("ComPort_1", "DtrEnable", "0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_DtrEnable_1 = (ValTemp.ToString() == "1" ? true : false);

                #endregion

                #region [ComPort_2]

                IniHelper.GetIniStr("ComPort_2", "PortName", "COM0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_PortName_2 = ValTemp.ToString();

                IniHelper.GetIniStr("ComPort_2", "BaudRate", "9600", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_BaudRate_2 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_2", "DataBits", "8", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_DataBits_2 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_2", "StopBits", "One", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out stopBitsValue))
                {
                    struct_NormalINI.stru_StopBits_2 = stopBitsValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_2 StopBits设置。");
                }

                IniHelper.GetIniStr("ComPort_2", "Parity", "None", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out parityValue))
                {
                    struct_NormalINI.stru_Parity_2 = parityValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_2 Parity设置。");
                }

                IniHelper.GetIniStr("ComPort_2", "RtsEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_RtsEnable_2 = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("ComPort_2", "DtrEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_DtrEnable_2 = (ValTemp.ToString() == "1" ? true : false);

                #endregion

                #region [ComPort_3]

                IniHelper.GetIniStr("ComPort_3", "PortName", "COM0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_PortName_3 = ValTemp.ToString();

                IniHelper.GetIniStr("ComPort_3", "BaudRate", "9600", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_BaudRate_3 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_3", "DataBits", "8", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_DataBits_3 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_3", "StopBits", "One", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out stopBitsValue))
                {
                    struct_NormalINI.stru_StopBits_3 = stopBitsValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_3 StopBits设置。");
                }

                IniHelper.GetIniStr("ComPort_3", "Parity", "None", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out parityValue))
                {
                    struct_NormalINI.stru_Parity_3 = parityValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_3 Parity设置。");
                }

                IniHelper.GetIniStr("ComPort_3", "RtsEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_RtsEnable_3 = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("ComPort_3", "DtrEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_DtrEnable_3 = (ValTemp.ToString() == "1" ? true : false);

                #endregion

                #region [ComPort_4]

                IniHelper.GetIniStr("ComPort_4", "PortName", "COM0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_PortName_4 = ValTemp.ToString();

                IniHelper.GetIniStr("ComPort_4", "BaudRate", "9600", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_BaudRate_4 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_4", "DataBits", "8", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_DataBits_4 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_4", "StopBits", "One", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out stopBitsValue))
                {
                    struct_NormalINI.stru_StopBits_4 = stopBitsValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_4 StopBits设置。");
                }


                IniHelper.GetIniStr("ComPort_4", "Parity", "None", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out parityValue))
                {
                    struct_NormalINI.stru_Parity_4 = parityValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_4 Parity设置。");
                }

                IniHelper.GetIniStr("ComPort_4", "RtsEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_RtsEnable_4 = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("ComPort_4", "DtrEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_DtrEnable_4 = (ValTemp.ToString() == "1" ? true : false);

                #endregion

                #region [ComPort_5]

                IniHelper.GetIniStr("ComPort_5", "PortName", "COM0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_PortName_5 = ValTemp.ToString();

                IniHelper.GetIniStr("ComPort_5", "BaudRate", "9600", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_BaudRate_5 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_5", "DataBits", "8", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_DataBits_5 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_5", "StopBits", "One", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out stopBitsValue))
                {
                    struct_NormalINI.stru_StopBits_5 = stopBitsValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_5 StopBits设置。");
                }

                IniHelper.GetIniStr("ComPort_5", "Parity", "None", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out parityValue))
                {
                    struct_NormalINI.stru_Parity_5 = parityValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_5 Parity设置。");
                }

                IniHelper.GetIniStr("ComPort_5", "RtsEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_RtsEnable_5 = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("ComPort_5", "DtrEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_DtrEnable_5 = (ValTemp.ToString() == "1" ? true : false);

                #endregion

                #region [ComPort_6]

                IniHelper.GetIniStr("ComPort_6", "PortName", "COM0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_str_PortName_6 = ValTemp.ToString();

                IniHelper.GetIniStr("ComPort_6", "BaudRate", "9600", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_BaudRate_6 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_6", "DataBits", "8", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_DataBits_6 = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("ComPort_6", "StopBits", "One", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out stopBitsValue))
                {
                    struct_NormalINI.stru_StopBits_6 = stopBitsValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_6 StopBits设置。");
                }

                IniHelper.GetIniStr("ComPort_6", "Parity", "None", ValTemp, 10, str_IniPath);
                if (Enum.TryParse(ValTemp.ToString(), true, out parityValue))
                {
                    struct_NormalINI.stru_Parity_6 = parityValue;
                }
                else
                {
                    // 如果转换失败，可以抛出异常或返回默认值
                    throw new InvalidOperationException("无法从INI文件中读取ComPort_6 Parity设置。");
                }

                IniHelper.GetIniStr("ComPort_6", "RtsEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_RtsEnable_6 = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("ComPort_6", "DtrEnable", "1", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_DtrEnable_6 = (ValTemp.ToString() == "1" ? true : false);

                #endregion

                #region [InstrumentControl]

                IniHelper.GetIniStr("InstrumentControl", "InstrumentName_1", "Manual_Control", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_InstrumentName_1 = ValTemp.ToString();

                IniHelper.GetIniStr("InstrumentControl", "InstrumentName_2", "Manual_Control", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_InstrumentName_2 = ValTemp.ToString();

                IniHelper.GetIniStr("InstrumentControl", "InstrumentName_3", "Manual_Control", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_InstrumentName_3 = ValTemp.ToString();

                IniHelper.GetIniStr("InstrumentControl", "InstrumentAddr_1", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_InstrumentAddr_1 = ValTemp.ToString();

                IniHelper.GetIniStr("InstrumentControl", "InstrumentAddr_2", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_InstrumentAddr_2 = ValTemp.ToString();

                IniHelper.GetIniStr("InstrumentControl", "InstrumentAddr_3", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_InstrumentAddr_3 = ValTemp.ToString();

                #endregion

                #region [Mode]

                IniHelper.GetIniStr("Mode", "DebugMode", "0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_DebugMode = (ValTemp.ToString() == "1" ? true : false);

                #endregion

                #region [ServerLog]

                IniHelper.GetIniStr("ServerLog", "OpenSwitch", "0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_LogServerSwitch = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("ServerLog", "ServerUser", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_LogServerUser = ValTemp.ToString();

                IniHelper.GetIniStr("ServerLog", "ServerPassword", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_LogServerPassword = ValTemp.ToString();

                IniHelper.GetIniStr("ServerLog", "ServerPath", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_LogServerPath = ValTemp.ToString();

                #endregion

                #region [FTP]

                IniHelper.GetIniStr("FTP", "Switch", "0", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_b_FTPSwitch = (ValTemp.ToString() == "1" ? true : false);

                IniHelper.GetIniStr("FTP", "IP", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_FTP_IP = ValTemp.ToString();

                IniHelper.GetIniStr("FTP", "Port", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_i_FTP_Port = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("FTP", "User", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_FTP_User = ValTemp.ToString();

                IniHelper.GetIniStr("FTP", "Password", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_FTP_Password = ValTemp.ToString();

                IniHelper.GetIniStr("FTP", "SavePath", "null", ValTemp, 100, str_IniPath);
                struct_NormalINI.stru_str_FTP_Path = ValTemp.ToString();

                #endregion

                #region [TestTimes]

                IniHelper.GetIniStr("TestTimes", "Fail", "null", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_TestTimes_Fail = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("TestTimes", "Pass", "null", ValTemp, 10, str_IniPath);
                struct_NormalINI.stru_i_TestTimes_Pass = int.Parse(ValTemp.ToString());

                #endregion


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取{str_CaseProjectName} ini failed:{ex.StackTrace}");
                UIHandleHelper.ShowRunLog($"读取{str_CaseProjectName} ini failed:{ex.StackTrace}", false);
                return false;
            }


            return true;
        }

        public bool ReadEncryptIni()
        {
            StringBuilder ValTemp = new StringBuilder();
            string str_CaseFolder = new string(str_CaseProjectName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{str_CaseProjectName}\\{str_CaseProjectName}_Encrypt.ini";

            try
            {
                if (File.Exists(str_IniPath) == false)
                {
                    throw new ArgumentException($"{str_CaseProjectName}_Encrypt.ini文件不存在。");
                }

                #region [Model]

                IniHelper.GetIniStr("Model", "ProjectName", "null", ValTemp, 100, str_IniPath);
                struct_EncryptINI.stru_str_ProjectName = ValTemp.ToString();

                IniHelper.GetIniStr("Model", "Version", "null", ValTemp, 50, str_IniPath);
                struct_EncryptINI.stru_str_CaseVersion = ValTemp.ToString();

                IniHelper.GetIniStr("Model", "SFCNumber", "null", ValTemp, 10, str_IniPath);
                struct_EncryptINI.stru_str_SFCNumber = ValTemp.ToString();

                #endregion

                #region [MD5_INFO]

                IniHelper.GetIniStr("MD5_INFO", "MD5_INFO", "null", ValTemp, 50, str_IniPath);
                struct_EncryptINI.stru_str_MD5_INFO = ValTemp.ToString();

                MD5Helper mD5Helper = new MD5Helper();
                if (!mD5Helper.CheckEncryptMd5(str_IniPath, "MD5_INFO", struct_EncryptINI.stru_str_MD5_INFO))
                {
                    UIHandleHelper.ShowRunLog($"检查机种{str_CaseProjectName}加密文件的MD5失败", true);
                    return false;
                }


                IniHelper.GetIniStr("MD5_INFO", "Check_EXE", "0", ValTemp, 50, str_IniPath);
                if (int.Parse(ValTemp.ToString()) != 0 )
                {
                    //检查exe的md5和加密档中的exe的md5
                    IniHelper.GetIniStr("MD5_INFO", "EXE_MD5", "null", ValTemp, 50, str_IniPath);
                    string md5Hash = MD5Helper.CalculateMD5($"{FileProcessHelper.GetCurrentExeDirectory() + "\\" + Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)}");
                    MessageBox.Show($"{FileProcessHelper.GetCurrentExeDirectory() + Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)}");
                    MessageBox.Show(md5Hash);
                    if (md5Hash != ValTemp.ToString())
                    {
                        UIHandleHelper.ShowRunLog($"检查机种{str_CaseProjectName}程式exe的MD5失败, 请确认当前的exe是否使用正确！", true);
                        return false;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"检查机种{str_CaseProjectName}程式exe的MD5 Pass！", false);
                    }

                }
                else
                {
                    UIHandleHelper.ShowRunLog($"无需检查机种{str_CaseProjectName}程式exe的MD5！", true);
                }

                #endregion


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取{str_CaseProjectName} ini failed:{ex.StackTrace}");
                UIHandleHelper.ShowRunLog($"读取{str_CaseProjectName} ini failed:{ex.StackTrace}", false);
                return false;
            }

            return true;
        }

        #endregion

        #region 提供一个可以循环测试的方法

        /// <summary>
        /// 提供一个可以循环测试的方法
        /// </summary>
        /// <param name="func"></param>
        /// <param name="number"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        protected static bool RepeatExecute(Func<bool> func, int number, int interval)
        {
            for (int n = 0; n < number; n++)
            {
                if (func())
                {
                    return true;
                }

                Task.Delay(interval).Wait();
            }

            return false;
        }

        protected static bool RepeatExecute(Func<object[], bool> func, object[] args, int number, int interval)
        {
            for (int n = 0; n < number; n++)
            {
                if (func(args))
                {
                    return true;
                }

                Task.Delay(interval).Wait();
            }

            return false;
        }

        protected static string RepeatExecute(Func<List<byte[]>, string> func, List<byte[]> b1, int number, int interval)
        {
            for (int n = 0; n < number; n++)
            {
                string str_recv = func(b1);
                if (str_recv != "")
                {
                    return str_recv;
                }

                Task.Delay(interval).Wait();
            }

            return "";
        }

        protected static bool RepeatExecute<T>(Func<T, bool> func, T arg, int number, int interval)
        {
            for (int n = 0; n < number; n++)
            {
                if (func(arg))
                {
                    return true;
                }

                Task.Delay(interval).Wait();
            }

            return false;
        }

        protected static bool RepeatExecute<T1, T2>(Func<T1, T2, bool> func, T1 arg1, T2 arg2, int number, int interval)
        {
            for (int n = 0; n < number; n++)
            {
                if (func(arg1, arg2))
                {
                    return true;
                }

                Task.Delay(interval).Wait();
            }

            return false;
        }

        protected static bool RepeatExecute<T1, T2, T3>(Func<T1, T2, T3, bool> func, T1 arg1, T2 arg2, T3 arg3, int number, int interval)
        {
            for (int n = 0; n < number; n++)
            {
                if (func(arg1, arg2, arg3))
                {
                    return true;
                }

                Task.Delay(interval).Wait();
            }

            return false;
        }


        #endregion

        #region 写入INI

        public bool WriteNormalIni()
        {
            string str_Tmp = "";
            string str_CaseFolder = new string(str_CaseProjectName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{str_CaseProjectName}\\{str_CaseProjectName}_Normal.ini";

            try
            {

                if (File.Exists(str_IniPath) == false)
                {
                    throw new ArgumentException($"{str_CaseProjectName}_Normal.ini文件不存在。");
                }

                #region [Barcode]

                str_Tmp = struct_NormalINI.stru_b_CheckMacSix_Switch ? "1" : "0";
                IniHelper.WriteIniStr("Barcode", "CheckMacSix_Switch", str_Tmp, str_IniPath);
                IniHelper.WriteIniStr("Barcode", "CheckMacID1", struct_NormalINI.stru_str_CheckMacID1, str_IniPath);
                IniHelper.WriteIniStr("Barcode", "CheckMacID2", struct_NormalINI.stru_str_CheckMacID2, str_IniPath);
                IniHelper.WriteIniStr("Barcode", "CheckMacID3", struct_NormalINI.stru_str_CheckMacID3, str_IniPath);

                #endregion

                #region [ShieldingBox]

                IniHelper.WriteIniStr("ShieldingBox", "ShieldingBoxCOM", struct_NormalINI.stru_i_ShieldingBoxCOM.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ShieldingBox", "SleepCycleWhenOpen", struct_NormalINI.stru_i_SleepCycleWhenOpen.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ShieldingBox", "SleepCycleWhenClose", struct_NormalINI.stru_i_SleepCycleWhenClose.ToString(), str_IniPath);

                #endregion

                #region [PEM]

                str_Tmp = struct_NormalINI.stru_b_IsOpenPEM ? "1" : "0";
                IniHelper.WriteIniStr("PEM", "IsOpenPEM", str_Tmp, str_IniPath);
                IniHelper.WriteIniStr("PEM", "DeviceName1", struct_NormalINI.stru_str_DeviceName1, str_IniPath);
                IniHelper.WriteIniStr("PEM", "DeviceName2", struct_NormalINI.stru_str_DeviceName2, str_IniPath);

                #endregion

                #region [Path]

                IniHelper.WriteIniStr("Path", "SFCFilePath", struct_NormalINI.stru_str_SFCFilePath, str_IniPath);
                IniHelper.WriteIniStr("Path", "LogFilePath", struct_NormalINI.stru_str_LogFilePath, str_IniPath);
                IniHelper.WriteIniStr("Path", "FreeSpaceLimit", struct_NormalINI.stru_i_FreeSpaceLimit.ToString(), str_IniPath);

                #endregion

                #region [TCP_IP]

                IniHelper.WriteIniStr("TCP_IP", "HostIP", struct_NormalINI.stru_str_RobotClientHostIP, str_IniPath);
                IniHelper.WriteIniStr("TCP_IP", "HostPort", struct_NormalINI.stru_str_RobotClientHostPort, str_IniPath);
                IniHelper.WriteIniStr("TCP_IP", "TestCompNum", struct_NormalINI.stru_i_TestCompNum.ToString(), str_IniPath);

                #endregion

                #region [Multi_DUT]

                str_Tmp = struct_NormalINI.stru_b_Multi_Switch ? "1" : "0";
                IniHelper.WriteIniStr("Multi_DUT", "Multi_Switch", str_Tmp, str_IniPath);
                IniHelper.WriteIniStr("Multi_DUT", "SelectMode", struct_NormalINI.stru_i_SelectMode.ToString(), str_IniPath);
                IniHelper.WriteIniStr("Multi_DUT", "Multi_Server_IP", struct_NormalINI.stru_str_Multi_Server_IP, str_IniPath);
                IniHelper.WriteIniStr("Multi_DUT", "Multi_Server_Port", struct_NormalINI.stru_str_Multi_Server_Port, str_IniPath);

                #endregion

                #region [TesterPort]

                IniHelper.WriteIniStr("TesterPort", "PortName", struct_NormalINI.stru_str_TesterPort, str_IniPath);

                #endregion

                #region [ComPort_1]

                IniHelper.WriteIniStr("ComPort_1", "PortName", struct_NormalINI.stru_str_PortName_1, str_IniPath);
                IniHelper.WriteIniStr("ComPort_1", "BaudRate", struct_NormalINI.stru_i_BaudRate_1.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_1", "DataBits", struct_NormalINI.stru_i_DataBits_1.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_1", "StopBits", struct_NormalINI.stru_StopBits_1.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_1", "Parity", struct_NormalINI.stru_Parity_1.ToString(), str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_RtsEnable_1 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_1", "RtsEnable", str_Tmp, str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_DtrEnable_1 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_1", "DtrEnable", str_Tmp, str_IniPath);

                #endregion

                #region [ComPort_2]

                IniHelper.WriteIniStr("ComPort_2", "PortName", struct_NormalINI.stru_str_PortName_2, str_IniPath);
                IniHelper.WriteIniStr("ComPort_2", "BaudRate", struct_NormalINI.stru_i_BaudRate_2.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_2", "DataBits", struct_NormalINI.stru_i_DataBits_2.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_2", "StopBits", struct_NormalINI.stru_StopBits_2.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_2", "Parity", struct_NormalINI.stru_Parity_2.ToString(), str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_RtsEnable_2 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_2", "RtsEnable", str_Tmp, str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_DtrEnable_2 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_2", "DtrEnable", str_Tmp, str_IniPath);

                #endregion

                #region [ComPort_3]

                IniHelper.WriteIniStr("ComPort_3", "PortName", struct_NormalINI.stru_str_PortName_3, str_IniPath);
                IniHelper.WriteIniStr("ComPort_3", "BaudRate", struct_NormalINI.stru_i_BaudRate_3.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_3", "DataBits", struct_NormalINI.stru_i_DataBits_3.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_3", "StopBits", struct_NormalINI.stru_StopBits_3.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_3", "Parity", struct_NormalINI.stru_Parity_3.ToString(), str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_RtsEnable_3 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_3", "RtsEnable", str_Tmp, str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_DtrEnable_3 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_3", "DtrEnable", str_Tmp, str_IniPath);

                #endregion

                #region [ComPort_4]

                IniHelper.WriteIniStr("ComPort_4", "PortName", struct_NormalINI.stru_str_PortName_4, str_IniPath);
                IniHelper.WriteIniStr("ComPort_4", "BaudRate", struct_NormalINI.stru_i_BaudRate_4.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_4", "DataBits", struct_NormalINI.stru_i_DataBits_4.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_4", "StopBits", struct_NormalINI.stru_StopBits_4.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_4", "Parity", struct_NormalINI.stru_Parity_4.ToString(), str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_RtsEnable_4 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_4", "RtsEnable", str_Tmp, str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_DtrEnable_4 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_4", "DtrEnable", str_Tmp, str_IniPath);

                #endregion

                #region [ComPort_5]

                IniHelper.WriteIniStr("ComPort_5", "PortName", struct_NormalINI.stru_str_PortName_5, str_IniPath);
                IniHelper.WriteIniStr("ComPort_5", "BaudRate", struct_NormalINI.stru_i_BaudRate_5.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_5", "DataBits", struct_NormalINI.stru_i_DataBits_5.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_5", "StopBits", struct_NormalINI.stru_StopBits_5.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_5", "Parity", struct_NormalINI.stru_Parity_5.ToString(), str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_RtsEnable_5 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_5", "RtsEnable", str_Tmp, str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_DtrEnable_5 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_5", "DtrEnable", str_Tmp, str_IniPath);

                #endregion

                #region [ComPort_6]

                IniHelper.WriteIniStr("ComPort_6", "PortName", struct_NormalINI.stru_str_PortName_6, str_IniPath);
                IniHelper.WriteIniStr("ComPort_6", "BaudRate", struct_NormalINI.stru_i_BaudRate_6.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_6", "DataBits", struct_NormalINI.stru_i_DataBits_6.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_6", "StopBits", struct_NormalINI.stru_StopBits_6.ToString(), str_IniPath);
                IniHelper.WriteIniStr("ComPort_6", "Parity", struct_NormalINI.stru_Parity_6.ToString(), str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_RtsEnable_6 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_6", "RtsEnable", str_Tmp, str_IniPath);
                str_Tmp = struct_NormalINI.stru_b_DtrEnable_6 ? "1" : "0";
                IniHelper.WriteIniStr("ComPort_6", "DtrEnable", str_Tmp, str_IniPath);

                #endregion

                #region [InstrumentControl]

                IniHelper.WriteIniStr("InstrumentControl", "InstrumentName_1", struct_NormalINI.stru_str_InstrumentName_1, str_IniPath);
                IniHelper.WriteIniStr("InstrumentControl", "InstrumentName_2", struct_NormalINI.stru_str_InstrumentName_2, str_IniPath);
                IniHelper.WriteIniStr("InstrumentControl", "InstrumentName_3", struct_NormalINI.stru_str_InstrumentName_3, str_IniPath);
                IniHelper.WriteIniStr("InstrumentControl", "InstrumentAddr_1", struct_NormalINI.stru_str_InstrumentAddr_1, str_IniPath);
                IniHelper.WriteIniStr("InstrumentControl", "InstrumentAddr_2", struct_NormalINI.stru_str_InstrumentAddr_2, str_IniPath);
                IniHelper.WriteIniStr("InstrumentControl", "InstrumentAddr_3", struct_NormalINI.stru_str_InstrumentAddr_3, str_IniPath);

                #endregion

                #region [Mode]

                str_Tmp = struct_NormalINI.stru_b_DebugMode ? "1" : "0";
                IniHelper.WriteIniStr("Mode", "DebugMode", str_Tmp, str_IniPath);

                #endregion

                #region [ServerLog]

                str_Tmp = struct_NormalINI.stru_b_LogServerSwitch ? "1" : "0";
                IniHelper.WriteIniStr("ServerLog", "OpenSwitch", str_Tmp, str_IniPath);
                IniHelper.WriteIniStr("ServerLog", "ServerUser", struct_NormalINI.stru_str_LogServerUser, str_IniPath);
                IniHelper.WriteIniStr("ServerLog", "ServerPassword", struct_NormalINI.stru_str_LogServerPassword, str_IniPath);
                IniHelper.WriteIniStr("ServerLog", "ServerPath", struct_NormalINI.stru_str_LogServerPath, str_IniPath);

                #endregion

                #region [FTP]

                str_Tmp = struct_NormalINI.stru_b_FTPSwitch ? "1" : "0";
                IniHelper.WriteIniStr("FTP", "Switch", str_Tmp, str_IniPath);
                IniHelper.WriteIniStr("FTP", "IP", struct_NormalINI.stru_str_FTP_IP, str_IniPath);
                IniHelper.WriteIniStr("FTP", "Port", struct_NormalINI.stru_i_FTP_Port.ToString(), str_IniPath);
                IniHelper.WriteIniStr("FTP", "User", struct_NormalINI.stru_str_FTP_User, str_IniPath);
                IniHelper.WriteIniStr("FTP", "Password", struct_NormalINI.stru_str_FTP_Password, str_IniPath);
                IniHelper.WriteIniStr("FTP", "SavePath", struct_NormalINI.stru_str_FTP_Path, str_IniPath);

                #endregion


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"写入{str_CaseProjectName} ini failed:{ex.StackTrace}");
                UIHandleHelper.ShowRunLog($"写入{str_CaseProjectName} ini failed:{ex.StackTrace}", false);
                return false;
            }


            return true;
        }

        #endregion

        #region ReadSampleIni

        public bool ReadSampleIni(string str_SampleMac)
        {
            StringBuilder ValTemp = new StringBuilder();
            string str_CaseFolder = new string(str_CaseProjectName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{str_CaseProjectName}\\{str_SampleMac}_Sample.ini";

            //Sample
            // 创建一个List<float>对象，用于存储浮点数
            struct_TestVariable.floatSampleIniPowerList = new List<float>();

            float result = 0;

            try
            {

                if (File.Exists(str_IniPath) == false)
                {
                    throw new ArgumentException($"{str_SampleMac}_Sample.ini文件不存在。");
                }

                #region [Wifi_Real_Power]


                /*旧方法
                #region 2G

                IniHelper.GetIniStr("Wifi_Real_Power", "2G_Low_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }


                IniHelper.GetIniStr("Wifi_Real_Power", "2G_Mid_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }

                IniHelper.GetIniStr("Wifi_Real_Power", "2G_High_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }


                #endregion

                #region 5G

                IniHelper.GetIniStr("Wifi_Real_Power", "5G_Low_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }


                IniHelper.GetIniStr("Wifi_Real_Power", "5G_Mid_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }

                IniHelper.GetIniStr("Wifi_Real_Power", "5G_High_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }

                #endregion

                #region 6G

                IniHelper.GetIniStr("Wifi_Real_Power", "6G_Low_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }


                IniHelper.GetIniStr("Wifi_Real_Power", "6G_Mid_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }

                IniHelper.GetIniStr("Wifi_Real_Power", "6G_High_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }

                #endregion

                #region 7G

                IniHelper.GetIniStr("Wifi_Real_Power", "7G_Low_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }


                IniHelper.GetIniStr("Wifi_Real_Power", "7G_Mid_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }

                IniHelper.GetIniStr("Wifi_Real_Power", "7G_High_CH", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }

                #endregion

                #region [Bt_Real_Power]

                IniHelper.GetIniStr("Bt_Real_Power", "Ch_Low", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }


                IniHelper.GetIniStr("Bt_Real_Power", "Ch_Mid", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }

                IniHelper.GetIniStr("Bt_Real_Power", "Ch_High", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSampleIniPowerList.Add(result);
                }
                #endregion

                */

                ReadNonEmptyChannelPowers(str_IniPath);

                #endregion




                #region [Power_Range]

                IniHelper.GetIniStr("Power_Range", "PowerRange", "", ValTemp, 50, str_IniPath);

                result = 0;
                if (float.TryParse(ValTemp.ToString(), out result))
                {
                    struct_TestVariable.floatSamplePowerRange = result;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"读取{str_SampleMac}_Sample ini PowerRange failed;", true);
                    return false;
                }

                #endregion


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取{str_SampleMac}_Sample ini failed:{ex.StackTrace}");
                UIHandleHelper.ShowRunLog($"读取{str_SampleMac}_Sample ini failed:{ex.StackTrace}", true);
                return false;
            }

            return true;
        }


        public void ReadNonEmptyChannelPowers(string iniPath)
        {
            struct_TestVariable.floatSampleIniPowerList.Clear();

            // 读取Wifi配置
            ReadWifiChannelPowers(iniPath);

            // 读取蓝牙配置
            ReadBluetoothChannelPowers(iniPath);

            Console.WriteLine($"\n读取完成: 找到 {struct_TestVariable.floatSampleIniPowerList.Count} 个有效功率值");
        }

        private void ReadWifiChannelPowers(string iniPath)
        {
            Console.WriteLine("=== 读取Wifi功率配置 ===");

            // 所有可能的频段
            string[] bands = { "2G", "5G", "6G", "7G" };
            string[] types = { "Low_CH", "Mid_CH", "High_CH" };

            foreach (string band in bands)
            {
                foreach (string type in types)
                {
                    ReadChannelValues("Wifi_Real_Power", $"{band}_{type}", iniPath);
                }
            }
        }

        private void ReadBluetoothChannelPowers(string iniPath)
        {
            Console.WriteLine("\n=== 读取蓝牙功率配置 ===");

            // 蓝牙配置键
            string[] btKeys = { "Low_CH", "Mid_CH", "High_CH" };

            foreach (string key in btKeys)
            {
                ReadChannelValues("Bt_Real_Power", key, iniPath);
            }
        }

        private void ReadChannelValues(string section, string keyBase, string iniPath)
        {
            int index = 0; // 0表示尝试无数字后缀，1开始是带数字

            while (true)
            {
                string keyName = (index == 0) ? keyBase : $"{keyBase}{index}";

                StringBuilder buffer = new StringBuilder(256);

                // 使用long接收返回值
                long charsRead = IniHelper.GetIniStr(
                    section,
                    keyName,
                    "",
                    buffer,
                    buffer.Capacity,
                    iniPath
                );

                // 检查是否读取成功且值非空
                if (charsRead > 0)
                {
                    string value = buffer.ToString().Trim();

                    // 跳过空值
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (float.TryParse(value, out float powerValue))
                        {
                            struct_TestVariable.floatSampleIniPowerList.Add(powerValue);
                            Console.WriteLine($"[{section}] {keyName} = {powerValue}");
                        }
                        else
                        {
                            Console.WriteLine($"[{section}] 格式错误: {keyName} = '{value}'");
                        }
                        index++;
                    }
                    else
                    {
                        // 值为空，跳过此项
                        Console.WriteLine($"[{section}] 跳过 {keyName} (值为空)");
                        index++;
                    }
                }
                else
                {
                    // 键不存在
                    if (index == 0)
                    {
                        // 第一次尝试（无数字后缀）失败，尝试带数字后缀
                        index = 1;
                    }
                    else
                    {
                        // 已经没有更多配置
                        break;
                    }
                }
            }
        }



        #endregion




    }
}
