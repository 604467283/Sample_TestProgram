
namespace Liteon_TestProgram.TestFunc.iTest
{
    internal class CPKCalculator
    {
        public static double? CalculateCPK(Dictionary<string, List<RFTestResult>> dict, string str_Control_ItemName, string str_CustomLowerLimit, string str_CustomUpperLimit)
        {

            string frequency = string.Empty;
            string dataRate = string.Empty;
            string antenna = string.Empty;
            string metric = string.Empty;

            var vItem = str_Control_ItemName.Split('#');

            //Wifi
            if (vItem.Length == 4) 
            {
                frequency = vItem[0];
                dataRate = vItem[1];
                antenna = vItem[2];
                metric = vItem[3];
            }

            //Bt
            if (vItem.Length == 3)
            {
                frequency = vItem[0];
                dataRate = vItem[1];
                metric = vItem[2];
            }


            // Step 1: Filter data based on the given conditions
            List<RFTestResult> filteredResults_ = null;
            if (vItem.Length == 4)
            {
               var filteredResults = dict.Values
               .SelectMany(list => list)
               .Where(result => result.Frequency == frequency &&
                               result.DataRate == dataRate &&
                               result.Antenna == antenna)
               .ToList();

                filteredResults_ = filteredResults;
            }

            if (vItem.Length == 3)
            {
                var filteredResults = dict.Values
               .SelectMany(list => list)
               .Where(result => result.Frequency == frequency &&
                               result.DataRate == dataRate)
               .ToList();

                filteredResults_ = filteredResults;
            }


            // Step 2: Extract the metric values and ranges
            var metricValues = new List<double>();
            var metricRanges = new List<string>();

            foreach (var result in filteredResults_)
            {
                var wifiMetricsTx = result.Metrics_Tx;
                var wifiMetricsRx = result.Metrics_Rx;
                var btMetricsTx = result.Metrics_BtTx;
                var btMetricsRx = result.Metrics_BtRx;

                #region WIFI

                if (wifiMetricsTx != null && metric == "EVM")
                {
                    if (float.TryParse(wifiMetricsTx.EVM, out float evm))
                    {
                        metricValues.Add(evm);
                        metricRanges.Add(wifiMetricsTx.EVM_Range);
                    }
                }

                if (wifiMetricsTx != null && metric == "Power")
                {
                    if (float.TryParse(wifiMetricsTx.Power, out float power))
                    {
                        metricValues.Add(power);
                        metricRanges.Add(wifiMetricsTx.Power_Range);
                    }
                }

                //2437#CCK-11#NON_HT_BW-20#ANT1#FreqError
                if (wifiMetricsTx != null && metric == "FreqError")
                {
                    if (float.TryParse(wifiMetricsTx.Power, out float FreqError))
                    {
                        metricValues.Add(FreqError);
                        metricRanges.Add(wifiMetricsTx.FreqError_Range);
                    }
                }

                //2437#CCK-11#NON_HT_BW-20#ANT1#SymClkError
                if (wifiMetricsTx != null && metric == "MaskErr")
                {
                    if (float.TryParse(wifiMetricsTx.MaskErr, out float MaskErr))
                    {
                        metricValues.Add(MaskErr);
                        metricRanges.Add(wifiMetricsTx.MaskErr_Range);
                    }
                }


                //2412#MCS7#HT_MF_BW-20#ANT1#Rx
                if (wifiMetricsRx != null && metric == "Rx")
                {
                    if (float.TryParse(wifiMetricsRx.PER, out float Rx))
                    {
                        metricValues.Add(Rx);
                        metricRanges.Add(wifiMetricsRx.PER_Range);
                    }
                }

                #endregion

                #region BT

                //2402#TX_LE_1LE#FreqOffset
                if (btMetricsTx != null && metric == "InitFreqErr")
                {
                    if (float.TryParse(btMetricsTx.InitFreqErr, out float InitFreqErr))
                    {
                        metricValues.Add(InitFreqErr);
                        metricRanges.Add(btMetricsTx.InitFreqErr_Range);
                    }
                }

                //2402#TX_LE_1LE#Power
                if (btMetricsTx != null && metric == "Power")
                {
                    if (float.TryParse(btMetricsTx.Power, out float Power))
                    {
                        metricValues.Add(Power);
                        metricRanges.Add(btMetricsTx.Power_Range);
                    }
                }

                //2440#RX_LE_125KLE#Rx
                if (btMetricsRx != null && metric == "Rx")
                {
                    if (float.TryParse(btMetricsRx.PER, out float Rx))
                    {
                        metricValues.Add(Rx);
                        metricRanges.Add(btMetricsRx.PER_Range);
                    }
                }

                #endregion

                // Add similar checks for other metrics...
            }

            // Step 3: Calculate CPK
            if (metricValues.Count == 0 || metricRanges.Count == 0)
                return null;

            double mean = metricValues.Average();
            double stdDev = CalculateStandardDeviation(metricValues);

            double? cpk = null;

            //手动指定的上下限计算CPK
            if (!String.IsNullOrEmpty(str_CustomUpperLimit) && !String.IsNullOrEmpty(str_CustomLowerLimit))
            {
                if (double.TryParse(str_CustomLowerLimit, out double lowerLimit) &&
                       double.TryParse(str_CustomUpperLimit, out double upperLimit))
                {
                    // 计算 CPU 和 CPL
                    double cpu = (upperLimit - mean) / (3 * stdDev);
                    double cpl = (mean - lowerLimit) / (3 * stdDev);
                    cpk = Math.Min(cpu, cpl);
                    return cpk;
                }
            }

            foreach (var range in metricRanges.Distinct())
            {
                if (string.IsNullOrEmpty(range))
                    continue;

                string range_Temp = range;
                var rangeParts = range_Temp.Replace(",", "/").Replace("(", "").Replace(")", "/").Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (rangeParts.Length != 1 && rangeParts.Length != 2)
                    continue;


                // 单边规格：只有上限或下限
                if (rangeParts.Length == 1)
                {
                    //(6,)  (,6)  (,)
                    string str_range = range_Temp.Replace(" ", "").Replace("(,", "upper/").Replace(",)", "/lower").Replace("(", "/lower").Replace(")", "/lower");
                    string str_limit = str_range.Replace("upper/", "").Replace("/lower", "");


                    // 尝试解析为上限或下限
                    if (double.TryParse(str_limit, out double limit))
                    {
                        // 判断是上限还是下限
                        if (str_range.Contains("upper")) // 假设上限标识为 "max" 或 "upper"
                        {
                            // 只有上限，计算 CPU
                            double cpu = (limit - mean) / (3 * stdDev);
                            cpk = cpu;
                        }
                        else if (str_range.Contains("lower")) // 假设下限标识为 "min" 或 "lower"
                        {
                            // 只有下限，计算 CPL
                            double cpl = (mean - limit) / (3 * stdDev);
                            cpk = cpl;
                        }
                        break;
                    }
                }
                // 双边规格：有上限和下限
                else if (rangeParts.Length == 2)
                {
                    if (double.TryParse(rangeParts[0], out double lowerLimit) &&
                        double.TryParse(rangeParts[1], out double upperLimit))
                    {
                        // 计算 CPU 和 CPL
                        double cpu = (upperLimit - mean) / (3 * stdDev);
                        double cpl = (mean - lowerLimit) / (3 * stdDev);
                        cpk = Math.Min(cpu, cpl);
                        break;
                    }
                }
            }

            return cpk;
        }

        private static double CalculateStandardDeviation(List<double> values)
        {
            double mean = values.Average();
            double sumOfSquares = values.Sum(value => Math.Pow(value - mean, 2));
            return Math.Sqrt(sumOfSquares / values.Count);
        }


    }
}
