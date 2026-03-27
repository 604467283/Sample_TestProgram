using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Utilities;
using Liteon_TestProgram.Utilities.IOHelpers.Extensions;
using static Liteon_TestProgram.Base.Class_Variable;
using Liteon_TestProgram.TestFunc.ICT.LogDataCollection_V1;
using OfficeOpenXml.Drawing.Slicer.Style;
using System.Text;



namespace Liteon_TestProgram.CaseProject
{
    internal class Base_ICT : CaseCodeBase
    {


        /// <summary>
        /// //用于判断ICT Flow是否正确    =>  <EVO   >  最多可以替换成6个字符，可以是日期250605
        /// </summary>
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

        /// <summary>
        /// 用于添加MAC的前缀，比如“23S”，默认是空
        /// </summary>
        protected virtual string str_MacPrefix { get; set; } = "";


        /// <summary>
        /// 用于添加MAC的后缀，比如“A”，默认是空
        /// </summary>
        protected virtual string str_MacSuffix { get; set; } = "";

        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; set; }
        public TestForm _testForm;
        string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public Base_ICT(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
          : base(mf, tf, pc, str_CaseProject)
        {
            struct_TestVariable.bNeedCreateSFCFile = true;
            _CaseProjectName = str_CaseProjectName = str_CaseProject == GetType().Name ? str_CaseProject : GetType().Name;
            _testForm = tf;

            Type sfcFileType = typeof(SfcFile); // 如果你已经有了SfcFile的实例，可以用instance.GetType()        
            struct_TestVariable.str_LogDataCollectionType = sfcFileType.FullName; // 使用反射获取类型的完整名称（包括命名空间）

            List<string> list_str = new List<string>();
            list_str = CreateTestStringList();
            string[] array_str = list_str.ToArray();
            ShowTipsMessage(str_CaseProjectName + "测试提要", array_str);
        }


        /// <summary>
        /// 测试须知填写处,会显示到测试UI上
        /// </summary>
        /// <returns></returns>
        public override List<string> CreateTestStringList()
        {
            List<string> list_str = new List<string>();
            list_str.Add("1.COMA请填ICT测试的串口, 波特率38400\r\n");
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

            return bInitResult;
        }

        public override bool Func_TestFlow()
        {
            bool bTestResult = false;


            bTestResult = ICTTest();

            if (bTestResult == false)
            {
                bTestResult = ICTTest();
            }


            return bTestResult;
        }

        public override bool Func_TestEnd(bool bTestResult)
        {
            bool bEndResult = true;

            try
            {
                if (struct_TestVariable.iBDLength ==0)
                {
                    sfcFile.CreateSfcFile(struct_TestVariable.bNeedCreateSFCFile, bTestResult, null,
                                              str_MacPrefix + struct_Barcode.stru_str_sRevDUTMac + str_MacSuffix, "", struct_EncryptINI.stru_str_ProjectName,
                                              struct_EncryptINI.stru_str_CaseVersion, struct_NormalINI.stru_str_SFCFilePath, struct_TestVariable.str_ErrorCode);
                }
                else
                {
                    sfcFile.CreateSfcFile(struct_TestVariable.bNeedCreateSFCFile, bTestResult, null,
                                             str_MacPrefix + struct_Barcode.stru_str_sRevDUTMac + str_MacSuffix, str_MacPrefix + struct_Barcode.stru_str_sRevDUTBD + str_MacSuffix, struct_EncryptINI.stru_str_ProjectName,
                                             struct_EncryptINI.stru_str_CaseVersion, struct_NormalINI.stru_str_SFCFilePath, struct_TestVariable.str_ErrorCode);
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

        }




        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================


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



        public bool ICTTest()
        {
            bool bResult = false;

            UIHandleHelper.ShowRunLog("=============Test ICT===============");

            byte[] ICT_Reset = new byte[] { 0x01, 0x05, 0x00, 0x11, 0xFF, 0x00, 0xDC, 0x3F };
            byte[] ICT_Start = new byte[] { 0x01, 0x05, 0x00, 0x10, 0xFF, 0x00, 0x8D, 0xFF };

            COM1.RunCommandLine(ICT_Reset, "", out string message0, 1000);
            Thread.Sleep(1000);

            bResult = COM1.RunCommandLine(ICT_Start, "END;", out string message1, iICTTestReadTimeout, 2);

            if (bResult)
            {
                var vResult = CheckICTResult(message1);
                bResult = vResult.Item1;
                if (!bResult)
                {
                    UIHandleHelper.ShowRunLog(vResult.Item2,
                                                         FailColor: true,
                                                         ShowGridView: true,
                                                         TestItemName: "ICT",
                                                         TestItemContent: vResult.Item2,
                                                         TestItemResult: false,
                                                         ErrorCode: vResult.Item3);
                }
                else
                {
                    UIHandleHelper.ShowRunLog(vResult.Item2,
                                                         FailColor: false,
                                                         ShowGridView: true,
                                                         TestItemName: "ICT",
                                                         TestItemContent: vResult.Item2,
                                                         TestItemResult: true);
                }
            }
            else
            {
                UIHandleHelper.ShowRunLog("ICT测试返回的数据不完整",
                                                          FailColor: true,
                                                          ShowGridView: true,
                                                          TestItemName: "ICT",
                                                          TestItemContent: "ICT测试返回的数据不完整",
                                                          TestItemResult: false,
                                                          ErrorCode: TestErrorCode.ErrorCode.Err000.ToString());

                return false;
            }

            return bResult;
        }

        public (bool, string, string) CheckICTResult(string str_Result)
        {
            string str_ShowDataGridViewInfo = "";

            //if (str_Result.Contains("," + str_ICTFlowMark + "#"))
            //{
            //    UIHandleHelper.ShowRunLog($"ICT test flow check pass: {str_ICTFlowMark}.");
            //}
            //else
            //{
            //    return (false, "ICT test flow check fail", TestErrorCode.ErrorCode.Err000.ToString());
            //}


            if (str_Result.Contains("PASS"))
            {
                return (true, "ICT Test Pass.", "");
            }
            else if (str_Result.Contains("FAIL"))
            {

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
                return (false, str_ShowDataGridViewInfo, struct_TestVariable.str_ErrorCode);
            }

            UIHandleHelper.ShowRunLog("ICT Test Error", true);
            str_ShowDataGridViewInfo = "ICT Test Error";
            return (false, str_ShowDataGridViewInfo, TestErrorCode.ErrorCode.Err000.ToString());
        }



    }
}
