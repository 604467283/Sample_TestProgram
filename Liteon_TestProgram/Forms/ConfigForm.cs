using Liteon_TestProgram.Base;
using Liteon_TestProgram.NetworkServer;
using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Liteon_TestProgram.Forms
{
    public partial class ConfigForm : Form
    {
        [DllImport("user32.dll")]  //需添加using System.Runtime.InteropServices
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;




        // 定义一个事件，用于在变量变化时通知外部
        public event EventHandler VariableChanged;

        private int myVariable = 0;

        public string _strCaseName { get; private set; }

        private Class_Variable.struct_NormalINI_Variable _normalINI;
        private Class_Variable.struct_EncryptINI_Variable _encryptINI;

        public ConfigForm()
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



        private void comboBox_com_DropDown(object sender, EventArgs e)
        {
            Control clickedControl = sender as Control;
            if (clickedControl.Name.Contains("comboBox_com"))
            {
                System.Windows.Forms.ComboBox comboBox = sender as System.Windows.Forms.ComboBox;
                FillComboBoxWithComPorts(comboBox);
            }


            ComboBoxWidth(sender);
        }

        public void FillComboBoxWithComPorts(System.Windows.Forms.ComboBox comboBox)
        {
            comboBox.Items.Clear(); // 清除ComboBox中的现有项
            string[] ports = SerialPort.GetPortNames(); // 获取所有COM端口名称
            foreach (string port in ports)
            {
                comboBox.Items.Add(port); // 将每个端口名称添加到ComboBox中
            }

        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            _strCaseName = CaseForm.str_CaseName;

            //这一步很重要
            _normalINI = CaseCodeBase.struct_NormalINI;
            _encryptINI = CaseCodeBase.struct_EncryptINI;

            LoadIniToUI();

        }

        public void LoadIniToUI()
        {

            #region 设备信息

            comboBox_DeviceName1.Text = _normalINI.stru_str_InstrumentName_1;
            comboBox_DeviceName2.Text = _normalINI.stru_str_InstrumentName_2;
            comboBox_DeviceName3.Text = _normalINI.stru_str_InstrumentName_3;

            comboBox_DeviceAddr1.Text = _normalINI.stru_str_InstrumentAddr_1;
            comboBox_DeviceAddr2.Text = _normalINI.stru_str_InstrumentAddr_2;
            comboBox_DeviceAddr3.Text = _normalINI.stru_str_InstrumentAddr_3;

            #endregion

            #region 串口1

            comboBox_com1.Text = _normalINI.stru_str_PortName_1;
            comboBox_baud1.Text = _normalINI.stru_i_BaudRate_1.ToString();
            comboBox_dataBit1.Text = _normalINI.stru_i_DataBits_1.ToString();
            comboBox_Parity1.Text = _normalINI.stru_Parity_1.ToString();
            comboBox_stop1.Text = _normalINI.stru_StopBits_1.ToString();
            checkBox_DTR1.Checked = _normalINI.stru_b_DtrEnable_1;
            checkBox_RTS1.Checked = _normalINI.stru_b_RtsEnable_1;

            #endregion

            #region 串口2

            comboBox_com2.Text = _normalINI.stru_str_PortName_2;
            comboBox_baud2.Text = _normalINI.stru_i_BaudRate_2.ToString();
            comboBox_dataBit2.Text = _normalINI.stru_i_DataBits_2.ToString();
            comboBox_Parity2.Text = _normalINI.stru_Parity_2.ToString();
            comboBox_stop2.Text = _normalINI.stru_StopBits_2.ToString();
            checkBox_DTR2.Checked = _normalINI.stru_b_DtrEnable_2;
            checkBox_RTS2.Checked = _normalINI.stru_b_RtsEnable_2;

            #endregion

            #region 串口3

            comboBox_com3.Text = _normalINI.stru_str_PortName_3;
            comboBox_baud3.Text = _normalINI.stru_i_BaudRate_3.ToString();
            comboBox_dataBit3.Text = _normalINI.stru_i_DataBits_3.ToString();
            comboBox_Parity3.Text = _normalINI.stru_Parity_3.ToString();
            comboBox_stop3.Text = _normalINI.stru_StopBits_3.ToString();
            checkBox_DTR3.Checked = _normalINI.stru_b_DtrEnable_3;
            checkBox_RTS3.Checked = _normalINI.stru_b_RtsEnable_3;

            #endregion

            #region 串口4

            comboBox_com4.Text = _normalINI.stru_str_PortName_4;
            comboBox_baud4.Text = _normalINI.stru_i_BaudRate_4.ToString();
            comboBox_dataBit4.Text = _normalINI.stru_i_DataBits_4.ToString();
            comboBox_Parity4.Text = _normalINI.stru_Parity_4.ToString();
            comboBox_stop4.Text = _normalINI.stru_StopBits_4.ToString();
            checkBox_DTR4.Checked = _normalINI.stru_b_DtrEnable_4;
            checkBox_RTS4.Checked = _normalINI.stru_b_RtsEnable_4;

            #endregion

            #region 串口5

            comboBox_com5.Text = _normalINI.stru_str_PortName_5;
            comboBox_baud5.Text = _normalINI.stru_i_BaudRate_5.ToString();
            comboBox_dataBit5.Text = _normalINI.stru_i_DataBits_5.ToString();
            comboBox_Parity5.Text = _normalINI.stru_Parity_5.ToString();
            comboBox_stop5.Text = _normalINI.stru_StopBits_5.ToString();
            checkBox_DTR5.Checked = _normalINI.stru_b_DtrEnable_5;
            checkBox_RTS5.Checked = _normalINI.stru_b_RtsEnable_5;

            #endregion

            #region 串口6

            comboBox_com6.Text = _normalINI.stru_str_PortName_6;
            comboBox_baud6.Text = _normalINI.stru_i_BaudRate_6.ToString();
            comboBox_dataBit6.Text = _normalINI.stru_i_DataBits_6.ToString();
            comboBox_Parity6.Text = _normalINI.stru_Parity_6.ToString();
            comboBox_stop6.Text = _normalINI.stru_StopBits_6.ToString();
            checkBox_DTR6.Checked = _normalINI.stru_b_DtrEnable_6;
            checkBox_RTS6.Checked = _normalINI.stru_b_RtsEnable_6;

            #endregion

            #region 前六码

            checkBox_CheckMacSix.Checked = _normalINI.stru_b_CheckMacSix_Switch;
            textBox_CheckMacSix1.Text = _normalINI.stru_str_CheckMacID1;
            textBox_CheckMacSix2.Text = _normalINI.stru_str_CheckMacID2;
            textBox_CheckMacSix3.Text = _normalINI.stru_str_CheckMacID3;

            if (checkBox_CheckMacSix.Checked == false)
            {
                UIHandleHelper.ControlHandle(textBox_CheckMacSix1, () => textBox_CheckMacSix1.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_CheckMacSix2, () => textBox_CheckMacSix2.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_CheckMacSix3, () => textBox_CheckMacSix3.Enabled = false);
            }


            #endregion

            #region PEM

            checkBox_IsUsePEM.Checked = _normalINI.stru_b_IsOpenPEM;
            checkBox_IsOpenDUT.Checked = _normalINI.stru_b_IsOpenDUT;
            textBox_DeviceName1.Text = _normalINI.stru_str_DeviceName1;
            textBox_DeviceName2.Text = _normalINI.stru_str_DeviceName2;

            if (checkBox_IsUsePEM.Checked == false)
            {
                UIHandleHelper.ControlHandle(checkBox_IsOpenDUT, () => checkBox_IsOpenDUT.Enabled = false);
                //UIHandleHelper.ControlHandle(textBox_DeviceName1, () => textBox_DeviceName1.Enabled = false);
                //UIHandleHelper.ControlHandle(textBox_DeviceName2, () => textBox_DeviceName2.Enabled = false);
            }

            #endregion

            #region 屏蔽箱

            textBox_ShieldCom.Text = _normalINI.stru_i_ShieldingBoxCOM.ToString();
            textBox_OpenTimeout.Text = _normalINI.stru_i_SleepCycleWhenOpen.ToString();
            textBox_CloseTimeout.Text = _normalINI.stru_i_SleepCycleWhenClose.ToString();

            if (textBox_ShieldCom.Text == "0")
            {
                UIHandleHelper.ControlHandle(textBox_OpenTimeout, () => textBox_OpenTimeout.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_CloseTimeout, () => textBox_CloseTimeout.Enabled = false);
            }

            #endregion

            #region 手臂设定

            checkBox_IsDebugMode.Checked = _normalINI.stru_b_DebugMode;
            textBox_PCNum.Text = _normalINI.stru_i_TestCompNum.ToString();
            textBox_RobotIP.Text = _normalINI.stru_str_RobotClientHostIP.ToString();
            textBox_RobotPort.Text = _normalINI.stru_str_RobotClientHostPort.ToString();

            if (checkBox_IsDebugMode.Checked == false)
            {
                UIHandleHelper.ControlHandle(textBox_RobotIP, () => textBox_RobotIP.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_RobotPort, () => textBox_RobotPort.Enabled = true);
            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_RobotIP, () => textBox_RobotIP.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_RobotPort, () => textBox_RobotPort.Enabled = false);
            }

            #endregion

            #region Multi设定

            checkBox_IsUseMulti.Checked = _normalINI.stru_b_Multi_Switch;

            if (checkBox_IsUseMulti.Checked == false)
            {
                UIHandleHelper.ControlHandle(textBox_MultiIP, () => textBox_MultiIP.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_MultiPort, () => textBox_MultiPort.Enabled = false);
                UIHandleHelper.ControlHandle(radioButton_MultiServer, () => radioButton_MultiServer.Enabled = false);
                UIHandleHelper.ControlHandle(radioButton_MultiClient, () => radioButton_MultiClient.Enabled = false);
            }

            if (_normalINI.stru_i_SelectMode == 1)
            {
                radioButton_MultiServer.Checked = true;
            }
            else
            {
                radioButton_MultiClient.Checked = true;
            }

            textBox_MultiIP.Text = _normalINI.stru_str_Multi_Server_IP.ToString();
            textBox_MultiPort.Text = _normalINI.stru_str_Multi_Server_Port.ToString();

            #endregion

            #region TesterPort

            comboBox_TesterPortName.Text = _normalINI.stru_str_TesterPort;

            #endregion

            #region LogPath

            textBox_FreeSpaceLimit.Text = _normalINI.stru_i_FreeSpaceLimit.ToString();
            textBox_LogFilePath.Text = _normalINI.stru_str_LogFilePath;
            textBox_SFCFilePath.Text = _normalINI.stru_str_SFCFilePath;

            #endregion

            #region LogServer

            checkBox_IsUseLogServer.Checked = _normalINI.stru_b_LogServerSwitch;
            textBox_LogServerName.Text = _normalINI.stru_str_LogServerUser;
            textBox_LogServerKey.Text = _normalINI.stru_str_LogServerPassword;
            textBox_LogServerPath.Text = _normalINI.stru_str_LogServerPath;

            if (checkBox_IsUseLogServer.Checked == false)
            {
                UIHandleHelper.ControlHandle(textBox_LogServerName, () => textBox_LogServerName.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_LogServerKey, () => textBox_LogServerKey.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_LogServerPath, () => textBox_LogServerPath.Enabled = false);
            }

            #endregion

            #region FTP

            checkBox_UseFTP.Checked = _normalINI.stru_b_FTPSwitch;
            textBox_FTP_IP.Text = _normalINI.stru_str_FTP_IP;
            textBox_FTP_Port.Text = _normalINI.stru_i_FTP_Port.ToString();
            textBox_FTP_User.Text = _normalINI.stru_str_FTP_User;
            textBox_FTP_Password.Text = _normalINI.stru_str_FTP_Password;
            textBox_FTP_Path.Text = _normalINI.stru_str_FTP_Path;

            if (checkBox_UseFTP.Checked == false)
            {
                UIHandleHelper.ControlHandle(textBox_FTP_IP, () => textBox_FTP_IP.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_FTP_Port, () => textBox_FTP_Port.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_FTP_User, () => textBox_FTP_User.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_FTP_Password, () => textBox_FTP_Password.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_FTP_Path, () => textBox_FTP_Path.Enabled = false);
            }

            #endregion

        }

        private async void button_Confirm_Click(object sender, EventArgs e)
        {
            
            
            try
            {
                GetControlValue();
            }
            catch (Exception ex)
            {
                MessageBoxEX.Show($"请检查错误：\r\n {ex}", true);
                return;
            }

            #region 检查logserver

            if (checkBox_IsUseLogServer.Checked)
            {
                // 使用Task.Run启动后台任务
                bool bResult = await Task.Run(() =>
                {
                    MapDriveHelper mapDriveHelper = new();
                    return mapDriveHelper.CheckNetworkConnection(CaseCodeBase.struct_NormalINI.stru_str_LogServerPath);
                });


                if (!bResult)
                {
                    MessageBoxEX.Show("请确认LogServer的地址是否存在！！", true);
                    return; // 直接从button_Confirm_Click返回，不再执行后面的代码
                }


                bResult = await Task.Run(() =>
                {
                    MapDriveHelper mapDriveHelper = new();
                    mapDriveHelper.DeleteExistingConnections_MapDrive();
                    return mapDriveHelper.LoginServer_MapDrive(CaseCodeBase.struct_NormalINI.stru_str_LogServerPath, CaseCodeBase.struct_NormalINI.stru_str_LogServerUser, CaseCodeBase.struct_NormalINI.stru_str_LogServerPassword);
                });

                if (!bResult)
                {
                    MessageBoxEX.Show("请确认LogServer是否可以登陆使用！！", true);
                    return; // 直接从button_Confirm_Click返回，不再执行后面的代码
                }

            }

            #endregion

            #region 检查FTP的连接

            if (checkBox_UseFTP.Checked)
            {
                // 使用Task.Run启动后台任务
                bool bResult = await Task.Run(() =>
                {
                    FTPHelper ftpHelper = new(CaseCodeBase.struct_NormalINI.stru_str_FTP_IP,
                                                        CaseCodeBase.struct_NormalINI.stru_str_FTP_User,
                                                        CaseCodeBase.struct_NormalINI.stru_str_FTP_Password,
                                                        CaseCodeBase.struct_NormalINI.stru_i_FTP_Port);
                    return ftpHelper.Login();
                });


                if (!bResult)
                {
                    MessageBoxEX.Show($"无法登陆{CaseCodeBase.struct_NormalINI.stru_str_FTP_IP} FTP！！", true);
                    return; // 直接从button_Confirm_Click返回，不再执行后面的代码
                }
            }

            #endregion

            // 触发VariableChanged事件
            myVariable = 1;
            OnVariableChanged(EventArgs.Empty);

        }

        // 触发VariableChanged事件的方法
        protected virtual void OnVariableChanged(EventArgs e)
        {
            VariableChanged?.Invoke(this, e);
        }

        public void GetControlValue()
        {

            #region 设备信息

            CaseCodeBase.struct_NormalINI.stru_str_InstrumentName_1 = comboBox_DeviceName1.Text;
            CaseCodeBase.struct_NormalINI.stru_str_InstrumentName_2 = comboBox_DeviceName2.Text;
            CaseCodeBase.struct_NormalINI.stru_str_InstrumentName_3 = comboBox_DeviceName3.Text;

            CaseCodeBase.struct_NormalINI.stru_str_InstrumentAddr_1 = comboBox_DeviceAddr1.Text;
            CaseCodeBase.struct_NormalINI.stru_str_InstrumentAddr_2 = comboBox_DeviceAddr2.Text;
            CaseCodeBase.struct_NormalINI.stru_str_InstrumentAddr_3 = comboBox_DeviceAddr3.Text;

            #endregion

            #region 串口1

            CaseCodeBase.struct_NormalINI.stru_str_PortName_1 = comboBox_com1.Text;
            CaseCodeBase.struct_NormalINI.stru_i_BaudRate_1 = int.Parse(comboBox_baud1.Text);
            CaseCodeBase.struct_NormalINI.stru_i_DataBits_1 = int.Parse(comboBox_dataBit1.Text);

            if (Enum.TryParse(comboBox_Parity1.Text, true, out Parity parityValue))
            {
                CaseCodeBase.struct_NormalINI.stru_Parity_1 = parityValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com1的Parity设置。");
            }

            if (Enum.TryParse(comboBox_stop1.Text, true, out StopBits stopBitsValue))
            {
                CaseCodeBase.struct_NormalINI.stru_StopBits_1 = stopBitsValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com1的StopBits设置。");
            }

            CaseCodeBase.struct_NormalINI.stru_b_DtrEnable_1 = checkBox_DTR1.Checked;
            CaseCodeBase.struct_NormalINI.stru_b_RtsEnable_1 = checkBox_RTS1.Checked;

            #endregion

            #region 串口2

            CaseCodeBase.struct_NormalINI.stru_str_PortName_2 = comboBox_com2.Text;
            CaseCodeBase.struct_NormalINI.stru_i_BaudRate_2 = int.Parse(comboBox_baud2.Text);
            CaseCodeBase.struct_NormalINI.stru_i_DataBits_2 = int.Parse(comboBox_dataBit2.Text);

            if (Enum.TryParse(comboBox_Parity2.Text, true, out parityValue))
            {
                CaseCodeBase.struct_NormalINI.stru_Parity_2 = parityValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com2的Parity设置。");
            }

            if (Enum.TryParse(comboBox_stop2.Text, true, out stopBitsValue))
            {
                CaseCodeBase.struct_NormalINI.stru_StopBits_2 = stopBitsValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com2的StopBits设置。");
            }

            CaseCodeBase.struct_NormalINI.stru_b_DtrEnable_2 = checkBox_DTR2.Checked;
            CaseCodeBase.struct_NormalINI.stru_b_RtsEnable_2 = checkBox_RTS2.Checked;

            #endregion

            #region 串口3

            CaseCodeBase.struct_NormalINI.stru_str_PortName_3 = comboBox_com3.Text;
            CaseCodeBase.struct_NormalINI.stru_i_BaudRate_3 = int.Parse(comboBox_baud3.Text);
            CaseCodeBase.struct_NormalINI.stru_i_DataBits_3 = int.Parse(comboBox_dataBit3.Text);

            if (Enum.TryParse(comboBox_Parity3.Text, true, out parityValue))
            {
                CaseCodeBase.struct_NormalINI.stru_Parity_3 = parityValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com3的Parity设置。");
            }

            if (Enum.TryParse(comboBox_stop3.Text, true, out stopBitsValue))
            {
                CaseCodeBase.struct_NormalINI.stru_StopBits_3 = stopBitsValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com3的StopBits设置。");
            }

            CaseCodeBase.struct_NormalINI.stru_b_DtrEnable_3 = checkBox_DTR3.Checked;
            CaseCodeBase.struct_NormalINI.stru_b_RtsEnable_3 = checkBox_RTS3.Checked;

            #endregion

            #region 串口4

            CaseCodeBase.struct_NormalINI.stru_str_PortName_4 = comboBox_com4.Text;
            CaseCodeBase.struct_NormalINI.stru_i_BaudRate_4 = int.Parse(comboBox_baud4.Text);
            CaseCodeBase.struct_NormalINI.stru_i_DataBits_4 = int.Parse(comboBox_dataBit4.Text);

            if (Enum.TryParse(comboBox_Parity4.Text, true, out parityValue))
            {
                CaseCodeBase.struct_NormalINI.stru_Parity_4 = parityValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com4的Parity设置。");
            }

            if (Enum.TryParse(comboBox_stop4.Text, true, out stopBitsValue))
            {
                CaseCodeBase.struct_NormalINI.stru_StopBits_4 = stopBitsValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com4的StopBits设置。");
            }

            CaseCodeBase.struct_NormalINI.stru_b_DtrEnable_4 = checkBox_DTR4.Checked;
            CaseCodeBase.struct_NormalINI.stru_b_RtsEnable_4 = checkBox_RTS4.Checked;

            #endregion

            #region 串口5

            CaseCodeBase.struct_NormalINI.stru_str_PortName_5 = comboBox_com5.Text;
            CaseCodeBase.struct_NormalINI.stru_i_BaudRate_5 = int.Parse(comboBox_baud5.Text);
            CaseCodeBase.struct_NormalINI.stru_i_DataBits_5 = int.Parse(comboBox_dataBit5.Text);

            if (Enum.TryParse(comboBox_Parity5.Text, true, out parityValue))
            {
                CaseCodeBase.struct_NormalINI.stru_Parity_5 = parityValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com5的Parity设置。");
            }

            if (Enum.TryParse(comboBox_stop5.Text, true, out stopBitsValue))
            {
                CaseCodeBase.struct_NormalINI.stru_StopBits_5 = stopBitsValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com5的StopBits设置。");
            }

            CaseCodeBase.struct_NormalINI.stru_b_DtrEnable_5 = checkBox_DTR5.Checked;
            CaseCodeBase.struct_NormalINI.stru_b_RtsEnable_5 = checkBox_RTS5.Checked;

            #endregion

            #region 串口6

            CaseCodeBase.struct_NormalINI.stru_str_PortName_6 = comboBox_com6.Text;
            CaseCodeBase.struct_NormalINI.stru_i_BaudRate_6 = int.Parse(comboBox_baud6.Text);
            CaseCodeBase.struct_NormalINI.stru_i_DataBits_6 = int.Parse(comboBox_dataBit6.Text);

            if (Enum.TryParse(comboBox_Parity6.Text, true, out parityValue))
            {
                CaseCodeBase.struct_NormalINI.stru_Parity_6 = parityValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com6的Parity设置。");
            }

            if (Enum.TryParse(comboBox_stop6.Text, true, out stopBitsValue))
            {
                CaseCodeBase.struct_NormalINI.stru_StopBits_6 = stopBitsValue;
            }
            else
            {
                // 如果转换失败，可以抛出异常或返回默认值
                throw new InvalidOperationException("无法从控件中读取Com6的StopBits设置。");
            }

            CaseCodeBase.struct_NormalINI.stru_b_DtrEnable_6 = checkBox_DTR6.Checked;
            CaseCodeBase.struct_NormalINI.stru_b_RtsEnable_6 = checkBox_RTS6.Checked;

            #endregion

            #region 前六码

            CaseCodeBase.struct_NormalINI.stru_b_CheckMacSix_Switch = checkBox_CheckMacSix.Checked;
            CaseCodeBase.struct_NormalINI.stru_str_CheckMacID1 = textBox_CheckMacSix1.Text;
            CaseCodeBase.struct_NormalINI.stru_str_CheckMacID2 = textBox_CheckMacSix2.Text;
            CaseCodeBase.struct_NormalINI.stru_str_CheckMacID3 = textBox_CheckMacSix3.Text;

            #endregion

            #region PEM

            CaseCodeBase.struct_NormalINI.stru_b_IsOpenPEM = checkBox_IsUsePEM.Checked;
            CaseCodeBase.struct_NormalINI.stru_b_IsOpenDUT = checkBox_IsOpenDUT.Checked;
            CaseCodeBase.struct_NormalINI.stru_str_DeviceName1 = textBox_DeviceName1.Text;
            CaseCodeBase.struct_NormalINI.stru_str_DeviceName2 = textBox_DeviceName2.Text;

            #endregion

            #region 屏蔽箱

            CaseCodeBase.struct_NormalINI.stru_i_ShieldingBoxCOM = int.Parse(textBox_ShieldCom.Text);
            CaseCodeBase.struct_NormalINI.stru_i_SleepCycleWhenOpen = int.Parse(textBox_OpenTimeout.Text);
            CaseCodeBase.struct_NormalINI.stru_i_SleepCycleWhenClose = int.Parse(textBox_CloseTimeout.Text);

            #endregion

            #region 手臂设定

            CaseCodeBase.struct_NormalINI.stru_b_DebugMode = checkBox_IsDebugMode.Checked;
            CaseCodeBase.struct_NormalINI.stru_i_TestCompNum = int.Parse(textBox_PCNum.Text);
            CaseCodeBase.struct_NormalINI.stru_str_RobotClientHostIP = textBox_RobotIP.Text;
            CaseCodeBase.struct_NormalINI.stru_str_RobotClientHostPort = textBox_RobotPort.Text;

            #endregion

            #region Multi设定

            CaseCodeBase.struct_NormalINI.stru_b_Multi_Switch = checkBox_IsUseMulti.Checked;
            CaseCodeBase.struct_NormalINI.stru_str_Multi_Server_IP = textBox_MultiIP.Text;
            CaseCodeBase.struct_NormalINI.stru_str_Multi_Server_Port = textBox_MultiPort.Text;
            if (radioButton_MultiServer.Checked)
            {
                CaseCodeBase.struct_NormalINI.stru_i_SelectMode = 1;
            }
            else
            {
                CaseCodeBase.struct_NormalINI.stru_i_SelectMode = 0;
            }

            #endregion

            #region TesterPort

            CaseCodeBase.struct_NormalINI.stru_str_TesterPort = comboBox_TesterPortName.Text;

            #endregion

            #region LogPath

            CaseCodeBase.struct_NormalINI.stru_i_FreeSpaceLimit = int.Parse(textBox_FreeSpaceLimit.Text);
            CaseCodeBase.struct_NormalINI.stru_str_LogFilePath = textBox_LogFilePath.Text;
            CaseCodeBase.struct_NormalINI.stru_str_SFCFilePath = textBox_SFCFilePath.Text;

            #endregion

            #region LogServer

            CaseCodeBase.struct_NormalINI.stru_b_LogServerSwitch = checkBox_IsUseLogServer.Checked;
            CaseCodeBase.struct_NormalINI.stru_str_LogServerUser = textBox_LogServerName.Text;
            CaseCodeBase.struct_NormalINI.stru_str_LogServerPassword = textBox_LogServerKey.Text;
            CaseCodeBase.struct_NormalINI.stru_str_LogServerPath = textBox_LogServerPath.Text;

            #endregion

            #region FTP

            CaseCodeBase.struct_NormalINI.stru_b_FTPSwitch = checkBox_UseFTP.Checked;
            CaseCodeBase.struct_NormalINI.stru_str_FTP_IP = textBox_FTP_IP.Text;
            CaseCodeBase.struct_NormalINI.stru_i_FTP_Port = int.Parse(textBox_FTP_Port.Text);
            CaseCodeBase.struct_NormalINI.stru_str_FTP_User = textBox_FTP_User.Text;
            CaseCodeBase.struct_NormalINI.stru_str_FTP_Password = textBox_FTP_Password.Text;
            CaseCodeBase.struct_NormalINI.stru_str_FTP_Path = textBox_FTP_Path.Text;

            #endregion


        }



        private void checkBox_CheckMacSix_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_CheckMacSix.Checked == false)
            {
                UIHandleHelper.ControlHandle(textBox_CheckMacSix1, () => textBox_CheckMacSix1.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_CheckMacSix2, () => textBox_CheckMacSix2.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_CheckMacSix3, () => textBox_CheckMacSix3.Enabled = false);
            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_CheckMacSix1, () => textBox_CheckMacSix1.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_CheckMacSix2, () => textBox_CheckMacSix2.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_CheckMacSix3, () => textBox_CheckMacSix3.Enabled = true);
            }
        }



        private void checkBox_IsUsePEM_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_IsUsePEM.Checked == false)
            {
                UIHandleHelper.ControlHandle(checkBox_IsOpenDUT, () => checkBox_IsOpenDUT.Enabled = false);
                //UIHandleHelper.ControlHandle(textBox_DeviceName1, () => textBox_DeviceName1.Enabled = false);
                //UIHandleHelper.ControlHandle(textBox_DeviceName2, () => textBox_DeviceName2.Enabled = false);
            }
            else
            {
                UIHandleHelper.ControlHandle(checkBox_IsOpenDUT, () => checkBox_IsOpenDUT.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_DeviceName1, () => textBox_DeviceName1.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_DeviceName2, () => textBox_DeviceName2.Enabled = true);
            }
        }

        private void textBox_ShieldCom_TextChanged(object sender, EventArgs e)
        {
            if (textBox_ShieldCom.Text == "0")
            {
                UIHandleHelper.ControlHandle(textBox_OpenTimeout, () => textBox_OpenTimeout.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_CloseTimeout, () => textBox_CloseTimeout.Enabled = false);
            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_OpenTimeout, () => textBox_OpenTimeout.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_CloseTimeout, () => textBox_CloseTimeout.Enabled = true);
            }
        }

        private void checkBox_IsDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_IsDebugMode.Checked)
            {
                UIHandleHelper.ControlHandle(textBox_RobotIP, () => textBox_RobotIP.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_RobotPort, () => textBox_RobotPort.Enabled = false);
            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_RobotIP, () => textBox_RobotIP.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_RobotPort, () => textBox_RobotPort.Enabled = true);
            }
        }

        private void checkBox_IsUseMulti_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_IsUseMulti.Checked == false)
            {
                UIHandleHelper.ControlHandle(textBox_MultiIP, () => textBox_MultiIP.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_MultiPort, () => textBox_MultiPort.Enabled = false);
                UIHandleHelper.ControlHandle(radioButton_MultiServer, () => radioButton_MultiServer.Enabled = false);
                UIHandleHelper.ControlHandle(radioButton_MultiClient, () => radioButton_MultiClient.Enabled = false);
            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_MultiIP, () => textBox_MultiIP.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_MultiPort, () => textBox_MultiPort.Enabled = true);
                UIHandleHelper.ControlHandle(radioButton_MultiServer, () => radioButton_MultiServer.Enabled = true);
                UIHandleHelper.ControlHandle(radioButton_MultiClient, () => radioButton_MultiClient.Enabled = true);
            }
        }

        private void textBox_LogFilePath_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // 设置对话框的标题
            folderBrowserDialog.Description = "请选择文件夹";

            // 显示对话框，如果用户点击了“确定”并且选择了一个文件夹，则将其路径设置到TextBox中
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_LogFilePath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void textBox_SFCFilePath_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // 设置对话框的标题
            folderBrowserDialog.Description = "请选择文件夹";

            // 显示对话框，如果用户点击了“确定”并且选择了一个文件夹，则将其路径设置到TextBox中
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_SFCFilePath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void checkBox_IsUseLogServer_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_IsUseLogServer.Checked == false)
            {
                UIHandleHelper.ControlHandle(textBox_LogServerName, () => textBox_LogServerName.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_LogServerKey, () => textBox_LogServerKey.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_LogServerPath, () => textBox_LogServerPath.Enabled = false);
            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_LogServerName, () => textBox_LogServerName.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_LogServerKey, () => textBox_LogServerKey.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_LogServerPath, () => textBox_LogServerPath.Enabled = true);
            }
        }

        private void checkBox_UseFTP_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_UseFTP.Checked == false)
            {
                UIHandleHelper.ControlHandle(textBox_FTP_IP, () => textBox_FTP_IP.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_FTP_Port, () => textBox_FTP_Port.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_FTP_User, () => textBox_FTP_User.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_FTP_Password, () => textBox_FTP_Password.Enabled = false);
                UIHandleHelper.ControlHandle(textBox_FTP_Path, () => textBox_FTP_Path.Enabled = false);
            }
            else
            {
                UIHandleHelper.ControlHandle(textBox_FTP_IP, () => textBox_FTP_IP.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_FTP_Port, () => textBox_FTP_Port.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_FTP_User, () => textBox_FTP_User.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_FTP_Password, () => textBox_FTP_Password.Enabled = true);
                UIHandleHelper.ControlHandle(textBox_FTP_Path, () => textBox_FTP_Path.Enabled = true);
            }
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;  //取消关闭
        }


        public void ComboBoxWidth(object sender)
        {
            System.Windows.Forms.ComboBox senderComboBox = (System.Windows.Forms.ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;
            int newWidth;

            foreach (string s in ((System.Windows.Forms.ComboBox)sender).Items)
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

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            await OpenLogFolder();
        }

        private async Task OpenLogFolder()
        {
            await Task.Run(() =>
            {
                if (Directory.Exists( ".\\Model_Config\\" + _strCaseName.Substring(0, _strCaseName.IndexOf("_")) + "\\" +_strCaseName))
                { 
                    Process.Start("explorer.exe", ".\\Model_Config\\" + _strCaseName.Substring(0, _strCaseName.IndexOf("_")) + "\\" + _strCaseName );
                }
                else
                {
                    MessageBoxEX.Show($"指定的文件夹路径无效或不存在！{".\\Model_Config\\" + _strCaseName.Substring(0, _strCaseName.IndexOf("_")) + "\\" + _strCaseName}", true);
                }
            });
        }


    }
}
