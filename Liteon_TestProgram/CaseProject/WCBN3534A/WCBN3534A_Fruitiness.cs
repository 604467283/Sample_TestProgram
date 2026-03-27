using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Utilities;
using Liteon_TestProgram.Utilities.IOHelpers.Extensions;
using static Liteon_TestProgram.Base.Class_Variable;
using Liteon_TestProgram.TestFunc.ICT.LogDataCollection_V1;
using Liteon_TestProgram.Utilities.TCPIP_Helper;
using OfficeOpenXml.Drawing.Slicer.Style;
using System.Text;
using static OfficeOpenXml.ExcelErrorValue;
using ScottPlot;
using OpenTK;

namespace Liteon_TestProgram.CaseProject
{
    internal class WCBN3534A_Fruitiness : CaseCodeBase
    {
        string str_ShowDataGridViewInfo = "";

        private float f_2G_CableLoss;
        private float f_5G_CableLoss;

        private float f_2G_UpperLimit;
        private float f_2G_LowerLimit;
        private float f_5G_UpperLimit;
        private float f_5G_LowerLimit;
        private string str_BDF_MD5;

        static string str_ExeName = "adb.exe";
        string str_ExeFolderPath => $"{PathHelper.GetParentDirectoryPath(PathHelper.GetCurrentExeDirPath())}\\platform-tools";



        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; set; }
        public TestForm _testForm;
        string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public WCBN3534A_Fruitiness(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.需要使用到IQ-Xel\r\n");
            return list_str;
        }


        public override void Func_TestMac()
        {
            macBD_Relation = MacBD_Relation.PlusOne;
            if (struct_NormalINI.stru_b_DebugMode)
            {
                struct_TestVariable.iMAC_OriginalLength = 25;
                struct_TestVariable.iMAC_ExtractStartPosition = 0;
                struct_TestVariable.iMACLength = 12;

                struct_TestVariable.iBD_OriginalLength = 25;
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
            bool bResult = true;

            bResult = ReadCustomerEncryptIni();

            if (bResult)
            {
                bResult = ReadTestIni();
            }
          

            return bResult;
        }

        public override bool Func_TestInit()
        {
            bool bInitResult = true;

            UIHandleHelper.ShowRunLog("Wait for the startup for 20 seconds...");
                                                              
            Thread.Sleep(20000);


            return bInitResult;
        }

        public override bool Func_TestFlow()
        {
            bool bTestResult = false;


            bTestResult = CheckDeviceOnline();

            if (bTestResult)
            {
                bTestResult = CheckBDFMD5("shell md5sum /lib/firmware/bdwlang.elf");
            }



            if (bTestResult)
            {
                UIHandleHelper.ShowRunLog("=============2412 Ant1===============");

                RunStartRFCmd("shell \"su -c 'rmmod wlan'\"", 1, "");
                Thread.Sleep(1000);
                RunStartRFCmd("shell \"su -c 'insmod /lib/modules/5.15.47-mtk+g-usi-v2.2-87e4e4a45d60/wlan.ko country_code=US con_mode_ftm=5'\"", 1, "");
                Thread.Sleep(1000);
                RunStartRFCmd("shell \"su -c 'myftm -i wlan0 -J -B dbs'\"", 2, "qca6174SetDBS() success = 0");
                RunStartRFCmd("shell \"su -c 'myftm -i wlan0 -J -I 1 --dpdflag -M 1 -r 22 -f 2412 -c 0 -p 15 -a 1 -t 3'\"", 2, "WlanATSetWifiTX done with sucess");

                bTestResult = GetPowerFromIQXel(1, true);

                bool bStopResult = RunStopRFCmd("shell \"su -c 'myftm -i wlan0 -J -I 1 --dpdflag -M 1 -r 22 -f 2412 -c 0 -p 15 -a 1 -t 0'\"");

                bTestResult = bTestResult&& bStopResult;
            }

            if (bTestResult)
            {
                UIHandleHelper.ShowRunLog("=============2412 Ant2===============");

                RunStartRFCmd("shell \"su -c 'rmmod wlan'\"", 1, "");
                Thread.Sleep(1000);
                RunStartRFCmd("shell \"su -c 'insmod /lib/modules/5.15.47-mtk+g-usi-v2.2-87e4e4a45d60/wlan.ko country_code=US con_mode_ftm=5'\"", 1, "");
                Thread.Sleep(1000);
                RunStartRFCmd("shell \"su -c 'myftm -i wlan0 -J -B dbs'\"", 2, "qca6174SetDBS() success = 0");
                RunStartRFCmd("shell \"su -c 'myftm -i wlan0 -J -I 1 --dpdflag -M 1 -r 22 -f 2412 -c 0 -p 15 -a 2 -t 3'\"", 2, "WlanATSetWifiTX done with sucess");


                bTestResult = GetPowerFromIQXel(2, true);

                bool bStopResult = RunStopRFCmd("shell \"su -c 'myftm -i wlan0 -J -I 1 --dpdflag -M 1 -r 22 -f 2412 -c 0 -p 15 -a 2 -t 0'\"");

                bTestResult = bTestResult && bStopResult;
            }

            if (bTestResult)
            {
                UIHandleHelper.ShowRunLog("=============5180 Ant1===============");

                RunStartRFCmd("shell \"su -c 'rmmod wlan'\"", 1, "");
                Thread.Sleep(1000);
                RunStartRFCmd("shell \"su -c 'insmod /lib/modules/5.15.47-mtk+g-usi-v2.2-87e4e4a45d60/wlan.ko country_code=US con_mode_ftm=5'\"", 1, "");
                Thread.Sleep(1000);
                RunStartRFCmd("shell \"su -c 'myftm -i wlan0 -J -B dbs'\"", 2, "qca6174SetDBS() success = 0");
                RunStartRFCmd("shell \"su -c 'myftm -i wlan0 -J -I 0 -M 1 -r 22 -f 5180 -c 0 -p 12.5 -a 1 -t 3'\"", 2, "WlanATSetWifiTX done with sucess");

                bTestResult = GetPowerFromIQXel(1, false);

                bool bStopResult = RunStopRFCmd("shell \"su -c 'myftm -i wlan0 -J -I 0 -M 1 -r 22 -f 5180 -c 0 -p 12.5 -a 1 -t 0'\"");

                bTestResult = bTestResult && bStopResult;
            }

            if (bTestResult)
            {
                UIHandleHelper.ShowRunLog("=============5180 Ant2===============");

                RunStartRFCmd("shell \"su -c 'rmmod wlan'\"", 1, "");
                Thread.Sleep(1000);
                RunStartRFCmd("shell \"su -c 'insmod /lib/modules/5.15.47-mtk+g-usi-v2.2-87e4e4a45d60/wlan.ko country_code=US con_mode_ftm=5'\"", 1, "");
                Thread.Sleep(1000);
                RunStartRFCmd("shell \"su -c 'myftm -i wlan0 -J -B dbs'\"", 2, "qca6174SetDBS() success = 0");
                RunStartRFCmd("shell \"su -c 'myftm -i wlan0 -J -I 0 -M 1 -r 22 -f 5180 -c 0 -p 12.5 -a 2 -t 3'\"", 2, "WlanATSetWifiTX done with sucess");

                bTestResult = GetPowerFromIQXel(2, false);

                bool bStopResult = RunStopRFCmd("shell \"su -c 'myftm -i wlan0 -J -I 0 -M 1 -r 22 -f 5180 -c 0 -p 12.5 -a 2 -t 0'\"");

                bTestResult = bTestResult && bStopResult;
            }

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
                UIHandleHelper.DataGridViewShow("End", "Failed to process the file", false);
                bEndResult = false;
            }


            return bEndResult;
        }


        public override void Func_ResourceRelease()
        {

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



                #region [PowerSpec]

                IniHelper.GetIniStr("PowerSpec", "2G_UpperLimit", "null", ValTemp, 50, str_IniPath);
                f_2G_UpperLimit = float.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("PowerSpec", "2G_LowerLimit", "null", ValTemp, 10, str_IniPath);
                f_2G_LowerLimit = float.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("PowerSpec", "5G_UpperLimit", "null", ValTemp, 50, str_IniPath);
                f_5G_UpperLimit = float.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("PowerSpec", "5G_LowerLimit", "null", ValTemp, 10, str_IniPath);
                f_5G_LowerLimit = float.Parse(ValTemp.ToString());

                #endregion


                IniHelper.GetIniStr("BDF", "BDF_MD5", "null", ValTemp, 100, str_IniPath);
                str_BDF_MD5 = ValTemp.ToString();



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

                #region [CableLoss]

                IniHelper.GetIniStr("CableLoss", "2G_CableLoss", "null", ValTemp, 50, str_IniPath);
                f_2G_CableLoss = float.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("CableLoss", "5G_CableLoss", "null", ValTemp, 50, str_IniPath);
                f_5G_CableLoss = float.Parse(ValTemp.ToString());

                #endregion




            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取{str_CaseProjectName} Normal ini failed:{ex.StackTrace}");
                UIHandleHelper.ShowRunLog($"读取{str_CaseProjectName} Normal ini failed:{ex.StackTrace}", false);
                return false;
            }

            return true;
        }




        public async Task<string> TCPIP_CommandAsync(bool bSendAndRecv, string str_CMD, int iDelaymillisecond)
        {
            string str_Data = "";
            try
            {
                // 创建TCP客户端实例
                using var tcpClient = new TCPIP_ClientHelper("192.168.100.254", 24000);

                // 连接到服务器
                await tcpClient.ConnectAsync();
                UIHandleHelper.ShowRunLog("已连接到服务器");

                if (bSendAndRecv)
                {
                    // 发送数据并接收响应
                    UIHandleHelper.ShowRunLog($"Send: {str_CMD}");
                    string response = await tcpClient.SendAndReceiveAsync(str_CMD);
                    UIHandleHelper.ShowRunLog($"Recv: {response}");

                    str_Data = response;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Send: {str_CMD}");
                    // 单独发送数据
                    await tcpClient.SendAsync(str_CMD);

                    // 单独接收数据
                    //string anotherResponse = await tcpClient.ReceiveAsync();
                    //MessageBox.Show($"收到另一个响应: {anotherResponse}");
                }

                await Task.Delay(iDelaymillisecond);

            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"发生错误: {ex.Message}", true);
                return str_Data = "CatchError";
            }

            return str_Data;
        }





        public  bool GetPowerFromIQXel(int iAntNum, bool b_2GOr5G)
        {
            string str_CMD = @"SYS
DCL:HMOD
*CLS
*RST
FORM:READ:DATA ASC
ROUT1;
PORT:RES RF1A,VSA
PORT:RES RF2A,VSG
";

            Task<string> task_TCPIP = TCPIP_CommandAsync(false, str_CMD, 10);
            task_TCPIP.Wait();
            string sResult = task_TCPIP.Result;

            if (sResult == "CatchError")
            {
                return false;
            }


            if (b_2GOr5G)
            {
                str_CMD = @"VSA1;FREQ:cent 2412000000
VSA1;CAPT:TIME 0.03
VSA1;RLEV:AUTO:TIME 0.01
";
            }
            else
            {
                str_CMD = @"VSA1;FREQ:cent 5180000000
VSA1;CAPT:TIME 0.03
VSA1;RLEV:AUTO:TIME 0.01
";
            }
           

            task_TCPIP = TCPIP_CommandAsync(false, str_CMD, 10);
            task_TCPIP.Wait();
            sResult = task_TCPIP.Result;
            if (sResult == "CatchError")
            {
                return false;
            }



            str_CMD = @"VSA1 ;RLEVel:AUTO
";

            task_TCPIP = TCPIP_CommandAsync(false, str_CMD, 2000);
            task_TCPIP.Wait();
            sResult = task_TCPIP.Result;
            if (sResult == "CatchError")
            {
                return false;
            }




            str_CMD = @"CHAN1
VSA1 ;init
WIFI
calc:pow 0, 1
calc:txq 0, 1
calc:ccdf 0, 1
calc:ramp 0, 1
calc:spec 0, 1
";

            task_TCPIP = TCPIP_CommandAsync(false, str_CMD, 3000);
            task_TCPIP.Wait();
            sResult = task_TCPIP.Result;
            if (sResult == "CatchError")
            {
                return false;
            }



            str_CMD = @"WIFI
CALC:POW 0,5
FETC:POW:AVER?
";

            task_TCPIP = TCPIP_CommandAsync(true, str_CMD, 10);
            task_TCPIP.Wait();
            sResult = task_TCPIP.Result;

            if (sResult == "CatchError")
            {
                return false;
            }
            else if (sResult.ToLower().Contains("e+") || sResult.ToLower().Contains("e-"))
            {
                if (GetPowerAvg(sResult, b_2GOr5G, iAntNum))
                {
                    return true;
                }
                return false;
            }
            else
            {
                UIHandleHelper.ShowRunLog($"获取Power返回信息格式错误: {sResult}",
                                                                FailColor: true,
                                                                ShowGridView: true,
                                                                TestItemName: $"获取Power返回信息",
                                                                TestItemContent: $"格式错误: {sResult}",
                                                                TestItemResult: false,
                                                                ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                return false;
            }

        }

        public bool GetPowerAvg(string input, bool b_2GOr5G, int iAntNum)
        {
            // 1. 以逗号分割字符串
            string[] parts = input.Split(',');

            // 2. 过滤掉 "0" 并转换为 double
            var numbers = parts
                .Where(p => p != "0")
                .Select(p => double.Parse(p, System.Globalization.NumberStyles.Float))
                .ToList();

            // 3. 计算平均值
            double average = numbers.Sum() / numbers.Count;

            if (b_2GOr5G)
            {
                
                average = average + f_2G_CableLoss;
                average = (double)Math.Round(average, 3);

                if (average >= f_2G_LowerLimit && average <= f_2G_UpperLimit)
                {
                    UIHandleHelper.ShowRunLog($"Check 2G {iAntNum} Power Pass: {average} [{f_2G_LowerLimit}, {f_2G_UpperLimit}]",
                                                                  FailColor: false,
                                                                  ShowGridView: true,
                                                                  TestItemName: $"Check 2G  {iAntNum} Power",
                                                                  TestItemContent: $"{average} [{f_2G_LowerLimit}, {f_2G_UpperLimit}]",
                                                                  TestItemResult: true);

                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Check 2G {iAntNum} Power Fail: {average} [{f_2G_LowerLimit}, {f_2G_UpperLimit}]",
                                                                 FailColor: true,
                                                                 ShowGridView: true,
                                                                 TestItemName: $"Check 2G {iAntNum} Power",
                                                                 TestItemContent: $"{average} [{f_2G_LowerLimit}, {f_2G_UpperLimit}]",
                                                                 TestItemResult: false,
                                                                 ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                    return false;
                }
            }
            else
            {
                average = average + f_5G_CableLoss;
                average = (double)Math.Round(average, 3);

                if (average >= f_5G_LowerLimit && average <= f_5G_UpperLimit)
                {
                    UIHandleHelper.ShowRunLog($"Check 5G Power {iAntNum} Pass: {average} [{f_5G_LowerLimit}, {f_5G_UpperLimit}]",
                                                                  FailColor: false,
                                                                  ShowGridView: true,
                                                                  TestItemName: $"Check 5G {iAntNum} Power",
                                                                  TestItemContent: $"{average} [{f_5G_LowerLimit}, {f_5G_UpperLimit}]",
                                                                  TestItemResult: true);

                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Check 5G {iAntNum} Power Fail: {average} [{f_5G_LowerLimit}, {f_5G_UpperLimit}]",
                                                                 FailColor: true,
                                                                 ShowGridView: true,
                                                                 TestItemName: $"Check 5G {iAntNum} Power",
                                                                 TestItemContent: $"{average} [{f_5G_LowerLimit}, {f_5G_UpperLimit}]",
                                                                 TestItemResult: false,
                                                                 ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                    return false;
                }
            }

        }

        public bool CheckDeviceOnline()
        {
            /*
             C:\Users\Administrator\Desktop\platform-tools>adb devices
            List of devices attached
            762b2fb43f1d1f26        device
             */
            bool bRetVal= false;
            CustomProcess customProcess = new($"{str_ExeFolderPath}\\{str_ExeName}");
            bRetVal = customProcess.RunCommandLine("devices", "\tdevice", out string msg, 1000, 3);

            if (bRetVal)
            {
                if (msg.Contains("\tdevice"))
                {
                    UIHandleHelper.ShowRunLog("Check Device Online Pass",
                                                             FailColor: false,
                                                             ShowGridView: true,
                                                             TestItemName: "Check Device Online",
                                                             TestItemContent: $"{msg.Replace("List of devices attached", "").Replace("\r", "").Replace("\n", "")}",
                                                             TestItemResult: true);


                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Check Device Online Fail",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Check Device Online",
                                                               TestItemContent: $"{msg.Replace("List of devices attached", "").Replace("\r", "").Replace("\n", "")}",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                    return false;
                }
            }
            else
            {
                UIHandleHelper.ShowRunLog($"Check Device Online error: {msg}",
                                                              FailColor: true,
                                                              ShowGridView: true,
                                                              TestItemName: "Check Device Online",
                                                              TestItemContent: $"Check Device Online error: {msg}",
                                                              TestItemResult: false,
                                                              ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                return false;
            }



        }




        public bool RunStopRFCmd(string strCmd)
        {
            bool bRetVal = false;
            CustomProcess customProcess = new($"{str_ExeFolderPath}\\{str_ExeName}");
            bRetVal = customProcess.RunCommandLine(strCmd, "WlanATSetWifiTX done with sucess", out string msg, 5000, 3);

            if (bRetVal)
            {
                if (msg.Contains("WlanATSetWifiTX done with sucess"))
                {
                    UIHandleHelper.ShowRunLog("Stop RF Pass",
                                                             FailColor: false,
                                                             ShowGridView: true,
                                                             TestItemName: "STop RF",
                                                             TestItemContent: "WlanATSetWifiTX done with sucess",
                                                             TestItemResult: true);


                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Stop RF Fail",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Stop RF",
                                                               TestItemContent: $"{msg}",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                    return false;
                }
            }
            else
            {
                UIHandleHelper.ShowRunLog($"Stop RF Fail error: {msg}",
                                                              FailColor: true,
                                                              ShowGridView: true,
                                                              TestItemName: "Stop RF",
                                                              TestItemContent: $"Stop RF error: {msg}",
                                                              TestItemResult: false,
                                                              ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                return false;
            }



        }


        public bool RunStartRFCmd(string strCmd, int iRetryTimes, string str_SucessMark)
        {
            bool bRetVal = false;
            CustomProcess customProcess = new($"{str_ExeFolderPath}\\{str_ExeName}");
            bRetVal = customProcess.RunCommandLine(strCmd, str_SucessMark, out string msg, 5000, iRetryTimes);

            if (bRetVal)
            {
                if (msg.Contains(str_SucessMark))
                {
                    UIHandleHelper.ShowRunLog($"Send RF Cmd Pass {strCmd}");
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Send RF Cmd Fail  {strCmd}", true);

                    return false;
                }
            }
            else
            {
                UIHandleHelper.ShowRunLog($"Send RF Cmd error: {msg}", true);

                return false;
            }



        }


        public bool CheckBDFMD5(string strCmd)
        {
            /*
             * C:\Users\Administrator\Desktop\WCBN3534A_c\ADB>adb shell md5sum /lib/firmware/bdwlang.elf
                ec4e9015ec5d69306609ddef3927fcc8  /lib/firmware/bdwlang.elf
             */
            bool bRetVal = false;
            CustomProcess customProcess = new($"{str_ExeFolderPath}\\{str_ExeName}");
            bRetVal = customProcess.RunCommandLine(strCmd, "bdwlang.elf", out string msg, 2000, 3);

            if (bRetVal)
            {
                string str_Temp = msg.Replace("\r", "").Replace("\n", "");
                if (msg.Contains(str_BDF_MD5))
                {
                    UIHandleHelper.ShowRunLog($"Check BDF MD5 Pass {str_Temp}",
                                                             FailColor: false,
                                                             ShowGridView: true,
                                                             TestItemName: "BDF MD5",
                                                             TestItemContent: $"{str_Temp}",
                                                             TestItemResult: true);


                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Check BDF MD5 Fail {str_Temp}",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "BDF MD5",
                                                               TestItemContent: $"{str_Temp}",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                    return false;
                }
            }
            else
            {
                UIHandleHelper.ShowRunLog($"Check BDF MD5 error: {msg}",
                                                              FailColor: true,
                                                              ShowGridView: true,
                                                              TestItemName: "BDF MD5",
                                                              TestItemContent: $"Check BDF MD5 error: {msg}",
                                                              TestItemResult: false,
                                                              ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                return false;
            }



        }






    }
}
