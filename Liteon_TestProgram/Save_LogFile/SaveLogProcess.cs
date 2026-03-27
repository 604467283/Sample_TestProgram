using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Save_LogFile
{
    internal class SaveLogProcess
    {
        public static bool SaveLog_Local(bool bTestResult, string strLogfilePath, string strCaseProjectName, string strBarcode, string strSourceFile, string strDestinationFileSuffix = "_UI.txt")
        {
            if (File.Exists(strSourceFile) == false)
            {
                UIHandleHelper.ShowRunLog("The source file log does not exist.", true);
                return false;
            }

            bool bResult = false;
            string strCurrentTime_YMD = DateTime.Now.ToString("yyyyMMdd");
            string strCurrentTime_Detail = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string strFileDir = strLogfilePath + "\\" + strCaseProjectName + "\\" + strCurrentTime_YMD;

            if (CreateDirectoryIfNotExists(strFileDir + "\\PASS\\") == false)
            {
                UIHandleHelper.ShowRunLog("Failed to create the path of the local log pass folder.", true);
            }

            if (CreateDirectoryIfNotExists(strFileDir + "\\FAIL\\") == false)
            {
                UIHandleHelper.ShowRunLog("Failed to create the path of the local log fail folder.", true);
            }

            if (bTestResult) 
            {
                if (CopyFile(strSourceFile, strFileDir + "\\PASS\\" + strBarcode + "_" + strCurrentTime_Detail + strDestinationFileSuffix) == false)
                {
                    UIHandleHelper.ShowRunLog($"Failed to copy test log ({strDestinationFileSuffix}) to pass folder.", true);
                    bResult = false;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Copy test log({strDestinationFileSuffix})to pass folder successfully.");
                    bResult = true;
                }
            }
            else
            {
                if (CopyFile(strSourceFile, strFileDir + "\\FAIL\\" + strBarcode + "_" + strCurrentTime_Detail + strDestinationFileSuffix) == false)
                {
                    UIHandleHelper.ShowRunLog($"Failed to copy test log ({strDestinationFileSuffix}) to fail folder.", true);
                    bResult = false;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Copy test log({strDestinationFileSuffix})to fail folder successfully.");
                    bResult = true;
                }
            }

           
            return bResult;
        }


        public static bool SaveLog_Local(bool bTestResult, string strLogfilePath, string strCaseProjectName, string strBarcode, DataGridView dgv, double totalTime, string strDestinationFileSuffix = "_UI.html")
        {

            bool bResult = false;
            string strCurrentTime_YMD = DateTime.Now.ToString("yyyyMMdd");
            string strCurrentTime_Detail = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string strFileDir = strLogfilePath + "\\" + strCaseProjectName + "\\" + strCurrentTime_YMD;

            if (CreateDirectoryIfNotExists(strFileDir + "\\PASS\\") == false)
            {
                UIHandleHelper.ShowRunLog("Failed to create the path of the local log pass folder.", true);
            }

            if (CreateDirectoryIfNotExists(strFileDir + "\\FAIL\\") == false)
            {
                UIHandleHelper.ShowRunLog("Failed to create the path of the local log fail folder.", true);
            }

            if (bTestResult)
            {
                if (ExportToHTML(dgv, strFileDir + "\\PASS\\" + strBarcode + "_" + strCurrentTime_Detail + strDestinationFileSuffix, bTestResult, strBarcode, totalTime) == false)
                {
                    UIHandleHelper.ShowRunLog($"Failed to export DataGridView to html ({strDestinationFileSuffix}) to pass folder.", true);
                    bResult = false;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Export DataGridView to html ({strDestinationFileSuffix})to pass folder successfully.");
                    bResult = true;
                }
            }
            else
            {
                if (ExportToHTML(dgv, strFileDir + "\\FAIL\\" + strBarcode + "_" + strCurrentTime_Detail + strDestinationFileSuffix, bTestResult, strBarcode, totalTime) == false)
                {
                    UIHandleHelper.ShowRunLog($"Failed to export DataGridView to html ({strDestinationFileSuffix}) to fail folder.", true);
                    bResult = false;
                }
                else
                {
                    UIHandleHelper.ShowRunLog($"Export DataGridView to html ({strDestinationFileSuffix})to fail folder successfully.");
                    bResult = true;
                }
            }


            return bResult;
        }


        public static bool CreateDirectoryIfNotExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowRunLog($"An error occurred creating a directory: {ex}", true);
                    return false;
                }
            }

            return true; // 目录已存在
        }

        public static bool CopyFile(string sourcePath, string destinationPath)
        {
            try
            {
                File.Copy(sourcePath, destinationPath, true); // true 表示如果目标文件已存在，则覆盖它
                return true;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"An error occurred while copying the file: {ex}", true);
                return false;
            }
        }


        private static bool ExportToHTML(DataGridView dgv, string filePath, bool bTestResult, string strBarcode, double totalTime)
        {
            try
            {
                // 检查是否需要跨线程调用
                if (dgv.InvokeRequired)
                {
                    return (bool)dgv.Invoke(new Func<DataGridView, string, bool, string, double, bool>(ExportToHTML),
                        new object[] { dgv, filePath, bTestResult, strBarcode, totalTime });
                }

                // 收集Time数据和项目名称
                var timeData = new List<(string Name, double Value)>();
                double maxTimeValue = 0.0;
                double totalTestTime = 0.0;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        var timeCell = row.Cells["Time"]; // 假设列名是 "Time"
                        var nameCell = row.Cells["item"]; // 假设第一列是项目名称

                        if (timeCell != null && timeCell.Value != null && nameCell != null && nameCell.Value != null)
                        {
                            string timeStr = timeCell.Value.ToString();
                            string name = nameCell.Value.ToString();

                            if (timeStr.EndsWith(" s"))
                            {
                                if (double.TryParse(timeStr.Replace(" s", ""), out double timeValue))
                                {
                                    timeData.Add((Name: name, Value: timeValue));
                                    totalTestTime += timeValue;

                                    if (timeValue > maxTimeValue)
                                    {
                                        maxTimeValue = timeValue;
                                    }
                                }
                            }
                        }
                    }
                }

                // 计算"其他"时间 = 总时间 - 各项目时间总和
                double otherTime = totalTime - totalTestTime;
                if (otherTime > 0)
                {
                    timeData.Add(("其他", otherTime));
                    // 更新最大时间值（如果需要）
                    if (otherTime > maxTimeValue)
                    {
                        maxTimeValue = otherTime;
                    }
                }

                // 以下代码在UI线程执行
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html>");
                sb.AppendLine("<head>");
                sb.AppendLine("    <meta charset=\"UTF-8\">"); // 添加meta标签声明编码
                sb.AppendLine("<style>");
                sb.AppendLine("table { border-collapse: collapse; width: 100%; }");
                sb.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
                sb.AppendLine("th { background-color: #f2f2f2; }");
                sb.AppendLine(".fail { background-color: #ED1C24; }"); // 失败单元格的红色背景样式
                sb.AppendLine(".info-header { margin-bottom: 20px; font-family: Arial; font-size: 20px; }");
                sb.AppendLine(".result { font-weight: bold; font-size: 25px; }"); // 加大字体
                sb.AppendLine(".pass { color: #4CAF50; }"); // PASS 绿色
                sb.AppendLine(".fail-result { color: #FF0000; }"); // FAIL 红色

                // 添加直条图样式
                sb.AppendLine(".chart-container { margin-top: 30px; }");
                sb.AppendLine(".chart-title { font-weight: bold; margin-bottom: 10px; }");
                sb.AppendLine(".chart-bar-container { margin-bottom: 15px; }");
                sb.AppendLine(".chart-bar-label { margin-right: 10px; width: 200px; display: inline-block; }");
                sb.AppendLine(".chart-bar { height: 20px; background-color: #4CAF50; display: inline-block; vertical-align: middle; }");
                sb.AppendLine(".chart-bar-value { margin-left: 10px; display: inline-block; }");
                sb.AppendLine("</style>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");

                // 添加首行信息（字体加大，PASS/FAIL 颜色区分）
                sb.AppendLine("<div class=\"info-header\">");
                sb.AppendLine($"<span class=\"result {(bTestResult ? "pass" : "fail-result")}\">Result: {(bTestResult ? "PASS" : "FAIL")}</span><br/>");
                sb.AppendLine($"Mac: {strBarcode}<br/>");
                sb.AppendLine($"ItemsTestTime: {totalTestTime:F2} s<br/>"); // 显示总测试时间，保留两位小数
                sb.AppendLine($"TotalTime: {totalTime:F2} s<br/>"); // 显示总时间
                sb.AppendLine($"Data: {DateTime.Now.ToString("yyyy/MM/dd HH:mm")}<br/>");
                sb.AppendLine("</div>");

                sb.AppendLine("<table>");

                // 添加表头
                sb.AppendLine("<tr>");
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    sb.AppendLine($"<th>{column.HeaderText}</th>");
                }
                sb.AppendLine("</tr>");

                // 添加数据行
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        sb.AppendLine("<tr>");
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            string cellValue = cell.Value?.ToString() ?? "";
                            string cellClass = "";

                            // 检查是否是"结果"列且值为"Fail"
                            if (cell.OwningColumn.HeaderText == "结果" && cellValue.Equals("Fail", StringComparison.OrdinalIgnoreCase))
                            {
                                cellClass = " class=\"fail\"";
                            }

                            sb.AppendLine($"<td{cellClass}>{cellValue}</td>");
                        }
                        sb.AppendLine("</tr>");
                    }
                }

                sb.AppendLine("</table>");

                // 添加直条图部分（已添加序号）
                sb.AppendLine("<div class=\"chart-container\">");
                sb.AppendLine("<div class=\"chart-title\">各项目测试时间分布 (单位:秒)</div>");

                int itemIndex = 1; // 序号计数器
                foreach (var item in timeData)
                {
                    // 计算直条长度（基于最大值的比例）
                    int barWidth = maxTimeValue > 0 ? (int)((item.Value / maxTimeValue) * 500) : 0;

                    sb.AppendLine("<div class=\"chart-bar-container\">");
                    sb.AppendLine($"<span class=\"chart-bar-label\">{itemIndex}. {item.Name}</span>"); // 添加序号
                    sb.AppendLine($"<div class=\"chart-bar\" style=\"width: {barWidth}px;\"></div>");
                    sb.AppendLine($"<span class=\"chart-bar-value\">{item.Value:F2} s</span>");
                    sb.AppendLine("</div>");

                    itemIndex++; // 序号递增
                }

                sb.AppendLine("</div>"); // 关闭chart-container

                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                // 文件写入不需要委托，因为不涉及UI控件
                // 替换原来的：File.WriteAllText(filePath, sb.ToString());
                // 改为使用UTF-8编码（无BOM）
                File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(false));
                return true;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"An error occurred while export to HTML: {ex}", true);
                return false;
            }
        }




    }
}
