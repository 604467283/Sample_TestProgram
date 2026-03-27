using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Utilities;
using Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V2;
using static Liteon_TestProgram.Base.Class_Variable;
using OfficeOpenXml;



namespace Liteon_TestProgram.CaseProject
{
    internal class WCBN3602M_FT4 : CaseCodeBase
    {





        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; set; }
        public TestForm _testForm;
        string str_ErrorCode = "Err000";
        SfcFile sfcFile = new();


        public WCBN3602M_FT4(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.序列化Execel检查程式");

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
            list_str.Add("1.此程式用于检查序列化Excel\r\n");
            list_str.Add("2.请随意输入12位Mac信息，点击开始测试，然后按提示选择excel\r\n");
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
            bool bInitResult = true;

            return bInitResult;
        }

        public override bool Func_TestFlow()
        {

            bool bTestResult = false;

            bTestResult = DealwithExcel();

            return bTestResult;
        }

        public override bool Func_TestEnd(bool bTestResult)
        {
            bool bEndResult = true;

            return bEndResult;
        }


        public override void Func_ResourceRelease()
        {
            
        }




        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================


        public async Task<string> SelectExcelAsync()
        {
            string filePath = "";
            await Task.Run(() =>
            {
                var form = Application.OpenForms["TestForm"];
                // 使用 Invoke 将代码切换到 UI 线程
                form.Invoke((MethodInvoker)delegate
                {
                    // 创建 OpenFileDialog 实例
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    // 设置文件过滤器，只显示 Excel 文件
                    openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

                    // 设置初始目录（可选）
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    // 显示对话框并检查用户是否选择了文件
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 获取用户选择的文件路径
                        filePath = openFileDialog.FileName;
                    }
                });
            });

            return filePath;
        }


        public bool DealwithExcel()
        {
            string strExcelPath = string.Empty;

            #region 选择Excel

            try
            {
                Task<string> task_SelectFile = SelectExcelAsync();
                task_SelectFile.Wait();
                strExcelPath = task_SelectFile.Result;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Select Excel Async Err: {ex.Message}",
                                                          FailColor: true,
                                                          ShowGridView: true,
                                                          TestItemName: "Select Excel",
                                                          TestItemContent: $"Err: {ex.Message}",
                                                          TestItemResult: false,
                                                          ErrorCode: TestErrorCode.ErrorCode.Err061.ToString()
                                                          );

                return false;
            }

            if (strExcelPath != string.Empty)
            {

                UIHandleHelper.ShowRunLog($"Select Excel: {strExcelPath}",
                                                              FailColor: false,
                                                              ShowGridView: true,
                                                              TestItemName: "Select Excel",
                                                              TestItemContent: $"{strExcelPath}",
                                                              TestItemResult: true);
            }
            else
            {
                UIHandleHelper.ShowRunLog($"Select Excel Path is Null",
                                                          FailColor: true,
                                                          ShowGridView: true,
                                                          TestItemName: "Select Excel",
                                                          TestItemContent: "Path is Null",
                                                          TestItemResult: false,
                                                          ErrorCode: TestErrorCode.ErrorCode.Err061.ToString()
                                                          );

                return false;
            }

            #endregion


            #region 处理Excel

            // 设置EPPlus的LicenseContext
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            bool bRecordFail = false;

            // 打开Excel文件
            using (var package = new ExcelPackage(new FileInfo(strExcelPath)))
            {
                // 获取第一个工作表
                var worksheet = package.Workbook.Worksheets[0];

                // 获取工作表的行数和列数
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

             
                // 遍历每一行
                for (int row = 2; row <= rowCount; row++)
                {
                    // 获取MAC地址和Check Serialization Upload的状态
                    string macAddress = worksheet.Cells[row, 4].Text;
                    string status = worksheet.Cells[row, 7].Text;

                    bool bCheckResult = false;
                    // 检查状态是否为"OK"
                    if (status != "OK")
                    {

                        UIHandleHelper.ShowRunLog($"{row - 1}->MAC地址 {macAddress} 的Check Serialization Upload状态不是OK，而是 {status}",
                                                          FailColor: true,
                                                          ShowGridView: true,
                                                          TestItemName: "Check Excel",
                                                          TestItemContent: $"{row - 1}->MAC {macAddress} Check Serialization 状态不是OK，而是 <{status}>",
                                                          TestItemResult: false,
                                                          ErrorCode: TestErrorCode.ErrorCode.Err061.ToString()
                                                          );
                        bCheckResult = false ;
                        bRecordFail = true ;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"{row - 1}->MAC地址 {macAddress} 的Check Serialization Upload状态是OK");
                        bCheckResult = true ;
                    }

                    try
                    {

                        sfcFile.CreateSfcFile(struct_TestVariable.bNeedCreateSFCFile, bCheckResult, null,
                                                       macAddress, null, struct_EncryptINI.stru_str_ProjectName,
                                                       struct_EncryptINI.stru_str_CaseVersion, struct_NormalINI.stru_str_SFCFilePath, struct_TestVariable.str_ErrorCode);

                    }
                    catch (Exception ex)
                    {
                        UIHandleHelper.ShowRunLog($"Create Sfc Filec Err: {ex.Message}",
                                                         FailColor: true,
                                                         ShowGridView: true,
                                                         TestItemName: "Create Sfc File",
                                                         TestItemContent: $"Err: {ex.Message}",
                                                         TestItemResult: false,
                                                         ErrorCode: TestErrorCode.ErrorCode.Err061.ToString()
                                                         );
                    }


                }
            }


            #endregion

            if (bRecordFail ==false)
            {
                UIHandleHelper.ShowRunLog($"Check Excel Pass",
                                                             FailColor: false,
                                                             ShowGridView: true,
                                                             TestItemName: "Check Excel",
                                                             TestItemContent: "All Pass",
                                                             TestItemResult: true);

                return true;
            }

            UIHandleHelper.ShowRunLog("Check Excel Fail", true);
            return false;
        }



    }
}
