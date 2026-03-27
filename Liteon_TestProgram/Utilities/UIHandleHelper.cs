using FluentFTP;
using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using OpenTK.Audio.OpenAL;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Liteon_TestProgram.Base.Class_Variable;


namespace Liteon_TestProgram.Utilities
{
    internal class UIHandleHelper
    {

        public static RichTextBox LogBox { get; set; }
        public static RichTextBox ParseLogBox { get; set; }
        public static RichTextBox FlowLogBox { get; set; }
        public static RichTextBox PEMLogBox { get; set; }
        public static FormsPlot formsPlot_RF { get; set; }
        public static DataGridView dataGridView { get; set; }

        public static ToolStripStatusLabel toolStripStatusLabel_Robot;
        public static ToolStripStatusLabel toolStripStatusLabel_Multi;
        public static StatusStrip statusStrip;


        // 静态变量用于时间测量
        private static Stopwatch _globalStopwatch = new Stopwatch();
        private static Stopwatch _lastGridViewStopwatch = new Stopwatch();
        private static DateTime? _lastGridViewTime = null;



        // 原始方法改造成包装方法
        public static void ShowCollectionLogsRFData(string content, bool isFail = false)
            => ShowRichTextBoxInfo(ParseLogBox, content, isFail);

        public static void ShowCreateFlowInfo(string content, bool isFail = false)
            => ShowRichTextBoxInfo(FlowLogBox, content, isFail);

        public static void ShowPEMInfo(string content, bool isFail = false)
           => ShowRichTextBoxInfo(PEMLogBox, content, isFail);


        #region ShowRunLog老方法

        /*
            public static void ShowRunLog(string str_LogContent, bool FailColor = false, 
                                                       bool ShowGridView = false,
                                                       string TestItemName = null, 
                                                       string TestItemContent = null, 
                                                       bool TestItemResult = false, 
                                                       string ErrorCode = "Err000")
            {
                if (LogBox == null)
                {
                    throw new ArgumentException("RichTextBox控件信息为空.");
                }

                StringBuilder sbLog = new StringBuilder();
                sbLog.Append($"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] ");
                sbLog.Append(str_LogContent);
                sbLog.AppendLine();

                string filePath = ".\\UI_Log.txt";
                const int maxRetries = 3; // 最大重试次数
                const int retryDelay = 100; // 重试间隔(毫秒)

                for (int attempt = 0; attempt < maxRetries; attempt++)
                {
                    try
                    {
                        // 使用更宽松的共享模式 FileShare.ReadWrite
                        using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                        using (var sw = new StreamWriter(fs))
                        {
                            sw.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] {str_LogContent}");
                            break; // 成功则退出重试循环
                        }
                    }
                    catch (IOException) when (attempt < maxRetries - 1)
                    {
                        Thread.Sleep(retryDelay); // 等待后重试
                    }
                }



                if (LogBox.InvokeRequired)
                {
                    LogBox.Invoke(new Action(() =>
                    {
                        if (FailColor)
                        {
                            LogBox.SelectionColor = System.Drawing.Color.Red;
                            LogBox.SelectionFont = new Font(LogBox.Font, FontStyle.Bold);
                        }
                        LogBox.AppendText(sbLog.ToString());
                        LogBox.Focus();
                        LogBox.Select(LogBox.TextLength, 0);
                        LogBox.ScrollToCaret();
                    }));
                }
                else
                {
                    if (FailColor)
                    {
                        LogBox.SelectionColor = System.Drawing.Color.Red;
                        LogBox.SelectionFont = new Font(LogBox.Font, FontStyle.Bold);
                    }
                    LogBox.AppendText(sbLog.ToString());
                    LogBox.Focus();
                    LogBox.Select(LogBox.TextLength, 0);
                    LogBox.ScrollToCaret();
                }

                if (ShowGridView)
                {
                    if (string.IsNullOrEmpty(TestItemName) || string.IsNullOrEmpty(TestItemContent))
                    {
                        MessageBoxEX.Show("GridView TestItemName 和 TestItemContent不能为空", true);
                    }

                    if (FailColor == TestItemResult)
                    {
                        MessageBoxEX.Show("GridView 测试结果和log显示设定有误", true);
                    }

                    DataGridViewShow(TestItemName, TestItemContent, TestItemResult);

                    CaseCodeBase.struct_TestVariable.str_ErrorCode = ErrorCode;
                }

            }


            public static void DataGridViewShow(string str_TestItemName, string str_TestItemContent, bool bTestItemResult)
            {
                if (dataGridView.InvokeRequired)
                {
                    dataGridView.Invoke(() =>
                    {
                        int rowCount = dataGridView.RowCount;
                        string strResult = "";
                        var color = Color.Red;
                        if (bTestItemResult)
                        {
                            strResult = "Pass";
                            color = Color.Green;
                        }
                        else
                        {
                            strResult = "Fail";
                            color = Color.Red;
                        }


                        var newData = new
                        {
                            Column1 = (rowCount + 1).ToString(),
                            Column2 = str_TestItemName,
                            Column3 = str_TestItemContent,
                            Column4 = strResult
                        };

                        dataGridView.Rows.Add(newData.Column1, newData.Column2, newData.Column3, newData.Column4);
                        dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;
                        dataGridView.Rows[rowCount].Cells[3].Style.BackColor = color;

                    });
                }
                else
                {
                    int rowCount = dataGridView.RowCount;
                    string strResult = "";
                    var color = Color.Red;
                    if (bTestItemResult)
                    {
                        strResult = "Pass";
                        color = Color.Green;
                    }
                    else
                    {
                        strResult = "Fail";
                        color = Color.Red;
                    }


                    var newData = new
                    {
                        Column1 = (rowCount + 1).ToString(),
                        Column2 = str_TestItemName,
                        Column3 = str_TestItemContent,
                        Column4 = strResult
                    };

                    dataGridView.Rows.Add(newData.Column1, newData.Column2, newData.Column3, newData.Column4);
                    dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;
                    dataGridView.Rows[rowCount].Cells[3].Style.BackColor = color;

                }
            }
            */

        #endregion

        #region ShowRunLog新方法

        //逻辑
        // 当ShowGridView为true时，则计算上一个ShowGridView为true的ShowRunLog 到这个ShowRunLog的之间的运行时间；
        //如果找不到，则从程序中public async Task OnOK()开始运行计算到这个ShowRunLog的之间的运行时间；使用Stopwatch类来精确测量执行时间
        //将两个ShowGridView为true的ShowRunLog的间隔时间传给DataGridViewShow方法，在DataGridView的Time类中使用秒为单位显示出来

        // 初始化计时器
        // 重置所有计时器
        public static void ResetTiming()
        {
            _globalStopwatch.Reset();
            _lastGridViewStopwatch.Reset();
            _lastGridViewTime = null;
        }

        // 初始化计时器（每次测试开始时调用）
        public static void InitializeTiming()
        {
            ResetTiming(); // 重置计时器
            _globalStopwatch.Start(); // 开始新的计时
        }


        public static void ShowRunLog(string str_LogContent, bool FailColor = false,
                                                   bool ShowGridView = false,
                                                   string TestItemName = null,
                                                   string TestItemContent = null,
                                                   bool TestItemResult = false,
                                                   string ErrorCode = "Err000")
        {
            if (LogBox == null)
            {
                throw new ArgumentException("RichTextBox控件信息为空.");
            }

            StringBuilder sbLog = new StringBuilder();
            sbLog.Append($"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] ");
            sbLog.Append(str_LogContent);
            sbLog.AppendLine();

            string filePath = ".\\UI_Log.txt";
            const int maxRetries = 3; // 最大重试次数
            const int retryDelay = 100; // 重试间隔(毫秒)

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    // 使用更宽松的共享模式 FileShare.ReadWrite
                    using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (var sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] {str_LogContent}");
                        break; // 成功则退出重试循环
                    }
                }
                catch (IOException) when (attempt < maxRetries - 1)
                {
                    Thread.Sleep(retryDelay); // 等待后重试
                }
            }

            if (LogBox.InvokeRequired)
            {
                LogBox.Invoke(new Action(() =>
                {
                    if (FailColor)
                    {
                        LogBox.SelectionColor = System.Drawing.Color.Red;
                        LogBox.SelectionFont = new Font(LogBox.Font, FontStyle.Bold);
                    }
                    LogBox.AppendText(sbLog.ToString());
                    LogBox.Focus();
                    LogBox.Select(LogBox.TextLength, 0);
                    LogBox.ScrollToCaret();
                }));
            }
            else
            {
                if (FailColor)
                {
                    LogBox.SelectionColor = System.Drawing.Color.Red;
                    LogBox.SelectionFont = new Font(LogBox.Font, FontStyle.Bold);
                }
                LogBox.AppendText(sbLog.ToString());
                LogBox.Focus();
                LogBox.Select(LogBox.TextLength, 0);
                LogBox.ScrollToCaret();
            }

            if (ShowGridView)
            {
                if (string.IsNullOrEmpty(TestItemName) || string.IsNullOrEmpty(TestItemContent))
                {
                    MessageBoxEX.Show("GridView TestItemName 和 TestItemContent不能为空", true);
                }

                if (FailColor == TestItemResult)
                {
                    MessageBoxEX.Show("GridView 测试结果和log显示设定有误", true);
                }

                // 计算时间间隔
                double elapsedSeconds = 0;

                if (_lastGridViewTime.HasValue)
                {
                    // 使用Stopwatch获取更精确的时间间隔
                    if (_lastGridViewStopwatch.IsRunning)
                    {
                        _lastGridViewStopwatch.Stop();
                        elapsedSeconds = _lastGridViewStopwatch.Elapsed.TotalSeconds;
                    }
                }
                else
                {
                    // 如果没有上一次记录，使用全局计时器
                    elapsedSeconds = _globalStopwatch.Elapsed.TotalSeconds;
                }

                // 更新最后一次ShowGridView=true的时间和计时器
                _lastGridViewTime = DateTime.Now;
                _lastGridViewStopwatch.Restart();

                // 调用DataGridViewShow并传入时间参数
                DataGridViewShow(TestItemName, TestItemContent, TestItemResult, elapsedSeconds);

                CaseCodeBase.struct_TestVariable.str_ErrorCode = ErrorCode;
            }
        }

        public static void DataGridViewShow(string str_TestItemName, string str_TestItemContent,
                                          bool bTestItemResult, double elapsedSeconds = 0)
        {
            if (dataGridView.InvokeRequired)
            {
                dataGridView.Invoke(() =>
                {
                    int rowCount = dataGridView.RowCount;
                    string strResult = "";
                    var color = Color.Red;
                    if (bTestItemResult)
                    {
                        strResult = "Pass";
                        color = Color.Green;
                    }
                    else
                    {
                        strResult = "Fail";
                        color = Color.Red;
                    }

                    // 确保DataGridView有Time列
                    if (dataGridView.Columns.Count < 5)
                    {
                        dataGridView.Columns.Add("Time", "Time(s)");
                    }

                    var newData = new
                    {
                        Column1 = (rowCount + 1).ToString(),
                        Column2 = str_TestItemName,
                        Column3 = str_TestItemContent,
                        Column4 = $"{elapsedSeconds:F2} s", // 显示时间，保留2位小数
                        Column5 = strResult,

                    };

                    dataGridView.Rows.Add(newData.Column1, newData.Column2, newData.Column3,
                                        newData.Column4, newData.Column5);
                    dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;
                    dataGridView.Rows[rowCount].Cells[4].Style.BackColor = color;
                });
            }
            else
            {
                int rowCount = dataGridView.RowCount;
                string strResult = "";
                var color = Color.Red;
                if (bTestItemResult)
                {
                    strResult = "Pass";
                    color = Color.Green;
                }
                else
                {
                    strResult = "Fail";
                    color = Color.Red;
                }

                // 确保DataGridView有Time列
                if (dataGridView.Columns.Count < 5)
                {
                    dataGridView.Columns.Add("Time", "Time(s)");
                }

                var newData = new
                {
                    Column1 = (rowCount + 1).ToString(),
                    Column2 = str_TestItemName,
                    Column3 = str_TestItemContent,
                    Column4 = strResult,
                    Column5 = $"{elapsedSeconds:F2}" // 显示时间，保留2位小数
                };

                dataGridView.Rows.Add(newData.Column1, newData.Column2, newData.Column3,
                                    newData.Column4, newData.Column5);
                dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;
                dataGridView.Rows[rowCount].Cells[3].Style.BackColor = color;
            }
        }


        #endregion





        public static void ShowRichTextBoxInfo(RichTextBox richTextBox, string str_LogContent, bool bFailColor = false)
        {
            if (richTextBox == null)
            {
                throw new ArgumentException("RichTextBox控件信息为空.");
            }

            StringBuilder sbLog = new StringBuilder();
            sbLog.Append($"[{DateTime.Now.ToString("HH:mm:ss")}] ");
            sbLog.Append(str_LogContent);
            sbLog.AppendLine();


            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action(() =>
                {
                    if (bFailColor)
                    {
                        richTextBox.SelectionColor = System.Drawing.Color.Red;
                        richTextBox.SelectionFont = new Font(richTextBox.Font, FontStyle.Bold);
                    }
                    richTextBox.AppendText(sbLog.ToString());
                    richTextBox.Focus();
                    richTextBox.Select(richTextBox.TextLength, 0);
                    richTextBox.ScrollToCaret();
                }));
            }
            else
            {
                if (bFailColor)
                {
                    richTextBox.SelectionColor = System.Drawing.Color.Red;
                    richTextBox.SelectionFont = new Font(richTextBox.Font, FontStyle.Bold);
                }
                richTextBox.AppendText(sbLog.ToString());
                richTextBox.Focus();
                richTextBox.Select(richTextBox.TextLength, 0);
                richTextBox.ScrollToCaret();
            }
        }



        public static void ShowToolStripStatus_Robot(string str_Content)
        {
            if (toolStripStatusLabel_Robot == null)
            {
                throw new ArgumentException("ToolStripStatusLabel_Robot 控件信息为空.");
            }

            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new Action(() =>
                {
                    toolStripStatusLabel_Robot.Text = str_Content;
                }));
            }
            else
            {
                toolStripStatusLabel_Robot.Text = str_Content;
            }
        }

        public static void ShowToolStripStatus_Multi(string str_Content)
        {
            if (toolStripStatusLabel_Multi == null)
            {
                throw new ArgumentException("ToolStripStatusLabel_Multi 控件信息为空.");
            }

            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new Action(() =>
                {
                    toolStripStatusLabel_Multi.Text = str_Content;
                }));
            }
            else
            {
                toolStripStatusLabel_Multi.Text = str_Content;
            }
        }

        public static void ControlHandle(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static string GetControlText(Control control)
        {
            if (control.InvokeRequired)
            {
                return (string)control.Invoke(new Func<string>(() => control.Text));
            }
            else
            {
                return control.Text;
            }
        }




        public static void DataGridViewClear()
        {
            dataGridView.Rows.Clear(); // 清空所有行
            dataGridView.Columns.Clear(); // 清空所有列
        }

        public static void DataGridViewShow_SampleSetHeader()
        {
            if (dataGridView.InvokeRequired)
            {
                dataGridView.Invoke(() =>
                {

                    DataGridViewColumn newColumn = new DataGridViewTextBoxColumn();
                    newColumn.HeaderText = "项目"; // 列标题
                    newColumn.Name = "newColumnName_Item"; // 列名
                    newColumn.ValueType = typeof(string); // 列数据类型

                    // 将新列添加到DataGridView中
                    dataGridView.Columns.Add(newColumn);


                    DataGridViewColumn newColumn1 = new DataGridViewTextBoxColumn();
                    newColumn1.HeaderText = "频率"; // 列标题
                    newColumn1.Name = "newColumnName_Ch"; // 列名
                    newColumn1.ValueType = typeof(string); // 列数据类型

                    // 将新列添加到DataGridView中
                    dataGridView.Columns.Add(newColumn1);


                    DataGridViewColumn newColumn2 = new DataGridViewTextBoxColumn();
                    newColumn2.HeaderText = "模式"; // 列标题
                    newColumn2.Name = "newColumnName2"; // 列名
                    newColumn2.ValueType = typeof(string); // 列数据类型

                    // 将新列添加到DataGridView中
                    dataGridView.Columns.Add(newColumn2);


                    DataGridViewColumn newColumn3 = new DataGridViewTextBoxColumn();
                    newColumn3.HeaderText = "测试的值"; // 列标题
                    newColumn3.Name = "newColumnName_TestValue"; // 列名
                    newColumn3.ValueType = typeof(string); // 列数据类型

                    // 将新列添加到DataGridView中
                    dataGridView.Columns.Add(newColumn3);


                    DataGridViewColumn newColumn4 = new DataGridViewTextBoxColumn();
                    newColumn4.HeaderText = "中心值"; // 列标题
                    newColumn4.Name = "newColumnName_Spec"; // 列名
                    newColumn4.ValueType = typeof(string); // 列数据类型

                    // 将新列添加到DataGridView中
                    dataGridView.Columns.Add(newColumn4);


                    DataGridViewColumn newColumn5 = new DataGridViewTextBoxColumn();
                    newColumn5.HeaderText = "公差"; // 列标题
                    newColumn5.Name = "newColumnName_Tolerance"; // 列名
                    newColumn5.ValueType = typeof(string); // 列数据类型

                    // 将新列添加到DataGridView中
                    dataGridView.Columns.Add(newColumn5);


                    DataGridViewColumn newColumn6 = new DataGridViewTextBoxColumn();
                    
                    newColumn6.HeaderText = "差值"; // 列标题
                    newColumn6.Name = "newColumnName_DiffValue"; // 列名
                    newColumn6.ValueType = typeof(string); // 列数据类型
                    newColumn6.ReadOnly = false;
                   
                    // 将新列添加到DataGridView中
                    dataGridView.Columns.Add(newColumn6);


                    DataGridViewColumn newColumn7 = new DataGridViewTextBoxColumn();
                    newColumn7.HeaderText = "结果"; // 列标题
                    newColumn7.Name = "newColumnName_Result"; // 列名
                    newColumn7.ValueType = typeof(string); // 列数据类型

                    // 将新列添加到DataGridView中
                    dataGridView.Columns.Add(newColumn7);



                });
            }
            else
            {
                DataGridViewColumn newColumn = new DataGridViewTextBoxColumn();
                newColumn.HeaderText = "项目"; // 列标题
                newColumn.Name = "newColumnName_Item"; // 列名
                newColumn.ValueType = typeof(string); // 列数据类型

                // 将新列添加到DataGridView中
                dataGridView.Columns.Add(newColumn);


                DataGridViewColumn newColumn1 = new DataGridViewTextBoxColumn();
                newColumn1.HeaderText = "频率"; // 列标题
                newColumn1.Name = "newColumnName_Ch"; // 列名
                newColumn1.ValueType = typeof(string); // 列数据类型

                // 将新列添加到DataGridView中
                dataGridView.Columns.Add(newColumn1);


                DataGridViewColumn newColumn2 = new DataGridViewTextBoxColumn();
                newColumn2.HeaderText = "模式"; // 列标题
                newColumn2.Name = "newColumnName2"; // 列名
                newColumn2.ValueType = typeof(string); // 列数据类型

                // 将新列添加到DataGridView中
                dataGridView.Columns.Add(newColumn2);


                DataGridViewColumn newColumn3 = new DataGridViewTextBoxColumn();
                newColumn3.HeaderText = "测试的值"; // 列标题
                newColumn3.Name = "newColumnName_TestValue"; // 列名
                newColumn3.ValueType = typeof(string); // 列数据类型

                // 将新列添加到DataGridView中
                dataGridView.Columns.Add(newColumn3);


                DataGridViewColumn newColumn4 = new DataGridViewTextBoxColumn();
                newColumn4.HeaderText = "中心值"; // 列标题
                newColumn4.Name = "newColumnName_Spec"; // 列名
                newColumn4.ValueType = typeof(string); // 列数据类型

                // 将新列添加到DataGridView中
                dataGridView.Columns.Add(newColumn4);


                DataGridViewColumn newColumn5 = new DataGridViewTextBoxColumn();
                newColumn5.HeaderText = "公差"; // 列标题
                newColumn5.Name = "newColumnName_Tolerance"; // 列名
                newColumn5.ValueType = typeof(string); // 列数据类型

                // 将新列添加到DataGridView中
                dataGridView.Columns.Add(newColumn5);


                DataGridViewColumn newColumn6 = new DataGridViewTextBoxColumn();

                newColumn6.HeaderText = "差值"; // 列标题
                newColumn6.Name = "newColumnName_DiffValue"; // 列名
                newColumn6.ValueType = typeof(string); // 列数据类型
                newColumn6.ReadOnly = false;

                // 将新列添加到DataGridView中
                dataGridView.Columns.Add(newColumn6);


                DataGridViewColumn newColumn7 = new DataGridViewTextBoxColumn();
                newColumn7.HeaderText = "结果"; // 列标题
                newColumn7.Name = "newColumnName_Result"; // 列名
                newColumn7.ValueType = typeof(string); // 列数据类型

                // 将新列添加到DataGridView中
                dataGridView.Columns.Add(newColumn7);


            }



        }

        public static bool DataGridViewShow_SampleSetTestData(List<TestFunc.Litepoint.RFTestResult> RfTestResultList, float[] floatArray, float fPowerRange)
        {
            bool bresult = false;
            if (dataGridView.InvokeRequired)
            {
                #region InvokeRequired

                dataGridView.Invoke(() =>
                {
                    int rowCount = dataGridView.RowCount;
                    string str_CH = "";
                    string str_Mode = "";
                    string str_TestValue = "";
                    float f_Spec = 0;
                    float f_Range = fPowerRange;
                    string str_Result = "";
                    string str_DiffValue = "";

                    float f_SpecUpper = 0;
                    float f_SpecLower = 0;
                   
                    int iTestItems = floatArray.Length;
                    int iTestPassTimes = 0;

                    int iCount  = 0;
                    foreach (var result in RfTestResultList)
                    {
                         rowCount = dataGridView.RowCount;
                         str_CH = "";
                         str_Mode = "";
                         str_TestValue = "";
                         f_Spec = 0;
                        f_Range = 0;
                         str_Result = "";
                         str_DiffValue = "";

                        if (result.Bandwidth == null && result.Antenna == null)
                        {
                            str_CH = result.Frequency;
                            str_Mode = $"{result.DataRate}";
                        }
                        else
                        {
                            str_CH = result.Frequency;
                            str_Mode = $"{result.DataRate}_{result.Bandwidth}_{result.Antenna}";
                        }

                        if (result.Metrics_Tx != null)
                        {
                            str_TestValue =  $"{result.Metrics_Tx.Power}";
                        }

  

                        if (result.Metrics_BtTx != null)
                        {
                            str_TestValue = $"{result.Metrics_BtTx.Power}";
                        }


                        f_Spec = floatArray[iCount];

                        f_SpecUpper = f_Spec + fPowerRange;
                        f_SpecLower = f_Spec - fPowerRange;

                        if (f_SpecUpper>= float.Parse( str_TestValue) && f_SpecLower <= float.Parse(str_TestValue))
                        {
                            str_Result = "Pass";
                            iTestPassTimes = iTestPassTimes + 1;
                        }
                        else
                        {
                            str_Result = "Fail";
                        }


                        var color = Color.Red;
                        if (str_Result == "Pass")
                        {
                            color = Color.Green;
                        }
                        else
                        {
                            color = Color.Red;
                        }

                        //格式化，保留两位小数
                        str_DiffValue =(  f_Spec - float.Parse(str_TestValue) ).ToString("0.00");

                        var newData = new
                        {
                            Column1 = (rowCount + 1).ToString(),
                            Column2 = str_CH,
                            Column3 = str_Mode,
                            Column4 = str_TestValue,
                            Column5 = f_Spec.ToString("0.00"),
                            Column6 = $"+/-{fPowerRange}",
                            Column7 = str_DiffValue,
                            Column8 = str_Result,

                        };

                        dataGridView.Rows.Add(newData.Column1, newData.Column2, newData.Column3, newData.Column4,
                                                           newData.Column5, newData.Column6, newData.Column7, newData.Column8);
                        dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;
                        dataGridView.Rows[rowCount].Cells[7].Style.BackColor = color;


                        iCount++;
                    }


                    if (iTestPassTimes == iTestItems)
                    {
                        bresult= true;
                    }
                    else
                    {
                        bresult = false;
                    }

                });

                return bresult;

                #endregion
            }
            else
            {
                #region normal

                int rowCount = dataGridView.RowCount;
                string str_CH = "";
                string str_Mode = "";
                string str_TestValue = "";
                float f_Spec = 0;
                float f_Range = fPowerRange;
                string str_Result = "";
                string str_DiffValue = "";

                float f_SpecUpper = 0;
                float f_SpecLower = 0;

                int iTestItems = floatArray.Length;
                int iTestPassTimes = 0;

                int iCount = 0;
                foreach (var result in RfTestResultList)
                {
                    rowCount = dataGridView.RowCount;
                    str_CH = "";
                    str_Mode = "";
                    str_TestValue = "";
                    f_Spec = 0;
                    f_Range = 0;
                    str_Result = "";
                    str_DiffValue = "";

                    if (result.Bandwidth == null && result.Antenna == null)
                    {
                        str_CH = result.Frequency;
                        str_Mode = $"{result.DataRate}";
                    }
                    else
                    {
                        str_CH = result.Frequency;
                        str_Mode = $"{result.DataRate}_{result.Bandwidth}_{result.Antenna}";
                    }

                    if (result.Metrics_Tx != null)
                    {
                        str_TestValue = $"{result.Metrics_Tx.Power}";
                    }



                    if (result.Metrics_BtTx != null)
                    {
                        str_TestValue = $"{result.Metrics_BtTx.Power}";
                    }


                    f_Spec = floatArray[iCount];//获取ini中的中心power

                    f_SpecUpper = f_Spec + fPowerRange;
                    f_SpecLower = f_Spec - fPowerRange;

                    if (f_SpecUpper >= float.Parse(str_TestValue) && f_SpecLower <= float.Parse(str_TestValue))
                    {
                        str_Result = "Pass";
                        iTestPassTimes = iTestPassTimes + 1;
                    }
                    else
                    {
                        str_Result = "Fail";
                    }


                    var color = Color.Red;
                    if (str_Result == "Pass")
                    {
                        color = Color.Green;
                    }
                    else
                    {
                        color = Color.Red;
                    }

                    //格式化，保留两位小数
                    str_DiffValue = (f_Spec - float.Parse(str_TestValue)).ToString("0.00");

                    var newData = new
                    {
                        Column1 = (rowCount + 1).ToString(),
                        Column2 = str_CH,
                        Column3 = str_Mode,
                        Column4 = str_TestValue,
                        Column5 = f_Spec.ToString("0.00"),
                        Column6 = $"+/-{fPowerRange}",
                        Column7 = str_DiffValue,
                        Column8 = str_Result,
                    };

                    dataGridView.Rows.Add(newData.Column1, newData.Column2, newData.Column3, newData.Column4,
                                                       newData.Column5, newData.Column6, newData.Column7, newData.Column8);
                    dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;
                    dataGridView.Rows[rowCount].Cells[7].Style.BackColor = color;


                    iCount++;
                }



                if (iTestPassTimes == iTestItems)
                {
                    return true;
                }
                else
                {
                    return false;
                }

                #endregion
            }

        }




    }
}
