using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.TestFunc.ACE.LogDataCollection_V1
{
    internal class ParseRFTestResults
    {
        public (List<RFTestResult>, bool )ParseWifiTestResults(string filePath)
        {
            bool bSuccess = false;
            var results = new List<RFTestResult>();
            RFTestResult currentResult = null;

            bool bWifiTx = false;
            bool bWifiRx = false;
            bool bBtTx = false;
            bool bBtRx = false;


            foreach (var line in File.ReadAllLines(filePath))
            {
                try
                {
                    //All 标题提取
                    if (line.Contains("Frequency: ") && line.Contains("Power:"))
                    {
                        // 初始化新的测试结果对象
                        currentResult = new RFTestResult();

                        //用于记录各个部分的测试时间
                        bWifiTx = false;
                        bWifiRx = false;
                        bBtTx = false;
                        bBtRx = false;

                        #region WIFI


                        if (line.Contains("Bandwidth:") && line.Contains("Tx Power:"))
                        {
                            currentResult.Metrics_Tx = new RFTestResult.WifiMetrics_Tx();
                            bWifiTx = true;
                        }

                        if (line.Contains("Bandwidth:") && line.Contains("Rx Power:"))
                        {
                            currentResult.Metrics_Rx = new RFTestResult.WifiMetrics_Rx();
                            bWifiRx = true;
                        }

                        //Wifi Tx Rx标题提取
                        if (line.Contains("Bandwidth:") && line.Contains("Power:"))
                        {

                            string[] str_temp = line.Split(' ');
                            str_temp = RemoveEmptyStrings(str_temp);
                            currentResult.Frequency = str_temp[1].Replace(",", "");
                            currentResult.DataRate = str_temp[4].Replace(",", "");
                            currentResult.Bandwidth = str_temp[6].Replace(",", "");
                            currentResult.Antenna = str_temp[8].Replace(",", "");
                            currentResult.Power_ExpectedOrUsed = str_temp[11].Replace(",", "");

                        }

                        #endregion

                        #region BT

                        //Bt tx
                        if ((line.Contains("Packet Type:") && line.Contains("Tx Power:")))
                        {
                            currentResult.Metrics_BtTx = new RFTestResult.BtMetrics_Tx();
                            bBtTx = true;
                        }

                        //BT Rx
                        if ((line.Contains("Packet Type:") && line.Contains("Rx Power:")))
                        {
                            currentResult.Metrics_BtRx = new RFTestResult.BtMetrics_Rx();
                            bBtRx = true;
                        }

                        //BT TxRx
                        if (line.Contains("Packet Type:") && line.Contains("Power:"))
                        {

                            string[] str_temp = line.Split(',');
                            str_temp = RemoveEmptyStrings(str_temp);

                            currentResult.Frequency = str_temp[0].Split(':')[1].Replace(" ", "");
                            currentResult.DataRate = str_temp[1].Split(':')[1].Replace(" ", "");
                            currentResult.Power_ExpectedOrUsed = str_temp[2].Split(':')[1].Replace(" ", "");
                        }

                        #endregion

                        results.Add(currentResult);
                    }
                    else if (line.Contains("* P A S S *") || line.Contains("* F A I L *"))
                    {
                        if (line.Contains("* P A S S *"))
                        {
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }

                        break;
                    }
                    else if (currentResult != null)
                    {
                        string str_TestTime = "Test time:";
                        string[] Arrary_WifiMark = { "Power", "EVM", "Freq Error", "Sym Clk Error", "LO Leakage", "Spectrum Mask",
                                                         "PER",
                                                         "Test time:",
                                                        };

                        string[] Arrary_BtMark = { "Ini Freq Error", "Power",
                                                           "PER", "BER",
                                                           "Test time:",
                                                          };
                        #region 测试时间

                        //测试时间
                        if (line.Contains(str_TestTime))
                        {
                            if (bWifiTx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_Tx.TestTime = str_temp[2].Replace("sec", "");
                            }

                            if (bWifiRx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_Rx.TestTime = str_temp[2].Replace("sec", "");
                            }

                            if (bBtTx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_BtTx.TestTime = str_temp[2].Replace("sec", "");
                            }

                            if (bBtRx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_BtRx.TestTime = str_temp[2].Replace("sec", "");
                            }

                        }


                        #endregion

                        #region Wifi_TX_RX

                        if (bWifiTx || bWifiRx)
                        {
                            int iCount = 0;
                            foreach (var item in Arrary_WifiMark)
                            {

                                if (line.Contains(item))
                                {
                                    //Power
                                    if (iCount == 0)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.Power = str_temp[1];
                                        currentResult.Metrics_Tx.Power_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.Power_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }


                                    //EVM
                                    if (iCount == 1)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.EVM = str_temp[1];
                                        currentResult.Metrics_Tx.EVM_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.EVM_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }


                                    //Freq Error
                                    if (iCount == 2)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.FreqError = str_temp[2];
                                        currentResult.Metrics_Tx.FreqError_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.FreqError_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }


                                    //Sym Clk Error
                                    if (iCount == 3)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.SymClkError = str_temp[3];
                                        currentResult.Metrics_Tx.SymClkError_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.SymClkError_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }

                                    //LO Leakage
                                    if (iCount == 4)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.LOLeakage = str_temp[2];
                                        currentResult.Metrics_Tx.LOLeakage_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.LOLeakage_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }

                                    //Spectrum Mask
                                    if (iCount == 5)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.SpectrumMask = str_temp[2];
                                        currentResult.Metrics_Tx.SpectrumMask_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.SpectrumMask_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }




                                    //===============================WifiRX===================
                                    //===============================WifiRX===================
                                    //===============================WifiRX===================

                                    //PER
                                    if (iCount == 6)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Rx.PER = str_temp[1];
                                        currentResult.Metrics_Rx.PER_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Rx.PER_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }



                                    if (iCount == 7)
                                    {
                                        bWifiTx = false;
                                        bWifiRx = false;
                                    }


                                }

                                iCount++;
                            }
                        }

                        #endregion

                        #region BT_TX_RX

                        if (bBtTx || bBtRx)
                        {
                            int iCount = 0;
                            foreach (var item in Arrary_BtMark)
                            {
                                if (line.Contains(item))
                                {
                                    //Ini Freq Error
                                    if (iCount == 0)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_BtTx.FreqOffset = str_temp[3];
                                        currentResult.Metrics_BtTx.FreqOffset_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_BtTx.FreqOffset_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }


                                    //Power
                                    if (iCount == 1)
                                    {
                                        if (line.Contains("dBm"))
                                        {
                                            string[] str_temp = line.Split(' ');
                                            str_temp = RemoveEmptyStrings(str_temp);

                                            currentResult.Metrics_BtTx.Power = str_temp[1];
                                            currentResult.Metrics_BtTx.Power_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                            currentResult.Metrics_BtTx.Power_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                        }
                                    }




                                    //PER
                                    if (iCount == 2 || iCount == 3)
                                    {
                                        if (line.Contains("PER "))
                                        {
                                            string[] str_temp = line.Split(' ');
                                            str_temp = RemoveEmptyStrings(str_temp);

                                            currentResult.Metrics_BtRx.PER = str_temp[1];
                                            currentResult.Metrics_BtRx.PER_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                            currentResult.Metrics_BtRx.PER_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                        }
                                        else if (line.Contains("BER "))
                                        {
                                            string[] str_temp = line.Split(' ');
                                            str_temp = RemoveEmptyStrings(str_temp);

                                            currentResult.Metrics_BtRx.PER = str_temp[1];
                                            currentResult.Metrics_BtRx.PER_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                            currentResult.Metrics_BtRx.PER_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                        }
                                    }


                                    if (iCount == 4)
                                    {
                                        bBtTx = false;
                                        bBtRx = false;
                                    }
                                }

                                iCount++;
                            }
                        }


                        #endregion


                    }
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowCollectionLogsRFData($"{Path.GetFileName(filePath)}, Parse data error: \r\n line:{line} \r\n {ex}", true);
                    throw;
                }
            }


            return (results, bSuccess);
        }

        public string[] RemoveEmptyStrings(string[] input)
        {
            return input.Where(s => !string.IsNullOrEmpty(s)).ToArray();
        }

        public string Array_SpliceBetweenElements(string[] input, string strA, string strB)
        {
            //string[] str_temp = { "Hello", "(", "World", ")", "Test" };
            //例子。输出(World)

            // 查找包含 '(' 和 ')' 的元素索引
            int leftIndex = -1, rightIndex = -1;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Contains(strA))
                {
                    leftIndex = i;
                }
                if (input[i].Contains(strB))
                {
                    rightIndex = i;
                    break; // 假设只有一对括号
                }
            }

            // 检查是否找到了括号
            if (leftIndex != -1 && rightIndex != -1)
            {
                // 拼接 leftIndex 到 rightIndex 之间的字符串（包括两端）
                string result = string.Join("", input.Skip(leftIndex).Take(rightIndex - leftIndex + 1));
                return result;
            }
            else
            {
                return "null";
            }
        }



    }
}
