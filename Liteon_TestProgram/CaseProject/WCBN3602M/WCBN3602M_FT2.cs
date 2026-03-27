using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using static Liteon_TestProgram.Forms.TestForm;
using Liteon_TestProgram.Utilities.IOHelpers.Extensions;
using Liteon_TestProgram.InstrumentControl;
using System.IO;
using Liteon_TestProgram.Utilities.IOHelpers;
using System.Reflection;
using FluentFTP.Helpers;
using System.Text.RegularExpressions;
using Liteon_TestProgram.TestFunc.Litepoint;
using Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V2;
using static Liteon_TestProgram.Base.Class_Variable;
using Renci.SshNet.Sftp;
using Liteon_TestProgram.Save_LogFile;
using SkiaSharp;
using static System.Collections.Specialized.BitVector32;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using System.Security.Cryptography;
using Liteon_TestProgram.Utilities.HttpClient;
using Liteon_TestProgram.Utilities.WIFIHelper;
using Liteon_TestProgram.Utilities.PEM;
using static OfficeOpenXml.ExcelErrorValue;
using ScottPlot.Colormaps;


namespace Liteon_TestProgram.CaseProject
{
    internal class WCBN3602M_FT2 : CaseCodeBase
    {
        #region RF测试

        LitepointFlowTestHelper litepointHelper = new();

        public static string str_IQTestProgramFolderName = "IQfact_NXP_W61x_4.0.0.6.2_Lock";
        public string str_IQTestProgramFolderPath = $"{PathHelper.GetExeParentDirectory()}\\{str_IQTestProgramFolderName}";

        public static string str_LabtoolFolderName = "labtool";
        public string str_LabtoolExeName = "DutApiSisoApApp_RW610.exe";
        public string str_LabtoolFolderPath = $"{PathHelper.GetExeParentDirectory()}\\{str_IQTestProgramFolderName}\\{str_LabtoolFolderName}";

        public static string str_TestFWFolderName = "Test_Firmware";
        public string str_TestFWBurnBatName = "prog_flash_RW610_Test.bat";
        public string str_TestFWFolderPath = $"{PathHelper.GetParentDirectoryPath(PathHelper.GetCurrentExeDirPath())}\\{str_TestFWFolderName}";

        public static string str_CustomerFWFolderName = "Customer_Firmware";
        public string str_CustomerFWFolderPath = $"{PathHelper.GetParentDirectoryPath(PathHelper.GetCurrentExeDirPath())}\\{str_CustomerFWFolderName}";

        public string str_WIFI_BDF_Name = null;
        public string str_WIFI_BDF_MD5 = null;
        public string str_BT_BDF_Name = null;
        public string str_BT_BDF_MD5 = null;
        public int iFlash_CustomerFW = 1;
        public int iFlash_TestFW = 0;

        #endregion

        public int iClearSerialization = 1;
        public string str_UpLoad_ITDB_IP = null;


        #region 客户FW功能测试

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

        public int iCommand_Send_Interval_Second = 0;

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


        #endregion



        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get;  set; }
        public TestForm _testForm;
        string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public WCBN3602M_FT2(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.目前测试FW仅支持GD烧录IC");
            list_str.Add("2.(2025-04-25)变更客户FW: NIUV_v1.0.0Vrc1_A29040201A-S00104401A_supplier");
            list_str.Add("3.(2025-05-20)变更客户FW: NIUV_v1.0.0Vrc2_A29040201A-S00104401B_supplier");
            list_str.Add("4.(2025-07-14)变更客户FW: NIUV_v1.0.0Vrc3_full_flash_support_no_secureboot");
            list_str.Add("5.(2025-08-12)变更客户FW: NIUV_v1.0.0V_A29040201A-S00104402A_supplier");
            list_str.Add("6.(2025-11-07)将原先的RF测试和客户FW功能测试合站");
            list_str.Add("7.(2025-12-22)添加iperf测试");

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
            list_str.Add("2.COMA TB022小板 1O 1F控制上电，\r\n 2O 2F控制短接boot到GND，进入烧录客户固件模式\r\n");
            list_str.Add("3.COMB请填测试产品的串口\r\n");
            list_str.Add("4.测试电脑需要使用到无线网卡和蓝牙适配器.\r\n");
            list_str.Add("5.用到无线网卡名需要填到设备名称1中.\r\n");
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

            COM1.RunCommandLine("FF", "");

            bResult = ReadCustomerEncryptIni();

            return bResult;
        }

        public override bool Func_TestInit()
        {
            bool bInitResult = false;

            #region testinit的额外补充

            try
            {
                DevconHelper.DisableByName(struct_NormalINI.stru_str_DeviceName1);
                Thread.Sleep(100);



                #region 文件log处理

                FileProcessHelper.DeleteFile(@$"{str_IQTestProgramFolderPath}\Bin\Log\", "*.txt");
                FileProcessHelper.DeleteFile(@$"{str_IQTestProgramFolderPath}\Bin\Log\", "*.iqvsa");

                if (System.IO.File.Exists(@$"{str_IQTestProgramFolderPath}\labtool\Test.txt"))
                {
                    System.IO.File.Delete(@$"{str_IQTestProgramFolderPath}\labtool\Test.txt");
                }

                if (System.IO.File.Exists($"{str_IperfFolderPath}\\{str_IperfLogName}"))
                {
                    System.IO.File.Delete($"{str_IperfFolderPath}\\{str_IperfLogName}");
                }
                if (System.IO.File.Exists(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfBatName}"))
                {
                    System.IO.File.Delete(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfLogName}");
                }

        

                #endregion

                #region 检查BDF_MD5

                bInitResult = CheckBDF_Md5();

                //if (System.IO.File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\BDF\\RD_RW61x_CSP_FEM_1-Antenna.conf"))
                //{
                //    System.IO.File.Copy($"{str_IQTestProgramFolderPath}\\Bin\\BDF\\RD_RW61x_CSP_FEM_1-Antenna.conf", $"{str_IQTestProgramFolderPath}\\Bin\\RD_RW61x_CSP_FEM_1-Antenna.conf", true); // 第三个参数为true表示覆盖现有文件
                //}

                //if (System.IO.File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\BDF\\BtCalData_ext.conf"))
                //{
                //    System.IO.File.Copy($"{str_IQTestProgramFolderPath}\\Bin\\BDF\\BtCalData_ext.conf", $"{str_IQTestProgramFolderPath}\\Bin\\BtCalData_ext.conf", true); // 第三个参数为true表示覆盖现有文件
                //}

                #endregion

                #region 电源和烧录模式

                if (bInitResult)
                {
                    bInitResult = COM1.RunCommandLine("2F", "");
                    Thread.Sleep(500);
                }

                if (bInitResult)
                {
                    bInitResult = COM1.RunCommandLine("1F", "");
                    Thread.Sleep(500);
                }

                if (bInitResult)
                {
                    bInitResult = COM1.RunCommandLine("1O", "");
                    Thread.Sleep(5000);
                }



                #endregion

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
            UIHandleHelper.ShowRunLog("=============Test Flow===============");

            string str_FlowBatName = "";
            bool bTestResult = false;
            int iBoardStatus = -1;  //0：检查的mac为F，空板；1：检查的Mac和板子写的是一样的；2：检查不到Mac，可能是写了客户固件；3：不符合以上的情况判定为fail；
            bool bSecurtboot = false;



            #region 检查板子状态

            iBoardStatus = CheckInfo_Before();

            if (iBoardStatus == 2)  //2：检查不到Mac，可能是写了客户固件；
            {
                bTestResult = DealWithiBoardStatus_2(ref bSecurtboot);

                if (bTestResult && bSecurtboot)
                {
                   goto CustomerFWFun; //已经是客户FW，直接测试客户功能
                }
                else
                {
                    //=====也有可呢是没有烧录测试FW的板

                    if (iFlash_TestFW == 1)
                    {
                        #region 烧录测试固件

                        var testResult = TestFWBurn(str_TestFWFolderPath, str_TestFWBurnBatName);

                        bTestResult = testResult.Item1;

                        if (bTestResult)
                        {
                            if (testResult.Item2 == 0)
                            {
                                iBoardStatus = CheckInfo_Before();

                                if (iBoardStatus == 2 || iBoardStatus == 3)
                                {
                                    bTestResult = false;
                                }
                            }
                            else
                            {
                                iBoardStatus = 0;
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        bTestResult = false;
                    }

                }
            }
            else if (iBoardStatus == 3)  //3：不符合以上的情况判定为fail
            {
                bTestResult = DealWithiBoardStatus_3();

                return false;
            }
            else if (iBoardStatus == 0 || iBoardStatus == 1)  // //0：检查的mac为F，空板；1：检查的Mac和板子写的是一样的；
            {
                bTestResult = true;
            }
            else
            {
                bTestResult = false;
            }

            #endregion

            #region 清空上传序列化结果到ITDB

            if (bTestResult && iClearSerialization == 1)
            {
                int iResult = -1;

                iResult = UpdateSerializedResult(false);

                if (iResult == 1)
                {
                    bool bResult = true;
                    bTestResult = bResult && bTestResult;
                }
                else
                {
                    bool bResult = false;
                    bTestResult = bResult && bTestResult;
                }

            }

            #endregion


            #region 测试RF

            if (bTestResult)
            {                
                bTestResult = litepointHelper.ChangeSerialMAC(str_IQTestProgramFolderPath,
                                                                                        struct_Barcode.stru_str_sRevDUTMac,
                                                                                        struct_Barcode.stru_str_sRevDUTBD);

                if (bTestResult)
                {
                    if (iBoardStatus==0)
                    {
                        WriteSetUpINI(1);
                    }
                    else
                    {
                        WriteSetUpINI(2);
                    }
                    var bBatResult = litepointHelper.CreateLitePointBat(iBoardStatus == 0 ? true : false,
                                                                                                str_IQTestProgramFolderPath, "IQfactRun_Console.exe",
                                                                                                struct_NormalINI.stru_str_TesterPort, true);

                    if (bBatResult.Item1)
                    {
                        bTestResult = true;
                        str_FlowBatName = bBatResult.Item2;
                    }
                    else
                    {
                        bTestResult = false;
                    }
                }

                if (bTestResult)
                {
                    COM1.RunCommandLine("1F", "");
                    Thread.Sleep(500);
                    COM1.RunCommandLine("1O", "");
                    Thread.Sleep(5000);

                    _testForm.Multi_lockStatus();
                    bTestResult = RFTest(str_FlowBatName);


                    if (bTestResult == false)
                    {
                        COM1.RunCommandLine("1F", "");
                        Thread.Sleep(500);
                        _testForm.OpenCloseShieldingBox(true);
                        _testForm.OpenCloseShieldingBox(false);
                        COM1.RunCommandLine("1O", "");
                        Thread.Sleep(5000);

                        bTestResult = RFTest(str_FlowBatName);
                    }
                    _testForm.Multi_releaseStatus();
                }


                if (bTestResult)
                {
                    WriteSetUpINI(2);
                    COM1.RunCommandLine("1F", "");
                    Thread.Sleep(500);
                    COM1.Close(true);
                    COM1.RunCommandLine("1O", "");
                    Thread.Sleep(5000);

                    var bBatResult = litepointHelper.CreateLitePointReadMacBat(str_IQTestProgramFolderPath, "IQ_ReadMac", "IQfactRun_Console.exe", true);

                    if (bBatResult.Item1)
                    {
                        bTestResult = true;
                        str_FlowBatName = bBatResult.Item2;
                    }
                    else
                    {
                        bTestResult = false;
                    }


                }

            }

            #endregion

            #region 检查IDs

            if (bTestResult)
            {
                bTestResult = CheckMacBT(str_FlowBatName);
                if (bTestResult == false)
                {

                    COM1.RunCommandLine("1F", "");
                    Thread.Sleep(500);
                    COM1.Close(true);
                    _testForm.OpenCloseShieldingBox(true);
                    _testForm.OpenCloseShieldingBox(false);
                    COM1.RunCommandLine("1O", "");
                    Thread.Sleep(5000);
                    bTestResult = CheckMacBT(str_FlowBatName);
                }
            }

            #endregion

            #region 烧录客户固件


            if (bTestResult && iFlash_CustomerFW == 1)
            {

                COM1.RunCommandLine("FF", "");
                Thread.Sleep(500);
                COM1.RunCommandLine("2O", "");
                Thread.Sleep(500);
                COM1.RunCommandLine("1O", "");
                Thread.Sleep(1500);


                bTestResult = CustomerFWBurn(str_CustomerFWFolderPath, COM2.BaseSerialPort.PortName);

                COM1.RunCommandLine("2F", "");
                Thread.Sleep(500);
                COM1.RunCommandLine("1F", "");
                Thread.Sleep(500);
                COM1.Close(true);
                Thread.Sleep(1000);
            }


            #endregion


            #region 没烧录客户FW提前退出

            if (iFlash_CustomerFW == 0)
            {
                return bTestResult;
            }

            #endregion


            #region 连接WIFI和RSSI

            CustomerFWFun:

            if(bTestResult)
            {
                //_testForm.OpenCloseShieldingBox(true);
                //_testForm.OpenCloseShieldingBox(false);
                COM1.RunCommandLine("1O", "");
                Thread.Sleep(5000);
                bTestResult = WifiConnectTest();
            }

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

            if(bTestResult)
            {
                Thread.Sleep(iIperfSleepTime*1000 + 1000);
                ProcessHelper.KillProcessByName("iperf");
                bTestResult = CheckIperfResult();
            }



            #endregion

            #region BLE和RSSI

            if (bTestResult)
            {
                Thread.Sleep(iCommand_Send_Interval_Second + 2000);
                bTestResult = BleTest();
            }

            #endregion




            return bTestResult;
        }

        public override bool Func_TestEnd(bool bTestResult)
        {
            bool bEndResult = true;

            try
            {
                if (System.IO.File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_RF.txt"))
                {
                    SaveLogProcess.SaveLog_Local(bTestResult, struct_NormalINI.stru_str_LogFilePath,
                                                                    struct_EncryptINI.stru_str_ProjectName,
                                                                    struct_Barcode.stru_str_sRevDUTMac,
                                                                    $"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_RF.txt", "_IQ.txt");
                }

                if (System.IO.File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_ReadMac.txt"))
                {
                    SaveLogProcess.SaveLog_Local(bTestResult, struct_NormalINI.stru_str_LogFilePath,
                                                                struct_EncryptINI.stru_str_ProjectName,
                                                                struct_Barcode.stru_str_sRevDUTMac,
                                                                $"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_ReadMac.txt", "_ReadMac.txt");
                }

                string str_RFLog = string.Empty;
                if (System.IO.File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_RF.txt"))
                {
                    str_RFLog = $"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_RF.txt";
                }

                sfcFile.CreateSfcFile(struct_TestVariable.bNeedCreateSFCFile, bTestResult, str_RFLog,
                                            struct_Barcode.stru_str_sRevDUTMac, struct_Barcode.stru_str_sRevDUTBD, struct_EncryptINI.stru_str_ProjectName,
                                            struct_EncryptINI.stru_str_CaseVersion, struct_NormalINI.stru_str_SFCFilePath, struct_TestVariable.str_ErrorCode);


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

            COM1.RunCommandLine("1F", "");
            COM1.RunCommandLine("2F", "");
            COM1.Close(true);
            COM2.Close(true);
            Thread.Sleep(1000);
        }




        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================

        public bool ReadCustomerEncryptIni()
        {
            StringBuilder ValTemp = new StringBuilder();
            string str_CaseFolder = new string(str_CaseProjectName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{str_CaseProjectName}\\{str_CaseProjectName}_Encrypt.ini";

            try
            {

                if (System.IO.File.Exists(str_IniPath) == false)
                {
                    throw new ArgumentException($"{str_CaseProjectName}_Encrypt.ini文件不存在。");
                }

                #region [BDF_Info]

                IniHelper.GetIniStr("BDF_Info", "WIFI_BDF_Name", "null", ValTemp, 200, str_IniPath);
                str_WIFI_BDF_Name = ValTemp.ToString();

                IniHelper.GetIniStr("BDF_Info", "WIFI_BDF_MD5", "null", ValTemp, 200, str_IniPath);
                str_WIFI_BDF_MD5 = ValTemp.ToString();


                IniHelper.GetIniStr("BDF_Info", "BT_BDF_Name", "null", ValTemp, 200, str_IniPath);
                str_BT_BDF_Name = ValTemp.ToString();

                IniHelper.GetIniStr("BDF_Info", "BT_BDF_MD5", "null", ValTemp, 200, str_IniPath);
                str_BT_BDF_MD5 = ValTemp.ToString();

                #endregion


                #region TestConfigCustomerFW

                IniHelper.GetIniStr("TestConfig", "Flash_CustomerFW", "1", ValTemp, 200, str_IniPath);
                iFlash_CustomerFW = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("TestConfig", "Flash_TestFW", "0", ValTemp, 200, str_IniPath);
                iFlash_TestFW = int.Parse(ValTemp.ToString());

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


                #region SerializationConfig

                IniHelper.GetIniStr("SerializationConfig", "UpLoad_ITDB_IP", "null", ValTemp, 100, str_IniPath);
                str_UpLoad_ITDB_IP = ValTemp.ToString();

                IniHelper.GetIniStr("SerializationConfig", "Clear_Switch", "null", ValTemp, 10, str_IniPath);
                iClearSerialization = int.Parse(ValTemp.ToString());

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


        public (bool, int )TestFWBurn(string strFWPath, string str_BatName)
        {
            UIHandleHelper.ShowRunLog("Burn test firmware...");

            ExeProcessOnceHelper exeProcessHelper = new("cmd.exe", strFWPath);
            bool bResult = exeProcessHelper.RunCommandLine($"/c {strFWPath}\\{str_BatName}\r\n", out string outputData, out string errorData, 100000);

            int iResult = -1;

            if (bResult)
            {
                UIHandleHelper.ShowRunLog("Jlink out: \r\n" + outputData);
                UIHandleHelper.ShowRunLog("Jlink err: \r\n" + errorData);

                int iCount_1 = 0;
                int iCount_2 = 0;
                if (outputData is not null && outputData.ToLower().Contains("error") == false)
                {
                    iCount_1 = outputData.Split("\r\n").Count((x) => x == "O.K.");
                    iCount_2 = outputData.Split(new[] { "Contents already match" }, StringSplitOptions.None).Length - 1;
                }

                if (iCount_1 == 3)
                {
                    bResult = true;
                    UIHandleHelper.ShowRunLog("Firmware burning completed.",
                                                              FailColor: false,
                                                              ShowGridView: true,
                                                              TestItemName: "Test FW",
                                                              TestItemContent: "Firmware burning completed.",
                                                              TestItemResult: true);

                    if (iCount_2 == 3)
                    {
                        UIHandleHelper.ShowRunLog("The firmware has already been burned in.",
                                                             FailColor: false,
                                                             ShowGridView: true,
                                                             TestItemName: "Test FW",
                                                             TestItemContent: "The firmware has already been burned in.",
                                                             TestItemResult: true);
                        iResult = 0;
                    }
                }
                else
                {
                    bResult = false;

                    UIHandleHelper.ShowRunLog("Burn Test FW Fail;",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Test FW",
                                                               TestItemContent: "Burn failed",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err028.ToString());
                }
            }
            else
            {
                bResult = false;
                UIHandleHelper.ShowRunLog("Burn Test FW Error;",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Test FW",
                                                               TestItemContent: "Burn error",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err028.ToString());
            }

            return (bResult, iResult);
        }



        public bool CustomerFWBurn(string strFWPath, string strComNum)
        {
            UIHandleHelper.ShowRunLog("Burn customer firmware...");

            bool bRetVal = false;
            CustomProcess customProcess = new("blhost.exe");
            bRetVal = customProcess.RunCommandLine($" -p {strComNum},115200 -- receive-sb-file {strFWPath}\\rw61x_devhsm_loader_fw\\RW61x_DevHSM_Loader_ISP_Boot_FW.sb3", "Success", out string msg0, 10000, 2);


            if (bRetVal)
            {
                bRetVal = customProcess.RunCommandLine($" -p {strComNum},115200 -- receive-sb-file {strFWPath}\\dev_hsm_prov.sb", "Success", out string msg1, 10000, 2);
            }

            if (bRetVal)
            {
                bRetVal = customProcess.RunCommandLine($" -t 30000 -p {strComNum},1000000 -- receive-sb-file {strFWPath}\\NIUV.s.bin.sb", "Success", out string msg2, 300000, 2);
            }


            if (bRetVal)
            {
                UIHandleHelper.ShowRunLog("Burn Customer FW Pass;",
                                                              FailColor: false,
                                                              ShowGridView: true,
                                                              TestItemName: "Customer FW",
                                                              TestItemContent: "Burn Customer FW Pass;",
                                                              TestItemResult: true);
            }
            else
            {
                UIHandleHelper.ShowRunLog("Burn Customer FW Fail;",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Customer FW",
                                                               TestItemContent: "Burn Customer FW Fail;",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err028.ToString());
            }

            return bRetVal;
        }


  
        /// <summary>
        /// 返回值，0：检查的mac为F，空板；1：检查的Mac和板子写的是一样的；2：检查不到Mac，可能是写了客户固件；3：不符合以上的情况判定为fail；
        /// </summary>
        /// <returns></returns>
        public int CheckInfo_Before()
        {
            try
            {
                UIHandleHelper.ShowRunLog("Check info before testing...");
                string strTempMAC = "";


                WriteSetUpINI(2);



                if (System.IO.File.Exists($"{str_LabtoolFolderPath}\\Test.txt"))
                {
                    System.IO.File.Delete($"{str_LabtoolFolderPath}\\Test.txt");
                }


                if (System.IO.File.Exists(@$"{str_IQTestProgramFolderPath}\labtool\Test.txt"))
                {
                    System.IO.File.Delete(@$"{str_IQTestProgramFolderPath}\labtool\Test.txt");
                }

                ExeProcessContinueHelper exeProcessContinueHelper = new(str_LabtoolFolderPath, $"{str_LabtoolFolderPath}\\{str_LabtoolExeName}");

                exeProcessContinueHelper.RunCommandLine("1\r\n", "DutIf_InitConnection: 0", out string mseeage, 1000, 1);

                exeProcessContinueHelper.RunCommandLine("45\r\n", "DutIf_GetMACAddress:", out string mseeage1, 2000, 1);

                exeProcessContinueHelper.RunCommandLine("99\r\n", "DutIf_GetMACAddress:", out string mseeage3, 1000, 1);



                string str_logs = "";
                str_logs = System.IO.File.ReadAllText($"{str_LabtoolFolderPath}\\Test.txt");
                UIHandleHelper.ShowRunLog($"labeltool cmd 45 data:\r\n {str_logs}");

                ProcessHelper.KillProcessByName(str_LabtoolExeName.Replace(".exe", ""));

                if (str_logs.Contains("00.00.00.00"))
                {
                    UIHandleHelper.ShowRunLog("Maybe, Customer FW Have Write.",
                                                               FailColor: false,
                                                               ShowGridView:true,
                                                               TestItemName: "Check Status",
                                                               TestItemContent: "Maybe, Customer FW Have Write.",
                                                               TestItemResult: true
                                                               );
                    return 2;
                }
                else if (str_logs.Contains("DutIf_GetMACAddress"))
                {
                    string str_all = str_logs.Replace("DutIf_GetMACAddress: 0x", "").Replace("\r", "").Replace("\n", "");
                    string _str = str_all.Substring(str_all.IndexOf("DutIf_GetMACAddress"));
                    _str = _str.Substring(_str.IndexOf("DutIf_GetMACAddress") + 21, 17);
                    strTempMAC = _str.Replace(".", "");
                    strTempMAC = strTempMAC.ToUpper();
                    UIHandleHelper.ShowRunLog($"Read DUT Mac Address----->{strTempMAC}");
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Call labtool Read mac error.",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Check Status",
                                                               TestItemContent: "Call labtool Read mac error.",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err019.ToString());
                    return 2;
                }

                if (strTempMAC == struct_Barcode.stru_str_sRevDUTMac)
                {
                    UIHandleHelper.ShowRunLog("The MAC address of the product has already been written.",
                                                               FailColor: false,
                                                               ShowGridView: true,
                                                               TestItemName: "Check Status",
                                                               TestItemContent: $"MAC address already been written:{strTempMAC}",
                                                               TestItemResult: true);
                    return 1;
                }
                else if (strTempMAC == "FFFFFFFFFFFF")
                {
                    UIHandleHelper.ShowRunLog("The product has not been tested and is empty.",
                                                               FailColor: false,
                                                               ShowGridView: true,
                                                               TestItemName: "Check Status",
                                                               TestItemContent: "The product has not been tested and is empty.",
                                                               TestItemResult: true);

                    return 0;
                }
                else
                {
                    UIHandleHelper.ShowRunLog("It doesn't conform to any situation;",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Check Status",
                                                               TestItemContent: "It doesn't conform to any situation;",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err019.ToString());
                    return 3;
                }
            }
            catch(Exception ex)
            {
                ProcessHelper.KillProcessByName(str_LabtoolExeName.Replace(".exe", ""));
                UIHandleHelper.ShowRunLog($"Call labtool Read mac error: {ex}",
                                                              FailColor: true,
                                                              ShowGridView: true,
                                                              TestItemName: "Check Status",
                                                              TestItemContent: $"Call labtool Read mac error: {ex}",
                                                              TestItemResult: false,
                                                              ErrorCode: TestErrorCode.ErrorCode.Err019.ToString());
                return 3;
            }
        }

        public bool DealWithiBoardStatus_2(ref bool bSecurtboot)
        {
            UIHandleHelper.ShowRunLog("Check whether it is customer firmware mode....");

            bool bResult = false;
            try
            {
                COM2.Open(true);

                COM1.RunCommandLine("1F", "");
                Thread.Sleep(1000);
                COM1.RunCommandLine("1O", "");

                string strRecv = "";

                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(1000);

                    Byte[] byteArray = COM2.ReadBytes();
                    strRecv += BitConverter.ToString(byteArray).Replace("-", "");

                    if (strRecv.Contains("095060") || strRecv.Contains("4E25AB") || strRecv.Contains("C63102"))
                    {
                        bSecurtboot = true;
                        bResult = true;
                        UIHandleHelper.ShowRunLog("The product has burned the customer's firmware;",
                                                             FailColor: false,
                                                             ShowGridView: true,
                                                             TestItemName: "Check Customer FW",
                                                             TestItemContent: "The product has burned the customer's firmware;",
                                                             TestItemResult: true);
                        break;
                    }
                }

                COM2.Close(true);

                if (!bResult)
                {
                    bSecurtboot = false;
                    bResult = false;
                    UIHandleHelper.ShowRunLog("Can not Read MAC and Find Secure Boot Data.",
                                                              FailColor: true,
                                                              ShowGridView: true,
                                                              TestItemName: "Check Customer FW",
                                                              TestItemContent: "Can not Read MAC and Find Secure Boot Data.",
                                                              TestItemResult: false,
                                                              ErrorCode: TestErrorCode.ErrorCode.Err031.ToString());
                }
            }
            catch (Exception ex)
            {

                UIHandleHelper.ShowRunLog($"Check Secure Boot Data Err: {ex.Message}",
                                                              FailColor: true,
                                                              ShowGridView: true,
                                                              TestItemName: "Check Customer FW",
                                                              TestItemContent: $"Check Secure Boot Data Err: {ex.Message}",
                                                              TestItemResult: false,
                                                              ErrorCode: TestErrorCode.ErrorCode.Err031.ToString());
                bSecurtboot = false;
                bResult = false;
            }


            return bResult;
        }

        public bool DealWithiBoardStatus_3()
        {
            UIHandleHelper.ShowRunLog("The mac is not default ID and test ID.",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Check Status",
                                                               TestItemContent: "The mac is not default ID and test ID.",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err019.ToString());
            return false;
        }

        public bool CheckInfo_After(string strMac, string strBD)
        {
            try
            {
                UIHandleHelper.ShowRunLog("Check info after testing...");

                if (System.IO.File.Exists($"{str_LabtoolFolderPath}\\Test.txt"))
                {
                    System.IO.File.Delete($"{str_LabtoolFolderPath}\\Test.txt");
                }

                //WIFI
                ExeProcessContinueHelper exeProcessContinueHelper = new(str_LabtoolFolderPath, $"{str_LabtoolFolderPath}\\{str_LabtoolExeName}");
                exeProcessContinueHelper.RunCommandLine("1\r\n", "DutIf_InitConnection: 0", out string mseeage0, 2000, 3);
                string str_logs = System.IO.File.ReadAllText($"{str_LabtoolFolderPath}\\Test.txt");
               
                if (str_logs.Contains("DutIf_InitConnection: 0") == false)
                {
                    UIHandleHelper.ShowRunLog($"labeltool cmd 1 data:\r\n {str_logs}");
                    UIHandleHelper.ShowRunLog("labeltool cmd 1 init dut fail", true);
                    return false;
                }

                exeProcessContinueHelper.RunCommandLine("45\r\n", "DutIf_GetMACAddress:", out string mseeage1, 2000, 3);
                exeProcessContinueHelper.RunCommandLine("99\r\n", "Exit", out string mseeage2, 2000, 3);

                //BT
                exeProcessContinueHelper.RunCommandLine("2\r\n", "Dut_Bt_OpenDevice:", out string mseeage3, 2000, 3);
                str_logs = System.IO.File.ReadAllText($"{str_LabtoolFolderPath}\\Test.txt");
                
                if (str_logs.Contains("Dut_Bt_OpenDevice:") == false)
                {
                    UIHandleHelper.ShowRunLog($"labeltool cmd 2 data:\r\n {str_logs}");
                    UIHandleHelper.ShowRunLog("labeltool cmd 2 init dut fail", true);
                    return false;
                }

                exeProcessContinueHelper.RunCommandLine("45\r\n", "BD_ADDRESS:", out string mseeage4, 2000, 3);
                exeProcessContinueHelper.RunCommandLine("99\r\n", "Exit", out string mseeage5, 2000, 3);


                //解析txtlog
                string TempMAC = "", TempBT = "";
                str_logs = "";
                str_logs = System.IO.File.ReadAllText($"{str_LabtoolFolderPath}\\Test.txt");
                UIHandleHelper.ShowRunLog($"labeltool run data:\r\n {str_logs}");

                if (str_logs.Contains("DutIf_GetMACAddress") && str_logs.Contains("BD_ADDRESS"))
                {
                    string _str = str_logs.Substring(str_logs.IndexOf("DutIf_GetMACAddress") + 19);
                    _str = _str.Substring(_str.IndexOf("DutIf_GetMACAddress") + 21, 17);
                    TempMAC = _str.Replace(".", "");
                    TempMAC = TempMAC.ToUpper();
                    
   
                    string _strBT = str_logs.Substring(str_logs.IndexOf("BD_ADDRESS:") + 12, 17);
                    TempBT = _strBT.Replace("-", "");
                    TempBT = TempBT.ToUpper();
                    
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Call labtool read info fail.", true);
                    return false;
                }

                //比对Mac和BD
                if (TempMAC == strMac)
                {
                    UIHandleHelper.ShowRunLog($"Read and check dut mac address pass ----->{TempMAC}");
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Read and check dut mac address fail ----->{TempMAC}", true);
                    return false;
                }

                if (TempBT == strBD)
                {
                    UIHandleHelper.ShowRunLog($"Read and check dut bt address----->{TempBT}");
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Read and check dut bt address fail ----->{TempBT}", true);
                    return false;
                }


            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Call labtool Read info error: {ex}", true);
                ProcessHelper.KillProcessByName(str_LabtoolExeName.Replace(".exe", ""));
                ProcessHelper.KillProcessByName("cmd");
                return false;
            }

        }

        public bool RFTest(string str_FlowBatName)
        {
            bool bRet = false;

            bRet=  litepointHelper.CallFlowTest($"{FileProcessHelper.GetCurrentExeDirectory()}\\{str_FlowBatName}", 
                                                              $"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt" );

            if (System.IO.File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt") )
            {
                System.IO.File.Copy($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt", $"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput_RF.txt", true);
            }

            if (bRet == false)
            {
                str_ErrorCode = ParseErrorCode.ParseFileForErrorCodes($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput_RF.txt").ToString();
            }

            if (bRet)
            {
                UIHandleHelper.ShowRunLog("RF Test Pass.",
                                                              FailColor: false,
                                                              ShowGridView: true,
                                                              TestItemName: "RF Test",
                                                              TestItemContent: "RF Test Pass.",
                                                              TestItemResult: true);
            }
            else
            {
                UIHandleHelper.ShowRunLog("RF Test Fail.",
                                                             FailColor: true,
                                                             ShowGridView: true,
                                                             TestItemName: "RF Test",
                                                             TestItemContent: "RF Test Fail.",
                                                             TestItemResult: false,
                                                             ErrorCode: str_ErrorCode);
            }

            return bRet;
        }

        public bool CheckMacBT(string str_FlowBatName)
        {
            bool bRet = false;

            for (int i = 0;i<2;i++)
            {
                bRet = litepointHelper.CallFlowTest_ReadMac($"{FileProcessHelper.GetCurrentExeDirectory()}\\{str_FlowBatName}",
                                                  $"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt",
                                                  $"MAC_ADDRESS:{struct_Barcode.stru_str_sRevDUTMac}", $"BD_ADDRESS:{struct_Barcode.stru_str_sRevDUTBD}",
                                                  50, "IQfactRun_Console"
                                                  );

                if (System.IO.File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt"))
                {
                    System.IO.File.Copy($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt", $"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput_ReadMac.txt", true);
                }

                if (bRet)
                {
                    break;
                }
            }

            if (!bRet)
            {
                UIHandleHelper.ShowRunLog("Check Mac and BT Fail.",
                                                             FailColor: true,
                                                             ShowGridView: true,
                                                             TestItemName: "Check Address",
                                                             TestItemContent: "Check Mac and BT Fail.",
                                                             TestItemResult: false,
                                                             ErrorCode: TestErrorCode.ErrorCode.Err021.ToString());
            }
            else
            {
                UIHandleHelper.ShowRunLog("Check Mac and BT Pass.",
                                                             FailColor: false,
                                                             ShowGridView: true,
                                                             TestItemName: "Check Address",
                                                             TestItemContent: "Check Mac and BT Pass.",
                                                             TestItemResult: true);
            }

            return bRet;
        }

        public bool CheckBDF_Md5()
        {
            bool bRet = false;

            MD5Helper mD5Helper = new();

            string wifi_BDF_Md5 = mD5Helper.ComputeFileMD5Hash($"{str_IQTestProgramFolderPath}\\bin\\{str_WIFI_BDF_Name}");

            if (string.IsNullOrEmpty(wifi_BDF_Md5))
            {
                UIHandleHelper.ShowRunLog($"{str_WIFI_BDF_Name} Md5 is null", 
                                                            FailColor: true,
                                                            ShowGridView: true,
                                                            TestItemName: "WIFI BDF MD5",
                                                            TestItemContent: wifi_BDF_Md5,
                                                            TestItemResult: false,
                                                            ErrorCode: TestErrorCode.ErrorCode.Err000.ToString());

                bRet = false;
            }
            else
            {
                bRet = true;
            }

            if (bRet)
            {
                if (wifi_BDF_Md5 == str_WIFI_BDF_MD5)
                {
                    UIHandleHelper.ShowRunLog($"Check {str_WIFI_BDF_Name} Md5 pass",
                                                                FailColor: false,
                                                                ShowGridView: true,
                                                                TestItemName: "WIFI BDF MD5",
                                                                TestItemContent: wifi_BDF_Md5,
                                                                TestItemResult: true);

                    bRet = true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Check {str_WIFI_BDF_Name} Md5 fail",
                                                                FailColor: true,
                                                                ShowGridView: true,
                                                                TestItemName: "WIFI BDF MD5",
                                                                TestItemContent: wifi_BDF_Md5,
                                                                TestItemResult: false,
                                                                ErrorCode: TestErrorCode.ErrorCode.Err000.ToString());
                    bRet = false;
                }

            }


            if (bRet)
            {
                string bt_BDF_Md5 = mD5Helper.ComputeFileMD5Hash($"{str_IQTestProgramFolderPath}\\bin\\{str_BT_BDF_Name}");

                if (string.IsNullOrEmpty(bt_BDF_Md5))
                {
                    UIHandleHelper.ShowRunLog($"{bt_BDF_Md5} Md5 is null",
                                                                FailColor: true,
                                                                ShowGridView: true,
                                                                TestItemName: "BT BDF MD5",
                                                                TestItemContent: bt_BDF_Md5,
                                                                TestItemResult: false,
                                                                ErrorCode: TestErrorCode.ErrorCode.Err000.ToString());

                    bRet = false;
                }

                if (bRet)
                {
                    if (bt_BDF_Md5 == str_BT_BDF_MD5)
                    {
                        UIHandleHelper.ShowRunLog($"Check {str_BT_BDF_Name} Md5 pass",
                                                                    FailColor: false,
                                                                    ShowGridView: true,
                                                                    TestItemName: "BT BDF MD5",
                                                                    TestItemContent: bt_BDF_Md5,
                                                                    TestItemResult: true);

                        bRet = true;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"Check {str_BT_BDF_Name} Md5 fail",
                                                                    FailColor: true,
                                                                    ShowGridView: true,
                                                                    TestItemName: "BT BDF MD5",
                                                                    TestItemContent: bt_BDF_Md5,
                                                                    TestItemResult: false,
                                                                    ErrorCode: TestErrorCode.ErrorCode.Err000.ToString());
                        bRet = false;
                    }

                }
            }




            return bRet;
        }

        /// <summary>
        /// 0 - EEPROM support     1 - NO_EEPROM support      2 - OTP support
        /// </summary>
        /// <param name="iDutInitSet"></param>
        public void WriteSetUpINI(int iDutInitSet)
        {
            IniHelper.WriteIniStr("COMSET", "ComNo", COM2.BaseSerialPort.PortName.Replace("COM", "").Replace(" ", ""), $"{str_LabtoolFolderPath}\\SetUp.ini");
            IniHelper.WriteIniStr("COMSET", "ComNo", COM2.BaseSerialPort.PortName.Replace("COM", "").Replace(" ", ""), $"{str_IQTestProgramFolderPath}\\Bin\\SetUp.ini");

            IniHelper.WriteIniStr("DutInitSet", "NO_EEPROM", iDutInitSet.ToString(), $"{str_LabtoolFolderPath}\\SetUp.ini");
            IniHelper.WriteIniStr("DutInitSet", "NO_EEPROM", iDutInitSet.ToString(), $"{str_IQTestProgramFolderPath}\\Bin\\SetUp.ini");
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
                    Thread.Sleep(10);
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


        public bool CreateIperfBat(string str_IperfPath, string str_Content, string str_BatName)
        {
            string str_Temp = "";

            try
            {
                UIHandleHelper.ShowRunLog("Create Iperf Bat...");
                str_Temp = "cd /d" + str_IperfPath + "\r\n" + str_Content + "\r\n";

                TextWriter Filewriter;//以寫方式打開文件
                Filewriter = System.IO.File.CreateText($"{FileProcessHelper.GetCurrentExeDirectory()}\\{str_BatName}");//創建或打開一個UTF-8的文件
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
                        if (float.Parse(str_Speed) >=iIperf_LowerLimit)
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
                bResult =  false;
            }

            return bResult;
        }


        public int UpdateSerializedResult(bool bResult)
        {
            int iResult = -1;
            string str_Result = "PASS";

            if (bResult)
            {
                str_Result = "PASS";
            }
            else
            {
                str_Result = "FAIL";
            }

            string str_Comm_ITDB_Command = Generate_ITDB_Comm_CMD(str_UpLoad_ITDB_IP, "add",
                                                                    struct_Barcode.stru_str_sRevDUTMac, "Serialized", str_Result);

            if (string.IsNullOrEmpty(str_Comm_ITDB_Command))
            {
                UIHandleHelper.ShowRunLog($"Update Serialized: Generate IT DB CMD Fail",
                                                             FailColor: true,
                                                             ShowGridView: true,
                                                             TestItemName: "Update Serialized",
                                                             TestItemContent: "Generate IT DB CMD Fail",
                                                             TestItemResult: false,
                                                             ErrorCode: TestErrorCode.ErrorCode.Err058.ToString());

                return -1;
            }


            iResult = UpdateFromITDB(str_Comm_ITDB_Command);
            if (iResult == -1)
            {
                if (UpdateFromITDB(str_Comm_ITDB_Command) == 1)
                {
                    iResult = 1;
                }
                else
                {
                    iResult = 0;
                }
            }




            if (iResult == 1)
            {
                UIHandleHelper.ShowRunLog("Serialization result uploaded successfully",
                                                                FailColor: false,
                                                                ShowGridView: true,
                                                                TestItemName: "Upload Serialized",
                                                                TestItemContent: "Serialization result uploaded successfully",
                                                                TestItemResult: true);
            }
            else
            {
                UIHandleHelper.ShowRunLog("Serialization result upload failed",
                                                            FailColor: true,
                                                            ShowGridView: true,
                                                            TestItemName: "Upload Serialized",
                                                            TestItemContent: "Serialization result upload failed;",
                                                            TestItemResult: false,
                                                             ErrorCode: TestErrorCode.ErrorCode.Err061.ToString());
            }



            return iResult;
        }

        int UpdateFromITDB(string str_Cmd)
        {
            int iResult = -1;
            try
            {
                Task<bool> httpTask_FWVer = Http_Get_Async("Update Serialized", str_Cmd, "OK");
                httpTask_FWVer.Wait();

                if (httpTask_FWVer.Result)
                {
                    iResult = 1;
                }
                else
                {
                    iResult = 0;
                }
            }
            catch (Exception)
            {
                iResult = -1;
            }

            return iResult;
        }


        public string Generate_ITDB_Comm_CMD(string str_ITDB_IP, string str_CmdType, string str_SN, string str_Key, string str_Result = "NULL")
        {
            string str_Comm_ITDB_Command = null;

            if (str_CmdType.ToLower() == "add")
            {
                str_Comm_ITDB_Command = $"http://{str_ITDB_IP}/mesinterface/go.aspx?c=add&f=attr&sn={str_SN}&key={str_Key}&value={str_Result}";
            }
            else if (str_CmdType.ToLower() == "query")
            {
                str_Comm_ITDB_Command = $"http://{str_ITDB_IP}/mesinterface/go.aspx?c=query&f=attr&sn={str_SN}&key={str_Key}";
            }

            return str_Comm_ITDB_Command;

        }



    }
}
