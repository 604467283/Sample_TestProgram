using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.TestFunc.iTest.LogDataCollection_V1
{
    internal class ParseRFTestResults
    {
        public (List<RFTestResult>, bool) ParseWifiTestResults(string filePath)
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
                    //标题提取
                    if (line.Contains("WT_VERIFY_"))
                    {
                        // 初始化新的测试结果对象
                        currentResult = new RFTestResult();

                        //用于记录各个部分的测试时间
                        bWifiTx = false;
                        bWifiRx = false;
                        bBtTx = false;
                        bBtRx = false;

                        #region WIFI


                        if (line.Contains("WT_VERIFY_TX_"))
                        {
                            currentResult.Metrics_Tx = new RFTestResult.WifiMetrics_Tx();
                            bWifiTx = true;
                        }

                        if (line.Contains("WT_VERIFY_RX_"))
                        {
                            currentResult.Metrics_Rx = new RFTestResult.WifiMetrics_Rx();
                            bWifiRx = true;
                        }

                        //标题提取
                        if (line.Contains("WT_VERIFY_TX_") || line.Contains("WT_VERIFY_RX_"))
                        {
                            string str_temp = "";
                            string str_FindMark = "WT_VERIFY_";
                            str_temp = line.Substring(line.IndexOf(str_FindMark) + str_FindMark.Length);
                            string[] arr_string = str_temp.Split(' ');

                            int iCount = 0;
                            foreach (var item in arr_string)
                            {
                                string strTemp = "";
                                strTemp = item;

                                if (strTemp.Contains("("))
                                {
                                    strTemp = strTemp.Substring(0, strTemp.IndexOf("("));   //例如，5190(38)， 去除(38)
                                }

                                if (int.TryParse(strTemp, out _))
                                {
                                    break; // 发现纯数字字符串，立即返回
                                }
                                iCount++;
                            }

                            currentResult.Frequency = arr_string[iCount];
                            currentResult.DataRate = arr_string[iCount + 1];
                            currentResult.Antenna = arr_string[iCount + 2];

                        }

                        #endregion

                        #region BT

                        //Bt
                        if (line.Contains("WT_VERIFY_BT_TX"))
                        {
                            currentResult.Metrics_BtTx = new RFTestResult.BtMetrics_Tx();
                            bBtTx = true;
                        }

                        if (line.Contains("WT_VERIFY_BT_RX"))
                        {
                            currentResult.Metrics_BtRx = new RFTestResult.BtMetrics_Rx();
                            bBtRx = true;
                        }

                        //提取BT的标题
                        if (line.Contains("WT_VERIFY_BT_TX") || line.Contains("WT_VERIFY_BT_RX"))
                        {
                            string str_temp = "";
                            string str_FindMark = "WT_VERIFY_BT_";
                            str_temp = line.Substring(line.IndexOf(str_FindMark) + str_FindMark.Length);
                            string[] arr_string = str_temp.Split(' ');

                            int iCount = 0;
                            foreach (var item in arr_string)
                            {
                                string strTemp = "";
                                strTemp = item;

                                if (strTemp.Contains("("))
                                {
                                    strTemp = strTemp.Substring(0, strTemp.IndexOf("("));   //例如，5190(38)， 去除(38)
                                }

                                if (int.TryParse(strTemp, out _))
                                {
                                    break; // 发现纯数字字符串，立即返回
                                }
                                iCount++;
                            }

                            currentResult.Frequency = arr_string[iCount];
                            currentResult.DataRate = arr_string[0].Replace(".", "") + "_" + arr_string[iCount + 1];

                        }

                        #endregion

                        results.Add(currentResult);
                    }
                    else if (line.Contains("Test Time Summary:")) //测试所有项目结束
                    {
                        break;
                    }
                    else if (line.Contains("Total Result:")) //测试结果
                    {
                        if (line.Contains("Total Result: PASS"))
                        {
                            bSuccess = true;
                        }
                        else
                        {
                            bSuccess = false;
                        }
                    }
                    else if (currentResult != null)
                    {
                        string str_TestTime = "Test Time: ";
                        string[] Arrary_WifiMark = { "Power", "EVM", "MaskErr", "FreqErr",    //Tx
                                                         "@",  "Fail:",  //Rx -> 发送的power， 接收到的百分比
                                                         "Test Time:",  //该项目测试结束的标志
                                                        };

                        string[] Arrary_BtMark = { "Power", "Init Freq Err",     //TX
                                                           "@", " Fail:",   //Rx -> 发送的power， 接收到的百分比
                                                           "Test Time:",  //该项目测试结束的标志
                                                          };
                        #region 测试时间

                        //测试时间
                        if (line.Contains(str_TestTime))
                        {
                            if (bWifiTx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_Tx.TestTime = str_temp[2];
                            }

                            if (bWifiRx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_Rx.TestTime = str_temp[2];
                            }

                            if (bBtTx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_BtTx.TestTime = str_temp[2];
                            }

                            if (bBtRx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_BtRx.TestTime = str_temp[2];
                            }

                        }


                        #endregion

                        #region Wifi_TX_RX

                        if (bWifiTx || bWifiRx)
                        {
                            int iCount = 0;
                            foreach (var item in Arrary_WifiMark)
                            {
                                if (line.Contains(item) && line.Contains("Power Analysis Fail") ==false)
                                {
                                    //POWER_AVG_DBM
                                    if (iCount == 0)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);
                                        string[] str_NewTemp = FindAndKeepAfterColon(":", str_temp);

                                        currentResult.Metrics_Tx.Power = str_NewTemp[1];
                                        currentResult.Metrics_Tx.Power_Range = $"({str_NewTemp[4].Replace("(", "")}/{str_NewTemp[6].Replace(")", "")})";
                                        currentResult.Metrics_Tx.Power_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }

                                    //EVM_DB_AVG
                                    if (iCount == 1)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);
                                        string[] str_NewTemp = FindAndKeepAfterColon(":", str_temp);

                                        currentResult.Metrics_Tx.EVM = str_NewTemp[1];
                                        currentResult.Metrics_Tx.EVM_Range = $"({str_NewTemp[4].Replace("(", "")}/{str_NewTemp[6].Replace(")", "")})";
                                        currentResult.Metrics_Tx.EVM_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }

                                    //MaskErr
                                    if (iCount == 2)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);
                                        string[] str_NewTemp = FindAndKeepAfterColon(":", str_temp);

                                        currentResult.Metrics_Tx.MaskErr = str_NewTemp[1];
                                        currentResult.Metrics_Tx.MaskErr_Range = $"({str_NewTemp[4].Replace("(", "")}/{str_NewTemp[6].Replace(")", "")})";
                                        currentResult.Metrics_Tx.MaskErr_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }

                                    //FREQ_ERROR_AVG
                                    if (iCount == 3)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);
                                        string[] str_NewTemp = FindAndKeepAfterColon(":", str_temp);

                                        currentResult.Metrics_Tx.FreqError = str_NewTemp[1];
                                        currentResult.Metrics_Tx.FreqError_Range = $"({str_NewTemp[4].Replace("(", "")}/{str_NewTemp[6].Replace(")", "")})";
                                        currentResult.Metrics_Tx.FreqError_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }




                                    //===============================WifiRX===================
                                    //===============================WifiRX===================
                                    //===============================WifiRX===================

                                    //Rx -> 发送的power
                                    if (iCount == 4)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);
                                        string[] str_NewTemp = FindAndKeepAfterColon("@", str_temp);

                                        currentResult.Metrics_Rx.PER_Power = str_NewTemp[1].Replace("dBm", "");
                                    }

                                    //PER 接收到的百分比
                                    if (iCount == 5)
                                    {


                                        string str_newLine = "";
                                        if (line.IndexOf("Fail:") != -1)
                                        {
                                            str_newLine = line.Replace("Fail:", "Fail: ");
                                        }
                                        string[] str_temp = str_newLine.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);
                                        string[] str_NewTemp = FindAndKeepAfterColon("Fail:", str_temp);


                                        currentResult.Metrics_Rx.PER_Result = line.Contains("X") ? "FAIL" : "PASS";


                                        currentResult.Metrics_Rx.PER = str_NewTemp[1].Replace("Pass:", "").Replace("%", "");
                                        currentResult.Metrics_Rx.PER_Range = $"({str_NewTemp[3].Replace("(", "")}/{str_NewTemp[5].Replace(")", "")})";


                                    }

                                    if (iCount == 6)
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
                                    //POWER
                                    if (iCount == 0)
                                    {
                                        if (line.Replace(" ", "").Contains("Power:"))
                                        {
                                            string[] str_temp = line.Split(' ');
                                            str_temp = RemoveEmptyStrings(str_temp);
                                            string[] str_NewTemp = FindAndKeepAfterColon(":", str_temp);

                                            currentResult.Metrics_BtTx.Power = str_NewTemp[1];
                                            currentResult.Metrics_BtTx.Power_Range = $"({str_NewTemp[4].Replace("(", "")}/{str_NewTemp[6].Replace(")", "")})";
                                            currentResult.Metrics_BtTx.Power_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                        }
                                    }

                                    //INITIAL_FREQ_OFFSET
                                    if (iCount == 1)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);
                                        string[] str_NewTemp = FindAndKeepAfterColon(":", str_temp);

                                        currentResult.Metrics_BtTx.InitFreqErr = str_NewTemp[1];
                                        currentResult.Metrics_BtTx.InitFreqErr_Range = $"({str_NewTemp[4].Replace("(", "")}/{str_NewTemp[6].Replace(")", "")})";
                                        currentResult.Metrics_BtTx.InitFreqErr_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }





                                    //RX_POWER_LEVEL
                                    if (iCount == 2)
                                    {

                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);
                                        string[] str_NewTemp = FindAndKeepAfterColon("@", str_temp);

                                        currentResult.Metrics_BtRx.PER_Power = str_NewTemp[1].Replace("dBm", "");
                                    }

                                    //PER
                                    if (iCount == 3)
                                    {
                                        string[] str_temp = line.Replace("(", "").Replace(")", "").Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);
                                        string[] str_NewTemp = FindAndKeepAfterColon("Fail:", str_temp);

                                        currentResult.Metrics_BtRx.PER = str_NewTemp[1].Replace("Pass:", "").Replace("%", "");
                                        currentResult.Metrics_BtRx.PER_Range = $"({str_NewTemp[2].Replace("(", "")}/{str_NewTemp[4].Replace(")", "")})";
                                        currentResult.Metrics_BtRx.PER_Result = line.Contains("X") ? "FAIL" : "PASS";
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

        public static string[] FindAndKeepAfterColon(string strSplitSymbol, string[] str_temp)
        {
            List<string> result = new List<string>();
            bool foundColon = false;

            foreach (var item in str_temp)
            {
                if (item.Contains(strSplitSymbol))
                {
                    foundColon = true;
                    result.Add(item); // 保留含有":"的元素
                }
                else if (foundColon)
                {
                    result.Add(item); // 保留":"之后的元素
                }
            }

            return result.ToArray();
        }


    }
}
