using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Utilities;
using System.Diagnostics;
using System.Text;
using Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V2;
using static Liteon_TestProgram.Base.Class_Variable;
using Liteon_TestProgram.Utilities.HttpClient;
using ScottPlot.Statistics;
using Liteon_TestProgram.Utilities.WIFIHelper;
using Liteon_TestProgram.Utilities.IOHelpers;
using ScottPlot.Colormaps;
using ScottPlot;
using SimpleWifi;
using System.Web;
using Liteon_TestProgram.Utilities.IOHelpers.Extensions;
using Liteon_TestProgram.Utilities.GPIOHelpers.RockMong;
using Liteon_TestProgram.Utilities.PEM;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Shapes;

namespace Liteon_TestProgram.CaseProject
{
    internal class CustomerFWTest_Base : CaseCodeBase
    {


        /// <summary>
        /// 用于添加MAC的前缀，比如“23S”，默认是空
        /// </summary>
        protected virtual string str_MacPrefix { get; set; } = "";


        /// <summary>
        /// 用于添加MAC的后缀，比如“A”，默认是空
        /// </summary>
        protected virtual string str_MacSuffix { get; set; } = "";



        public static string str_WifiPartName = null;
        public static int iWIFI_RSSI_Percent_LowerLimit = 0;
        public static int iBT_RSSI_dBm_LowerLimit = 0;
        public static int iBT_RSSI_dBm_UpperLimit = 0;
        public static string str_FW_Version = null;
        public static string str_Get_Version_Command = null;
        public static string str_Web_Services_Command = null;
        public static string str_Web_Services_Return = null;
        public static string str_IperfServerCommand = null;
        public static string str_IperfServer_PostData = null;
        public static string str_IperfServer_Return = null;
        public static int iIperfSleepTime = 0;
        public static int iIperf_LowerLimit = 0;
        public static string str_BLE_Test_Python_Path = null;
        public static string str_BLE_Test_Command = null;
        public static string str_BLE_Test_PostData = null;
        public static string str_BLE_Test_Return = null;
        public static string str_GPIO_Test_Command = null;

        public static float f_GPIO_Voltage_High = 0;
        public static float f_GPIO_Voltage_Low = 0;
        public static float f_GPIO_Voltage_Tolerance = 0;
        public static int iGPIOTestBoardPull = 0; //Pull：上拉下拉电阻。0，无。1，使能内部上拉。2，使能内部下拉

        #region GPIO0-12

        public static string str_GPIO0_Test_PostData = null;
        public static string str_GPIO0_Test_Return = null;

        public static string str_GPIO1_Test_PostData = null;
        public static string str_GPIO1_Test_Return = null;

        public static string str_GPIO2_Test_PostData = null;
        public static string str_GPIO2_Test_Return = null;

        public static string str_GPIO3_Test_PostData = null;
        public static string str_GPIO3_Test_Return = null;

        public static string str_GPIO4_Test_PostData = null;
        public static string str_GPIO4_Test_Return = null;

        public static string str_GPIO5_Test_PostData = null;
        public static string str_GPIO5_Test_Return = null;

        public static string str_GPIO6_Test_PostData = null;
        public static string str_GPIO6_Test_Return = null;

        public static string str_GPIO7_Test_PostData = null;
        public static string str_GPIO7_Test_Return = null;

        public static string str_GPIO8_Test_PostData = null;
        public static string str_GPIO8_Test_Return = null;

        public static string str_GPIO9_Test_PostData = null;
        public static string str_GPIO9_Test_Return = null;

        public static string str_GPIO10_Test_PostData = null;
        public static string str_GPIO10_Test_Return = null;

        public static string str_GPIO11_Test_PostData = null;
        public static string str_GPIO11_Test_Return = null;

        public static string str_GPIO12_Test_PostData = null;
        public static string str_GPIO12_Test_Return = null;

        #endregion

        #region GPIO15-21


        public static string str_GPIO15_Test_PostData = null;
        public static string str_GPIO15_Test_Return = null;

        public static string str_GPIO16_Test_PostData = null;
        public static string str_GPIO16_Test_Return = null;

        public static string str_GPIO17_Test_PostData = null;
        public static string str_GPIO17_Test_Return = null;

        public static string str_GPIO18_Test_PostData = null;
        public static string str_GPIO18_Test_Return = null;

        public static string str_GPIO19_Test_PostData = null;
        public static string str_GPIO19_Test_Return = null;

        public static string str_GPIO20_Test_PostData = null;
        public static string str_GPIO20_Test_Return = null;

        public static string str_GPIO21_Test_PostData = null;
        public static string str_GPIO21_Test_Return = null;

        #endregion

        #region GPIO22-23

        public static string str_GPIO22_23_Test_PostData = null;
        public static string str_GPIO22_23_Test_Grounded_Return = null;
        public static string str_GPIO22_23_Test_NonGrounded_Return = null;

        #endregion

        #region GPIO24-27


        public static string str_GPIO24_Test_PostData = null;
        public static string str_GPIO24_Test_Return = null;

        public static string str_GPIO25_Test_PostData = null;
        public static string str_GPIO25_Test_Return = null;

        public static string str_GPIO26_Test_PostData = null;
        public static string str_GPIO26_Test_Return = null;

        public static string str_GPIO27_Test_PostData = null;
        public static string str_GPIO27_Test_Return = null;


        #endregion

        #region GPIO42-55


        public static string str_GPIO42_Test_PostData = null;
        public static string str_GPIO42_Test_Return = null;

        public static string str_GPIO43_Test_PostData = null;
        public static string str_GPIO43_Test_Return = null;

        public static string str_GPIO44_Test_PostData = null;
        public static string str_GPIO44_Test_Return = null;

        public static string str_GPIO45_Test_PostData = null;
        public static string str_GPIO45_Test_Return = null;

        public static string str_GPIO46_Test_PostData = null;
        public static string str_GPIO46_Test_Return = null;

        public static string str_GPIO47_Test_PostData = null;
        public static string str_GPIO47_Test_Return = null;

        public static string str_GPIO48_Test_PostData = null;
        public static string str_GPIO48_Test_Return = null;

        public static string str_GPIO49_Test_PostData = null;
        public static string str_GPIO49_Test_Return = null;

        public static string str_GPIO50_Test_PostData = null;
        public static string str_GPIO50_Test_Return = null;

        public static string str_GPIO51_Test_PostData = null;
        public static string str_GPIO51_Test_Return = null;

        public static string str_GPIO52_Test_PostData = null;
        public static string str_GPIO52_Test_Return = null;

        public static string str_GPIO53_Test_PostData = null;
        public static string str_GPIO53_Test_Return = null;

        public static string str_GPIO54_Test_PostData = null;
        public static string str_GPIO54_Test_Return = null;

        public static string str_GPIO55_Test_PostData = null;
        public static string str_GPIO55_Test_Return = null;

        #endregion


        public int iCommand_Send_Interval_Second = 0;
        public int iTestSwitch_GPIO = 1;

        WifiHelper wifiHelper = new WifiHelper();
        HttpWithBodyHelper httpClientHelper = new();
        HttpGetHelper getter = new HttpGetHelper();


        public static string str_IperfFolderName = "iperf";
        public static string str_IperfFileName = "iperf.exe";
        public string str_IperfFolderPath = $"{PathHelper.GetParentDirectoryPath(PathHelper.GetCurrentExeDirPath())}\\{str_IperfFolderName}";
       
        public static string str_IperfBatName = "Testiperf.bat";
        public static string str_IperfLogName = "Testiperf.txt";
        public static string str_IperfBatContent = "iperf.exe -s >> Testiperf.txt";

        public static string str_BleFolderName = "ble";
        public static string str_BleFileName = "ble_disc.py";
        public string str_BleFolderPath = $"{PathHelper.GetParentDirectoryPath(PathHelper.GetCurrentExeDirPath())}\\{str_BleFolderName}";


        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get;  set; }
        public TestForm _testForm;
        public string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public CustomerFWTest_Base(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
          : base(mf, tf, pc, str_CaseProject)
        {
            struct_TestVariable.bNeedCreateSFCFile = true;
            _CaseProjectName = str_CaseProjectName = str_CaseProject == GetType().Name ? str_CaseProject : GetType().Name;
            _testForm = tf;

            Type sfcFileType = typeof(SfcFile); // 如果你已经有了SfcFile的实例，可以用instance.GetType()        
            struct_TestVariable.str_LogDataCollectionType = sfcFileType.FullName; // 使用反射获取类型的完整名称（包括命名空间）

            List<string> tipslist_str = new List<string>();
            tipslist_str = CreateTestStringList();
            string[] array_tipsstr = tipslist_str.ToArray();
            ShowTipsMessage(str_CaseProjectName + "测试提要", array_tipsstr);

            List<string> changelist_str = new List<string>();
            changelist_str = CreateChangeStringList();
            string[] array_changestr = changelist_str.ToArray();
            ShowChangeListMessage($"  <{str_CaseProjectName} 变更记录>", array_changestr);
        }


        /// <summary>
        /// 机种变更记录,会显示到UI标签页上
        /// </summary>
        /// <returns></returns>
        public override List<string> CreateChangeStringList()
        {
            List<string> list_str = new List<string>();

            #region V0.0.0.1

            list_str.Add("V0.0.0.1");
            list_str.Add("1.支持全PinGPIO测试，使用专用GPIO 3.3V测试板");
            list_str.Add("2.(2025-08-12)变更客户FW: NIUV_v1.0.0V_A29040201A-S00104402A_supplier");

            #endregion

            return list_str;
        }


        /// <summary>
        /// 测试须知填写处,会显示到测试UI上
        /// </summary>
        /// <returns></returns>
        public override List<string> CreateTestStringList()
        {
            List<string> list_str = new List<string>();
            list_str.Add("1.COMA请填测试TB022小板的串口\r\n");
            list_str.Add("2.测试TB022小板: 1O 1F控制给板子上下电\r\n");
            list_str.Add("3.测试TB022小板: 2O 2F 3O 3F控制给GPIO22 23接地\r\n");
            list_str.Add("4.测试电脑需要使用到无线网卡和蓝牙适配器.\r\n");
            list_str.Add("5.用到无线网卡名需要填到设备名称1中.\r\n");
            list_str.Add("6.GPIO读取小板(岩獴)需要两块，并要修改其SN，一个为3602001, 一个为3602002\r\n");
            list_str.Add("7.以下为接线对照，依次使用3602001和3602002的测试板：\r\n");
            list_str.Add(@"产品GPIO 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12
读取板Pin 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12
产品GPIO 15, 16, 17, 18, 19, 20, 21
读取板Pin 13, 14, 15, 16, 17, 18, 19
产品GPIO 24, 25, 26, 27
读取板Pin 20, 21, 22, 23
产品GPIO 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55 
读取板Pin 24, 25, 26, 27, 28, 29, 30, 31, 00, 01, 02, 03, 04 ,05");
            return list_str;
        }


        public override void Func_TestMac()
        {
            macBD_Relation = MacBD_Relation.PlusOne;
            if (struct_NormalINI.stru_b_DebugMode)
            {
                struct_TestVariable.iMAC_OriginalLength = 12;
                struct_TestVariable.iMAC_ExtractStartPosition = 0;
                struct_TestVariable.iMACLength = 12;

                struct_TestVariable.iBD_OriginalLength = 12;
                struct_TestVariable.iBD_ExtractStartPosition = 0;
                struct_TestVariable.iBDLength = 12;
            }
            else
            {
                struct_TestVariable.iMACLength = 12;
                struct_TestVariable.iBDLength = 12;
            }
        }


        public override bool Func_TestPre()
        {
            bool bResult = false;

            //读取额外的配置文件
            bResult = ReadCustomerEncryptIni();

            return bResult;
        }

        public override bool Func_TestInit()
        { 

            bool bInitResult = false;

            #region testinit的额外补充

            try
            {

                if (File.Exists(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfLogName}"))
                {
                    File.Delete(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfLogName}");
                }
                if (File.Exists(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfBatName}"))
                {
                    File.Delete(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfLogName}");
                }


                bInitResult = COM1.RunCommandLine("1F", "");

                if (bInitResult)
                {
                    Thread.Sleep(1000);
                    bInitResult = COM1.RunCommandLine("1O", "");
                }
                Thread.Sleep(5000);


            }
            catch (Exception ex)
            {
                bInitResult = false;
                UIHandleHelper.ShowRunLog($"init fail: {ex}");
            }

        

            #endregion

            return bInitResult;
        }

        public override bool Func_TestFlow()
        {        
            bool bTestResult = false;

            #region 连接WIFI和RSSI

            bTestResult = WifiConnectTest();

            #endregion

            #region FWVerCheck

            if (bTestResult)
            {
                bTestResult = CheckFWVer();
            }

            #endregion

            #region WebServices

            if (bTestResult)
            {
                Thread.Sleep(iCommand_Send_Interval_Second);
                bTestResult = WebServicesTest();
            }

            #endregion

            #region iperf_DVT_Skip

            if (bTestResult)
            {
                bTestResult = CreateIperfBat(str_IperfFolderPath, str_IperfBatContent, str_IperfBatName);
            }

            if (bTestResult)
            {
                ExecuteBatchFileNoWait($"{PathHelper.GetCurrentExeDirPath()}\\{str_IperfBatName}");

                Thread.Sleep(iCommand_Send_Interval_Second);
                Task<bool> httpTask_Iperf = Http_Post_Async("Set Server 192.168.142.2", str_IperfServerCommand, str_IperfServer_PostData, str_IperfServer_Return);
                httpTask_Iperf.Wait();
                bTestResult = httpTask_Iperf.Result;
            }

            if (bTestResult)
            {
                Thread.Sleep(iIperfSleepTime * 1000 + 1000);
                ProcessHelper.KillProcessByName("iperf");
                bTestResult = CheckIperfResult();
            }

            #endregion

            #region BLE和RSSI

            if (bTestResult)
            {
                Thread.Sleep(iCommand_Send_Interval_Second);
                bTestResult = BleTest();
            }

            #endregion

            #region GPIO

            if (bTestResult && (iTestSwitch_GPIO==1) )
            {
                Thread.Sleep(iCommand_Send_Interval_Second);
                bTestResult = GPIOTest();
            }

            #endregion





            return bTestResult;
        }

        public override bool Func_TestEnd(bool bTestResult)
        {
            bool bEndResult = true;

            try
            {
                if (struct_TestVariable.iBDLength == 0)
                {
                    sfcFile.CreateSfcFile(struct_TestVariable.bNeedCreateSFCFile, bTestResult, null,
                                                str_MacPrefix + struct_Barcode.stru_str_sRevDUTMac + str_MacSuffix, "",
                                                struct_EncryptINI.stru_str_ProjectName,
                                                struct_EncryptINI.stru_str_CaseVersion, struct_NormalINI.stru_str_SFCFilePath, str_ErrorCode);
                }
                else
                {
                    sfcFile.CreateSfcFile(struct_TestVariable.bNeedCreateSFCFile, bTestResult, null,
                                          str_MacPrefix + struct_Barcode.stru_str_sRevDUTMac + str_MacSuffix, str_MacPrefix + struct_Barcode.stru_str_sRevDUTBD + str_MacSuffix,
                                          struct_EncryptINI.stru_str_ProjectName,
                                          struct_EncryptINI.stru_str_CaseVersion, struct_NormalINI.stru_str_SFCFilePath, str_ErrorCode);
                }

            }
            catch (Exception)
            {
                UIHandleHelper.ShowRunLog("Failed to process the file",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "End",
                                                               TestItemContent: "Failed to process the file",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err000.ToString());
                bEndResult = false;
            }


            return bEndResult;
        }


        public override void Func_ResourceRelease()
        {
            DevconHelper.DisableByName(struct_NormalINI.stru_str_DeviceName1);
            Thread.Sleep(100);
            wifiHelper.DisConnectWifi();
            COM1.RunCommandLine("FF", "");
            COM1.Close(true);
            Thread.Sleep(1000);
        }




        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================


        public  bool ReadCustomerEncryptIni()
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

                #region [Test_Items]

                #region 指令发送间隔时间

                IniHelper.GetIniStr("Test_Items", "Command_Send_Interval_Second", "null", ValTemp, 50, str_IniPath);
                iCommand_Send_Interval_Second = (int)float.Parse(ValTemp.ToString())*1000;

                #endregion

                #region WifiPartName

                IniHelper.GetIniStr("Test_Items", "WifiPartName", "null", ValTemp, 100, str_IniPath);
                str_WifiPartName = ValTemp.ToString();

                #endregion

                #region RSSI

                IniHelper.GetIniStr("Test_Items", "WIFI_RSSI_Percent_LowerLimit", "null", ValTemp, 50, str_IniPath);
                iWIFI_RSSI_Percent_LowerLimit = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("Test_Items", "BT_RSSI_dBm_LowerLimit", "null", ValTemp, 10, str_IniPath);
                iBT_RSSI_dBm_LowerLimit = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("Test_Items", "BT_RSSI_dBm_UpperLimit", "null", ValTemp, 10, str_IniPath);
                iBT_RSSI_dBm_UpperLimit = int.Parse(ValTemp.ToString());

                #endregion

                #region FWVersion

                IniHelper.GetIniStr("Test_Items", "FW_Version", "null", ValTemp, 100, str_IniPath);
                str_FW_Version = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "Get_Version_Command", "null", ValTemp, 100, str_IniPath);
                str_Get_Version_Command = ValTemp.ToString();

                #endregion

                #region WebServices

                IniHelper.GetIniStr("Test_Items", "Web_Services_Command", "null", ValTemp, 100, str_IniPath);
                str_Web_Services_Command = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "Web_Services_Return", "null", ValTemp, 100, str_IniPath);
                str_Web_Services_Return = ValTemp.ToString();

                #endregion

                #region Iperf

                IniHelper.GetIniStr("Test_Items", "IperfServerCommand", "null", ValTemp, 100, str_IniPath);
                str_IperfServerCommand = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "IperfServer_PostData", "null", ValTemp, 100, str_IniPath);
                str_IperfServer_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "IperfServer_Return", "null", ValTemp, 100, str_IniPath);
                str_IperfServer_Return = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "IperfSleepTime", "null", ValTemp, 10, str_IniPath);
                iIperfSleepTime = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("Test_Items", "Iperf_LowerLimit", "null", ValTemp, 10, str_IniPath);
                iIperf_LowerLimit = int.Parse(ValTemp.ToString());

                #endregion

                #region BLE

                IniHelper.GetIniStr("Test_Items", "BLE_Test_Python_Path", "null", ValTemp, 100, str_IniPath);
                str_BLE_Test_Python_Path = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "BLE_Test_Command", "null", ValTemp, 100, str_IniPath);
                str_BLE_Test_Command = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "BLE_Test_PostData", "null", ValTemp, 100, str_IniPath);
                str_BLE_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "BLE_Test_Return", "null", ValTemp, 100, str_IniPath);
                str_BLE_Test_Return = ValTemp.ToString();

                #endregion


                #endregion


                #region GPIO

                IniHelper.GetIniStr("GPIO_Test", "TestSwitch", "1", ValTemp, 50, str_IniPath);
                iTestSwitch_GPIO = (int)float.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("GPIO_Test", "GPIO_Test_Command", "null", ValTemp, 500, str_IniPath);
                str_GPIO_Test_Command = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO_Voltage_High", "null", ValTemp, 500, str_IniPath);
                f_GPIO_Voltage_High = float.Parse( ValTemp.ToString());

                IniHelper.GetIniStr("GPIO_Test", "GPIO_Voltage_Low", "null", ValTemp, 500, str_IniPath);
                f_GPIO_Voltage_Low = float.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("GPIO_Test", "Voltage_Tolerance", "null", ValTemp, 500, str_IniPath);
                f_GPIO_Voltage_Tolerance = float.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("GPIO_Test", "GPIOTestBoardPull", "null", ValTemp, 500, str_IniPath);
                iGPIOTestBoardPull = int.Parse(ValTemp.ToString());

                #region GPIO0-12

                IniHelper.GetIniStr("GPIO_Test", "GPIO0_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO0_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO0_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO0_Test_Return = ValTemp.ToString();


                IniHelper.GetIniStr("GPIO_Test", "GPIO1_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO1_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO1_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO1_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO2_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO2_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO2_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO2_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO3_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO3_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO3_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO3_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO4_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO4_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO4_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO4_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO5_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO5_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO5_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO5_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO6_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO6_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO6_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO6_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO7_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO7_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO7_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO7_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO8_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO8_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO8_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO8_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO9_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO9_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO9_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO9_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO10_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO10_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO10_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO10_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO11_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO11_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO11_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO11_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO12_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO12_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO12_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO12_Test_Return = ValTemp.ToString();

                #endregion

                #region GPIO22-23

                IniHelper.GetIniStr("GPIO_Test", "GPIO22_23_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO22_23_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO22_23_Test_Grounded_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO22_23_Test_Grounded_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO22_23_Test_NonGrounded_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO22_23_Test_NonGrounded_Return = ValTemp.ToString();

                #endregion

                #region GPIO15-21 24-27

                IniHelper.GetIniStr("GPIO_Test", "GPIO15_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO15_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO15_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO15_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO16_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO16_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO16_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO16_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO17_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO17_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO17_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO17_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO18_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO18_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO18_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO18_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO19_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO19_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO19_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO19_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO20_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO20_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO20_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO20_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO21_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO21_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO21_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO21_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO24_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO24_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO24_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO24_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO25_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO25_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO25_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO25_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO26_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO26_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO26_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO26_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO27_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO27_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO27_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO27_Test_Return = ValTemp.ToString();

                #endregion

                #region GPIO42-55

                IniHelper.GetIniStr("GPIO_Test", "GPIO42_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO42_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO42_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO42_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO43_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO43_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO43_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO43_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO44_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO44_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO44_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO44_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO45_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO45_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO45_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO45_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO46_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO46_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO46_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO46_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO47_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO47_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO47_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO47_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO48_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO48_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO48_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO48_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO49_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO49_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO49_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO49_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO50_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO50_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO50_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO50_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO51_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO51_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO51_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO51_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO52_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO52_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO52_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO52_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO53_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO53_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO53_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO53_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO54_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO54_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO54_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO54_Test_Return = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO55_Test_PostData", "null", ValTemp, 500, str_IniPath);
                str_GPIO55_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("GPIO_Test", "GPIO55_Test_Return", "null", ValTemp, 500, str_IniPath);
                str_GPIO55_Test_Return = ValTemp.ToString();


                #endregion

                #endregion





            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取{str_CaseProjectName} Encrypt ini failed:{ex.StackTrace}");
                UIHandleHelper.ShowRunLog($"读取{str_CaseProjectName} Encrypt ini failed:{ex.StackTrace}", true);
                return false;
            }

            return true;
        }


        public bool ReadTestIni()
        {
            StringBuilder ValTemp = new StringBuilder();
            string str_CaseFolder = new string(str_CaseProjectName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{str_CaseProjectName}\\{str_CaseProjectName}_Normal.ini";

            try
            {

                if (File.Exists(str_IniPath) == false)
                {
                    throw new ArgumentException($"{str_CaseProjectName}_Normal.ini文件不存在。");
                }

                #region [Path_Config]


                #endregion




            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取{str_CaseProjectName} Normal ini failed:{ex.StackTrace}");
                UIHandleHelper.ShowRunLog($"读取{str_CaseProjectName} Normal ini failed:{ex.StackTrace}", false);
                return false;
            }

            return true;
        }


        public async Task<bool> Http_Post_Async(string str_Titel, string cmd, string content, string str_Expected)
        {
            UIHandleHelper.ShowRunLog($"============={str_Titel}===============");
            

            string url = cmd;

            if (string.IsNullOrEmpty(content))
            {
                content = "";
            }

            UIHandleHelper.ShowRunLog($"Cmd: {url}; Body: {content}");

            try
            {
                string responseJson = await httpClientHelper.PostJsonAsync(url, content);

                UIHandleHelper.ShowRunLog($"Post recv: {responseJson}");

                if (responseJson.Replace(" ", "").Contains(str_Expected.Replace(" ", "")))
                {

                    UIHandleHelper.ShowRunLog($"{str_Titel} pass: {responseJson}",
                                                              FailColor: false,
                                                              ShowGridView: true,
                                                              TestItemName: $"{str_Titel}",
                                                              TestItemContent: $"{str_Titel} pass: {responseJson}",
                                                              TestItemResult: true);
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"{str_Titel} fail: {responseJson}",
                                                             FailColor: true,
                                                             ShowGridView: true,
                                                             TestItemName: $"{str_Titel}",
                                                             TestItemContent: $"{str_Titel} fail: {responseJson}",
                                                             TestItemResult: false);
                }

            }
            catch (HttpRequestException e)
            {
                UIHandleHelper.ShowRunLog($"Post request error: {e.Message}",
                                                             FailColor: true,
                                                             ShowGridView: true,
                                                             TestItemName: $"{str_Titel}",
                                                             TestItemContent: $"Post request error: {e.Message}",
                                                             TestItemResult: false);
            }

            str_ErrorCode = TestErrorCode.ErrorCode.Err058.ToString();

            return false;
        }

        
        public bool CreateIperfBat(string str_IperfPath, string str_Content, string str_BatName)
        {
            string str_Temp = "";

            try
            {
                UIHandleHelper.ShowRunLog("Create Iperf Bat...");
                str_Temp = "cd /d" + str_IperfPath + "\r\n" + str_Content + "\r\n";

                TextWriter Filewriter;//以寫方式打開文件
                Filewriter = File.CreateText($"{FileProcessHelper.GetCurrentExeDirectory()}\\{str_BatName}");//創建或打開一個UTF-8的文件
                Filewriter.Write(str_Temp);//寫入
                Filewriter.Close();

            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Create Iperf Bat error: {ex}", true);
                return false;
            }

            UIHandleHelper.ShowRunLog("Create Iperf flow bat ok.");
            return true;
        }

        public void ExecuteBatchFileNoWait(string str_BatPathAndName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = str_BatPathAndName,
                UseShellExecute = true,
                CreateNoWindow = true
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
        }

        public string InsertEveryTwoCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                result.Append(input[i]);
                if ((i + 1) % 2 == 0 && (i + 1) != input.Length)
                    result.Append(':');
            }
            return result.ToString();
        }

        public bool BleTest()
        {
            bool bResult = false;
            string str_Python_Output = null;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Thread.Sleep(iCommand_Send_Interval_Second);
                    Task<bool> httpTask_Ble = Http_Post_Async("Start BLE Advertising", str_BLE_Test_Command, str_BLE_Test_PostData, str_BLE_Test_Return);
                    httpTask_Ble.Wait();
                    bResult = httpTask_Ble.Result;

                    if (bResult)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    ReConnectWifi();
                    WebServicesTest();
                }
            }



            if (bResult)
            {
                for (int i = 0; i < 3; i++)
                {
                    UIHandleHelper.ShowRunLog("Check ble mac and rssi...");

                    string str_workingDirectory = str_BLE_Test_Python_Path;
                    ExeProcessOnceHelper exeProcessHelper = new("python.exe", str_workingDirectory);
                    bResult = exeProcessHelper.RunCommandLine($" {str_BleFolderPath}\\{str_BleFileName}", out string outputData, out string errorData, 100000);

                    if (bResult)
                    {
                        str_Python_Output = outputData;
                        UIHandleHelper.ShowRunLog("python out: \r\n" + outputData);
                        UIHandleHelper.ShowRunLog("python err: \r\n" + errorData);

                        if (outputData is not null && outputData.ToLower().Contains(InsertEveryTwoCharacters(struct_Barcode.stru_str_sRevDUTBD).ToLower()))
                        {
                            bResult = true;
                            UIHandleHelper.ShowRunLog($"Check ble mac Pass, {struct_Barcode.stru_str_sRevDUTBD};",
                                                                       FailColor: false,
                                                                       ShowGridView: true,
                                                                       TestItemName: "Check ble mac",
                                                                       TestItemContent: $"Check ble mac Pass, {struct_Barcode.stru_str_sRevDUTBD};",
                                                                       TestItemResult: true);
                        }
                        else
                        {
                            bResult = false;
                            UIHandleHelper.ShowRunLog($"Check ble mac Fail, expect:{struct_Barcode.stru_str_sRevDUTBD};",
                                                                       FailColor: true,
                                                                       ShowGridView: true,
                                                                       TestItemName: "Check ble mac",
                                                                       TestItemContent: $"Check ble mac Fail, expect:{struct_Barcode.stru_str_sRevDUTBD};",
                                                                       TestItemResult: false,
                                                                       ErrorCode: TestErrorCode.ErrorCode.Err020.ToString());
                        }

                    }
                    else
                    {
                        bResult = false;
                        UIHandleHelper.ShowRunLog("Check ble mac Error",
                                                                      FailColor: true,
                                                                      ShowGridView: true,
                                                                      TestItemName: "Check ble mac",
                                                                      TestItemContent: "Check ble mac Error",
                                                                      TestItemResult: false,
                                                                      ErrorCode: TestErrorCode.ErrorCode.Err020.ToString());
                    }

                    //检查RSSI
                    if (bResult)
                    {
                        //44:3E:07:5B:01:B3: BLE TEST -28
                        bResult = false;
                        string[] lines = str_Python_Output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (var line in lines)
                        {
                            string str_mark = $"{InsertEveryTwoCharacters(struct_Barcode.stru_str_sRevDUTBD)}: BLE TEST";
                            if (line.Contains(str_mark))
                            {
                                string str_parts = line.Substring(line.IndexOf(str_mark) + str_mark.Length);
                                if (str_parts.Contains("-") && int.TryParse(str_parts, out int value))
                                {
                                    if (value >= iBT_RSSI_dBm_LowerLimit && value <= iBT_RSSI_dBm_UpperLimit)
                                    {
                                        UIHandleHelper.ShowRunLog($"Check ble rssi pass: {value} [{iBT_RSSI_dBm_LowerLimit}, {iBT_RSSI_dBm_UpperLimit}]",
                                                                          FailColor: false,
                                                                          ShowGridView: true,
                                                                          TestItemName: "Check ble rssi",
                                                                          TestItemContent: $"Check ble rssi pass: {value} [{iBT_RSSI_dBm_LowerLimit}, {iBT_RSSI_dBm_UpperLimit}]",
                                                                          TestItemResult: true);
                                        bResult = true;
                                    }
                                    else
                                    {
                                        UIHandleHelper.ShowRunLog($"Check ble rssi fail: {value} [{iBT_RSSI_dBm_LowerLimit}, {iBT_RSSI_dBm_UpperLimit}]",
                                                                        FailColor: true,
                                                                        ShowGridView: true,
                                                                        TestItemName: "Check ble rssi",
                                                                        TestItemContent: $"Check ble rssi fail: {value} [{iBT_RSSI_dBm_LowerLimit}, {iBT_RSSI_dBm_UpperLimit}]",
                                                                        TestItemResult: false,
                                                                        ErrorCode: TestErrorCode.ErrorCode.Err010.ToString());
                                        bResult = false;
                                    }
                                }
                                else
                                {
                                    UIHandleHelper.ShowRunLog("Check ble rssi fail: No ble mac was found",
                                                                        FailColor: true,
                                                                        ShowGridView: true,
                                                                        TestItemName: "Check ble rssi",
                                                                        TestItemContent: "Check ble rssi fail: No ble mac was found",
                                                                        TestItemResult: false,
                                                                        ErrorCode: TestErrorCode.ErrorCode.Err010.ToString());

                                    bResult = false;
                                }
                            }
                            

                        }
                    }


                    if (bResult)
                    {
                        break;
                    }

                }
            }


            return bResult;
        }

        public bool WifiConnectTest()
        {
            bool bResult = false;
            string str_Mac = InsertEveryTwoCharacters(struct_Barcode.stru_str_sRevDUTMac).ToLower();
            int iSignalStrength_WIFI = 0;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    DevconHelper.DisableByName(struct_NormalINI.stru_str_DeviceName1);
                    Thread.Sleep(100);
                    DevconHelper.EnableByName(struct_NormalINI.stru_str_DeviceName1);

                    DevconHelper.Rescan();

                    //wifiHelper.DisableWifiByName("Wireless");
                    //wifiHelper.EnableWifiByName("Wireless");
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowRunLog($"Wifi enable fail: {ex}", true,
                                                               ShowGridView: true,
                                                               TestItemName: "Wifi enable",
                                                               TestItemContent: $"Wifi enable fail:{ex}",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err010.ToString()
                                                               );
                    //return false;
                }

                Thread.Sleep(3000);
                var Result = wifiHelper.ConnectWifi(str_WifiPartName + str_Mac, "");
                bResult = Result.Item1;

                if (bResult)
                {

                    /*
                     Name                   : Wi-Fi 1
                    Description            : Tenda Wireless USB Adapter
                    GUID                   : d365f103-eb9c-4ebc-8b8b-f9d5e630cc9b
                    Physical address       : c8:3a:35:1e:88:20
                    State                  : connected
                    SSID                   : NAS_2G
                    BSSID                  : 1c:87:2c:e5:b6:b8
                    Network type           : Infrastructure
                    Radio type             : 802.11n
                    Authentication         : WPA2-Personal
                    Cipher                 : CCMP
                    Connection mode        : Profile
                    Channel                : 1
                    Receive rate (Mbps)    : 72
                    Transmit rate (Mbps)   : 72
                    Signal                 : 100%
                    Profile                : NAS_2G
                     */

                    //CustomProcess customProcess = new CustomProcess("netsh.exe");

                    //bool bRetVal = customProcess.RunCommandLine(" wlan show interfaces", $"{str_WifiPartName + str_Mac}", out string msg, 2000, 1);


                    int signalStrength = wifiHelper.GetSignalPercentage($"{str_WifiPartName + str_Mac}");

                    iSignalStrength_WIFI = signalStrength;

                    if (iSignalStrength_WIFI >= iWIFI_RSSI_Percent_LowerLimit)
                    {
                        UIHandleHelper.ShowRunLog($"Wifi rssi test pass: {iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -]",
                                                                   ShowGridView: true,
                                                                   TestItemName: "Wifi Rssi",
                                                                   TestItemContent: $"{iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -]",
                                                                   TestItemResult: true);
                        bResult = true;
                        break;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"Wifi rssi test fail: {iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -]", true,
                                                                   ShowGridView: true,
                                                                   TestItemName: "Wifi Rssi",
                                                                   TestItemContent: $"{iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -]",
                                                                   TestItemResult: false);

                        str_ErrorCode = TestErrorCode.ErrorCode.Err010.ToString();
                        bResult = false;
                    }







                    //iSignalStrength_WIFI = Result.Item2;

                    //if (iSignalStrength_WIFI >= iWIFI_RSSI_Percent_LowerLimit)
                    //{
                    //    UIHandleHelper.ShowRunLog($"Wifi rssi test pass: {iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -]",
                    //                                               ShowGridView: true,
                    //                                               TestItemName: "Wifi Rssi",
                    //                                               TestItemContent: $"{iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -]",
                    //                                               TestItemResult: true);
                    //    bResult = true;
                    //    break;
                    //}
                    //else
                    //{
                    //    UIHandleHelper.ShowRunLog($"Wifi rssi test fail: {iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -]", true,
                    //                                               ShowGridView: true,
                    //                                               TestItemName: "Wifi Rssi",
                    //                                               TestItemContent: $"{iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -]",
                    //                                               TestItemResult: false);

                    //    str_ErrorCode = TestErrorCode.ErrorCode.Err010.ToString();
                    //    bResult = false;
                    //}
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Wifi connect test fail", true,
                                                                   ShowGridView: true,
                                                                   TestItemName: "Wifi Connect",
                                                                   TestItemContent: "Connect test fail",
                                                                   TestItemResult: false);

                    str_ErrorCode = TestErrorCode.ErrorCode.Err001.ToString();
                    bResult = false;
                }


            }


            return bResult;
        }

        #region FWVerCheck

        public async Task<bool> Http_Get_Async(string str_Titel, string cmd, string str_Expected)
        {
            UIHandleHelper.ShowRunLog($"============={str_Titel}===============");

            string url = cmd;

            UIHandleHelper.ShowRunLog($"Cmd: {url}");

            try
            {
                var httpgetr = await getter.GetDataFromUrlAsync(url, str_Expected);

                if (httpgetr)
                {
                    UIHandleHelper.ShowRunLog($"{str_Titel} test pass: {str_Expected}",
                                                               ShowGridView: true,
                                                               TestItemName: $"{str_Titel}",
                                                               TestItemContent: $"{str_Titel} test pass: {str_Expected}",
                                                               TestItemResult: true);
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"{str_Titel} test fail",
                                                                   FailColor: true,
                                                                   ShowGridView: true,
                                                                   TestItemName: $"{str_Titel}",
                                                                   TestItemContent: $"{str_Titel} test fail",
                                                                   TestItemResult: false);

                    str_ErrorCode = TestErrorCode.ErrorCode.Err030.ToString();
                }


            }
            catch (HttpRequestException e)
            {
                UIHandleHelper.ShowRunLog($"{str_Titel} test error",
                                                                   FailColor: true,
                                                                   ShowGridView: true,
                                                                   TestItemName: $"{str_Titel}",
                                                                   TestItemContent: $"Get request error: {e.Message}",
                                                                   TestItemResult: false);

                str_ErrorCode = TestErrorCode.ErrorCode.Err030.ToString();
            }

            return false;
        }

        public bool CheckFWVer()
        {
            bool bResult = false;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Task<bool> httpTask_FWVer = Http_Get_Async("Check FW Ver", str_Get_Version_Command, str_FW_Version);
                    httpTask_FWVer.Wait();
                    bResult = httpTask_FWVer.Result;

                    if (bResult)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    bResult = false;
                    ReConnectWifi();
                }
            }


            return bResult;
        }


        #endregion

        public bool WebServicesTest()
        {
            bool bResult = false;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Thread.Sleep(iCommand_Send_Interval_Second);
                    Task<bool> httpTask_WebServices = Http_Post_Async("Start Test Web Services", str_Web_Services_Command, null, str_Web_Services_Return);
                    httpTask_WebServices.Wait();
                    bResult = httpTask_WebServices.Result;

                    if (bResult)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    bResult = false;
                    ReConnectWifi();
                }
            }
           

            return bResult;
        }




        public string[] CreateStringArrayWithParams(params string[] strings)
        {
            // 直接返回传入的字符串数组
            return strings;
        }


        // 添加空格格式化（每4位加一个空格）
        public string AddSpaces(string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    sb.Append(" ");
                }
                sb.Append(input[i]);
            }
            return sb.ToString();
        }


        public virtual bool GPIOTest()
        {
            bool bResult = false;


            string[] Arrary_GPIO_0To12_PostData = CreateStringArrayWithParams(str_GPIO0_Test_PostData, str_GPIO1_Test_PostData, str_GPIO2_Test_PostData,
                                                                                                     str_GPIO3_Test_PostData, str_GPIO4_Test_PostData, str_GPIO5_Test_PostData,
                                                                                                     str_GPIO6_Test_PostData, str_GPIO7_Test_PostData, str_GPIO8_Test_PostData,
                                                                                                     str_GPIO9_Test_PostData, str_GPIO10_Test_PostData, str_GPIO11_Test_PostData,
                                                                                                     str_GPIO12_Test_PostData
                                                                                                    );

            string[] Arrary_GPIO_0To12_Return = CreateStringArrayWithParams(str_GPIO0_Test_Return, str_GPIO1_Test_Return, str_GPIO2_Test_Return,
                                                                                                     str_GPIO3_Test_Return, str_GPIO4_Test_Return, str_GPIO5_Test_Return,
                                                                                                     str_GPIO6_Test_Return, str_GPIO7_Test_Return, str_GPIO8_Test_Return,
                                                                                                     str_GPIO9_Test_Return, str_GPIO10_Test_Return, str_GPIO11_Test_Return,
                                                                                                     str_GPIO12_Test_Return
                                                                                                    );



            string[] Arrary_GPIO_15To21_PostData = CreateStringArrayWithParams(str_GPIO15_Test_PostData, str_GPIO16_Test_PostData, str_GPIO17_Test_PostData,
                                                                                                   str_GPIO18_Test_PostData, str_GPIO19_Test_PostData, str_GPIO20_Test_PostData,
                                                                                                   str_GPIO21_Test_PostData
                                                                                                  );

            string[] Arrary_GPIO_15To21_Return = CreateStringArrayWithParams(str_GPIO15_Test_Return, str_GPIO16_Test_Return, str_GPIO17_Test_Return,
                                                                                                     str_GPIO18_Test_Return, str_GPIO19_Test_Return, str_GPIO20_Test_Return,
                                                                                                     str_GPIO21_Test_Return
                                                                                                    );



            string[] Arrary_GPIO_24To27_PostData = CreateStringArrayWithParams(str_GPIO24_Test_PostData, str_GPIO25_Test_PostData, str_GPIO26_Test_PostData,
                                                                                                 str_GPIO27_Test_PostData
                                                                                                );

            string[] Arrary_GPIO_24To27_Return = CreateStringArrayWithParams(str_GPIO24_Test_Return, str_GPIO25_Test_Return, str_GPIO26_Test_Return,
                                                                                                     str_GPIO27_Test_Return
                                                                                                    );


            string[] Arrary_GPIO_42To55_PostData = CreateStringArrayWithParams(str_GPIO42_Test_PostData, str_GPIO43_Test_PostData, str_GPIO44_Test_PostData,
                                                                                                   str_GPIO45_Test_PostData, str_GPIO46_Test_PostData, str_GPIO47_Test_PostData,
                                                                                                   str_GPIO48_Test_PostData, str_GPIO49_Test_PostData, str_GPIO50_Test_PostData,
                                                                                                   str_GPIO51_Test_PostData, str_GPIO52_Test_PostData, str_GPIO53_Test_PostData,
                                                                                                   str_GPIO54_Test_PostData, str_GPIO55_Test_PostData
                                                                                                  );

            string[] Arrary_GPIO_42To55_Return = CreateStringArrayWithParams(str_GPIO42_Test_Return, str_GPIO43_Test_Return, str_GPIO44_Test_Return,
                                                                                                     str_GPIO45_Test_Return, str_GPIO46_Test_Return, str_GPIO47_Test_Return,
                                                                                                     str_GPIO48_Test_Return, str_GPIO49_Test_Return, str_GPIO50_Test_Return,
                                                                                                     str_GPIO51_Test_Return, str_GPIO52_Test_Return, str_GPIO53_Test_Return,
                                                                                                     str_GPIO54_Test_Return, str_GPIO55_Test_Return
                                                                                                    );


            // 创建控制器
            using var gpioCard_001 = new RockMongSimpleGpioController(iGPIOTestBoardPull, 3602001);
            using var gpioCard_002 = new RockMongSimpleGpioController(iGPIOTestBoardPull, 3602002);

            // 初始字符串（包含空格）原始GPIO值
            string binaryStr = "1111 1111 1111 1111 1111 1111 1111 1111 1111 11"; //38个GPIO要测试

            // 去除空格用于处理
            string workingStr = binaryStr.Replace(" ", "");
            string str_TargetPinStatus = "";

            COM1.Open(true);


            #region GPIO0-12

            for (int i = 0; i < Arrary_GPIO_0To12_PostData.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        Thread.Sleep(iCommand_Send_Interval_Second);
                        Task<bool> httpTask_GPIO = Http_Post_Async($"GPIO{i} Test", str_GPIO_Test_Command, Arrary_GPIO_0To12_PostData[i], Arrary_GPIO_0To12_Return[i].Replace(" ", ""));
                        httpTask_GPIO.Wait();
                        bResult = httpTask_GPIO.Result;

                        if (bResult)
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        bResult = false;
                        ReConnectWifi();
                        WebServicesTest();
                    }
                }


                if (!bResult)
                {
                    str_ErrorCode = TestErrorCode.ErrorCode.Err032.ToString();
                    break;
                }

                Task.Delay(100);


                for (int k = 0; k < 3; k++)
                {
                    #region 读取每个GPIO电平(3.3V)   40个
                    //接线对用关系
                    //产品GPIO 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
                    //读取板Pin 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12

                    //0.7*3.3=2.31V以上是高电平   0.3*3.3=0.99V以下，是低电平
                    //0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 15, 16, 17, 18, 19, 20, 21, 24, 25, 26, 27, 42, 43, 44, 45, 46, 47, 48, 49
                    bool[] allPinStates_001 = gpioCard_001.ReadAllPins();
                    string str_PinStates_001 = string.Join("", allPinStates_001.Select(b => b ? "1" : "0"));

                    //读取多个指定引脚  //50, 51, 52, 53, 54, 55
                    var pinsToRead = new[] { 0, 1, 2, 3, 4, 5};
                    bool[] multiplePinStates_002 = gpioCard_002.ReadMultiplePins(pinsToRead);
                    string str_PinStates_002 = string.Join("", multiplePinStates_002.Select(b => b ? "1" : "0"));

                    string str_ReadAllPinStates = AddSpaces(str_PinStates_001 + str_PinStates_002);
                    UIHandleHelper.ShowRunLog($"Read GPIO Status: {str_ReadAllPinStates}");

                    #endregion

                    #region 需要达到的目标GPIO状态_读取板从pin0开始_到pin12(含)结束

                    // 创建字符数组便于修改
                    char[] chars = workingStr.ToCharArray();
                    // 将当前位设为'0'
                    chars[i] = '0';
                    // 如果不是第一位，将前一位恢复为'1'
                    if (i > 0)
                    {
                        chars[i - 1] = '1';
                    }
                    str_TargetPinStatus = AddSpaces(new string(chars));
                    UIHandleHelper.ShowRunLog($"Target GPIO Status: {str_TargetPinStatus}");

                    #endregion

                    #region 比对结果

                    //比较结果
                    if (str_ReadAllPinStates != str_TargetPinStatus)
                    {
                        UIHandleHelper.ShowRunLog($"GPIO{i} States test fail;",
                                                                    FailColor: true,
                                                                    ShowGridView: true,
                                                                    TestItemName: $"GPIO{i} States",
                                                                    TestItemContent: $"{str_ReadAllPinStates}",
                                                                    TestItemResult: false,
                                                                    ErrorCode: TestErrorCode.ErrorCode.Err032.ToString()
                                                                    );
                        bResult = false;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"GPIO{i} States test pass;",
                                                                    FailColor: false,
                                                                    ShowGridView: true,
                                                                    TestItemName: $"GPIO{i} States",
                                                                    TestItemContent: $"{str_ReadAllPinStates}",
                                                                    TestItemResult: true
                                                                    );
                        bResult = true;
                        break;
                    }

                    #endregion

                }

                // k循环结束后检查
                if (!bResult)
                {
                    break; // 退出i大循环
                }

            }



            #endregion

            #region GPIO15-21

            if (bResult)
            {
                for (int i = 15; i < Arrary_GPIO_15To21_PostData.Length + 15; i++)
                {

                    for (int j = 0; j < 3; j++)
                    {
                        try
                        {
                            Thread.Sleep(iCommand_Send_Interval_Second);
                            Task<bool> httpTask_GPIO = Http_Post_Async($"GPIO{i} Test", str_GPIO_Test_Command, Arrary_GPIO_15To21_PostData[i - 15], Arrary_GPIO_15To21_Return[i - 15].Replace(" ", ""));
                            httpTask_GPIO.Wait();
                            bResult = httpTask_GPIO.Result;

                            if (bResult)
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            bResult = false;
                            ReConnectWifi();
                            WebServicesTest();
                        }
                    }



                    if (!bResult)
                    {
                        str_ErrorCode = TestErrorCode.ErrorCode.Err032.ToString();
                        break;
                    }

                    Task.Delay(100);
                    for (int k = 0; k < 3; k++)
                    {
                        #region 读取每个GPIO电平(3.3V)   40个

                        //接线对用关系
                        //产品GPIO 15, 16, 17, 18, 19, 20, 21
                        //读取板Pin 13, 14, 15, 16, 17, 18, 19

                        //0.7*3.3=2.31V以上是高电平   0.3*3.3=0.99V以下，是低电平
                        //0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 15, 16, 17, 18, 19, 20, 21, 24, 25, 26, 27, 42, 43, 44, 45, 46, 47, 48, 49
                        bool[] allPinStates_001 = gpioCard_001.ReadAllPins();
                        string str_PinStates_001 = string.Join("", allPinStates_001.Select(b => b ? "1" : "0"));

                        //读取多个指定引脚  //50, 51, 52, 53, 54, 55
                        var pinsToRead = new[] { 0, 1, 2, 3, 4, 5};
                        bool[] multiplePinStates_002 = gpioCard_002.ReadMultiplePins(pinsToRead);
                        string str_PinStates_002 = string.Join("", multiplePinStates_002.Select(b => b ? "1" : "0"));

                        string str_ReadAllPinStates = AddSpaces(str_PinStates_001 + str_PinStates_002);
                        UIHandleHelper.ShowRunLog($"Read GPIO Status: {str_ReadAllPinStates}");
                        #endregion

                        #region 目标GPIO状态_读取板从pin13开始_到pin19(含)结束
                        int iPinnums = i - 2; //GPIOpin13 14没有要读取，故读取板的pin13 14给GPIO15 16使用了
                        str_TargetPinStatus = "";
                        // 创建字符数组便于修改
                        char[] chars = workingStr.ToCharArray();
                        // 将当前位设为'0'
                        chars[iPinnums] = '0';
                        // 如果不是第一位，将前一位恢复为'1'
                        if (iPinnums > 0)
                        {
                            chars[iPinnums - 1] = '1';
                        }
                        str_TargetPinStatus = AddSpaces(new string(chars));
                        UIHandleHelper.ShowRunLog($"Target GPIO Status: {str_TargetPinStatus}");

                        #endregion

                        #region 比对结果

                        //比较结果
                        if (str_ReadAllPinStates != str_TargetPinStatus)
                        {
                            UIHandleHelper.ShowRunLog($"GPIO{i} States test fail;",
                                                                        FailColor: true,
                                                                        ShowGridView: true,
                                                                        TestItemName: $"GPIO{i} States",
                                                                        TestItemContent: $"{str_ReadAllPinStates}",
                                                                        TestItemResult: false,
                                                                        ErrorCode: TestErrorCode.ErrorCode.Err032.ToString()
                                                                        );
                            bResult = false;
                        }
                        else
                        {
                            UIHandleHelper.ShowRunLog($"GPIO{i} States test pass;",
                                                                        FailColor: false,
                                                                        ShowGridView: true,
                                                                        TestItemName: $"GPIO{i} States",
                                                                        TestItemContent: $"{str_ReadAllPinStates}",
                                                                        TestItemResult: true
                                                                        );
                            bResult = true;
                            break;
                        }

                        #endregion

                    }

                    // k循环结束后检查
                    if (!bResult)
                    {
                        break; // 退出i大循环
                    }

                }
            }

            #endregion

            #region GPIO22-23

            if (bResult)
            {
                COM1.RunCommandLine("2F", "");
                Thread.Sleep(500);
                COM1.RunCommandLine("3F", "");
                Thread.Sleep(500);
                COM1.RunCommandLine("2O", "");
                Thread.Sleep(1000);
                COM1.RunCommandLine("3O", "");
                Thread.Sleep(1000);


                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        Thread.Sleep(iCommand_Send_Interval_Second);
                        Task<bool> httpTask_GPIO22_23_Grounded = Http_Post_Async("GPIO22-23 Grounded Test", str_GPIO_Test_Command, str_GPIO22_23_Test_PostData, str_GPIO22_23_Test_Grounded_Return.Replace(" ", ""));
                        httpTask_GPIO22_23_Grounded.Wait();
                        bResult = httpTask_GPIO22_23_Grounded.Result;

                        if (bResult)
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        bResult = false;
                        ReConnectWifi();
                        WebServicesTest();
                    }
                }



                if (bResult)
                {
                    COM1.RunCommandLine("2F", "");
                    Thread.Sleep(500);
                    COM1.RunCommandLine("3F", "");
                    Thread.Sleep(500);


                    for (int j = 0; j < 3; j++)
                    {
                        try
                        {
                            Task<bool> httpTask_GPIO22_23_NonGrounded = Http_Post_Async("GPIO22-23 NonGrounded Test", str_GPIO_Test_Command, str_GPIO22_23_Test_PostData, str_GPIO22_23_Test_NonGrounded_Return);
                            httpTask_GPIO22_23_NonGrounded.Wait();
                            bResult = httpTask_GPIO22_23_NonGrounded.Result;

                            if (bResult)
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            bResult = false;
                            ReConnectWifi();
                            WebServicesTest();
                        }
                    }

                }


                if (!bResult)
                {
                    str_ErrorCode = TestErrorCode.ErrorCode.Err032.ToString();
                    return bResult;
                }
            }

            #endregion

            #region GPIO24-27

            if (bResult)
            {
                for (int i = 24; i < Arrary_GPIO_24To27_PostData.Length +24; i++)
                {

                    for (int j = 0; j < 3; j++)
                    {
                        try
                        {
                            Thread.Sleep(iCommand_Send_Interval_Second);
                            Task<bool> httpTask_GPIO = Http_Post_Async($"GPIO{i} Test", str_GPIO_Test_Command, Arrary_GPIO_24To27_PostData[i - 24], Arrary_GPIO_24To27_Return[i - 24].Replace(" ", ""));
                            httpTask_GPIO.Wait();
                            bResult = httpTask_GPIO.Result;

                            if (bResult)
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            bResult = false;
                            ReConnectWifi();
                            WebServicesTest();
                        }
                    }


                    if (!bResult)
                    {
                        str_ErrorCode = TestErrorCode.ErrorCode.Err032.ToString();
                        break;
                    }

                    Task.Delay(100);

                    for (int k = 0; k < 3; k++)
                    {
                        #region 读取每个GPIO电平(3.3V)   40个

                        //接线对用关系
                        //产品GPIO 24, 25, 26, 27
                        //读取板Pin 20, 21, 22, 23

                        //0.7*3.3=2.31V以上是高电平   0.3*3.3=0.99V以下，是低电平
                        //0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 15, 16, 17, 18, 19, 20, 21, 24, 25, 26, 27, 42, 43, 44, 45, 46, 47, 48, 49
                        bool[] allPinStates_001 = gpioCard_001.ReadAllPins();
                        string str_PinStates_001 = string.Join("", allPinStates_001.Select(b => b ? "1" : "0"));

                        //读取多个指定引脚  //50, 51, 52, 53, 54, 55 
                        var pinsToRead = new[] { 0, 1, 2, 3, 4, 5};
                        bool[] multiplePinStates_002 = gpioCard_002.ReadMultiplePins(pinsToRead);
                        string str_PinStates_002 = string.Join("", multiplePinStates_002.Select(b => b ? "1" : "0"));

                        string str_ReadAllPinStates = AddSpaces(str_PinStates_001 + str_PinStates_002);
                        UIHandleHelper.ShowRunLog($"Read GPIO Status: {str_ReadAllPinStates}");
                        #endregion

                        #region 目标GPIO状态_读取板从pin20开始_到pin23(含)结束

                        str_TargetPinStatus = "";
                        // 创建字符数组便于修改
                        int iPinnums = i - 4;
                        char[] chars = workingStr.ToCharArray();
                        // 将当前位设为'0'
                        chars[iPinnums] = '0';
                        // 如果不是第一位，将前一位恢复为'1'
                        if (iPinnums > 0)
                        {
                            chars[iPinnums - 1] = '1';
                        }
                        str_TargetPinStatus = AddSpaces(new string(chars));
                        UIHandleHelper.ShowRunLog($"Target GPIO Status: {str_TargetPinStatus}");

                        #endregion

                        #region 比对结果

                        //比较结果
                        if (str_ReadAllPinStates != str_TargetPinStatus)
                        {
                            UIHandleHelper.ShowRunLog($"GPIO{i} States test fail;",
                                                                        FailColor: true,
                                                                        ShowGridView: true,
                                                                        TestItemName: $"GPIO{i} States",
                                                                        TestItemContent: $"{str_ReadAllPinStates}",
                                                                        TestItemResult: false,
                                                                        ErrorCode: TestErrorCode.ErrorCode.Err032.ToString()
                                                                        );
                            bResult = false;
                        }
                        else
                        {
                            UIHandleHelper.ShowRunLog($"GPIO{i} States test pass;",
                                                                        FailColor: false,
                                                                        ShowGridView: true,
                                                                        TestItemName: $"GPIO{i} States",
                                                                        TestItemContent: $"{str_ReadAllPinStates}",
                                                                        TestItemResult: true
                                                                        );
                            bResult = true;
                            break;
                        }

                        #endregion
                    }

                    // k循环结束后检查
                    if (!bResult)
                    {
                        break; // 退出i大循环
                    }

                }
            }

            #endregion

            #region GPIO42-55

            if (bResult)
            {
                for (int i = 42; i < Arrary_GPIO_42To55_PostData.Length + 42; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        try
                        {
                            Thread.Sleep(iCommand_Send_Interval_Second);
                            Task<bool> httpTask_GPIO = Http_Post_Async($"GPIO{i} Test", str_GPIO_Test_Command, Arrary_GPIO_42To55_PostData[i - 42], Arrary_GPIO_42To55_Return[i - 42].Replace(" ", ""));
                            httpTask_GPIO.Wait();
                            bResult = httpTask_GPIO.Result;

                            if (bResult)
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            bResult = false;
                            ReConnectWifi();
                            WebServicesTest();
                        }
                    }


                    if (!bResult)
                    {
                        str_ErrorCode = TestErrorCode.ErrorCode.Err032.ToString();
                        break;
                    }

                    Task.Delay(100);


                    for (int k = 0; k < 3; k++)
                    {
                        #region 读取每个GPIO电平(3.3V)   40个

                        //接线对用关系
                        //产品GPIO 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55 
                        //读取板Pin 24, 25, 26, 27, 28, 29, 30, 31, 00, 01, 02, 03, 04 ,05

                        //0.7*3.3=2.31V以上是高电平   0.3*3.3=0.99V以下，是低电平
                        //0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 15, 16, 17, 18, 19, 20, 21, 24, 25, 26, 27, 42, 43, 44, 45, 46, 47, 48, 49
                        bool[] allPinStates_001 = gpioCard_001.ReadAllPins();
                        string str_PinStates_001 = string.Join("", allPinStates_001.Select(b => b ? "1" : "0"));

                        //读取多个指定引脚  //50, 51, 52, 53, 54, 55 
                        var pinsToRead = new[] { 0, 1, 2, 3, 4, 5 };
                        bool[] multiplePinStates_002 = gpioCard_002.ReadMultiplePins(pinsToRead);
                        string str_PinStates_002 = string.Join("", multiplePinStates_002.Select(b => b ? "1" : "0"));

                        string str_ReadAllPinStates = AddSpaces(str_PinStates_001 + str_PinStates_002);
                        UIHandleHelper.ShowRunLog($"Read GPIO Status: {str_ReadAllPinStates}");
                        #endregion

                        #region 目标GPIO状态
                        str_TargetPinStatus = "";
                        // 创建字符数组便于修改
                        int iPinnums = i - 18;
                        char[] chars = workingStr.ToCharArray();
                        // 将当前位设为'0'
                        chars[iPinnums] = '0';
                        // 如果不是第一位，将前一位恢复为'1'
                        if (iPinnums > 0)
                        {
                            chars[iPinnums - 1] = '1';
                        }
                        str_TargetPinStatus = AddSpaces(new string(chars));
                        UIHandleHelper.ShowRunLog($"Target Status: {str_TargetPinStatus}");

                        #endregion

                        #region 比对结果

                        //比较结果
                        if (str_ReadAllPinStates != str_TargetPinStatus)
                        {
                            UIHandleHelper.ShowRunLog($"GPIO{i} States test fail;",
                                                                        FailColor: true,
                                                                        ShowGridView: true,
                                                                        TestItemName: $"GPIO{i} States",
                                                                        TestItemContent: $"{str_ReadAllPinStates};",
                                                                        TestItemResult: false,
                                                                        ErrorCode: TestErrorCode.ErrorCode.Err032.ToString()
                                                                        );
                            bResult = false;
                        }
                        else
                        {
                            UIHandleHelper.ShowRunLog($"GPIO{i} States test pass;",
                                                                        FailColor: false,
                                                                        ShowGridView: true,
                                                                        TestItemName: $"GPIO{i} States",
                                                                        TestItemContent: $"{str_ReadAllPinStates}",
                                                                        TestItemResult: true
                                                                        );
                            bResult = true;
                            break;
                        }

                 
                        #endregion


                    }

                    // k循环结束后检查
                    if (!bResult)
                    {
                        break; // 退出i大循环
                    }
                }
               
            }
            #endregion

            return bResult;
        }


        public double CalculateVoltage(string hex1, string hex2)
        {
            /*
             正常会返回4位HEX，例如B3 02； B3和02转换成10机制数，B3 的十进制为A， 02的十进制为B， 电压V=（A*4+B)*5/1024;
             */
            // 将十六进制字符串转换为十进制数
            int decimal1 = Convert.ToInt32(hex1, 16);
            int decimal2 = Convert.ToInt32(hex2, 16);

            // 根据公式计算电压V
            double voltage = ((decimal1 * 4 + decimal2) * 5) / 1024.0;

            return Math.Round(voltage, 3);
        }

        public void ReConnectWifi()
        {
            //int iWifiSataus = wifiHelper.GetWifiConnectStatus();

            //if (iWifiSataus == 0)
            {
                UIHandleHelper.ShowRunLog("Wifi connection is disconnected, try reconnect...");
                string str_Mac = InsertEveryTwoCharacters(struct_Barcode.stru_str_sRevDUTMac).ToLower();
                var Result = wifiHelper.ConnectWifi(str_WifiPartName + str_Mac, "");
                bool bResult = Result.Item1;
                Thread.Sleep(8000);
            }

        }

        public bool CheckIperfResult()
        {
            bool bResult = false;

            string filePath = $"{str_IperfFolderPath}\\{str_IperfLogName}";

            if (System.IO.File.Exists(filePath) == false)
            {
                UIHandleHelper.ShowRunLog("Check iperf fail: iperf result file exists is false;",
                                                                        FailColor: true,
                                                                        ShowGridView: true,
                                                                        TestItemName: "Check iperf",
                                                                        TestItemContent: "Check iperf fail: iperf result file exists is false;",
                                                                        TestItemResult: false,
                                                                        ErrorCode: TestErrorCode.ErrorCode.Err023.ToString());
                return false;
            }

            try
            {
                //0.0-10.0 sec  6.47 MBytes  5.43 Mbits/sec
                string content = System.IO.File.ReadAllText(filePath);

                if (content.IndexOf("bits/sec") == -1 || content.IndexOf("Bytes") == -1)
                {
                    throw new Exception("No rate keyword found.");
                }

                string str_Speed_temp = content.Substring(content.IndexOf("Bytes") + 5, content.IndexOf("bits/sec") - content.IndexOf("Bytes") - 5);

                if (string.IsNullOrEmpty(str_Speed_temp) == false)
                {
                    if (str_Speed_temp.IndexOf("M") != -1)
                    {
                        string str_Speed = str_Speed_temp.Replace("M", "");
                        if (float.Parse(str_Speed) >= iIperf_LowerLimit)
                        {
                            UIHandleHelper.ShowRunLog($"Check iperf speed pass: {float.Parse(str_Speed)} Mbits/sec [{iIperf_LowerLimit}, ]",
                                                                         FailColor: false,
                                                                         ShowGridView: true,
                                                                         TestItemName: "iperf speed",
                                                                         TestItemContent: $"Check iperf speed pass: {float.Parse(str_Speed)} Mbits/sec [{iIperf_LowerLimit}, ]",
                                                                         TestItemResult: true);
                            bResult = true;
                        }
                        else
                        {
                            UIHandleHelper.ShowRunLog($"Check iperf speed fail: {float.Parse(str_Speed)} Mbits/sec [{iIperf_LowerLimit}, ]",
                                                                       FailColor: true,
                                                                       ShowGridView: true,
                                                                       TestItemName: "iperf speed",
                                                                       TestItemContent: $"Check iperf speed fail: {float.Parse(str_Speed)} Mbits/sec [{iIperf_LowerLimit}, ]",
                                                                       TestItemResult: false,
                                                                       ErrorCode: TestErrorCode.ErrorCode.Err023.ToString());
                            bResult = false;
                        }
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"Check iperf speed fail: {float.Parse(str_Speed_temp)} bits/sec [{iIperf_LowerLimit}, ]",
                                                                      FailColor: true,
                                                                      ShowGridView: true,
                                                                      TestItemName: "iperf speed",
                                                                      TestItemContent: $"Check iperf speed fail: {float.Parse(str_Speed_temp)} bits/sec [{iIperf_LowerLimit}, ]",
                                                                      TestItemResult: false,
                                                                      ErrorCode: TestErrorCode.ErrorCode.Err023.ToString());
                        bResult = false;
                    }
                }
                else
                {
                    throw new Exception("Failed to obtain rate information.");
                }


            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Check iperf fail: Check Result File Error: {ex.Message};",
                                                                                       FailColor: true,
                                                                                       ShowGridView: true,
                                                                                       TestItemName: "Check iperf",
                                                                                       TestItemContent: $"Check iperf fail: Check Result File Error: {ex.Message}",
                                                                                       TestItemResult: false,
                                                                                       ErrorCode: TestErrorCode.ErrorCode.Err023.ToString());
                bResult = false;
            }

            return bResult;
        }


    }
}
