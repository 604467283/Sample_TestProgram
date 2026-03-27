using Liteon_TestProgram.Utilities;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Liteon_TestProgram.TestFunc.LogCollectItems;

namespace Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V1
{
    internal class LogsToExcel
    {
        FormsPlot plt;
        string str_Control_ItemName = "";
        ComboBox comboBox_RFMode;
        int iDictCount = 0;

        public Dictionary<string, List<RFTestResult>> dictionaryAllLogInfo = new Dictionary<string, List<RFTestResult>>();

        #region 挨个将每个mac对应的List<RFTestResult>存到字典中

        // 添加键值对到字典中
        public void AddToDictionary(Dictionary<string, List<RFTestResult>> dict, string key, List<RFTestResult> value)
        {
            if (dict.ContainsKey(key))
            {
                dict[$"{key}_{iDictCount}"] = value;
                iDictCount++;
            }
            else 
            {
                dict[key] = value;
            }
            
        }

        // 根据键获取字典中的值，如果键不存在则返回null
        public List<RFTestResult> GetValueFromDictionary(Dictionary<string, List<RFTestResult>> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                return null;
            }
        }

        #endregion

        public LogsToExcel(FormsPlot formsPlot, ComboBox _comboBox_RFMode)
        {
            plt = formsPlot;
            plt.Plot.Clear();

            comboBox_RFMode = _comboBox_RFMode;
        }

        // 递归搜索文件的函数
        void SearchFiles(DirectoryInfo directory, List<FileInfo> allFiles, string fileMark, HashSet<string> excludedFolders)
        {
            // 检查当前文件夹是否需要跳过
            if (excludedFolders.Contains(directory.Name))
            {
                return;
            }

            // 搜索当前文件夹中的文件
            var files = directory.GetFiles(fileMark, SearchOption.TopDirectoryOnly);
            allFiles.AddRange(files);

            // 递归搜索子文件夹
            foreach (var subDir in directory.GetDirectories())
            {
                SearchFiles(subDir, allFiles, fileMark, excludedFolders);
            }
        }

        public Dictionary<string, List<RFTestResult>> SaveLogsToExcel(string[] str_LogFolderPaths, string fileMark, HashSet<string> excludedFolders, TestConfiguration LogCollectConfig, string str_SaveExcelFilePathAndName)
        {
            List<RFTestResult> rFTestResults = new List<RFTestResult>();
            ParseRFTestResults parseRFTestResults = new ParseRFTestResults();
            ShowRFValuePlot showRFValuePlot = new ShowRFValuePlot();

            DirectoryInfo dirInfo = new DirectoryInfo("invalid/path");
            FileInfo[] files = new FileInfo[0];

            // 使用 List<FileInfo> 来动态收集文件
            List<FileInfo> allFiles = new List<FileInfo>();

            foreach (var str_LogFolderPath in str_LogFolderPaths)
            {
                dirInfo = new DirectoryInfo(str_LogFolderPath);
                SearchFiles(dirInfo, allFiles, fileMark, excludedFolders);
            }

            // 将 List<FileInfo> 转换为 FileInfo[]（如果需要）
            files = allFiles.ToArray();


            int iCount = 0;

            bool bFirst = true;
            // 遍历所有找到的.txt文件
            foreach (FileInfo file in files)
            {
                UIHandleHelper.ShowCollectionLogsRFData($"{file.Name}, Parse data...");
                try
                {
                    var bResult = parseRFTestResults.ParseWifiTestResults(file.FullName);

                    string str_fileName = file.Name;
                    string str_Mac = str_fileName.Substring(0, str_fileName.IndexOf("_"));

                    AddToDictionary(dictionaryAllLogInfo, str_Mac, bResult.Item1);

                    if (bFirst)
                    {
                        JointRFTestTitelToExcel(bResult.Item1, str_SaveExcelFilePathAndName, LogCollectConfig);
                    }           
                    JointRFTestDataToExcel(bResult.Item1, str_SaveExcelFilePathAndName, str_Mac, LogCollectConfig);

                    bFirst = false;

                    if (!bFirst)
                    {
                        iCount++;
                    }
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowCollectionLogsRFData($"{file.Name}, Parse data error: \r\n{ex}", true);
                    throw;
                }           
            }


            UIHandleHelper.ShowCollectionLogsRFData($"处理文件个数: {iCount}");
            UIHandleHelper.ShowCollectionLogsRFData($"CSV File: {str_SaveExcelFilePathAndName}");
            UIHandleHelper.ShowCollectionLogsRFData("Parse data... ... All OK");

            showRFValuePlot.PlotData(plt, dictionaryAllLogInfo, str_Control_ItemName, ScottPlot.Color.FromColor(System.Drawing.Color.Blue));
            return dictionaryAllLogInfo;

        }




        protected void JointRFTestDataToExcel(List<RFTestResult> abc, string str_SaveExcelFilePathAndName, string str_Mac, TestConfiguration LogCollectConfig)
        {
            try
            {
                if (File.Exists(str_SaveExcelFilePathAndName) == false)
                {

                }

                using (StreamWriter sw = new StreamWriter(str_SaveExcelFilePathAndName, true)) // 使用false来避免追加模式
                {
                    sw.WriteLine("");
                    sw.Write($"{str_Mac},");
                    foreach (var result in abc)
                    {
                        AppendDataToStreamWriter(result, sw, LogCollectConfig);
                    }
                }
            }
            catch (Exception ex)
            {
               UIHandleHelper.ShowCollectionLogsRFData($"Add CSV file Data error: \r\n {ex}", true);
            }
        }

        protected void JointRFTestTitelToExcel(List<RFTestResult> abc, string str_SaveExcelFilePathAndName, TestConfiguration LogCollectConfig)
        {

            try
            {
                if (File.Exists(str_SaveExcelFilePathAndName))
                {
                    File.Delete(str_SaveExcelFilePathAndName);
                }

                using (StreamWriter sw = new StreamWriter(str_SaveExcelFilePathAndName, true)) // 使用false来避免追加模式
                {
                    sw.Write("Mac,");
                    foreach (var result in abc)
                    {
                        AppendTitelToStreamWriter(result, sw, LogCollectConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowCollectionLogsRFData($"Add CSV file titel error: \r\n {ex}", true);
            }

        }

        private void AppendDataToStreamWriter(RFTestResult result, StreamWriter sw, TestConfiguration LogCollectConfig)
        {
            var sb = new StringBuilder();

            if (result.Metrics_Tx != null)
            {
                //AppendMetricData(sb, result, "EVM", result.Metrics_Tx.EVM, result.Metrics_Tx.EVM_Range, result.Metrics_Tx.EVM_Result);
                if (LogCollectConfig.Wifi.EVM)
                {
                    string range = LogCollectConfig.Standard ? result.Metrics_Tx.EVM_Range : "EMPTY";
                    string resultText = LogCollectConfig.Result ? result.Metrics_Tx.EVM_Result : "EMPTY";

                    AppendMetricData(sb, result, "EVM", result.Metrics_Tx.EVM, range, resultText);
                }

                //AppendMetricData(sb, result, "FreqError", result.Metrics_Tx.FreqError, result.Metrics_Tx.FreqError_Range, result.Metrics_Tx.FreqError_Result);
                if (LogCollectConfig.Wifi.FreqError)
                {
                    string range = LogCollectConfig.Standard ? result.Metrics_Tx.FreqError_Range : "EMPTY";
                    string resultText = LogCollectConfig.Result ? result.Metrics_Tx.FreqError_Result : "EMPTY";

                    AppendMetricData(sb, result, "FreqError", result.Metrics_Tx.FreqError, range, resultText);
                }

                //AppendMetricData(sb, result, "SymClkError", result.Metrics_Tx.SymClkError, result.Metrics_Tx.SymClkError_Range, result.Metrics_Tx.SymClkError_Result);
                if (LogCollectConfig.Wifi.SymClkError)
                {
                    string range = LogCollectConfig.Standard ? result.Metrics_Tx.SymClkError_Range : "EMPTY";
                    string resultText = LogCollectConfig.Result ? result.Metrics_Tx.SymClkError_Result : "EMPTY";

                    AppendMetricData(sb, result, "SymClkError", result.Metrics_Tx.SymClkError, range, resultText);
                }

                //AppendMetricData(sb, result, "Power", result.Metrics_Tx.Power, result.Metrics_Tx.Power_Range, result.Metrics_Tx.Power_Result);
                if (LogCollectConfig.Wifi.Power)
                {
                    string range = LogCollectConfig.Standard ? result.Metrics_Tx.Power_Range : "EMPTY";
                    string resultText = LogCollectConfig.Result ? result.Metrics_Tx.Power_Result : "EMPTY";

                    AppendMetricData(sb, result, "Power", result.Metrics_Tx.Power, range, resultText);
                }

                //AppendMetricData(sb, result, "LOLeakage", result.Metrics_Tx.LOLeakage, result.Metrics_Tx.LOLeakage_Range, result.Metrics_Tx.LOLeakage_Result);
                if (LogCollectConfig.Wifi.LOLeakage)
                {
                    string range = LogCollectConfig.Standard ? result.Metrics_Tx.LOLeakage_Range : "EMPTY";
                    string resultText = LogCollectConfig.Result ? result.Metrics_Tx.LOLeakage_Result : "EMPTY";

                    AppendMetricData(sb, result, "LOLeakage", result.Metrics_Tx.LOLeakage, range, resultText);
                }
            }

            if (result.Metrics_Rx != null)
            {
                //AppendMetricData(sb, result, "Rx_Power", result.Metrics_Rx.PER_Power, "EMPTY", "EMPTY");
                if (LogCollectConfig.Wifi.RxPower)
                {
                    AppendMetricData(sb, result, "Rx_Power", result.Metrics_Rx.PER_Power, "EMPTY", "EMPTY");
                }

                //AppendMetricData(sb, result, "Rx", result.Metrics_Rx.PER, result.Metrics_Rx.PER_Range, result.Metrics_Rx.PER_Result);
                if (LogCollectConfig.Wifi.RxPer)
                {
                    string range = LogCollectConfig.Standard ? result.Metrics_Rx.PER_Range : "EMPTY";
                    string resultText = LogCollectConfig.Result ? result.Metrics_Rx.PER_Result : "EMPTY";

                    AppendMetricData(sb, result, "Rx", result.Metrics_Rx.PER, range, resultText);
                }
            }

            if (result.Metrics_BtTx != null)
            {
                //AppendMetricData(sb, result, "FreqOffset", result.Metrics_BtTx.FreqOffset, result.Metrics_BtTx.FreqOffset_Range, result.Metrics_BtTx.FreqOffset_Result);
                if (LogCollectConfig.Bluetooth.FreqOffset)
                {
                    string range = LogCollectConfig.Standard ? result.Metrics_BtTx.FreqOffset_Range : "EMPTY";
                    string resultText = LogCollectConfig.Result ? result.Metrics_BtTx.FreqOffset_Result : "EMPTY";

                    AppendMetricData(sb, result, "FreqOffset", result.Metrics_BtTx.FreqOffset, range, resultText);
                }

                //AppendMetricData(sb, result, "Power", result.Metrics_BtTx.Power, result.Metrics_BtTx.Power_Range, result.Metrics_BtTx.Power_Result);
                if (LogCollectConfig.Bluetooth.Power)
                {
                    string range = LogCollectConfig.Standard ? result.Metrics_BtTx.Power_Range : "EMPTY";
                    string resultText = LogCollectConfig.Result ? result.Metrics_BtTx.Power_Result : "EMPTY";

                    AppendMetricData(sb, result, "Power", result.Metrics_BtTx.Power, range, resultText);
                }
            }


            if (result.Metrics_BtRx != null)
            {
                //AppendMetricData(sb, result, "Rx_Power", result.Metrics_BtRx.PER_Power, "EMPTY", "EMPTY");
                if (LogCollectConfig.Bluetooth.RxPower)
                {
                    AppendMetricData(sb, result, "Rx_Power", result.Metrics_BtRx.PER_Power, "EMPTY", "EMPTY");
                }

                //AppendMetricData(sb, result, "Rx", result.Metrics_BtRx.PER, result.Metrics_BtRx.PER_Range, result.Metrics_BtRx.PER_Result);
                if (LogCollectConfig.Bluetooth.RxPer)
                {
                    string range = LogCollectConfig.Standard ? result.Metrics_BtRx.PER_Range : "EMPTY";
                    string resultText = LogCollectConfig.Result ? result.Metrics_BtRx.PER_Result : "EMPTY";

                    AppendMetricData(sb, result, "Rx", result.Metrics_BtRx.PER, range, resultText);
                }
            }


            //sb.AppendLine(); // 添加换行符
            sw.Write(sb.ToString());
        }

        private void AppendTitelToStreamWriter(RFTestResult result, StreamWriter sw, TestConfiguration LogCollectConfig)
        {
            var sb = new StringBuilder();

            if (result.Metrics_Tx != null)
            {
                //AppendMetricTitel(sb, result, "EVM", "EVM_Range", "EVM_Result");
                if (LogCollectConfig.Wifi.EVM)
                {
                    string range = LogCollectConfig.Standard ? "EVM" : "";
                    string resultText = LogCollectConfig.Result ? "EVM_Range" : "";

                    AppendMetricTitel(sb, result, "EVM", range, resultText);
                }

                //AppendMetricTitel(sb, result, "FreqError", "FreqError_Range", "FreqError_Result");
                if (LogCollectConfig.Wifi.FreqError)
                {
                    string range = LogCollectConfig.Standard ? "FreqError_Range" : "";
                    string resultText = LogCollectConfig.Result ? "FreqError_Result" : "";

                    AppendMetricTitel(sb, result, "FreqError", range, resultText);
                }

                //AppendMetricTitel(sb, result, "SymClkError", "SymClkError_Range", "SymClkError_Result");
                if (LogCollectConfig.Wifi.SymClkError)
                {
                    string range = LogCollectConfig.Standard ? "SymClkError_Range" : "";
                    string resultText = LogCollectConfig.Result ? "SymClkError_Result" : "";

                    AppendMetricTitel(sb, result, "SymClkError", range, resultText);
                }

                //AppendMetricTitel(sb, result, "Power", "Power_Range", "Power_Result");
                if (LogCollectConfig.Wifi.Power)
                {
                    string range = LogCollectConfig.Standard ? "Power_Range" : "";
                    string resultText = LogCollectConfig.Result ? "Power_Result" : "";

                    AppendMetricTitel(sb, result, "Power", range, resultText);
                }

                //AppendMetricTitel(sb, result, "LOLeakage", "LOLeakage_Range", "LOLeakage_Result");
                if (LogCollectConfig.Wifi.LOLeakage)
                {
                    string range = LogCollectConfig.Standard ? "LOLeakage_Range" : "";
                    string resultText = LogCollectConfig.Result ? "LOLeakage_Result" : "";

                    AppendMetricTitel(sb, result, "LOLeakage", range, resultText);
                }
            }

            if (result.Metrics_Rx != null)
            {
                //AppendMetricTitel(sb, result, "Rx_Power", "", "");
                if (LogCollectConfig.Wifi.RxPower)
                {
                    AppendMetricTitel(sb, result, "Rx_Power", "", "");
                }

                //AppendMetricTitel(sb, result, "Rx", "Rx_Range", "Rx_Result");
                if (LogCollectConfig.Wifi.RxPer)
                {
                    string range = LogCollectConfig.Standard ? "Rx_Range" : "";
                    string resultText = LogCollectConfig.Result ? "Rx_Result" : "";

                    AppendMetricTitel(sb, result, "Rx", range, resultText);
                }
            }

            if (result.Metrics_BtTx != null)
            {
                //AppendMetricTitel(sb, result, "FreqOffset", "FreqOffset_Range", "FreqOffset_Result");
                if (LogCollectConfig.Bluetooth.FreqOffset)
                {
                    string range = LogCollectConfig.Standard ? "FreqOffset_Range" : "";
                    string resultText = LogCollectConfig.Result ? "FreqOffset_Result" : "";

                    AppendMetricTitel(sb, result, "FreqOffset", range, resultText);
                }

                //AppendMetricTitel(sb, result, "Power", "Power_Range", "Power_Result");
                if (LogCollectConfig.Bluetooth.Power)
                {
                    string range = LogCollectConfig.Standard ? "Power_Range" : "";
                    string resultText = LogCollectConfig.Result ? "Power_Result" : "";

                    AppendMetricTitel(sb, result, "Power", range, resultText);
                }
            }


            if (result.Metrics_BtRx != null)
            {
                //AppendMetricTitel(sb, result, "Rx_Power", "", "");
                if (LogCollectConfig.Bluetooth.RxPower)
                {
                    AppendMetricTitel(sb, result, "Rx_Power", "", "");
                }

                //AppendMetricTitel(sb, result, "Rx", "Rx_Range", "Rx_Result");
                if (LogCollectConfig.Bluetooth.RxPer)
                {
                    string range = LogCollectConfig.Standard ? "Rx_Range" : "";
                    string resultText = LogCollectConfig.Result ? "Rx_Result" : "";

                    AppendMetricTitel(sb, result, "Rx", range, resultText);
                }
            }

            sw.Write(sb.ToString());
        }

        private void AppendMetricTitel(StringBuilder sb, RFTestResult result, string metricNameValue1, string metricNameValue2, string metricNameValue3)
        {
            if (result.Metrics_Tx != null || result.Metrics_Rx != null)
            {
                if (string.IsNullOrEmpty(metricNameValue1) == false)
                {
                    sb.Append($"{result.Frequency}_{result.DataRate}_{result.Bandwidth}_{result.Antenna}_{metricNameValue1},");
                    if (metricNameValue1.Contains("Rx_Power") == false)
                    {
                        UIHandleHelper.ControlHandle(comboBox_RFMode, () => { comboBox_RFMode.Items.Add($"{result.Frequency}#{result.DataRate}#{result.Bandwidth}#{result.Antenna}#{metricNameValue1}"); });
                    }
                }

                if (string.IsNullOrEmpty(metricNameValue2) == false)
                {
                    sb.Append($"{result.Frequency}_{result.DataRate}_{result.Bandwidth}_{result.Antenna}_{metricNameValue2},");
                }

                if (string.IsNullOrEmpty(metricNameValue3) == false)
                {
                    sb.Append($"{result.Frequency}_{result.DataRate}_{result.Bandwidth}_{result.Antenna}_{metricNameValue3},");
                }
            }

            if (result.Metrics_BtTx != null || result.Metrics_BtRx != null)
            {
                if (string.IsNullOrEmpty(metricNameValue1) == false)
                {
                    sb.Append($"{result.Frequency}_{result.DataRate}_{metricNameValue1},");
                    if (metricNameValue1.Contains("Rx_Power") == false)
                    {
                        UIHandleHelper.ControlHandle(comboBox_RFMode, () => { comboBox_RFMode.Items.Add($"{result.Frequency}#{result.DataRate}#{metricNameValue1}"); });
                    }
                }

                if (string.IsNullOrEmpty(metricNameValue2) == false)
                {
                    sb.Append($"{result.Frequency}_{result.DataRate}_{metricNameValue2},");
                }

                if (string.IsNullOrEmpty(metricNameValue3) == false)
                {
                    sb.Append($"{result.Frequency}_{result.DataRate}_{metricNameValue3},");
                }
            }

            if (comboBox_RFMode.Items.Count != 0)
            {
                UIHandleHelper.ControlHandle(comboBox_RFMode, () => { comboBox_RFMode.SelectedIndex = 0; });
                str_Control_ItemName = UIHandleHelper.GetControlText(comboBox_RFMode);
            }

        }

        private void AppendMetricData(StringBuilder sb, RFTestResult result, string metricName, string value, string range, string resultStr)
        {
            if (value == "EMPTY")
            {

            }
            else if (string.IsNullOrEmpty(value) == false)
            {
                sb.Append($"{value},");
            }
            else
            {
                sb.Append("null,");
            }



            if (range == "EMPTY")
            {

            }
            else if (string.IsNullOrEmpty(range) == false)
            {
                range = range.Replace(",", " / ");
                sb.Append($"{range},");
            }
            else
            {
                sb.Append("null,");
            }


            if (resultStr == "EMPTY")
            {

            }
            else if (string.IsNullOrEmpty(resultStr) == false)
            {
                sb.Append(resultStr + ",");
            }
            else
            {
                sb.Append("null,");
            }



        }

    }
}
