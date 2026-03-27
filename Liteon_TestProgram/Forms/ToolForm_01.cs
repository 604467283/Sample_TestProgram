using Liteon_TestProgram.Base;
using Liteon_TestProgram.CaseProject;
using Liteon_TestProgram.TestFunc;
using Liteon_TestProgram.TestFunc.ICT.LogDataCollection_V1;
using Liteon_TestProgram.TestFunc.Litepoint;
using Liteon_TestProgram.Utilities;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Liteon_TestProgram.Base.Class_Variable;
using static Liteon_TestProgram.TestFunc.LogCollectItems;
using static System.Windows.Forms.LinkLabel;


namespace Liteon_TestProgram.Forms
{



    public partial class ToolForm_01 : Form
    {
        protected string _str_CaseName = "";
        string[] selectedFolders = Array.Empty<string>();
        Dictionary<string, List<TestFunc.ACE.RFTestResult>> dict_ACE;
        Dictionary<string, List<TestFunc.Litepoint.RFTestResult>> dict_Litepoint;
        Dictionary<string, List<TestFunc.iTest.RFTestResult>> dict_iTest;
        Dictionary<string, List<TestFunc.ICT.TestICTResult>> dict_ICT;


        private TestConfiguration LogCollectConfig = new TestConfiguration();


        public ToolForm_01(MainForm mainForm, string str_CaseName)
        {
            _str_CaseName = str_CaseName;
            InitializeComponent();
        }

        private void ToolForm_Load(object sender, EventArgs e)
        {
            UIHandleHelper.ParseLogBox = richTextBox_LogsData;
            UIHandleHelper.FlowLogBox = richTextBox_FlowsData;
            UIHandleHelper.formsPlot_RF = formsPlot_RFValues;


            double[] data = ScottPlot.Generate.Sin();
            formsPlot_RFValues.Plot.Title("RF Result Points Line");
            formsPlot_RFValues.Plot.Add.Signal(data);
            formsPlot_RFValues.Refresh();

            #region 加载ini名字

            string str_CaseFolder = new string(_str_CaseName.TakeWhile(c => c != '_').ToArray());
            string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{_str_CaseName}\\{_str_CaseName}_Encrypt.ini";
            label_EncryptIni.Text = $"{_str_CaseName}_Encrypt.ini";

            #endregion


        }

        private void textBox_Md5Key_TextChanged(object sender, EventArgs e)
        {
            if ("LITEON" + DateTime.Now.ToString("MMddHHmm") == textBox_Md5Key.Text)
            {
                btn_SetMd5.Enabled = true;
            }
        }

        private void btn_SetMd5_Click(object sender, EventArgs e)
        {
            try
            {
                MD5Helper mD5Helper = new MD5Helper();

                //生成的文件夹里的配置文件
                string str_CaseFolder = new string(_str_CaseName.TakeWhile(c => c != '_').ToArray());
                string str_IniPath = $"{FileProcessHelper.GetCurrentExeDirectory()}\\Model_Config\\{str_CaseFolder}\\{_str_CaseName}\\{_str_CaseName}_Encrypt.ini";
                string str_FileContent = mD5Helper.ReadFileToStringSkipLines(str_IniPath, "MD5_INFO");
                string str_NewMd5 = mD5Helper.ComputeMD5Hash(str_FileContent);
                IniHelper.WriteIniStr("MD5_INFO", "MD5_INFO", str_NewMd5, str_IniPath);

                //代码中的文件夹中的配置文件
                string str_IniPath_Code = $"{PathHelper.GetParentDirectoryPath(PathHelper.GetParentDirectoryPath(FileProcessHelper.GetCurrentExeDirectory()))}\\Model_Config\\{str_CaseFolder}\\{_str_CaseName}\\{_str_CaseName}_Encrypt.ini";

                if (File.Exists(str_IniPath_Code))
                {
                    string str_FileContent_Code = mD5Helper.ReadFileToStringSkipLines(str_IniPath_Code, "MD5_INFO");
                    string str_NewMd5_Code = mD5Helper.ComputeMD5Hash(str_FileContent_Code);
                    IniHelper.WriteIniStr("MD5_INFO", "MD5_INFO", str_NewMd5_Code, str_IniPath_Code);
                }


                btn_SetMd5.Text = "Set Md5 OK";

                // 获取当前应用程序的路径.退出重启本exe
                string applicationPath = Application.ExecutablePath;
                // 启动新的实例
                Process.Start(applicationPath);
                // 关闭当前实例
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBoxEX.Show($"Set Md5 Error: {ex}", true);
                MessageBox.Show($"Set Md5 Error: {ex}");
            }

        }

        private async void button_DealWithLogs_Click(object sender, EventArgs e)
        {

            if (button_DealWithLogs.Text == "...")
            {
                // 获取当前执行的exe文件所在的目录
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                string exeDir = Path.GetDirectoryName(exePath);

                //FolderBrowserDialog folderDlg = new FolderBrowserDialog();

                //// 设置对话框的标题
                //folderDlg.Description = "请选择需要处理的log文件夹";

                //// 设置默认打开的路径为exe文件所在的目录
                //folderDlg.SelectedPath = exeDir;

                //// 显示对话框，并判断是否点击了确定按钮
                //if (folderDlg.ShowDialog() == DialogResult.OK)
                //{
                //    // 获取用户选择的文件夹路径
                //    string folderPath = folderDlg.SelectedPath;
                //    textBox_TargetFolder.Text = folderPath;
                //    button_DealWithLogs.Text = "Start";
                //}

                VistaFolderBrowserDialog vistaFolderBrowserDialog = new();
                vistaFolderBrowserDialog.Multiselect = true;
                vistaFolderBrowserDialog.ShowNewFolderButton = false; // 可选：隐藏新建文件夹按钮

                var result = vistaFolderBrowserDialog.ShowDialog();
                if (result == true)
                {
                    // 获取用户选择的文件夹路径
                    selectedFolders = vistaFolderBrowserDialog.SelectedPaths;
                    textBox_TargetFolder.Text = string.Join(Environment.NewLine, selectedFolders);
                    button_DealWithLogs.Text = "Start";
                    UIHandleHelper.ShowCollectionLogsRFData("选择的文件夹如下: \r\n" + string.Join(Environment.NewLine, selectedFolders));
                }


            }
            else if (button_DealWithLogs.Text == "Start")
            {
                richTextBox_LogsData.Text = "";
                button_DealWithLogs.Enabled = false;
                comboBox_excludedFolders.Enabled = false;
                comboBox_FileMark.Enabled = false;

                //string str_Folder = textBox_TargetFolder.Text;

                string str_FileMark = comboBox_FileMark.Text; //SelectedText
                string str_ExcludedFolder = comboBox_excludedFolders.Text;

                // 定义需要跳过的特定文件夹名称
                HashSet<string> excludedFolders = new HashSet<string> { str_ExcludedFolder };

                await LogDataCollection(selectedFolders, str_FileMark, excludedFolders);


                button_DealWithLogs.Enabled = true;
                comboBox_excludedFolders.Enabled = true;
                comboBox_FileMark.Enabled = true;
                textBox_TargetFolder.Text = "";
                button_DealWithLogs.Text = "...";
            }

        }


        private async Task LogDataCollection(string[] str_Folder, string str_FileMark, HashSet<string> excludedFolders)
        {
            await Task.Run(() =>
            {
                UIHandleHelper.ControlHandle(comboBox_RFMode, () => { comboBox_RFMode.Items.Clear(); });
                UIHandleHelper.ControlHandle(formsPlot_RFValues, () => { formsPlot_RFValues.Plot.Clear(); });
                UIHandleHelper.ControlHandle(formsPlot_RFValues, () => { formsPlot_RFValues.Refresh(); });

                #region 处理Log的新方法

                //var logDataCollectionType = CaseCodeBase.struct_TestVariable.str_LogDataCollectionType;

                //var result = logDataCollectionType switch
                //{
                //    string type when type.Contains("TestFunc.Litepoint.LogDataCollection_V2") => HandleLitepointV2(),
                //    string type when type.Contains("TestFunc.Litepoint.LogDataCollection_V1") => HandleLitepointV1(),
                //    string type when type.Contains("TestFunc.iTest.LogDataCollection_V1") => HandleiTestV1(),
                //    string type when type.Contains("TestFunc.ACE.LogDataCollection_V1") => HandleACEV1(),
                //    string type when type.Contains("TestFunc.ICT.LogDataCollection_V1") => HandleICTV1(),
                //    _ => HandleUnknownType()
                //};

                //result.Invoke();

                //// 定义处理方法
                //Action HandleLitepointV2() => () =>
                //{
                //    if (dict_Litepoint != null) dict_Litepoint.Clear();
                //    var logsToExcel = new TestFunc.Litepoint.LogDataCollection_V2.LogsToExcel(formsPlot_RFValues, comboBox_RFMode);
                //    dict_Litepoint = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders,
                //        $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //};

                //Action HandleLitepointV1() => () =>
                //{
                //    if (dict_Litepoint != null) dict_Litepoint.Clear();
                //    var logsToExcel = new TestFunc.Litepoint.LogDataCollection_V1.LogsToExcel(formsPlot_RFValues, comboBox_RFMode);
                //    dict_Litepoint = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders,
                //        $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //};

                //Action HandleiTestV1() => () =>
                //{
                //    if (dict_iTest != null) dict_iTest.Clear();
                //    var logsToExcel = new TestFunc.iTest.LogDataCollection_V1.LogsToExcel(formsPlot_RFValues, comboBox_RFMode);
                //    dict_iTest = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders,
                //        $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //};

                //Action HandleACEV1() => () =>
                //{
                //    if (dict_ACE != null) dict_ACE.Clear();
                //    var logsToExcel = new TestFunc.ACE.LogDataCollection_V1.LogsToExcel(formsPlot_RFValues, comboBox_RFMode);
                //    dict_ACE = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders,
                //        $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //};

                //Action HandleICTV1() => () =>
                //{
                //    if (dict_ICT != null) dict_ICT.Clear();
                //    var logsToExcel = new TestFunc.ICT.LogDataCollection_V1.LogsToExcel(formsPlot_RFValues, comboBox_RFMode);
                //    dict_ICT = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders,
                //        $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //};

                //Action HandleUnknownType() => () =>
                //{
                //    MessageBoxEX.Show("LogDataCollection: 该机种数据收集的命名空间类型选择错误。", true);
                //};

                #endregion



                #region 处理Log的旧方法

                if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.Litepoint.LogDataCollection_V2"))
                {
                    if (dict_Litepoint != null)
                    {
                        dict_Litepoint.Clear();
                    }

                    TestFunc.Litepoint.LogDataCollection_V2.LogsToExcel logsToExcel = new(formsPlot_RFValues, comboBox_RFMode);
                    dict_Litepoint = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders, LogCollectConfig,
                                            $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
                }
                else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.Litepoint.LogDataCollection_V1"))
                {
                    if (dict_Litepoint != null)
                    {
                        dict_Litepoint.Clear();
                    }

                    TestFunc.Litepoint.LogDataCollection_V1.LogsToExcel logsToExcel = new(formsPlot_RFValues, comboBox_RFMode);
                    dict_Litepoint = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders, LogCollectConfig,
                                            $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
                }
                else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.iTest.LogDataCollection_V1"))
                {
                    if (dict_iTest != null)
                    {
                        dict_iTest.Clear();
                    }

                    TestFunc.iTest.LogDataCollection_V1.LogsToExcel logsToExcel = new(formsPlot_RFValues, comboBox_RFMode);
                    dict_iTest = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders, LogCollectConfig,
                                      $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");

                }
                else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.ACE.LogDataCollection_V1"))
                {
                    if (dict_ACE != null)
                    {
                        dict_ACE.Clear();
                    }

                    TestFunc.ACE.LogDataCollection_V1.LogsToExcel logsToExcel = new(formsPlot_RFValues, comboBox_RFMode);
                    dict_ACE = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders, LogCollectConfig,
                                      $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
                }
                else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.ICT.LogDataCollection_V1"))
                {
                    if (dict_ICT != null)
                    {
                        dict_ICT.Clear();
                    }

                    TestFunc.ICT.LogDataCollection_V1.LogsToExcel logsToExcel = new(formsPlot_RFValues, comboBox_RFMode);
                    dict_ICT = logsToExcel.SaveLogsToExcel(str_Folder, str_FileMark, excludedFolders, LogCollectConfig,
                                      $"{Path.GetDirectoryName(str_Folder[0])}\\{CaseCodeBase.struct_EncryptINI.stru_str_ProjectName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
                }
                else
                {
                    MessageBoxEX.Show("LogDataCollection: 该机种数据收集的命名空间类型选择错误。", true);
                }
                #endregion


            });

        }

        private void comboBox_RFMode_TextChanged(object sender, EventArgs e)
        {


            if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.Litepoint.LogDataCollection_V2"))
            {
                System.Drawing.Color randomColor;
                if (HasMoreColors())
                {
                    randomColor = GetRandomColor();
                    // 使用randomColor进行某些操作，例如设置背景色等
                    UIHandleHelper.ShowCollectionLogsRFData($"{GetRandomColor()}");
                }
                else
                {
                    MessageBoxEX.Show("所有的颜色都已经用完", true);
                    return;
                }

                if (dict_Litepoint != null)
                {
                    TestFunc.Litepoint.ShowRFValuePlot showRFValuePlot = new();
                    showRFValuePlot.PlotData(formsPlot_RFValues, dict_Litepoint, UIHandleHelper.GetControlText(comboBox_RFMode),
                                                         ScottPlot.Color.FromColor(GetRandomColor()));


                    // Calculate CPK
                    UIHandleHelper.ControlHandle(textBox_LowerLimit, () => { textBox_LowerLimit.Text = ""; });
                    UIHandleHelper.ControlHandle(textBox_UpperLimit, () => { textBox_UpperLimit.Text = ""; });
                    double? cpk = CPKCalculator.CalculateCPK(dict_Litepoint, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }

                }
            }
            else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.Litepoint.LogDataCollection_V1"))
            {
                System.Drawing.Color randomColor;
                if (HasMoreColors())
                {
                    randomColor = GetRandomColor();
                    // 使用randomColor进行某些操作，例如设置背景色等
                    UIHandleHelper.ShowCollectionLogsRFData($"{GetRandomColor()}");
                }
                else
                {
                    MessageBoxEX.Show("所有的颜色都已经用完", true);
                    return;
                }

                if (dict_Litepoint != null)
                {
                    TestFunc.Litepoint.ShowRFValuePlot showRFValuePlot = new();
                    showRFValuePlot.PlotData(formsPlot_RFValues, dict_Litepoint, UIHandleHelper.GetControlText(comboBox_RFMode),
                                                         ScottPlot.Color.FromColor(GetRandomColor()));

                    // Calculate CPK
                    UIHandleHelper.ControlHandle(textBox_LowerLimit, () => { textBox_LowerLimit.Text = ""; });
                    UIHandleHelper.ControlHandle(textBox_UpperLimit, () => { textBox_UpperLimit.Text = ""; });
                    double? cpk = CPKCalculator.CalculateCPK(dict_Litepoint, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }
                }
            }
            else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.iTest.LogDataCollection_V1"))
            {
                System.Drawing.Color randomColor;
                if (HasMoreColors())
                {
                    randomColor = GetRandomColor();
                    // 使用randomColor进行某些操作，例如设置背景色等
                    UIHandleHelper.ShowCollectionLogsRFData($"{GetRandomColor()}");
                }
                else
                {
                    MessageBoxEX.Show("所有的颜色都已经用完", true);
                    return;
                }

                if (dict_iTest != null)
                {
                    TestFunc.iTest.ShowRFValuePlot showRFValuePlot = new();
                    showRFValuePlot.PlotData(formsPlot_RFValues, dict_iTest, UIHandleHelper.GetControlText(comboBox_RFMode),
                                                         ScottPlot.Color.FromColor(GetRandomColor()));

                    // Calculate CPK
                    UIHandleHelper.ControlHandle(textBox_LowerLimit, () => { textBox_LowerLimit.Text = ""; });
                    UIHandleHelper.ControlHandle(textBox_UpperLimit, () => { textBox_UpperLimit.Text = ""; });
                    double? cpk = TestFunc.iTest.CPKCalculator.CalculateCPK(dict_iTest, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }

                }
            }
            else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.ACE.LogDataCollection_V1"))
            {

                System.Drawing.Color randomColor;
                if (HasMoreColors())
                {
                    randomColor = GetRandomColor();
                    // 使用randomColor进行某些操作，例如设置背景色等
                    UIHandleHelper.ShowCollectionLogsRFData($"{GetRandomColor()}");
                }
                else
                {
                    MessageBoxEX.Show("所有的颜色都已经用完", true);
                    return;
                }

                if (dict_ACE != null)
                {
                    TestFunc.ACE.ShowRFValuePlot showRFValuePlot = new();
                    showRFValuePlot.PlotData(formsPlot_RFValues, dict_ACE, UIHandleHelper.GetControlText(comboBox_RFMode),
                                                         ScottPlot.Color.FromColor(GetRandomColor()));

                    // Calculate CPK
                    UIHandleHelper.ControlHandle(textBox_LowerLimit, () => { textBox_LowerLimit.Text = ""; });
                    UIHandleHelper.ControlHandle(textBox_UpperLimit, () => { textBox_UpperLimit.Text = ""; });
                    double? cpk = TestFunc.ACE.CPKCalculator.CalculateCPK(dict_ACE, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }

                }
            }
            else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.ICT.LogDataCollection_V1"))
            {

                System.Drawing.Color randomColor;
                if (HasMoreColors())
                {
                    randomColor = GetRandomColor();
                    // 使用randomColor进行某些操作，例如设置背景色等
                    UIHandleHelper.ShowCollectionLogsRFData($"{GetRandomColor()}");
                }
                else
                {
                    MessageBoxEX.Show("所有的颜色都已经用完", true);
                    return;
                }

                if (dict_ICT != null)
                {
                    TestFunc.ICT.ShowICTValuePlot showRFValuePlot = new();
                    showRFValuePlot.PlotData(formsPlot_RFValues, dict_ICT, UIHandleHelper.GetControlText(comboBox_RFMode),
                                                         ScottPlot.Color.FromColor(GetRandomColor()));


                    // Calculate CPK
                    UIHandleHelper.ControlHandle(textBox_LowerLimit, () => { textBox_LowerLimit.Text = ""; });
                    UIHandleHelper.ControlHandle(textBox_UpperLimit, () => { textBox_UpperLimit.Text = ""; });
                    double? cpk = TestFunc.ICT.CPKCalculator.CalculateCPK(dict_ICT, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }
                }
            }
            else
            {
                MessageBoxEX.Show("comboBox_RFMode_TextChanged: 该机种数据收集的命名空间类型选择错误。", true);
            }


        }


        #region 颜色选择

        private static List<System.Drawing.Color> colors = AllColors.GetAllColorList();
        private List<int> availableIndices = Enumerable.Range(0, colors.Count).ToList();
        private Random random = new Random();

        public System.Drawing.Color GetRandomColor()
        {
            int index = availableIndices[random.Next(availableIndices.Count)];
            var color = colors[index];
            availableIndices.Remove(index);
            return color;
        }

        public bool HasMoreColors()
        {
            return availableIndices.Count > 0;
        }

        #endregion


        private void button_FlowFolder_Click(object sender, EventArgs e)
        {
            // 获取当前执行的exe文件所在的目录
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            string exeDir = Path.GetDirectoryName(exePath);

            FolderBrowserDialog folderDlg = new FolderBrowserDialog();

            // 设置对话框的标题
            folderDlg.Description = "请选择Flow的文件夹";

            // 设置默认打开的路径为exe文件所在的目录
            folderDlg.SelectedPath = exeDir;

            // 显示对话框，并判断是否点击了确定按钮
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的文件夹路径
                string folderPath = folderDlg.SelectedPath;
                textBox_FlowFolder.Text = folderPath;
            }
        }

        private async void button_MakeFlowStart_Click(object sender, EventArgs e)
        {
            UIHandleHelper.ControlHandle(richTextBox_FlowsData, () => richTextBox_FlowsData.Text = "");
            UIHandleHelper.ControlHandle(textBox_FlowFolder, () => textBox_FlowFolder.Enabled = false);
            UIHandleHelper.ControlHandle(button_FlowFolder, () => button_FlowFolder.Enabled = false);
            UIHandleHelper.ControlHandle(textBox_FlowName, () => textBox_FlowName.Enabled = false);
            UIHandleHelper.ControlHandle(comboBox_IQPortNums, () => comboBox_IQPortNums.Enabled = false);
            UIHandleHelper.ControlHandle(button_MakeFlowStart, () => button_MakeFlowStart.Enabled = false);

            string str_FlowFolder = textBox_FlowFolder.Text;
            string str_NewFlowName = textBox_FlowName.Text;
            int iPortNums = int.Parse(comboBox_IQPortNums.Text);

            await CreateFlow(str_FlowFolder, str_NewFlowName, iPortNums);

            UIHandleHelper.ControlHandle(textBox_FlowFolder, () => textBox_FlowFolder.Enabled = true);
            UIHandleHelper.ControlHandle(button_FlowFolder, () => button_FlowFolder.Enabled = true);
            UIHandleHelper.ControlHandle(textBox_FlowName, () => textBox_FlowName.Enabled = true);
            UIHandleHelper.ControlHandle(comboBox_IQPortNums, () => comboBox_IQPortNums.Enabled = true);
            UIHandleHelper.ControlHandle(button_MakeFlowStart, () => button_MakeFlowStart.Enabled = true);

        }

        private async Task CreateFlow(string str_FlowFolder, string str_NewFlowName, int iPortNums)
        {
            await Task.Run(() =>
            {
                GenerateMultiPortFlow generateMultiPortFlow = new();
                generateMultiPortFlow.CreateMultiPortFlow(str_FlowFolder, str_NewFlowName, iPortNums);
            });

        }

        private void comboBox_RFMode_DropDown(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;
            int newWidth;

            foreach (string s in ((ComboBox)sender).Items)
            {
                newWidth = (int)g.MeasureString(s, font).Width
                    + vertScrollBarWidth;

                if (width < newWidth)
                {
                    width = newWidth;
                }
            }

            senderComboBox.DropDownWidth = width;
        }

        private void button_ScottPlotClear_Click(object sender, EventArgs e)
        {
            formsPlot_RFValues.Plot.Clear();
            formsPlot_RFValues.Refresh();
        }

        private void button_CPK_Click(object sender, EventArgs e)
        {
            // Calculate CPK
            UIHandleHelper.ControlHandle(label_CPK, () => { label_CPK.Text = "NULL"; });

            if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.Litepoint.LogDataCollection_V2"))
            {
                if (dict_Litepoint != null)
                {
                    // Calculate CPK
                    double? cpk = CPKCalculator.CalculateCPK(dict_Litepoint, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }

                }
            }
            else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.Litepoint.LogDataCollection_V1"))
            {
                if (dict_Litepoint != null)
                {
                    // Calculate CPK
                    double? cpk = CPKCalculator.CalculateCPK(dict_Litepoint, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }
                }
            }
            else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.iTest.LogDataCollection_V1"))
            {
                if (dict_iTest != null)
                {
                    // Calculate CPK
                    double? cpk = TestFunc.iTest.CPKCalculator.CalculateCPK(dict_iTest, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }

                }
            }
            else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.ACE.LogDataCollection_V1"))
            {
                if (dict_ACE != null)
                {
                    // Calculate CPK
                    double? cpk = TestFunc.ACE.CPKCalculator.CalculateCPK(dict_ACE, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }

                }
            }
            else if (CaseCodeBase.struct_TestVariable.str_LogDataCollectionType.Contains("TestFunc.ICT.LogDataCollection_V1"))
            {
                if (dict_ICT != null)
                {
                    // Calculate CPK
                    double? cpk = TestFunc.ICT.CPKCalculator.CalculateCPK(dict_ICT, UIHandleHelper.GetControlText(comboBox_RFMode),
                        UIHandleHelper.GetControlText(textBox_LowerLimit), UIHandleHelper.GetControlText(textBox_UpperLimit));

                    if (cpk.HasValue)
                    {
                        label_CPK.Text = cpk.Value.ToString("F3");
                    }
                    else
                    {
                        MessageBoxEX.Show($"{UIHandleHelper.GetControlText(comboBox_RFMode)}, 此项不能计算CPK", true);
                    }
                }
            }
            else
            {
                MessageBoxEX.Show("comboBox_RFMode_TextChanged: 该机种数据收集的命名空间类型选择错误。", true);
            }

        }

        private void label_CPK_MouseMove(object sender, MouseEventArgs e)
        {
            string str_Tips_All = @"CPK 的意义
                     CPK > 1.33：过程能力良好，能够稳定满足规格要求。
                     ​1.0 ≤ CPK ≤ 1.33：过程能力一般，需要关注改进。
                     CPK < 1.0：过程能力不足，存在较高的不合格风险。";
            toolTip_MSG.SetToolTip(label_CPK, str_Tips_All);
        }


        /*
        private async void textBox_SelectFile_Md5_MouseDoubleClickAsync(object sender, MouseEventArgs e)
        {
            try
            {
                string selectedFilePath = await SelectFileAsync();
                UIHandleHelper.ControlHandle(textBox_SelectFile_Md5, () => textBox_SelectFile_Md5.Text = selectedFilePath);

                // 计算MD5
                string md5Hash = MD5Helper.CalculateMD5(selectedFilePath);
                UIHandleHelper.ControlHandle(richTextBox_FileMd5, () => richTextBox_FileMd5.Text = md5Hash);

            }
            catch (Exception ex)
            {
                UIHandleHelper.ControlHandle(richTextBox_FileMd5, () => richTextBox_FileMd5.Text = $"获取文件Md5 Error, {ex.Message}");
            }
        }

        public async Task<string> SelectFileAsync()
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

                    // 设置文件过滤器
                    openFileDialog.Filter = "Files|*.*";

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

        */

        private async void textBox_SelectFile_Md5_MouseDoubleClickAsync(object sender, MouseEventArgs e)
        {
            try
            {
                var selectedItems = await SelectFileOrFolderAsync();
                if (selectedItems == null || selectedItems.Count == 0) return;

                // 显示选择的第一个路径（如果是多选，可以显示第一个或显示数量）
                string displayPath = selectedItems.Count == 1 ? selectedItems[0] : $"{selectedItems.Count}个项";
                UIHandleHelper.ControlHandle(textBox_SelectFile_Md5, () => textBox_SelectFile_Md5.Text = displayPath);

                // 清空之前的MD5结果显示
                UIHandleHelper.ControlHandle(richTextBox_FileMd5, () => richTextBox_FileMd5.Clear());

                // 计算所有文件的MD5
                var md5Results = new StringBuilder();
                foreach (string path in selectedItems)
                {
                    if (File.Exists(path))
                    {
                        // 单个文件
                        string md5Hash = MD5Helper.CalculateMD5(path);
                        md5Results.AppendLine($"File: {Path.GetFileName(path)}");
                        md5Results.AppendLine($"Path: {path}");
                        md5Results.AppendLine($"MD5: {md5Hash}");
                        md5Results.AppendLine("----------------------------------------");
                    }
                    else if (Directory.Exists(path))
                    {
                        // 文件夹 - 只计算直接子文件，不递归子文件夹
                        var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                        if (files.Length == 0)
                        {
                            md5Results.AppendLine($"Folder: {Path.GetFileName(path)}");
                            md5Results.AppendLine($"Path: {path}");
                            md5Results.AppendLine("MD5: 空文件夹");
                            md5Results.AppendLine("----------------------------------------");
                            continue;
                        }

                        foreach (string file in files)
                        {
                            try
                            {
                                string md5Hash = MD5Helper.CalculateMD5(file);
                                md5Results.AppendLine($"File: {Path.GetFileName(file)}");
                                md5Results.AppendLine($"Path: {file}");
                                md5Results.AppendLine($"MD5: {md5Hash}");
                                md5Results.AppendLine("----------------------------------------");
                            }
                            catch (Exception fileEx)
                            {
                                md5Results.AppendLine($"File: {Path.GetFileName(file)}");
                                md5Results.AppendLine($"Path: {file}");
                                md5Results.AppendLine($"MD5: 计算失败 - {fileEx.Message}");
                                md5Results.AppendLine("----------------------------------------");
                            }
                        }
                    }
                }

                // 显示所有MD5结果
                UIHandleHelper.ControlHandle(richTextBox_FileMd5, () => richTextBox_FileMd5.Text = md5Results.ToString());

            }
            catch (Exception ex)
            {
                UIHandleHelper.ControlHandle(richTextBox_FileMd5, () => richTextBox_FileMd5.Text = $"获取MD5 Error, {ex.Message}");
            }
        }

        public async Task<List<string>> SelectFileOrFolderAsync()
        {
            var selectedPaths = new List<string>();

            await Task.Run(() =>
            {
                var form = Application.OpenForms["TestForm"];
                form.Invoke((MethodInvoker)delegate
                {
                    // 创建自定义对话框，让用户选择文件或文件夹
                    var dialogForm = new Form()
                    {
                        Text = "选择文件或文件夹",
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Width = 220,
                        Height = 180,
                        BackColor = Color.FromArgb(45, 45, 49),
                        StartPosition = FormStartPosition.CenterParent,
                    };

                    var fileButton = new Button() { Text = "选择文件", Left = 50, Top = 30, Width = 100, ForeColor = Color.White };
                    var folderButton = new Button() { Text = "选择文件夹", Left = 50, Top = 80, Width = 100, ForeColor = Color.White };

                    fileButton.Click += (s, e) =>
                    {
                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
                        {
                            openFileDialog.Filter = "所有文件|*.*";
                            openFileDialog.Multiselect = true;
                            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                selectedPaths.AddRange(openFileDialog.FileNames);
                            }
                        }
                        dialogForm.DialogResult = DialogResult.OK;
                        dialogForm.Close();
                    };

                    folderButton.Click += (s, e) =>
                    {
                        using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                        {
                            folderDialog.Description = "选择文件夹";
                            folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                            if (folderDialog.ShowDialog() == DialogResult.OK)
                            {
                                selectedPaths.Add(folderDialog.SelectedPath);
                            }
                        }
                        dialogForm.DialogResult = DialogResult.OK;
                        dialogForm.Close();
                    };

                    dialogForm.Controls.Add(fileButton);
                    dialogForm.Controls.Add(folderButton);
                    dialogForm.ShowDialog(form);
                });
            });

            return selectedPaths;
        }


        private void richTextBox_FileMd5_Click(object sender, EventArgs e)
        {
           
        }
        private void richTextBox_FileMd5_MouseUp(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                using (MsgBoxInfoDetails msgBoxInfoDetails = new MsgBoxInfoDetails("Md5InfoDetails", UIHandleHelper.GetControlText(richTextBox_FileMd5)))
                {
                    msgBoxInfoDetails.ShowDialog();
                }
            }
        }



        private void btn_LogCollectSettings_Click(object sender, EventArgs e)
        {

            using (LogCollectSettingForm logCollectSettingForm = new LogCollectSettingForm())
            {
                if (logCollectSettingForm.ShowDialog(this) == DialogResult.OK)
                {
                    #region 通用

                    LogCollectConfig.Standard = logCollectSettingForm._CheckBox_Standard;
                    LogCollectConfig.Result = logCollectSettingForm._CheckBox_Result;

                    #endregion

                    #region WIFI

                    LogCollectConfig.Wifi.EVM = logCollectSettingForm._CheckBox_EVM;
                    LogCollectConfig.Wifi.FreqError = logCollectSettingForm._CheckBox_FreqError;
                    LogCollectConfig.Wifi.SymClkError = logCollectSettingForm._CheckBox_SymClkError;
                    LogCollectConfig.Wifi.Power = logCollectSettingForm._CheckBox_Power;
                    LogCollectConfig.Wifi.LOLeakage = logCollectSettingForm._CheckBox_LOLeakage;
                    LogCollectConfig.Wifi.SpectrumMask = logCollectSettingForm._CheckBox_SpectrumMask;

                    LogCollectConfig.Wifi.RxRssi = logCollectSettingForm._CheckBox_Rssi;
                    LogCollectConfig.Wifi.RxPer = logCollectSettingForm._CheckBox_RxPer;
                    LogCollectConfig.Wifi.RxPower = logCollectSettingForm._CheckBox_RxPower;

                    #endregion

                    #region BT

                    LogCollectConfig.Bluetooth.Power = logCollectSettingForm._CheckBox_BtPower;
                    LogCollectConfig.Bluetooth.FreqOffset = logCollectSettingForm._CheckBox_BtFreqOffset;
                    LogCollectConfig.Bluetooth.InitFreqErr = logCollectSettingForm._CheckBox_BtInitFreqErr;

                    LogCollectConfig.Bluetooth.RxPer = logCollectSettingForm._CheckBox_BtRxPer;
                    LogCollectConfig.Bluetooth.RxPower = logCollectSettingForm._CheckBox_BtRxPower;

                    #endregion

                    #region ICT

                    LogCollectConfig.ICT.Value = logCollectSettingForm._CheckBox_IctValue;
                    LogCollectConfig.ICT.Unit = logCollectSettingForm._CheckBox_IctUint;

                    #endregion

                }
            }
        }

      
    }
}
