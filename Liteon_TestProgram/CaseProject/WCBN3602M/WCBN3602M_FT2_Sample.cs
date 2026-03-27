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
using NAudio.SoundFont;


namespace Liteon_TestProgram.CaseProject
{
    internal class WCBN3602M_FT2_Sample : CaseCodeBase
    {
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
        public string str_CustomerFWBurnBatName = "prog_flash_RW610_Customer.bat";
        public string str_CustomerFWFolderPath = $"{PathHelper.GetParentDirectoryPath(PathHelper.GetCurrentExeDirPath())}\\{str_CustomerFWFolderName}";

        public string str_ReadMacBatName = "ReadMac.bat";

        public string str_WorkerID = "";
        public string str_WorkingDirectory = "";



        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; private set; }
        TestForm _testForm;
        string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public WCBN3602M_FT2_Sample(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.初版Sample程式");
            list_str.Add("2.可以在mac_Sample.ini中的Real_Power下自己新增Item，比如2G_High_CH1和2G_High_CH2,实现多个高CH点检");

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
            list_str.Add("1.COMA请填测试TB008小板的串口\r\n");
            list_str.Add("2.COMB请填测试产品的串口");
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
            bool bResult = true;

            return bResult;
        }

        public override bool Func_TestInit()
        {
            bool bInitResult = false;

            #region testinit的额外补充

            try
            {
                FileProcessHelper.DeleteFile(@$"{str_IQTestProgramFolderPath}\Bin\Log\", "*.txt");
                FileProcessHelper.DeleteFile(@$"{str_IQTestProgramFolderPath}\Bin\Log\", "*.iqvsa");


                COM1.RunCommandLine("FF", "");
                Thread.Sleep(1000);
                COM1.RunCommandLine("1O", "");

                bInitResult = ReadSampleIni(struct_Barcode.stru_str_sRevDUTMac);

                WriteSetUpINI(2);

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

            bool bTestResult = false;



            string str_FlowBatName = "";
            bTestResult = litepointHelper.ChangeSerialMAC(str_IQTestProgramFolderPath,
                                                                                    struct_Barcode.stru_str_sRevDUTMac,
                                                                                    struct_Barcode.stru_str_sRevDUTBD);

            if (bTestResult)
            {
                var bBatResult = litepointHelper.CreateLitePointBat(false,
                                                                                            str_IQTestProgramFolderPath, "IQfactRun_Console.exe",
                                                                                            struct_NormalINI.stru_str_TesterPort, true, bSampleMode: true);

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
                COM1.RunCommandLine("FF", "");
                Thread.Sleep(1000);
                COM1.RunCommandLine("1O", "");
                Thread.Sleep(5000);

                _testForm.Multi_lockStatus();
                bTestResult = RFTest(str_FlowBatName);
                _testForm.Multi_releaseStatus();

          
            }

            if(bTestResult)
            {
                bool bSampleResult = false;
                var RfResult = sfcFile.GetRFDataFromLog($"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_RF.txt");
                List<RFTestResult> RfTestResult = RfResult.Item1;

                if (RfResult.Item2)
                {
                    UIHandleHelper.DataGridViewClear();
                    UIHandleHelper.DataGridViewShow_SampleSetHeader();
                    bSampleResult = UIHandleHelper.DataGridViewShow_SampleSetTestData(RfTestResult, 
                                                                                                                          struct_TestVariable.floatSampleIniPowerList.ToArray(), 
                                                                                                                          struct_TestVariable.floatSamplePowerRange);
                }

              
                 bTestResult = RfResult.Item2 && bSampleResult;


            }

            if (bTestResult)
            {
                UIHandleHelper.DataGridViewShow("RF Test", "Sample RF check test pass", true);
            }
            else
            {
                UIHandleHelper.DataGridViewShow("RF Test", "Sample RF check test fail", false);
            }


            #region MES上传
           
            #endregion



            return bTestResult;
        }

        public override bool Func_TestEnd(bool bTestResult)
        {
            bool bEndResult = false;

            try
            {
                if (File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_RF.txt"))
                {
                    SaveLogProcess.SaveLog_Local(bTestResult, struct_NormalINI.stru_str_LogFilePath,
                                                                    struct_EncryptINI.stru_str_ProjectName,
                                                                    struct_Barcode.stru_str_sRevDUTMac,
                                                                    $"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_RF.txt", "_IQ.txt");
                }



                //sfcFile.CreateSfcFile(struct_TestVariable.bNeedCreateSFCFile, bTestResult, $"{str_IQTestProgramFolderPath}\\Bin\\Log\\logOutput_RF.txt",
                //                            struct_Barcode.stru_str_sRevDUTMac, struct_Barcode.stru_str_sRevDUTBD, struct_EncryptINI.stru_str_ProjectName,
                //                            struct_EncryptINI.stru_str_CaseVersion, struct_NormalINI.stru_str_SFCFilePath, str_ErrorCode);

                bEndResult = ResultUploadMes(struct_Barcode.stru_str_sRevDUTMac, struct_EncryptINI.stru_str_ProjectName, struct_EncryptINI.stru_str_CaseVersion, str_ErrorCode, bTestResult);


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
            COM1.RunCommandLine("FF", "");
            COM1.Close(true);
            COM2.Close(true);
            Thread.Sleep(1000);
        }




        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================


        public bool RFTest(string str_FlowBatName)
        {
            bool bRet = false;

            bRet=  litepointHelper.CallFlowTest($"{FileProcessHelper.GetCurrentExeDirectory()}\\{str_FlowBatName}", 
                                                              $"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt" );

            if (File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt") )
            {
                File.Copy($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt", $"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput_RF.txt", true);
            }

            if (bRet == false)
            {
                str_ErrorCode = ParseErrorCode.ParseFileForErrorCodes($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput_RF.txt").ToString();
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



        public bool ReadCustomerTestIni()
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

                #region [Mes_Config]

                IniHelper.GetIniStr("Mes_Config", "WorkerID", "", ValTemp, 200, str_IniPath);
                str_WorkerID = ValTemp.ToString();

                IniHelper.GetIniStr("Mes_Config", "WorkingDirectory", "", ValTemp, 200, str_IniPath);
                str_WorkingDirectory = ValTemp.ToString();

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


        public bool ResultUploadMes(string str_Mac, string str_ProjectName, string str_ProjectVer, string str_ErrorCode, bool bTestResult)
        {
            bool bRet = false;

            // //-open -cmd 5 -upload  41003391;700894FF86AB;WCBN811L-L6_Test;V0.0.0.1;WFCV001;NG;SAMPLETest; -close

            string strResult = bTestResult ? "OK" : "NG";
            string str_CMD = $"-open -cmd 51 -upload {str_WorkerID};{str_Mac};{str_ProjectName};{str_ProjectVer};{str_ErrorCode};{strResult};SAMPLETest; -close";
            // 确保工作目录指向 SFCHelpers.exe 所在目录
            CustomProcess customProcess = new(
                $"{PathHelper.GetCurrentExeDirPath()}\\SFCHelpers.exe",
                PathHelper.GetCurrentExeDirPath()
            );
            bool bResult = customProcess.RunCommandLine(str_CMD, "MES Upload OK", out string msg, 15000, 2);

            if (bResult)
            {
                UIHandleHelper.ShowRunLog("Upload Test Result Pass",
                                                             FailColor: false,
                                                             ShowGridView: true,
                                                             TestItemName: "Upload Mes",
                                                             TestItemContent: $"MES:{msg}",
                                                             TestItemResult: true);
            }
            else
            {
                UIHandleHelper.ShowRunLog("Upload Test Result fail",
                                                            FailColor: true,
                                                            ShowGridView: true,
                                                            TestItemName: "Upload Mes",
                                                            TestItemContent: $"MES:{msg}",
                                                            TestItemResult: false,
                                                            ErrorCode: TestErrorCode.ErrorCode.Err059.ToString());
            }

            return bResult;



            return bRet;
        }


    }
}
