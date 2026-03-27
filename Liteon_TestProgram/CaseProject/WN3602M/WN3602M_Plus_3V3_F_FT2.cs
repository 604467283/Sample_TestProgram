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
using OpenTK.Audio.OpenAL;

namespace Liteon_TestProgram.CaseProject
{
    internal class WN3602M_Plus_3V3_F_FT2 : CaseCodeBase
    {

        public static string str_WifiPartName = null;
        public static int iWIFI_RSSI_Percent_LowerLimit = 0;
        public static string str_FW_Version = null;
        public static string str_Get_Version_Command = null;

        public static string str_IperfServerCommand = null;
        public static string str_IperfServer_PostData = null;
        public static string str_IperfServer_Return = null;
        public static int iIperfSleepTime = 0;
        public static int iIperf_LowerLimit = 0;

        public static int iVoltage_TestTimes = 0;
        public static float fVoltage_UpperLimit_3V3 = 0;
        public static float fVoltage_LowerLimit_3V3 = 0;

        public static string str_Web_Services_Command = null;
        public static string str_Web_Services_Return = null;

        public static string str_UART_Test_Command_1 = null;
        public static string str_UART_Test_PostData_1 = null;
        public static string str_UART_Test_Command_Return_1 = null;

        public static string str_UART_Test_Command_2 = null;
        public static string str_UART_Test_PostData_2 = null;
        public static string str_UART_Test_Command_Return_2 = null;


        public int iCommand_Send_Interval_Second = 0;

        public WifiHelper wifiHelper = new WifiHelper();
        public HttpWithBodyHelper httpClientHelper = new();
        public HttpGetHelper getter = new HttpGetHelper();


        public static string str_IperfFolderName = "iperf";
        public static string str_IperfFileName = "iperf.exe";
        public string str_IperfFolderPath = $"{PathHelper.GetParentDirectoryPath(PathHelper.GetCurrentExeDirPath())}\\{str_IperfFolderName}";

        public static string str_IperfBatName = "Testiperf.bat";
        public static string str_IperfLogName = "Testiperf.txt";
        public static string str_IperfBatContent = $"iperf.exe -s >> {str_IperfLogName}";



        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; set; }
        public TestForm _testForm;
        string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public WN3602M_Plus_3V3_F_FT2(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.初版");

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
            list_str.Add("1.测试电脑需要使用到无线网卡.\r\n");
            list_str.Add("2.COMA请填测试TB022小板的串口, 波特率9600\r\n");
            list_str.Add(@"3.测试TB022小板指令如下: 
R0 测试电压
1O 1F 控制测试板的Uart测试点短路开关
2O 2F 控制测试板USB_Power_5V开关
3O 3F 控制测试板USB_Power_3V开关                           
4O 4F 控制测试板电源开关
5O 5F 控制WifiCard电源开关");

            return list_str;
        }


        public override void Func_TestMac()
        {
            macBD_Relation = MacBD_Relation.OnlyMac;
            if (struct_NormalINI.stru_b_DebugMode)
            {
                struct_TestVariable.iMAC_OriginalLength = 12;
                struct_TestVariable.iMAC_ExtractStartPosition = 0;
                struct_TestVariable.iMACLength = 12;
            }
            else
            {
                struct_TestVariable.iMACLength = 12;
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
                ProcessHelper.KillProcessByName("iperf");

                Thread.Sleep(500);
                if (File.Exists(str_IperfFolderPath + "\\" + str_IperfLogName))
                {
                    File.Delete(str_IperfFolderPath + "\\" + str_IperfLogName);
                }

                if (File.Exists(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfBatName}"))
                {
                    File.Delete(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfLogName}");
                }

                FileProcessHelper.DeleteFile(@$"{PathHelper.GetCurrentExeDirPath()}\SFCFile\", "*.txt", 50);



                #region 测试板控制

                bInitResult = COM1.RunCommandLine("5F", "");
                UIHandleHelper.ShowRunLog("Wifi tool card power off...");
                Thread.Sleep(300);
                bInitResult = COM1.RunCommandLine("4F", "");
                UIHandleHelper.ShowRunLog("Test Board power off...");
                Thread.Sleep(300);
                bInitResult = COM1.RunCommandLine("2F", "");
                UIHandleHelper.ShowRunLog("Module 5V power off...");
                Thread.Sleep(300);
                bInitResult = COM1.RunCommandLine("3F", "");
                UIHandleHelper.ShowRunLog("Module 3V power off...");
                Thread.Sleep(300);
                bInitResult = COM1.RunCommandLine("1F", "");
                UIHandleHelper.ShowRunLog("Module Uart TP1 TP2 short off...");
                Thread.Sleep(1000);



                bInitResult = COM1.RunCommandLine("5O", "");
                UIHandleHelper.ShowRunLog("Wifi tool card power on...");
                Thread.Sleep(300);
                bInitResult = COM1.RunCommandLine("4O", "");
                UIHandleHelper.ShowRunLog("Test Board power on...");
                Thread.Sleep(300);

                if (_CaseProjectName.Contains("3.3V"))
                {
                    bInitResult = COM1.RunCommandLine("3O", "");
                    UIHandleHelper.ShowRunLog("Module 3V power on...");
                    Thread.Sleep(300);
                }

                if (_CaseProjectName.Contains("5V"))
                {
                    bInitResult = COM1.RunCommandLine("2O", "");
                    UIHandleHelper.ShowRunLog("Module 5V power on...");
                    Thread.Sleep(300);
                }

                bInitResult = COM1.RunCommandLine("4F", "");
                UIHandleHelper.ShowRunLog("Test Board power off...");
                Thread.Sleep(300);

                bInitResult = COM1.RunCommandLine("4O", "");
                UIHandleHelper.ShowRunLog("Test Board power on...");
                Thread.Sleep(3000);


                #endregion

            }
            catch (Exception ex)
            {
                bInitResult = false;
                UIHandleHelper.ShowRunLog($"init fail: {ex}", true);
            }



            #endregion

            return bInitResult;
        }

        public override bool Func_TestFlow()
        {
            bool bTestResult = false;

            #region 电压测试

            if (_CaseProjectName.Contains("5V"))
            {
                bTestResult = TestVoltage();
            }
            else
            {
                bTestResult = true;
            }

            #endregion



            #region 连接WIFI和RSSI

            if (bTestResult)
            {
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

            #region UartTest

            if (bTestResult)
            {
                Thread.Sleep(iCommand_Send_Interval_Second);
                bTestResult = UartTest();
            }

            #endregion


            #region iperfTest

            if (bTestResult)
            {
                bTestResult = IperfTest();
            }

            #endregion


            return bTestResult;
        }

        public override bool Func_TestEnd(bool bTestResult)
        {
            bool bEndResult = true;

            try
            {

                sfcFile.CreateSfcFile(struct_TestVariable.bNeedCreateSFCFile, bTestResult, null,
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
            wifiHelper.DisConnectWifi();
            COM1.RunCommandLine("FF", "");
            COM1.Close(true);
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

                if (File.Exists(str_IniPath) == false)
                {
                    throw new ArgumentException($"{str_CaseProjectName}_Encrypt.ini文件不存在。");
                }

            #region [Test_Items]

                #region 指令发送间隔时间

                IniHelper.GetIniStr("Test_Items", "Command_Send_Interval_Second", "null", ValTemp, 50, str_IniPath);
                iCommand_Send_Interval_Second = int.Parse(ValTemp.ToString()) * 1000;

                #endregion

                #region WifiPartName

                IniHelper.GetIniStr("Test_Items", "WifiPartName", "null", ValTemp, 100, str_IniPath);
                str_WifiPartName = ValTemp.ToString();

                #endregion


                #region RSSI

                IniHelper.GetIniStr("Test_Items", "WIFI_RSSI_Percent_LowerLimit", "null", ValTemp, 50, str_IniPath);
                iWIFI_RSSI_Percent_LowerLimit = int.Parse(ValTemp.ToString());

                #endregion


                #region Voltage_Test

                IniHelper.GetIniStr("Test_Items", "Voltage_TestTimes", "null", ValTemp, 50, str_IniPath);
                iVoltage_TestTimes = int.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("Test_Items", "Voltage_UpperLimit_3V3", "null", ValTemp, 50, str_IniPath);
                fVoltage_UpperLimit_3V3 = float.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("Test_Items", "Voltage_LowerLimit_3V3", "null", ValTemp, 50, str_IniPath);
                fVoltage_LowerLimit_3V3 = float.Parse(ValTemp.ToString());

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


                #region UartTest

                IniHelper.GetIniStr("Test_Items", "UART_Test_Command_1", "null", ValTemp, 100, str_IniPath);
                str_UART_Test_Command_1 = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "UART_Test_PostData_1", "null", ValTemp, 100, str_IniPath);
                str_UART_Test_PostData_1 = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "UART_Test_Command_Return_1", "null", ValTemp, 100, str_IniPath);
                str_UART_Test_Command_Return_1 = ValTemp.ToString();


                IniHelper.GetIniStr("Test_Items", "UART_Test_Command_2", "null", ValTemp, 100, str_IniPath);
                str_UART_Test_Command_2 = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "UART_Test_PostData_2", "null", ValTemp, 100, str_IniPath);
                str_UART_Test_PostData_2 = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "UART_Test_Command_Return_2", "null", ValTemp, 100, str_IniPath);
                str_UART_Test_Command_Return_2 = ValTemp.ToString();

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
                                                              TestItemContent: $"{responseJson}",
                                                              TestItemResult: true);
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"{str_Titel} fail: {responseJson}",
                                                             FailColor: true,
                                                             ShowGridView: true,
                                                             TestItemName: $"{str_Titel}",
                                                             TestItemContent: $"{responseJson}",
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
                UseShellExecute = false,
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


        public bool WifiConnectTest()
        {
            bool bResult = false;
            string str_Mac = InsertEveryTwoCharacters(struct_Barcode.stru_str_sRevDUTMac).ToLower();
            int iSignalStrength_WIFI = 0;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    wifiHelper.DisableWifiByName("Wireless");
                    wifiHelper.EnableWifiByName("Wireless");
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
                    return false;
                }

                Thread.Sleep(8000);
                var Result = wifiHelper.ConnectWifi(str_WifiPartName + str_Mac, "");
                bResult = Result.Item1;
                if (bResult)
                {
                    iSignalStrength_WIFI = Result.Item2;
                    break;
                }
            }

            if (bResult)
            {
                UIHandleHelper.ShowRunLog("Wait 5 seconds for WIFI connection to stabilize...");
                Thread.Sleep(5000);
            }

            if (bResult)
            {
                if (iSignalStrength_WIFI >= iWIFI_RSSI_Percent_LowerLimit)
                {
                    UIHandleHelper.ShowRunLog($"Wifi rssi test pass: {iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -] %",
                                                               ShowGridView: true,
                                                               TestItemName: "Wifi Rssi",
                                                               TestItemContent: $"{iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -] %",
                                                               TestItemResult: true);
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Wifi rssi test fail: {iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -] %", true,
                                                               ShowGridView: true,
                                                               TestItemName: "Wifi Rssi",
                                                               TestItemContent: $"{iSignalStrength_WIFI} [{iWIFI_RSSI_Percent_LowerLimit}, -] %",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err010.ToString());

                    return false;
                }
            }
            else
            {
                UIHandleHelper.ShowRunLog("Wifi connect test fail", true,
                                                               ShowGridView: true,
                                                               TestItemName: "Wifi Connect",
                                                               TestItemContent: "Connect test fail",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err001.ToString());

                return false;
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
                                                               TestItemContent: $"{str_Expected}",
                                                               TestItemResult: true);
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"{str_Titel} test fail",
                                                                   FailColor: true,
                                                                   ShowGridView: true,
                                                                   TestItemName: $"{str_Titel}",
                                                                   TestItemContent: $"test fail",
                                                                   TestItemResult: false, 
                                                                   ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                }


            }
            catch (HttpRequestException e)
            {
                UIHandleHelper.ShowRunLog($"{str_Titel} test error",
                                                                   FailColor: true,
                                                                   ShowGridView: true,
                                                                   TestItemName: $"{str_Titel}",
                                                                   TestItemContent: $"Get request error: {e.Message}",
                                                                   TestItemResult: false,
                                                                   ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

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
                }
                catch (Exception)
                {
                    bResult = false;
                    ReConnectWifi();
                }

                if (bResult)
                {
                    break;
                }

                
            }

            return bResult;
        }


        #endregion

        public bool WebServicesTest()
        {
            bool bResult = false;


            for (int i = 0; i<3; i++)
            {
                try
                {
                    Task<bool> httpTask_WebServices = Http_Post_Async("Start Test Web Services", str_Web_Services_Command, null, str_Web_Services_Return);
                    httpTask_WebServices.Wait();
                    bResult = httpTask_WebServices.Result;          
                }
                catch (Exception)
                {
                    bResult = false;
                    ReConnectWifi();
                }

                if (bResult)
                {
                    break;
                }

                
            }
           


            return bResult;
        }


        public bool TestVoltage()
        {
            bool bResult = false; 
            for (int i = 0; i < iVoltage_TestTimes; i++)
            {
                COM1.Open(true);
                COM1.RunCommandLine("R0", "");
                Thread.Sleep(500);
                Byte[] byteArray = COM1.ReadBytes();
                string str_V_ALL = BitConverter.ToString(byteArray);
                COM1.Close(true);

                if (string.IsNullOrEmpty(str_V_ALL))
                {
                    UIHandleHelper.ShowRunLog("Read voltage fail.",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Test voltage",
                                                               TestItemContent: "TB Read voltage value is null.",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err032.ToString());

                    bResult = false;
                    continue;
                }

                string str_V_1 = str_V_ALL.Split('-')[0];
                string str_V_2 = str_V_ALL.Split('-')[1];

                double d_V = CalculateVoltage(str_V_1, str_V_2);

                if (d_V <= fVoltage_UpperLimit_3V3 && d_V >= fVoltage_LowerLimit_3V3)
                {
                    UIHandleHelper.ShowRunLog($"{i+1}Times -> Test voltage test pass: {d_V} [{fVoltage_LowerLimit_3V3}, {fVoltage_UpperLimit_3V3}] V",
                                                                FailColor: false,
                                                                ShowGridView: true,
                                                                TestItemName: "Test voltage",
                                                                TestItemContent: $"{i + 1}Times -> Voltage: {d_V} [{fVoltage_LowerLimit_3V3}, {fVoltage_UpperLimit_3V3}] V",
                                                                TestItemResult: true);

                    bResult = true;
                    break;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"{i + 1}Times -> Test voltage test fail: {d_V} [{fVoltage_LowerLimit_3V3}, {fVoltage_UpperLimit_3V3}] V",
                                                                FailColor: true,
                                                                ShowGridView: true,
                                                                TestItemName: "Test voltage",
                                                                TestItemContent: $"{i + 1}Times -> Voltage: {d_V} [{fVoltage_LowerLimit_3V3}, {fVoltage_UpperLimit_3V3}] V",
                                                                TestItemResult: false,
                                                                ErrorCode: TestErrorCode.ErrorCode.Err032.ToString());

                    bResult = false;
                }

                if (i == iVoltage_TestTimes-1)
                {
                    break;
                }

            }

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

        public bool UartTest()
        {
            bool bResult = false;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Task<bool> httpTask_UartTest1 = Http_Post_Async("Uart Test Unshort TP1 TP2", str_UART_Test_Command_1, str_UART_Test_PostData_1, str_UART_Test_Command_Return_1);
                    httpTask_UartTest1.Wait();
                    bResult = httpTask_UartTest1.Result;
                }
                catch (Exception)
                {
                    bResult = false;
                    ReConnectWifi();
                }


                 
                

                if (bResult)
                {
                    COM1.RunCommandLine("1O", "");
                    UIHandleHelper.ShowRunLog("Module Uart TP1 TP2 short on...");
                    Thread.Sleep(iCommand_Send_Interval_Second);

                    try
                    {
                        Task<bool> httpTask_UartTest2 = Http_Post_Async("Uart Test Short TP1 TP2", str_UART_Test_Command_2, str_UART_Test_PostData_2, str_UART_Test_Command_Return_2);
                        httpTask_UartTest2.Wait();
                        bResult = httpTask_UartTest2.Result;
                    }
                    catch (Exception)
                    {
                        bResult = false;
                        ReConnectWifi();
                    }
                  

                    COM1.RunCommandLine("1F", "");
                    UIHandleHelper.ShowRunLog("Module Uart TP1 TP2 short off...");
                    Thread.Sleep(iCommand_Send_Interval_Second);
                }


                if (bResult)
                {
                    break;
                }

               

            }

            return bResult;
        }


        public bool IperfTest()
        {
            bool bResult = false;

            for (int i = 0; i < 3; i++)
            {
                ProcessHelper.KillProcessByName("iperf");

                Thread.Sleep(500);
                if (File.Exists(str_IperfFolderPath + "\\" + str_IperfLogName))
                {
                    File.Delete(str_IperfFolderPath + "\\" + str_IperfLogName);
                }

                if (File.Exists(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfBatName}"))
                {
                    File.Delete(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfLogName}");
                }
                Thread.Sleep(500);


                bResult = CreateIperfBat(str_IperfFolderPath, str_IperfBatContent, str_IperfBatName);

                if (bResult)
                {
                    ExecuteBatchFileNoWait($"{PathHelper.GetCurrentExeDirPath()}\\{str_IperfBatName}");

                    try
                    {
                        Thread.Sleep(1000);
                        Task<bool> httpTask_Iperf = Http_Post_Async("Set Server 192.168.142.2", str_IperfServerCommand, str_IperfServer_PostData, str_IperfServer_Return);
                        httpTask_Iperf.Wait();
                        bResult = httpTask_Iperf.Result;
                    }
                    catch (Exception)
                    {
                        bResult = false;
                        ReConnectWifi();
                    }

                    
                }






                ///检查iperf的log
                if (bResult)
                {
                    Thread.Sleep(10000);
                    //wifiHelper.DisConnectWifi();
                    ProcessHelper.KillProcessByName("iperf");
                    Thread.Sleep(1000);
                    try
                    {
                        string str_TxtReadAll = File.ReadAllText(str_IperfFolderPath + "\\" + str_IperfLogName);

                        if (str_TxtReadAll.Contains("Mbits/sec"))
                        {
                            string str_IperfSpeed = str_TxtReadAll.Substring(str_TxtReadAll.IndexOf("Bytes") + 5,
                                              str_TxtReadAll.IndexOf("Mbits/sec") - str_TxtReadAll.IndexOf("Bytes") - 5).Trim();

                            if (float.Parse(str_IperfSpeed) >= iIperf_LowerLimit)
                            {
                                UIHandleHelper.ShowRunLog($"Iperf speed test pass: {str_IperfSpeed} [{iIperf_LowerLimit}, ] Mbits/sec",
                                                                   FailColor: false,
                                                                   ShowGridView: true,
                                                                   TestItemName: "Test Iperf",
                                                                   TestItemContent: $"Iperf speed: {str_IperfSpeed} [{iIperf_LowerLimit}, ] Mbits/sec",
                                                                   TestItemResult: true);
                                bResult = true;
                                break;
                            }
                            else
                            {
                                UIHandleHelper.ShowRunLog($"Iperf speed test fail: {str_IperfSpeed} [{iIperf_LowerLimit}, ] Mbits/sec",
                                                                   FailColor: true,
                                                                   ShowGridView: true,
                                                                   TestItemName: "Test Iperf",
                                                                   TestItemContent: $"Iperf speed: {str_IperfSpeed} [{iIperf_LowerLimit}, ] Mbits/sec",
                                                                   TestItemResult: false,
                                                                   ErrorCode: TestErrorCode.ErrorCode.Err023.ToString());
                                bResult = false;
                            }

                        }
                        else
                        {
                            UIHandleHelper.ShowRunLog("Iperf speed test check log fail",
                                                                   FailColor: true,
                                                                   ShowGridView: true,
                                                                   TestItemName: "Test Iperf",
                                                                   TestItemContent: "Iperf speed test check log fail",
                                                                   TestItemResult: false,
                                                                   ErrorCode: TestErrorCode.ErrorCode.Err023.ToString());
                            bResult = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        UIHandleHelper.ShowRunLog($"Check Iperf Log Error: {ex}");
                        bResult = false;
                    }
                }

            }

            return bResult;
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


    }
}
