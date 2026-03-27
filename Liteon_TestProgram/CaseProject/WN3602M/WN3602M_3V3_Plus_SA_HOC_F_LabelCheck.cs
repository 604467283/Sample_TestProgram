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
using Liteon_TestProgram.Utilities.PEM;


namespace Liteon_TestProgram.CaseProject
{
    internal class WN3602M_3V3_Plus_SA_HOC_F_LabelCheck : CaseCodeBase
    {

        /// <summary>
        /// 用于添加MAC的前缀，比如“23S”，默认是空
        /// </summary>
        protected virtual string str_MacPrefix { get; set; } = "";


        /// <summary>
        /// 用于添加MAC的后缀，比如“A”，默认是空
        /// </summary>
        protected virtual string str_MacSuffix { get; set; } = "A";



        public string str_WifiPartName = null;
        public string str_Fixed_Code_Content = null; 
        public int iLabel_Date_Code_Length = 0; 
        public int iAP_WaitBootTimeSecond = 0;
        public double fPowerSupply_Voltage = 0;
        public double fPowerSupply_Current = 0;


        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; set; }
        public TestForm _testForm;
        string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public WN3602M_3V3_Plus_SA_HOC_F_LabelCheck(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.修改电源供电方式为直流稳压电源，并程控.");

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
            list_str.Add("1.此程式用于检查带线组壳的SA label check\r\n");
            list_str.Add("2.测试配置页中VISA名称1中选GWINSTEK_PSS2303\r\n");
            list_str.Add("3.测试配置页中VISA地址1中填写对应的VISA地址资源名称\r\n");
            list_str.Add("4.用到无线网卡名需要填到测试配置设定中的设备名称1中\r\n");
            list_str.Add("5.先插线，然后依次扫码, 点击测试\r\n");
            return list_str;
        }


        public override void Func_TestMac()
        {

            macBD_Relation = MacBD_Relation.Separate;
            sn_Relation = SN_Relation.SN;

            if (struct_NormalINI.stru_b_DebugMode)
            {
                struct_TestVariable.iMAC_OriginalLength = 13;
                struct_TestVariable.iMAC_ExtractStartPosition = 0;
                struct_TestVariable.iMACLength = 12;

                struct_TestVariable.iBD_OriginalLength = 13;
                struct_TestVariable.iBD_ExtractStartPosition = 1;
                struct_TestVariable.iBDLength = 12;

                struct_TestVariable.iSN_OriginalLength = 27;
                struct_TestVariable.iSN_ExtractStartPosition = 0;
                struct_TestVariable.iSNLength = 26;

            }
            else
            {
                struct_TestVariable.iMACLength = 12;
                struct_TestVariable.iBDLength = 12;
                struct_TestVariable.iSNLength = 26;
            }
        }


        public override bool Func_TestPre()
        {
            bool bResult = false;

            try
            {
                bResult = ReadCustomerEncryptIni();
                DevconHelper.DisableByName(struct_NormalINI.stru_str_DeviceName1);
                Thread.Sleep(100);
              

            }
            catch (Exception ex)
            {
                bResult = false;
                UIHandleHelper.ShowRunLog($"TestPre fail: {ex}");
            }

            return bResult;
        }

        public override bool Func_TestInit()
        {
            bool bInitResult = false;

            #region testinit的额外补充

            try
            {

                PSU1.SetVoltage(fPowerSupply_Voltage);
                Thread.Sleep(100);
                PSU1.SetCurrent(fPowerSupply_Current);
                Thread.Sleep(500);
                PSU1.PowerOff();
                Thread.Sleep(1000);

                if (struct_NormalINI.stru_i_TestTimes_Fail + struct_NormalINI.stru_i_TestTimes_Pass >= 100000)
                {
                    UIHandleHelper.ShowRunLog("测试次数已经100000次，请更换测试耗材；",
                                                              FailColor: true,
                                                              ShowGridView: true,
                                                              TestItemName: "Init",
                                                              TestItemContent: "测试次数已经100000次，请更换测试耗材；",
                                                              TestItemResult: false,
                                                              ErrorCode: TestErrorCode.ErrorCode.Err000.ToString());
                    bInitResult = false;
                }
                else
                {
                    bInitResult = true;
                }


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

            bTestResult = CheckLabel();

            if (bTestResult)
            {
                bTestResult = WifiConnectTest();
            }

            return bTestResult;
        }

        public override bool Func_TestEnd(bool bTestResult)
        {
            bool bEndResult = true;

            try
            {

                sfcFile.CreateSfcFile(struct_TestVariable.bNeedCreateSFCFile, bTestResult, null,
                                                   str_MacPrefix + struct_Barcode.stru_str_sRevDUTMac + str_MacSuffix, "",
                                                   struct_EncryptINI.stru_str_ProjectName,
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
            PSU1.PowerOff();
            Thread.Sleep(500);
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




                #region Label_Config

                IniHelper.GetIniStr("Label_Config", "Fixed_Code_Content", "null", ValTemp, 50, str_IniPath);
                str_Fixed_Code_Content = ValTemp.ToString();


                IniHelper.GetIniStr("Label_Config", "Date_Code_Length", "0", ValTemp, 10, str_IniPath);
                iLabel_Date_Code_Length = int.Parse(ValTemp.ToString());

                #endregion

                #region AP_Config

                IniHelper.GetIniStr("AP_Config", "WifiPartName", "null", ValTemp, 100, str_IniPath);
                str_WifiPartName = ValTemp.ToString();

                IniHelper.GetIniStr("AP_Config", "WaitBootTimeSecond", "0", ValTemp, 10, str_IniPath);
                iAP_WaitBootTimeSecond = int.Parse(ValTemp.ToString())*1000;

                #endregion

                #region PowerSupply

                IniHelper.GetIniStr("PowerSupply", "Voltage", "0", ValTemp, 10, str_IniPath);
                fPowerSupply_Voltage = double.Parse(ValTemp.ToString());

                IniHelper.GetIniStr("PowerSupply", "Current", "0", ValTemp, 10, str_IniPath);
                fPowerSupply_Current = double.Parse(ValTemp.ToString());

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

        public bool CheckLabel()
        {
            bool bResult = false;

            if (struct_Barcode.stru_str_sRevDUTMac == struct_Barcode.stru_str_sRevDUTBD)
            {
                UIHandleHelper.ShowRunLog("Check label1 and label2 pass",
                                                              FailColor: false,
                                                              ShowGridView: true,
                                                              TestItemName: "Label1 Label2 Check",
                                                              TestItemContent: $"{struct_Barcode.stru_str_sRevDUTMac} - {struct_Barcode.stru_str_sRevDUTBD}",
                                                              TestItemResult: true);
                bResult = true;
            }
            else
            {
                UIHandleHelper.ShowRunLog("Check label1 and label2 fail",
                                                               FailColor: true,
                                                               ShowGridView: true,
                                                               TestItemName: "Label1 Label2 Check",
                                                               TestItemContent: $"{struct_Barcode.stru_str_sRevDUTMac} - {struct_Barcode.stru_str_sRevDUTBD}",
                                                               TestItemResult: false,
                                                               ErrorCode: TestErrorCode.ErrorCode.Err019.ToString());
                bResult = false;
            }


            if (bResult)
            {
                if (struct_Barcode.stru_str_sRevDUTSN.StartsWith(str_Fixed_Code_Content))
                {

                    //ANC
                    UIHandleHelper.ShowRunLog($"Check label3 ANC pass, {str_Fixed_Code_Content}",
                                                              FailColor: false,
                                                              ShowGridView: true,
                                                              TestItemName: "ANC Check",
                                                              TestItemContent: $"{struct_Barcode.stru_str_sRevDUTSN.Substring(0, str_Fixed_Code_Content.Length)}",
                                                              TestItemResult: true);


                    //Mac
                    string str_Temp_MAC = struct_Barcode.stru_str_sRevDUTSN.Substring(str_Fixed_Code_Content.Length + iLabel_Date_Code_Length);

                    if (struct_Barcode.stru_str_sRevDUTMac == str_Temp_MAC)
                    {
                        UIHandleHelper.ShowRunLog("Check label3 Mac pass",
                                                                      FailColor: false,
                                                                      ShowGridView: true,
                                                                      TestItemName: "Label3 Mac Check",
                                                                      TestItemContent: $"{str_Temp_MAC}",
                                                                      TestItemResult: true);
                        bResult = true;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog("Check label3 Mac fail",
                                                                       FailColor: true,
                                                                       ShowGridView: true,
                                                                       TestItemName: "Label3 Mac Check",
                                                                       TestItemContent: $"{str_Temp_MAC}, Target Mac {struct_Barcode.stru_str_sRevDUTMac}",
                                                                       TestItemResult: false,
                                                                       ErrorCode: TestErrorCode.ErrorCode.Err019.ToString());
                        bResult = false;
                    }
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Check label3 ANC fail, {struct_Barcode.stru_str_sRevDUTSN.Substring(0, str_Fixed_Code_Content.Length)}, Target ANC {str_Fixed_Code_Content}",
                                                              FailColor: true,
                                                              ShowGridView: true,
                                                              TestItemName: "ANC Check",
                                                              TestItemContent: $"{struct_Barcode.stru_str_sRevDUTSN.Substring(0, str_Fixed_Code_Content.Length)}",
                                                              TestItemResult: false,
                                                              ErrorCode: TestErrorCode.ErrorCode.Err021.ToString());
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


        public bool WifiConnectTest()
        {
            bool bResult = false;
            string str_Mac = InsertEveryTwoCharacters(struct_Barcode.stru_str_sRevDUTMac).ToLower();

            WifiHelper wifiHelper = new WifiHelper();
            PSU1.PowerOn();

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    DevconHelper.DisableByName(struct_NormalINI.stru_str_DeviceName1);
                    Thread.Sleep(10);
                    DevconHelper.EnableByName(struct_NormalINI.stru_str_DeviceName1);

                    DevconHelper.Rescan();

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
                    continue;
                }

                Thread.Sleep(iAP_WaitBootTimeSecond);
                var Result = wifiHelper.ConnectWifi(str_WifiPartName + str_Mac, "");
                bResult = Result.Item1;

                if (bResult)
                {
                    UIHandleHelper.ShowRunLog("Wifi connect test pass", false,
                                                                   ShowGridView: true,
                                                                   TestItemName: "Wifi Connect",
                                                                   TestItemContent: $"{str_WifiPartName + str_Mac} Connected",
                                                                   TestItemResult: true);
                    bResult = true;
                    break;
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Wifi connect test fail", true,
                                                                   ShowGridView: true,
                                                                   TestItemName: "Wifi Connect",
                                                                   TestItemContent: $"{str_WifiPartName + str_Mac} Connect fail",
                                                                   TestItemResult: false);

                    str_ErrorCode = TestErrorCode.ErrorCode.Err001.ToString();
                    bResult = false;
                }


            }


            return bResult;
        }




    }
}
