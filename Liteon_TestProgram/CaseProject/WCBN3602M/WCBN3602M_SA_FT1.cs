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

namespace Liteon_TestProgram.CaseProject
{
    internal class WCBN3602M_SA_FT1 : CustomerFWTest_Base
    {

        /// <summary>
        /// 用于添加MAC的前缀，比如“23S”，默认是空
        /// </summary>
        protected override string str_MacPrefix { get; set; } = "";


        /// <summary>
        /// 用于添加MAC的后缀，比如“A”，默认是空
        /// </summary>
        protected override string str_MacSuffix { get; set; } = "A";

        #region ReadCustomerEncryptIniEx

        public static int iUART_Test_Switch = 1;
        public static string str_Uart_Test_Command = null;
        public static string str_Uart_Test_PostData = null;
        public static string str_Uart_UnshortTest_Return = null;
        public static string str_Uart_ShortTest_Return = null;
        public string str_UpLoad_ITDB_IP = null;

        #endregion





        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================


        public WCBN3602M_SA_FT1(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("2.增加Uart J3-J4测试(20250925)");

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
            list_str.Add("1.COMA请填测试Mini TB008小板的串口\r\n");
            list_str.Add("2.测试Mini TB008小板: 1O 1F控制给板子上下电\r\n");
            list_str.Add("3.测试Mini TB008小板: 2O 2F控制给J3 J4短路\r\n");
            list_str.Add("4.测试电脑需要使用到无线网卡和蓝牙适配器.\r\n");
            list_str.Add("5.用到无线网卡名需要填到设备名称1中.\r\n");
            list_str.Add("6.(2025-12-22)添加iperf测试");
            list_str.Add("7.(2026-03-13)添加上传客户FW版本到IT数据库，便于后面在线列印");

            return list_str;
        }



        public override bool Func_TestPre()
        {
            bool bResult = false;

            //读取额外的配置文件
            bResult = ReadCustomerEncryptIni();

            if (bResult)
            {
                bResult = ReadCustomerEncryptIniEx();
            }

            return bResult;
        }


        public override bool Func_TestInit()
        {

            bool bInitResult = false;

            #region testinit的额外补充

            try
            {

                if (System.IO.File.Exists($"{str_IperfFolderPath}\\{str_IperfLogName}"))
                {
                    System.IO.File.Delete($"{str_IperfFolderPath}\\{str_IperfLogName}");
                }
                if (File.Exists(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfBatName}"))
                {
                    File.Delete(@$"{PathHelper.GetCurrentExeDirPath()}\{str_IperfLogName}");
                }


                bInitResult = COM1.RunCommandLine("1F", "");
                bInitResult = COM1.RunCommandLine("2F", "");

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

            #region 上传客户FW版本

            if (bTestResult)
            {
                bTestResult = UploadCustomerFWVer(str_FW_Version);
            }

            #endregion

            #region WebServices

            if (bTestResult)
            {
                Thread.Sleep(iCommand_Send_Interval_Second);
                bTestResult = WebServicesTest();
            }

            #endregion

            #region J3-J4短路测试

            if (bTestResult && (iUART_Test_Switch == 1))
            {
                Thread.Sleep(iCommand_Send_Interval_Second);
                bTestResult = UartTest();
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

            if (bTestResult && (iTestSwitch_GPIO == 1))
            {
                Thread.Sleep(iCommand_Send_Interval_Second);
                bTestResult = GPIOTest();
            }

            #endregion





            return bTestResult;
        }





        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================



        public bool ReadCustomerEncryptIniEx()
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

                #region [UART_Test]

                IniHelper.GetIniStr("UART_Test", "TestSwitch", "1", ValTemp, 50, str_IniPath);
                iUART_Test_Switch = int.Parse(ValTemp.ToString());

                
                IniHelper.GetIniStr("UART_Test", "Uart_Test_Command", "null", ValTemp, 50, str_IniPath);
                str_Uart_Test_Command = ValTemp.ToString();

                IniHelper.GetIniStr("UART_Test", "Uart_Test_PostData", "null", ValTemp, 50, str_IniPath);
                str_Uart_Test_PostData = ValTemp.ToString();

                IniHelper.GetIniStr("UART_Test", "Uart_UnshortTest_Return", "null", ValTemp, 50, str_IniPath);
                str_Uart_UnshortTest_Return = ValTemp.ToString();

                IniHelper.GetIniStr("UART_Test", "Uart_ShortTest_Return", "null", ValTemp, 50, str_IniPath);
                str_Uart_ShortTest_Return = ValTemp.ToString();

                IniHelper.GetIniStr("Test_Items", "UpLoad_ITDB_IP", "null", ValTemp, 100, str_IniPath);
                str_UpLoad_ITDB_IP = ValTemp.ToString();

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


        public bool UartTest()
        {
            bool bResult = false;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Thread.Sleep(iCommand_Send_Interval_Second);
                    Task<bool> httpTask_WebServices = Http_Post_Async("Uart Unshort Test", str_Uart_Test_Command, str_Uart_Test_PostData, str_Uart_UnshortTest_Return);
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

            if (bResult)
            {
                bResult = COM1.RunCommandLine("2O", "");
                Thread.Sleep(500);
            }

            if (bResult)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        Thread.Sleep(iCommand_Send_Interval_Second);
                        Task<bool> httpTask_WebServices = Http_Post_Async("Uart Short Test", str_Uart_Test_Command, str_Uart_Test_PostData, str_Uart_ShortTest_Return);
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
            }


            return bResult;
        }

        public bool UploadCustomerFWVer(string str_Ver)
        {
            bool bResult = false;
 
            string str_Comm_ITDB_Command = Generate_ITDB_Comm_CMD(str_UpLoad_ITDB_IP, "add",
                                                                    struct_Barcode.stru_str_sRevDUTMac, "VER", str_Ver.Replace("\"", ""));

            if (string.IsNullOrEmpty(str_Comm_ITDB_Command))
            {
                UIHandleHelper.ShowRunLog($"Upload Customer FW: Generate IT DB CMD Fail",
                                                             FailColor: true,
                                                             ShowGridView: true,
                                                             TestItemName: "IT DB CMD",
                                                             TestItemContent: "Generate IT DB CMD Fail",
                                                             TestItemResult: false,
                                                             ErrorCode: TestErrorCode.ErrorCode.Err058.ToString());

                return false;
            }


            bResult = UpdateFromITDB(str_Comm_ITDB_Command);
            Thread.Sleep(200);
            if (bResult == false)
            {
                if (UpdateFromITDB(str_Comm_ITDB_Command) )
                {
                    bResult = true;
                }
                else
                {
                    bResult = false;
                }
            }




            if (bResult)
            {
                UIHandleHelper.ShowRunLog("Customer FW Ver uploaded successfully",
                                                                FailColor: false,
                                                                ShowGridView: true,
                                                                TestItemName: "Upload Cust FW Ver",
                                                                TestItemContent: "Customer FW Ver uploaded successfully",
                                                                TestItemResult: true);
            }
            else
            {
                UIHandleHelper.ShowRunLog("Customer FW Ver upload failed",
                                                            FailColor: true,
                                                            ShowGridView: true,
                                                            TestItemName: "Upload Cust FW Ver",
                                                            TestItemContent: "Customer FW Ver upload failed;",
                                                            TestItemResult: false,
                                                             ErrorCode: TestErrorCode.ErrorCode.Err061.ToString());
            }



            return bResult;
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


        public bool UpdateFromITDB(string str_Cmd)
        {
            bool bResult = false;
            try
            {
                Task<bool> httpTask_FWVer = Http_Get_Async("Upload Customer FW", str_Cmd, "OK");
                httpTask_FWVer.Wait();

                if (httpTask_FWVer.Result)
                {
                    bResult = true;
                }
                else
                {
                    bResult = false;
                }
            }
            catch (Exception)
            {
                bResult = false;
            }

            return bResult;
        }


    }
}
