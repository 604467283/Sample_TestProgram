using Liteon_TestProgram.Utilities;
using ScottPlot;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Liteon_TestProgram.TestFunc.LogCollectItems;

namespace Liteon_TestProgram.TestFunc.ICT.LogDataCollection_V1
{
    internal class LogsToExcel
    {
        FormsPlot plt;
        string str_Control_ItemName = "";
        ComboBox comboBox_ICTMode;
        int iDictCount = 0;
        public Dictionary<string, List<TestICTResult>> dictionaryAllLogInfo = new Dictionary<string, List<TestICTResult>>();



        #region 挨个将每个mac对应的List<TestICTResult>存到字典中

        // 添加键值对到字典中
        public void AddToDictionary(Dictionary<string, List<TestICTResult>> dict, string key, List<TestICTResult> value)
        {
            if (dict.ContainsKey(key))
            {
                iDictCount++;
                dict[$"{key}_{iDictCount}"] = value;
            }
            else
            {
                dict[key] = value;
            }
        }

        // 根据键获取字典中的值，如果键不存在则返回null
        public List<TestICTResult> GetValueFromDictionary(Dictionary<string, List<TestICTResult>> dict, string key)
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

            comboBox_ICTMode = _comboBox_RFMode;
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


        public Dictionary<string, List<TestICTResult>> SaveLogsToExcel(string[] str_LogFolderPaths, string fileMark, HashSet<string> excludedFolders, TestConfiguration LogCollectConfig, string str_SaveExcelFilePathAndName)
        {
            List<TestICTResult> rFTestResults = new List<TestICTResult>();
            ParseTestICTResults parseTestICTResults = new ParseTestICTResults();
            ShowICTValuePlot showICTValuePlot = new ShowICTValuePlot();

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
                    var bResult = parseTestICTResults.ParseWifiTestResults(file.FullName);
                    string str_fileName = file.Name;
                    string str_Mac = str_fileName.Substring(0, str_fileName.IndexOf("_"));

                    AddToDictionary(dictionaryAllLogInfo, str_Mac, bResult.Item1);

                    if (bFirst)
                    {
                        JointTestICTTitelToExcel(bResult.Item1, str_SaveExcelFilePathAndName, LogCollectConfig);
                    }
                    JointTestICTDataToExcel(bResult.Item1, str_SaveExcelFilePathAndName, str_Mac, LogCollectConfig);

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


            //showICTValuePlot.PlotData(plt, dictionaryAllLogInfo, str_Control_ItemName, ScottPlot.Color.FromColor(System.Drawing.Color.Blue));
            return dictionaryAllLogInfo;


        }



        protected void JointTestICTDataToExcel(List<TestICTResult> abc, string str_SaveExcelFilePathAndName, string str_Mac, TestConfiguration LogCollectConfig)
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

        protected void JointTestICTTitelToExcel(List<TestICTResult> abc, string str_SaveExcelFilePathAndName, TestConfiguration LogCollectConfig)
        {

            try
            {
                if (File.Exists(str_SaveExcelFilePathAndName))
                {
                    File.Delete(str_SaveExcelFilePathAndName);
                }

                using (StreamWriter sw = new StreamWriter(str_SaveExcelFilePathAndName, true)) // 使用false来避免追加模式
                {
                    sw.Write("SN,");
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



        private void AppendTitelToStreamWriter(TestICTResult result, StreamWriter sw, TestConfiguration LogCollectConfig)
        {
            var sb = new StringBuilder();

            if (result.Mac != null)
            {
                AppendICTTitel(sb, result, LogCollectConfig);
            }

            sw.Write(sb.ToString());
        }



        private void AppendDataToStreamWriter(TestICTResult result, StreamWriter sw, TestConfiguration LogCollectConfig)
        {
            var sb = new StringBuilder();

            if (result.Mac != null)
            {
                AppendICTData(sb, result, LogCollectConfig);
            }



            //sb.AppendLine(); // 添加换行符
            sw.Write(sb.ToString());
        }


        private void AppendICTTitel(StringBuilder sb, TestICTResult result, TestConfiguration LogCollectConfig)
        {
            if (LogCollectConfig.ICT.Value)
            {
                sb.Append($"{result.Type},");

                UIHandleHelper.ControlHandle(comboBox_ICTMode, () => { comboBox_ICTMode.Items.Add($"{result.Type}"); });
            }

            if (LogCollectConfig.ICT.Unit)
            {
                sb.Append("Uint,");
            }

            if (LogCollectConfig.Standard)
            {
                sb.Append("Range,");
            }

            if (LogCollectConfig.Result)
            {
                sb.Append("Result,");
            }
           

           

            if (comboBox_ICTMode.Items.Count != 0)
            {
                UIHandleHelper.ControlHandle(comboBox_ICTMode, () => { comboBox_ICTMode.SelectedIndex = 0; });
                str_Control_ItemName = UIHandleHelper.GetControlText(comboBox_ICTMode);
            }
        }


        private void AppendICTData(StringBuilder sb, TestICTResult result, TestConfiguration LogCollectConfig)
        {
            if (result.Value == "EMPTY")
            {

            }
            else if (string.IsNullOrEmpty(result.Value) == false && LogCollectConfig.ICT.Value)
            {
                sb.Append($"{result.Value},");
            }

            if (result.Uint == "EMPTY")
            {

            }
            else if (string.IsNullOrEmpty(result.Uint) == false && LogCollectConfig.ICT.Unit)
            {
                sb.Append($"{result.Uint},");
            }

            if (result.Uint == "EMPTY")
            {

            }
            else if (string.IsNullOrEmpty(result.Range) == false && LogCollectConfig.Standard)
            {
                sb.Append($"{result.Range},");
            }

            if (result.Uint == "EMPTY")
            {

            }
            else if (string.IsNullOrEmpty(result.Result) == false && LogCollectConfig.Result)
            {
                sb.Append($"{result.Result},");
            }
        }






    }
}
