using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Liteon_TestProgram.Base.Class_Variable;

namespace Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V2
{
    internal class SfcFile
    {
        public (List<RFTestResult>, bool)GetRFDataFromLog(string str_LogPathAndName)
        {
            ParseRFTestResults parseRFTestResults = new ParseRFTestResults();
            var bResult = parseRFTestResults.ParseWifiTestResults(str_LogPathAndName);
            return (bResult.Item1, bResult.Item2);
        }

        public void CreateSfcFile(bool bNoSaveSFC, bool bTestResult, string str_LogPathAndName, string str_Barcode1, string str_Barcode2, string str_CaseName, 
                                            string str_CaseTestProgramVersion, string str_SfcFilePath, string str_ErrorCode)
        {
            if (bNoSaveSFC)
            {
                List<RFTestResult> rFTestResults = new List<RFTestResult>();
                ParseRFTestResults parseRFTestResults = new ParseRFTestResults();

                if (string.IsNullOrEmpty(str_LogPathAndName) || string.IsNullOrWhiteSpace(str_LogPathAndName) 
                    || File.Exists(str_LogPathAndName) == false)
                {
                    PrintAllRFTestResults(null, bTestResult, str_LogPathAndName, str_Barcode1, str_Barcode2, str_CaseName,
                                                   str_CaseTestProgramVersion, str_SfcFilePath, str_ErrorCode);
                }
                else
                {
                    var bResult = parseRFTestResults.ParseWifiTestResults(str_LogPathAndName);
                    PrintAllRFTestResults(bResult.Item1, bTestResult, str_LogPathAndName, str_Barcode1, str_Barcode2, str_CaseName,
                                                   str_CaseTestProgramVersion, str_SfcFilePath, str_ErrorCode);
                }
                
            }
            else
            {
                UIHandleHelper.ShowRunLog("No need to create SFC file;");
            }

        }

        protected void PrintAllRFTestResults(List<RFTestResult> abc, bool bTestResult,
                                                       string str_LogPathAndName, string str_Barcode1, string str_Barcode2, string str_CaseName,
                                                       string str_CaseTestProgramVersion, string str_SfcFilePath, string str_ErrorCode)
        {
            string str_FileSuffix = "";
            if (bTestResult)
            {
                str_FileSuffix = "_PASS_SFC.txt";
            }
            else
            {
                str_FileSuffix = "_FAIL_SFC.txt";
            }

            string str_TempSfcFileName = $"{str_Barcode1}_{str_Barcode2}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}{str_FileSuffix}";

            if (string.IsNullOrEmpty(str_Barcode2) || string.IsNullOrWhiteSpace(str_Barcode2))
            {
                str_TempSfcFileName = $"{str_Barcode1}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}{str_FileSuffix}";
            }

            if (File.Exists($"{str_SfcFilePath}\\{str_TempSfcFileName}"))
            {
                File.Delete($"{str_SfcFilePath}\\{str_TempSfcFileName}");
            }



            try
            {
                if (!Directory.Exists(FileProcessHelper.GetCurrentExeDirectory() + "\\SFCFile"))
                {
                    Directory.CreateDirectory(FileProcessHelper.GetCurrentExeDirectory() + "\\SFCFile");
                }


                string str_TempSfcFilePathAndName = $"{FileProcessHelper.GetCurrentExeDirectory()}\\SFCFile\\{str_TempSfcFileName}";
                if (File.Exists(str_TempSfcFilePathAndName))
                {
                    File.Delete(str_TempSfcFilePathAndName);
                    Thread.Sleep(100);
                }

                StreamWriter sw = new StreamWriter(str_TempSfcFilePathAndName, true);

                if (bTestResult)
                {
                    sw.WriteLine("P");

                    sw.WriteLine(str_Barcode1);
                    if (string.IsNullOrEmpty(str_Barcode2) == false && string.IsNullOrWhiteSpace(str_Barcode2) == false)
                    {
                        sw.WriteLine(str_Barcode2);
                    }
                    
                    sw.WriteLine(str_CaseName);
                    sw.WriteLine(str_CaseTestProgramVersion);

                }
                else
                {
                    sw.WriteLine("F");

                    sw.WriteLine(str_Barcode1);
                    sw.WriteLine(str_ErrorCode);

                    if (string.IsNullOrEmpty(str_Barcode2) == false && string.IsNullOrWhiteSpace(str_Barcode2) == false)
                    {
                        sw.WriteLine(str_Barcode2);
                    }

                    sw.WriteLine(str_CaseName);
                    sw.WriteLine(str_CaseTestProgramVersion);
                }

                if (string.IsNullOrEmpty(str_LogPathAndName) == false && string.IsNullOrWhiteSpace(str_LogPathAndName) == false)
                {
                    foreach (var result in abc)
                    {

                        string str_Item = "";

                        if (result.Bandwidth == null && result.Antenna == null)
                        {
                            str_Item = $"{result.Frequency}_{result.DataRate}";
                        }
                        else
                        {
                            str_Item = $"{result.Frequency}_{result.DataRate}_{result.Bandwidth}_{result.Antenna}";
                        }

                        if (result.Metrics_Tx != null)
                        {
                            string str_Temp = str_Item + $"#EVM:{result.Metrics_Tx.EVM}#{result.Metrics_Tx.EVM_Result}";
                            sw.WriteLine(str_Temp);

                            str_Temp = str_Item + $"#Power:{result.Metrics_Tx.Power}#{result.Metrics_Tx.Power_Result}";
                            sw.WriteLine(str_Temp);

                            str_Temp = str_Item + $"#FreqError:{result.Metrics_Tx.FreqError}#{result.Metrics_Tx.FreqError_Result}";
                            sw.WriteLine(str_Temp);

                            // 可以继续添加其他TX相关的输出
                        }

                        if (result.Metrics_Rx != null)
                        {
                            string str_Temp = str_Item + $"#PER:{result.Metrics_Rx.PER}#{result.Metrics_Rx.PER_Result}";
                            sw.WriteLine(str_Temp);

                            // 可以继续添加其他RX相关的输出
                        }

                        if (result.Metrics_BtTx != null)
                        {
                            string str_Temp = str_Item + $"#Power:{result.Metrics_BtTx.Power}#{result.Metrics_BtTx.Power_Result}";
                            sw.WriteLine(str_Temp);

                            // 可以继续添加其他TX相关的输出
                        }

                        if (result.Metrics_BtRx != null)
                        {
                            string str_Temp = str_Item + $"#PER:{result.Metrics_BtRx.PER}#{result.Metrics_BtRx.PER_Result}";
                            sw.WriteLine(str_Temp);

                            // 可以继续添加其他RX相关的输出
                        }

                    }
                }

                sw.Close();
                Thread.Sleep(100);

                if (!Directory.Exists(str_SfcFilePath))
                {
                    Directory.CreateDirectory(str_SfcFilePath);
                }

                File.Copy(str_TempSfcFilePathAndName, $"{str_SfcFilePath}\\{str_TempSfcFileName}", true); // true 表示如果目标文件已存在，则覆盖它
                Thread.Sleep(100);
                UIHandleHelper.ShowRunLog("Create SFC file ok.");
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Create SFC flie error: \r\n {ex}");
            }



        
        }

    }
}
