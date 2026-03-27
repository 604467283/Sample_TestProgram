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
    internal class WCBN3602M_SA_MacCheck : CaseCodeBase
    {

        /// <summary>
        /// 用于添加MAC的前缀，比如“23S”，默认是空
        /// </summary>
        protected virtual string str_MacPrefix { get; set; } = "";


        /// <summary>
        /// 用于添加MAC的后缀，比如“A”，默认是空
        /// </summary>
        protected virtual string str_MacSuffix { get; set; } = "A";



        public string str_Fixed_Code_Content = null;
        public int iLabel_Date_Code_Length = 0;


        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; set; }
        public TestForm _testForm;
        string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public WCBN3602M_SA_MacCheck(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.比对两个Label和ANC.");

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
            list_str.Add("1.此程式用于检查无壳无线束 label check\r\n");
            list_str.Add("2.然后依次扫码, 点击测试\r\n");
            return list_str;
        }


        public override void Func_TestMac()
        {

            macBD_Relation = MacBD_Relation.OnlyMac;
            sn_Relation = SN_Relation.SN;

            if (struct_NormalINI.stru_b_DebugMode)
            {
                struct_TestVariable.iMAC_OriginalLength = 13;
                struct_TestVariable.iMAC_ExtractStartPosition = 0;
                struct_TestVariable.iMACLength = 12;

                struct_TestVariable.iSN_OriginalLength = 27;
                struct_TestVariable.iSN_ExtractStartPosition = 0;
                struct_TestVariable.iSNLength = 26;

            }
            else
            {
                struct_TestVariable.iMACLength = 12;
                struct_TestVariable.iSNLength = 26;
            }
        }


        public override bool Func_TestPre()
        {
            bool bResult = false;

            try
            {
                bResult = ReadCustomerEncryptIni();

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
            bool bInitResult = true;

           

            return bInitResult;
        }

        public override bool Func_TestFlow()
        {

            bool bTestResult = false;

            bTestResult = CheckLabel();

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




                #region Label_Config

                IniHelper.GetIniStr("Label_Config", "Fixed_Code_Content", "null", ValTemp, 50, str_IniPath);
                str_Fixed_Code_Content = ValTemp.ToString();


                IniHelper.GetIniStr("Label_Config", "Date_Code_Length", "0", ValTemp, 10, str_IniPath);
                iLabel_Date_Code_Length = int.Parse(ValTemp.ToString());

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


            if (struct_Barcode.stru_str_sRevDUTSN.StartsWith(str_Fixed_Code_Content))
            {

                //ANC
                UIHandleHelper.ShowRunLog($"Check label2 ANC pass, {str_Fixed_Code_Content}",
                                                          FailColor: false,
                                                          ShowGridView: true,
                                                          TestItemName: "ANC Check",
                                                          TestItemContent: $"{struct_Barcode.stru_str_sRevDUTSN.Substring(0, str_Fixed_Code_Content.Length)}",
                                                          TestItemResult: true);


                //Mac
                string str_Temp_MAC = struct_Barcode.stru_str_sRevDUTSN.Substring(str_Fixed_Code_Content.Length + iLabel_Date_Code_Length);

                if (struct_Barcode.stru_str_sRevDUTMac == str_Temp_MAC)
                {
                    UIHandleHelper.ShowRunLog($"Check label1 label2 Mac pass, {struct_Barcode.stru_str_sRevDUTMac} - {str_Temp_MAC}",
                                                                  FailColor: false,
                                                                  ShowGridView: true,
                                                                  TestItemName: "Label1 label2 Check",
                                                                  TestItemContent: $"{struct_Barcode.stru_str_sRevDUTMac} - {str_Temp_MAC}",
                                                                  TestItemResult: true);
                    bResult = true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Check label1 label2 Mac fail, {str_Temp_MAC}, Target Mac {struct_Barcode.stru_str_sRevDUTMac}",
                                                                   FailColor: true,
                                                                   ShowGridView: true,
                                                                   TestItemName: "Label2 Mac Check",
                                                                   TestItemContent: $"{str_Temp_MAC}, Target Mac {struct_Barcode.stru_str_sRevDUTMac}",
                                                                   TestItemResult: false,
                                                                   ErrorCode: TestErrorCode.ErrorCode.Err019.ToString());
                    bResult = false;
                }
            }
            else
            {
                UIHandleHelper.ShowRunLog($"Check label2 ANC fail, {struct_Barcode.stru_str_sRevDUTSN.Substring(0, str_Fixed_Code_Content.Length)}, Target ANC {str_Fixed_Code_Content}",
                                                          FailColor: true,
                                                          ShowGridView: true,
                                                          TestItemName: "ANC Check",
                                                          TestItemContent: $"{struct_Barcode.stru_str_sRevDUTSN.Substring(0, str_Fixed_Code_Content.Length)}, Target ANC {str_Fixed_Code_Content}",
                                                          TestItemResult: false,
                                                          ErrorCode: TestErrorCode.ErrorCode.Err021.ToString());
                bResult = false;
            }




            return bResult;
        }

       
   


    }
}
