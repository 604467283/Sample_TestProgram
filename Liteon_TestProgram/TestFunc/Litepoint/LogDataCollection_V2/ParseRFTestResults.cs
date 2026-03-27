using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V2
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

                
                    if ( (line.Contains("TEST_VERIFY ") ||  line.Contains("SWEEP_VERIFY ") || line.Contains("TX_") || line.Contains("RX_")  )
                        && line.Contains(" _____")
                        && line.Contains("CALIBRATION") == false 
                        && line.Contains("SET") == false
                        )
                    {
                        // 初始化新的测试结果对象
                        currentResult = new RFTestResult();

                        //用于记录各个部分的测试时间
                        bWifiTx = false;
                        bWifiRx = false;
                        bBtTx = false;
                        bBtRx = false;

                        #region WIFI


                        if (line.Contains("EVM ") || line.Contains("POWER "))
                        {
                            currentResult.Metrics_Tx = new RFTestResult.WifiMetrics_Tx();
                            bWifiTx = true;
                        }

                        if (line.Contains("PER ") || line.Contains("SENS "))
                        {
                            currentResult.Metrics_Rx = new RFTestResult.WifiMetrics_Rx();
                            bWifiRx = true;
                        }

                        //标题提取
                        if (line.Contains("EVM ") || line.Contains("PER ") || line.Contains("POWER ") || line.Contains("SENS "))
                        {
                            string str_temp = "";
                            string str_FindMark1 = "TEST_VERIFY ";
                            string str_FindMark2 = " _____";
                            int iFind1 = line.IndexOf(str_FindMark1) + str_FindMark1.Length;
                            int iFind2 = line.IndexOf(str_FindMark2);
                            str_temp = line.Substring(iFind1, iFind2 - iFind1);
                            string[] arr_string = str_temp.Split(' ');

                            int iCount = 0;
                            foreach (var item in arr_string)
                            {
                                if (int.TryParse(item, out _))
                                {
                                    break; // 发现纯数字字符串，立即返回
                                }
                                iCount++;
                            }

                            currentResult.Frequency = arr_string[iCount];
                            currentResult.DataRate = arr_string[iCount + 1];
                            currentResult.Bandwidth = arr_string[iCount + 2] + "_" + arr_string[iCount + 3];
                            currentResult.Antenna = arr_string[iCount + 4];
                        }

                        #endregion

                        #region BT

                        //Bt
                        if (line.Contains("TX_") && line.Contains(" __________") && line.Contains("VERIFY") == false && line.Contains("CALIBRATION") == false && line.Contains("SET") == false)
                        {
                            currentResult.Metrics_BtTx = new RFTestResult.BtMetrics_Tx();
                            bBtTx = true;
                        }

                        if (line.Contains("RX_") && line.Contains(" __________") && line.Contains("VERIFY") == false && line.Contains("CALIBRATION") == false && line.Contains("SET") == false)
                        {
                            currentResult.Metrics_BtRx = new RFTestResult.BtMetrics_Rx();
                            bBtRx = true;
                        }


                        if ((line.Contains("TX_") && line.Contains(" __________") && line.Contains("VERIFY") == false && line.Contains("CALIBRATION") == false && line.Contains("SET") == false) 
                            || (line.Contains("RX_") && line.Contains(" __________") && line.Contains("VERIFY") == false && line.Contains("CALIBRATION") == false && line.Contains("SET") == false)
                            )
                        {
                            string str_temp = "";
                            string str_FindMark1 = "X_ ";  //TX_  RX_
                            string str_FindMark2 = " ______";
                            int iFind1 = line.IndexOf(str_FindMark1) + str_FindMark1.Length;
                            int iFind2 = line.IndexOf(str_FindMark2);
                            str_temp = line.Substring(iFind1, iFind2 - iFind1);
                            string[] arr_string = str_temp.Split(' ');

                            int iCount = 0;
                            foreach (var item in arr_string)
                            {
                                if (int.TryParse(item, out _))
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
                    else if (line.Contains("*  P A S S  *") || line.Contains("*  F A I L  *"))
                    {
                        if (line.Contains("*  P A S S  *"))
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
                        string str_TestTime = "Test Time =";
                        string[] Arrary_WifiMark = { "EVM_DB_AVG", "FREQ_ERROR_AVG", "SYMBOL_CL", "POWER", "LO_LEAKAGE_",
                                                         "RX_POWER_DBM", "SENS_POWER_LEVEL", "PER", "RSSI_RX",
                                                         "[Info] Function completed",
                                                        };

                        string[] Arrary_BtMark = { "FREQ_", "POWER_AVERAGE_DBM",
                                                           "RX_POWER_LEVEL", "PER", "BER",
                                                           "[Info] Function completed",
                                                          };

                        //某些项目寻找关键字的补充
                        string[] Arrary_WifiTx_PowerMark = {/*  "POWER_RMS_AVG_VSA" ,*/"POWER_AVG_DBM" }; 
                        string[] Arrary_BtTx_FreqOffsetMark = { "INITIAL_FREQ_OFFSET", "FREQ_DEVIATION" };


                        #region 测试时间

                        //测试时间
                        if (line.Contains(str_TestTime))
                        {
                            if (bWifiTx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_Tx.TestTime = str_temp[3];
                            }

                            if (bWifiRx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_Rx.TestTime = str_temp[3];
                            }

                            if (bBtTx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_BtTx.TestTime = str_temp[3];
                            }

                            if (bBtRx)
                            {
                                string[] str_temp = line.Split(' ');
                                str_temp = RemoveEmptyStrings(str_temp);
                                currentResult.Metrics_BtRx.TestTime = str_temp[3];
                            }

                        }


                        #endregion

                        #region Wifi_TX_RX

                        if (bWifiTx || bWifiRx)
                        {
                            int iCount = 0;
                            foreach (var item in Arrary_WifiMark)
                            {
                                //EVM_DB_AVG
                                if (line.Contains(item))
                                {
                                    //EVM_DB_AVG
                                    if (iCount == 0 && line.Contains("PK_") == false)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.EVM = str_temp[2];
                                        currentResult.Metrics_Tx.EVM_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.EVM_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }

                                    //FREQ_ERROR_AVG
                                    if (iCount == 1)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.FreqError = str_temp[2];
                                        currentResult.Metrics_Tx.FreqError_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.FreqError_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }

                                    //SYMBOL_CLK_ERR_ALL
                                    if (iCount == 2)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.SymClkError = str_temp[2];
                                        currentResult.Metrics_Tx.SymClkError_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.SymClkError_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }

                                    //POWER_AVG_DBM
                                    if (iCount == 3)
                                    {
                                        if (Arrary_WifiTx_PowerMark.Any(target => line.Contains(target)))
                                        {
                                            string[] str_temp = line.Split(' ');
                                            str_temp = RemoveEmptyStrings(str_temp);

                                            currentResult.Metrics_Tx.Power = str_temp[2];
                                            currentResult.Metrics_Tx.Power_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                            currentResult.Metrics_Tx.Power_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                        }
                                    }

                                    //LO_LEAKAGE_DBC
                                    if (iCount == 4)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Tx.LOLeakage = str_temp[2];
                                        currentResult.Metrics_Tx.LOLeakage_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Tx.LOLeakage_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }




                                    //===============================WifiRX===================
                                    //===============================WifiRX===================
                                    //===============================WifiRX===================

                                    //RX_POWER_DBM
                                    if (iCount == 5 || iCount == 6)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Rx.PER_Power = str_temp[2];
                                    }

                                    //PER
                                    if (iCount == 7)
                                    {
                                        if (line.Replace(" ", "").Contains("PER:"))
                                        {
                                            string[] str_temp = line.Split(' ');
                                            str_temp = RemoveEmptyStrings(str_temp);

                                            currentResult.Metrics_Rx.PER = str_temp[2];
                                            currentResult.Metrics_Rx.PER_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                            currentResult.Metrics_Rx.PER_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                        }
                                    }

                                    //RSSI_RX
                                    if (iCount == 8)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_Rx.RSSI = str_temp[2];
                                        currentResult.Metrics_Rx.RSSI_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_Rx.RSSI_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }

                                    if (iCount == 9)
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
                                    //INITIAL_FREQ_OFFSET
                                    if (iCount == 0)
                                    {
                                        if (Arrary_BtTx_FreqOffsetMark.Any(target => line.Contains(target)))
                                        {
                                            string[] str_temp = line.Split(' ');
                                            str_temp = RemoveEmptyStrings(str_temp);

                                            currentResult.Metrics_BtTx.FreqOffset = str_temp[2];
                                            currentResult.Metrics_BtTx.FreqOffset_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                            currentResult.Metrics_BtTx.FreqOffset_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                        }
                                    }


                                    //POWER_AVERAGE_DBM
                                    if (iCount == 1)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_BtTx.Power = str_temp[2];
                                        currentResult.Metrics_BtTx.Power_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                        currentResult.Metrics_BtTx.Power_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                    }


                                    //RX_POWER_LEVEL
                                    if (iCount == 2)
                                    {
                                        string[] str_temp = line.Split(' ');
                                        str_temp = RemoveEmptyStrings(str_temp);

                                        currentResult.Metrics_BtRx.PER_Power = str_temp[2];
                                    }

                                    //PER
                                    if (iCount == 3 || iCount == 4)
                                    {
                                        if (line.Replace(" ", "").Contains("PER:"))
                                        {
                                            string[] str_temp = line.Split(' ');
                                            str_temp = RemoveEmptyStrings(str_temp);

                                            currentResult.Metrics_BtRx.PER = str_temp[2];
                                            currentResult.Metrics_BtRx.PER_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                            currentResult.Metrics_BtRx.PER_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                        }
                                        else if (line.Replace(" ", "").Contains("BER:"))
                                        {
                                            string[] str_temp = line.Split(' ');
                                            str_temp = RemoveEmptyStrings(str_temp);

                                            currentResult.Metrics_BtRx.PER = str_temp[2];
                                            currentResult.Metrics_BtRx.PER_Range = Array_SpliceBetweenElements(str_temp, "(", ")");
                                            currentResult.Metrics_BtRx.PER_Result = line.ToLower().Contains("fail") ? "FAIL" : "PASS";
                                        }
                                    }


                                    if (iCount == 5)
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
