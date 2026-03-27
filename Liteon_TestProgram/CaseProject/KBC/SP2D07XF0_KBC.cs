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
using static Liteon_TestProgram.Base.Class_Variable;
using Renci.SshNet.Sftp;
using Liteon_TestProgram.Save_LogFile;
using ScottPlot.Colormaps;
using static SkiaSharp.SKImageFilter;
using Liteon_TestProgram.TestFunc.ICT.LogDataCollection_V1;
using System.Reflection.Metadata;


namespace Liteon_TestProgram.CaseProject
{
    internal class SP2D07XF0_KBC : CaseCodeBase
    {
        public string str_ShowDataGridViewInfo = "";

        public string str_Ini_KBC_Ver = "";

        public virtual string str_ReadKBCVerExeName { get; set; } = "ReadKBCFW(555-000826-01r1).exe";
        public virtual string str_ReadKBCVerExeFolderName { get; set; } = "ReadKBCFW(555-000826-01r1)";

        //表达式体属性（=>), 每次访问时动态计算, 基于当前值实时更新
        public virtual string str_ReadKBCVerExeFolderPath => $"{PathHelper.GetParentDirectoryPath(PathHelper.GetCurrentExeDirPath())}\\{str_ReadKBCVerExeFolderName}"; 

        public  string str_ReadKBCVerLogName = "FWResult.txt";
        public  string str_ReadKBCCFGName = "KBCFW.txt";




        //用于判断ICT Flow是否正确    =>  <EVO   >  最多可以替换成6个字符
        protected virtual string str_ICTFlowMark { get; set; } = "EVO   ";


        /// <summary>
        /// 用于检查ICT Flow的Md5
        /// </summary>
        protected virtual string str_ICTFlowMd5 { get; set; } = "12345";

        /// <summary>
        /// 用于读取ICT测试数据返回的超时时间 ms
        /// </summary>
        protected virtual int iICTTestReadTimeout { get; set; } = 10000;


        /// <summary>
        /// 用于读取ICT测试数据返回的超时时间 ms
        /// </summary>
        protected virtual int iICTTestFlowImportTimeout { get; set; } = 15000;

        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; set; }
        public TestForm _testForm;
        public string str_ErrorCode = "Err000";
        public SfcFile sfcFile = new();


        public SP2D07XF0_KBC(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.COMA请填ICT测试的串口, 波特率38400\r\n");
            list_str.Add("2.COMB请填TB测试的串口, 波特率9600,指令1O 1F, 主要用于控制是否和ICT共地\r\n");
            return list_str;
        }


        public override void Func_TestMac()
        {
            macBD_Relation = MacBD_Relation.OnlyMac;
            if (struct_NormalINI.stru_b_DebugMode)
            {
                struct_TestVariable.iMAC_OriginalLength = 24;
                struct_TestVariable.iMAC_ExtractStartPosition = 0;
                struct_TestVariable.iMACLength = 24;

                struct_TestVariable.iBD_OriginalLength = 24;
                struct_TestVariable.iBD_ExtractStartPosition = 0;
                struct_TestVariable.iBDLength = 24;
            }
            else
            {
                struct_TestVariable.iMACLength = 24;
                struct_TestVariable.iBDLength = 24;
            }
        }


        public override bool Func_TestPre()
        {
            bool bResult = false;

            bResult = ReadCustomerEncryptIni();

            #region 自动加载ICTFlow

            if (bResult)
            {
                string str_CaseFolder = new string(str_CaseProjectName.TakeWhile(c => c != '_').ToArray());
                string str_FlowPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{str_CaseProjectName}\\{str_CaseProjectName}_ICTFlow.txt";


                //比对flow的MD5值，如果不一致，直接退出；

                bool isValid = MD5Helper.VerifyMD5(str_FlowPath, str_ICTFlowMd5);

                if (!isValid)
                {
                    MessageBoxEX.Show("ICT Flow MD5值校验失败，请检查ICT Flow文件是否正确", true);
                    Environment.Exit(0);
                }
                else
                {
                    UIHandleHelper.ShowRunLog("ICT Flow MD5值校验Pass.");
                }

                string str_ImportCmd = $"{COM1.BaseSerialPort.PortName} 38400 {str_FlowPath}";

                CustomProcess customProcess = new CustomProcess("zhiDE_CmdLine.exe");

                bResult = customProcess.RunCommandLine(str_ImportCmd, "Pass", out string msg, iICTTestFlowImportTimeout, 1);

                if (bResult)
                {
                    UIHandleHelper.ShowRunLog("Import ICT Flow Pass.");
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Import ICT Flow Fail.", true);
                }
            }

            #endregion


            return bResult;
        }

        public override bool Func_TestInit()
        {
            bool bInitResult = true;

            #region testinit的额外补充

            if (File.Exists($"{str_ReadKBCVerExeFolderPath}\\{str_ReadKBCVerLogName}"))
            {
                File.Delete($"{str_ReadKBCVerExeFolderPath}\\{str_ReadKBCVerLogName}");
            }

            if (File.Exists($"{str_ReadKBCVerExeFolderPath}\\{str_ReadKBCCFGName}") == false)  //KBCFW.txt
            {
                UIHandleHelper.ShowRunLog($"Check file does not exist: {str_ReadKBCCFGName}",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Init - Check CFG",
                                                               TestItemContent: $"Check file does not exist: {str_ReadKBCCFGName}",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err000.ToString());
                bInitResult = false;
            }

            if (bInitResult)
            {
                try
                {
                    COM2.RunCommandLine("1O", "");
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    bInitResult = false;
                }
            }

            #endregion


            return bInitResult;
        }

        public override bool Func_TestFlow()
        {
            bool bTestResult = false;

            UIHandleHelper.ShowRunLog("=============Check Ver===============");

            bTestResult = CreateKBCFWTxt($"{str_ReadKBCVerExeFolderPath}\\KBCFW.txt", str_Ini_KBC_Ver);

            if (bTestResult)
            {
                bTestResult = Check_KBC_Ver();
            }

            if (bTestResult)
            {
                COM2.RunCommandLine("1F", "");
                Thread.Sleep(1000);
            }

            if (bTestResult)
            {
                bTestResult = ICTTest();

                if (bTestResult == false)
                {
                    bTestResult = ICTTest();
                }
            }

            return bTestResult;
        }

        public override bool Func_TestEnd(bool bTestResult)
        {
            bool bEndResult = true;

            COM2.RunCommandLine("1F", "");
            Thread.Sleep(1000);

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
            COM1.Close(true);
            COM2.RunCommandLine("1F", "");
            Thread.Sleep(1000);
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

                if (File.Exists(str_IniPath) == false)
                {
                    throw new ArgumentException($"{str_CaseProjectName}_Encrypt.ini文件不存在。");
                }

                #region [KBC_Ver]

                IniHelper.GetIniStr("KBC_Ver", "Version", "null", ValTemp, 200, str_IniPath);
                str_Ini_KBC_Ver = ValTemp.ToString();
                UIHandleHelper.ShowRunLog($"Target KBC Ver: {str_Ini_KBC_Ver}");

                #endregion



                #region [ICT_Flow]

                IniHelper.GetIniStr("ICT_Flow", "ICT_FlowMark", "null", ValTemp, 10, str_IniPath);
                str_ICTFlowMark = ValTemp.ToString();

                IniHelper.GetIniStr("ICT_Flow", "ICT_FlowMD5", "null", ValTemp, 100, str_IniPath);
                str_ICTFlowMd5 = ValTemp.ToString();

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


        public bool CreateKBCFWTxt(string filePath, string content)
        {
            try
            {
                // 检查文件是否存在
                if (File.Exists(filePath))
                {
                    // 如果存在，删除文件
                    File.Delete(filePath);
                    Console.WriteLine($"{filePath}文件已存在，已删除旧文件。");
                    Thread.Sleep(500);
                }

                // 将字符串写入文件
                File.WriteAllText(filePath, content.Replace(";", "\r\n"));

                Console.WriteLine("文件已成功生成并写入内容。");

                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog("创建KBC FW txt发生错误: " + ex.Message, true);
                return false;
            }

            UIHandleHelper.ShowRunLog("Create KBC FW txt OK.");
            return true;
        }


        #region 接收的数据例子

        /*
         * ------------------------[ASCII],,PASS,EVO   #1,4,8,879mS,p1,00,5897400008E00710,v892.25;
                OPEN#01,50,<=50 R,0,OK;
                SHOR#01,30000,>=200 R,200,OK;
                v--1,800,580mV,500,OK;
                v--2,800,580mV,500,OK;
                v--3,800,580mV,500,OK;
                v--4,800,578mV,500,OK;
                v--5,800,580mV,500,OK;
                v--6,800,580mV,500,OK;
                v--7,800,581mV,500,OK;
                v--8,800,579mV,500,OK;
                v--9,800,579mV,500,OK;
                v-10,800,578mV,500,OK;
                v-11,800,579mV,500,OK;
                v-12,800,580mV,500,OK;
                v-13,800,580mV,500,OK;
                v-14,800,580mV,500,OK;
                v-15,800,576mV,500,OK;
                v-16,800,579mV,500,OK;
                v-17,800,576mV,500,OK;
                v-18,800,570mV,500,OK;
                v-19,800,576mV,500,OK;
                v-20,800,578mV,500,OK;
                v-21,800,577mV,500,OK;
                v-22,800,576mV,500,OK;
                v-23,800,568mV,500,OK;
                v-24,800,559mV,500,OK;
                v-25,800,575mV,500,OK;
                v-26,800,578mV,500,OK;
                v-27,800,571mV,500,OK;
                v-28,800,574mV,500,OK;
                END;
         * */

        #endregion


        public bool ICTTest()
        {
            bool bResult = false;

            UIHandleHelper.ShowRunLog("=============Test ICT===============");

            str_ShowDataGridViewInfo = "";

            byte[] ICT_Reset = new byte[] { 0x01, 0x05, 0x00, 0x11, 0xFF, 0x00, 0xDC, 0x3F };
            byte[] ICT_Start = new byte[] { 0x01, 0x05, 0x00, 0x10, 0xFF, 0x00, 0x8D, 0xFF };

            COM1.RunCommandLine(ICT_Reset, "", out string message0, 1000);
            Thread.Sleep(1000);

            bResult = COM1.RunCommandLine(ICT_Start, "END;", out string message1, 10000, 2);

            if (bResult)
            {
                bResult = CheckICTResult(message1);
            }
            else
            {
                str_ShowDataGridViewInfo = "ICT测试返回的数据不完整";
                UIHandleHelper.ShowRunLog(str_ShowDataGridViewInfo, true);
            }

            UIHandleHelper.DataGridViewShow("ICT", str_ShowDataGridViewInfo, bResult);

            return bResult;
        }

        public bool CheckICTResult(string str_Result)
        {

            if (str_Result.Contains( "," +str_ICTFlowMark + "#"))
            {
                UIHandleHelper.ShowRunLog($"ICT test flow check pass: {str_ICTFlowMark}.");
            }
            else
            {
                UIHandleHelper.ShowRunLog("ICT test flow check fail.",
                                                           FailColor: true,
                                                           ShowGridView: true,
                                                           TestItemName: "ICT test",
                                                           TestItemContent: "ICT flow check fail.",
                                                           TestItemResult: false,
                                                           ErrorCode: TestErrorCode.ErrorCode.Err000.ToString());

                return false;
            }


            if (str_Result.Contains("PASS"))
            {
                UIHandleHelper.ShowRunLog("ICT Test Pass.");
                str_ShowDataGridViewInfo = "ICT Test Pass.";
                return true;
            }
            else if (str_Result.Contains("FAIL")) 
            {
                str_ShowDataGridViewInfo = "ICT fail: ";
                UIHandleHelper.ShowRunLog("ICT Test Fail", true);

                string[] strLine = str_Result.Split('\n');
                foreach (string str in strLine)
                {
                    if (str.Contains("NG"))
                    {
                        string[] sTump = str.Split(',');

                        if (str.Contains("OPEN#"))
                        {
                            string errPin = sTump[0].Substring(sTump[0].IndexOf("OPEN#") + 5).Replace(" ", "");
                            str_ErrorCode = string.Format("ICTErr_{0}", errPin);
                        }

                        if (str.Contains("SHOR#"))
                        {
                            string errPin = sTump[0].Substring(sTump[0].IndexOf("SHOR#") + 5).Replace(" ", "");
                            str_ErrorCode = string.Format("ICTErr_{0}", errPin);
                        }

                        if (str.Contains("v-"))
                        {
                            string errPin = sTump[0].Substring(sTump[0].IndexOf(": v") + 3).Replace('-', ' ').Replace(" ", "");
                            str_ErrorCode = string.Format("ICTErr_{0}", errPin);
                        }

                        if (str.Contains("R-"))
                        {
                            string errPin = sTump[0].Substring(sTump[0].IndexOf(": R") + 3).Replace('-', ' ').Replace(" ", "");
                            str_ErrorCode = string.Format("ICTErr_{0}", errPin);
                        }

                        struct_TestVariable.str_ErrorCode = str_ErrorCode;
                        str_ShowDataGridViewInfo += $"{str_ErrorCode}  ";
                    }
                }
                return false;
            }

            UIHandleHelper.ShowRunLog("ICT Test Error", true);
            str_ShowDataGridViewInfo = "ICT Test Error";
            return false;
        }


        public bool Check_KBC_Ver()
        {
            bool bRetVal = false;

            CustomProcess customProcess = new($"{str_ReadKBCVerExeFolderPath}\\{str_ReadKBCVerExeName}");
            bRetVal = customProcess.RunCommandLine("", ";", out string msg, 8000, 3);

            if (bRetVal)
            {
                if (msg.Contains(";PASS"))
                {
                    UIHandleHelper.ShowRunLog($"KBC ver check pass: {msg}",
                                                             FailColor: false,
                                                             ShowGridView: true,
                                                             TestItemName: "Check Ver",
                                                             TestItemContent: $"KBC ver check pass: {msg}",
                                                             TestItemResult: true);
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"KBC ver check fail: {msg}", 
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Check Ver",
                                                               TestItemContent: $"KBC ver check fail: {msg}",
                                                               TestItemResult: false,
                                                               ErrorCode:TestErrorCode.ErrorCode.Err030.ToString());

                    return false;
                }
            }
            else
            {
                UIHandleHelper.ShowRunLog($"KBC ver check error: {msg}",
                                                              FailColor: true,
                                                              ShowGridView: true,
                                                              TestItemName: "Check Ver",
                                                              TestItemContent: $"KBC ver check error: {msg}",
                                                              TestItemResult: false,
                                                              ErrorCode: TestErrorCode.ErrorCode.Err030.ToString());

                return false;
            }
            
            return bRetVal;
        }


    }
}
