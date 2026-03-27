using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Utilities;
using Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V2;
using static Liteon_TestProgram.Base.Class_Variable;
using System.Diagnostics;
using System.Text;
using Liteon_TestProgram.Utilities.HttpClient;



namespace Liteon_TestProgram.CaseProject
{
    internal class WCBN3602M_FT3 : CaseCodeBase
    {

        public int iCommand_Send_Interval_Second = 0;
        public string str_UpLoad_ITDB_IP = null;
        public string str_WifiPartName = null;
        public string str_Serialization_Command = null;
        public string str_Serialization_Post_Data = null;
        public string str_Serialization_Return_Data = null;
        public string str_Serialization_PCs = null;
        public string str_Serialization_CurrentPC = null;



        public HttpWithBodyHelper httpClientHelper = new();
        public HttpGetHelper getter = new HttpGetHelper();


        
      
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; set; }
        public TestForm _testForm;
        string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public WCBN3602M_FT3(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("(20260304) 1.序列化测试初始版本，去除从数据库查询是否序列化");

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
            list_str.Add("2.测试TB小板: LO LF控制给板子上下电\r\n");
            list_str.Add("3.测试副机电脑需要使用到无线网卡适配器.\r\n");
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

            if (bResult) 
            {
                bResult = ReadAddTestIni();
            }

            if (bResult)
            {
                using (SelectionWindow selectionForm = new SelectionWindow("NOAH PC Select",
                                                                            str_Serialization_PCs, str_Serialization_CurrentPC))
                {
                    if (selectionForm.ShowDialog(testForm) == DialogResult.OK)
                    {
                        string selectedValue = selectionForm.SelectedValue;
                        UIHandleHelper.ShowRunLog($"Serialization PC -> {selectedValue}");
                        str_Serialization_CurrentPC = selectedValue;
                        str_Serialization_Command = str_Serialization_Command.Replace("PCCode", selectedValue);
                        bResult = WriteAddNormalIni();
                    }
                }
            }

            return bResult;
        }

        public override bool Func_TestInit()
        {
            bool bInitResult = false;

            #region testinit的额外补充

            try
            {

                bInitResult = COM1.RunCommandLine("LF", "");

                if (bInitResult)
                {
                    Thread.Sleep(1000);
                    bInitResult = COM1.RunCommandLine("LO", "");
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


            #region ITDB检查是否序列化过

            int iResult = CheckSerializedResult();
            if (iResult == 0 || iResult == 1)
            {
                bTestResult = true;
            }

            #endregion


            #region 序列化

            if (bTestResult && iResult == 0)
            {
                Thread.Sleep(iCommand_Send_Interval_Second);
                bTestResult = SerializationTest();
            }

            #endregion


            #region 上传序列化结果到ITDB

            iResult = UpdateSerializedResult(bTestResult);

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
            COM1.RunCommandLine("LF", "");
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


                #region 指令发送间隔时间

                IniHelper.GetIniStr("Config", "Command_Send_Interval_Second", "null", ValTemp, 50, str_IniPath);
                iCommand_Send_Interval_Second = int.Parse(ValTemp.ToString()) * 1000;

                #endregion

                #region WifiPartName

                IniHelper.GetIniStr("Config", "WifiPartName", "null", ValTemp, 100, str_IniPath);
                str_WifiPartName = ValTemp.ToString();

                #endregion

                #region UpLoad_ITDB_IP

                IniHelper.GetIniStr("Config", "UpLoad_ITDB_IP", "null", ValTemp, 100, str_IniPath);
                str_UpLoad_ITDB_IP = ValTemp.ToString();

                #endregion

                #region SerializationConfig

                IniHelper.GetIniStr("SerializationConfig", "Serialization_CurrentPC", "null", ValTemp, 300, str_IniPath);
                str_Serialization_CurrentPC = ValTemp.ToString();

                IniHelper.GetIniStr("SerializationConfig", "Serialization_PCs", "null", ValTemp, 300, str_IniPath);
                str_Serialization_PCs = ValTemp.ToString();

                IniHelper.GetIniStr("SerializationConfig", "Serialization_Command", "null", ValTemp, 300, str_IniPath);
                str_Serialization_Command = ValTemp.ToString();

                IniHelper.GetIniStr("SerializationConfig", "Serialization_Post_Data", "null", ValTemp, 300, str_IniPath);
                str_Serialization_Post_Data = ValTemp.ToString();

                IniHelper.GetIniStr("SerializationConfig", "Serialization_Return_Data", "null", ValTemp, 300, str_IniPath);
                str_Serialization_Return_Data = ValTemp.ToString();

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


        public bool ReadAddTestIni()
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

                #region [SerializationConfig]

                IniHelper.GetIniStr("SerializationConfig", "Serialization_CurrentPC", "null", ValTemp, 100, str_IniPath);
                str_Serialization_CurrentPC = ValTemp.ToString();

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


        public bool WriteAddNormalIni()
        {
            string str_CaseFolder = new string(str_CaseProjectName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{str_CaseProjectName}\\{str_CaseProjectName}_Normal.ini";

            try
            {

                if (File.Exists(str_IniPath) == false)
                {
                    throw new ArgumentException($"{str_CaseProjectName}_Normal.ini文件不存在。");
                }

                #region [SerializationConfig]

                IniHelper.WriteIniStr("SerializationConfig", "Serialization_CurrentPC", str_Serialization_CurrentPC, str_IniPath);

                #endregion

            


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"写入{str_CaseProjectName} ini failed:{ex.StackTrace}");
                UIHandleHelper.ShowRunLog($"写入{str_CaseProjectName} ini failed:{ex.StackTrace}", true);
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
                    UIHandleHelper.ShowRunLog($"{str_Titel} test pass: {str_Expected}");
                                                            
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"{str_Titel} test fail", true);
                }


            }
            catch (HttpRequestException e)
            {
                UIHandleHelper.ShowRunLog($"{str_Titel} test error, Get request error: {e.Message}", true);
            }

            return false;
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

        int CheckFromITDB(string str_Cmd)
        {
            int iResult = -1;
            try
            {
                Task<bool> httpTask_FWVer = Http_Get_Async("Check Serialized", str_Cmd, "PASS");
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
                iResult =  - 1;
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

        /// <summary>
        /// 1: 序列化过   0:没有序列化过   -1:出错
        /// </summary>
        /// <returns></returns>
        public int CheckSerializedResult()
        {
            int iResult = -1;

            string str_Comm_ITDB_Command = Generate_ITDB_Comm_CMD(str_UpLoad_ITDB_IP, "query",
                                                                    struct_Barcode.stru_str_sRevDUTMac, "Serialized");

            if (string.IsNullOrEmpty(str_Comm_ITDB_Command))
            {
                UIHandleHelper.ShowRunLog($"Check Serialized: Generate IT DB CMD Fail",
                                                             FailColor: true,
                                                             ShowGridView: true,
                                                             TestItemName: "Check Serialized",
                                                             TestItemContent: "Generate IT DB CMD Fail",
                                                             TestItemResult: false,
                                                             ErrorCode: TestErrorCode.ErrorCode.Err058.ToString());
           
                return -1;
            }

            iResult = CheckFromITDB(str_Comm_ITDB_Command);
            if (iResult == -1)
            {
                if (CheckFromITDB(str_Comm_ITDB_Command) == 1)
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
                UIHandleHelper.ShowRunLog("This product has been serialized",
                                                                FailColor: false,
                                                                ShowGridView: true,
                                                                TestItemName: "Check Serialized",
                                                                TestItemContent: "This product has been serialized",
                                                                TestItemResult: true);
            }
            else
            {
                UIHandleHelper.ShowRunLog("This product has not been serialized",
                                                            FailColor: false,
                                                            ShowGridView: true,
                                                            TestItemName: "Check Serialized",
                                                            TestItemContent: "This product has not been serialized",
                                                            TestItemResult: true
                                                            );
            }



            return iResult;
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


        public bool SerializationTest()
        {
            bool bResult = false;

            string str_Body = str_Serialization_Post_Data.Replace("macaddress", struct_Barcode.stru_str_sRevDUTMac);

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    Thread.Sleep(1000);
                    Task<bool> httpTask_Iperf = Http_Post_Async("Serialization Test", str_Serialization_Command, str_Body, str_Serialization_Return_Data);
                    httpTask_Iperf.Wait();
                    bResult = httpTask_Iperf.Result;
                }
                catch (Exception)
                {
                    bResult = false;
                }

                if (bResult)
                {
                    break;
                }

            }

            return bResult;
        }


    }
}
