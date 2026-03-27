namespace Liteon_TestProgram.TestFunc.ICT
{
    internal class CPKCalculator
    {
        public static double? CalculateCPK(Dictionary<string, List<TestICTResult>> dict, string str_Control_ItemName, string str_CustomLowerLimit, string str_CustomUpperLimit)
        {

            string type = string.Empty;


            var vItem = str_Control_ItemName.Split('-');

            //Wifi
            if (vItem.Length == 2) 
            {
                type = str_Control_ItemName;
            }



            // Step 1: Filter data based on the given conditions
            List<TestICTResult> filteredResults_ = null;
            if (vItem.Length == 2)
            {
               var filteredResults = dict.Values
               .SelectMany(list => list)
               .Where(result => result.Type == type)
               .ToList();

                filteredResults_ = filteredResults;
            }




            // Step 2: Extract the metric values and ranges
            var metricValues = new List<double>();
            var metricRanges = new List<string>();

            foreach (var result in filteredResults_)
            {

                if (result != null && result.Type == str_Control_ItemName)
                {
                    if (float.TryParse(result.Value, out float value))
                    {
                        metricValues.Add(value);
                        metricRanges.Add(result.Range);
                    }
                }
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

                string range_Temp = "("+range +")";
                var rangeParts = range_Temp.Replace("-", "/").Replace("(", "").Replace(")", "/").Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (rangeParts.Length != 1 && rangeParts.Length != 2)
                    continue;


                // 单边规格：只有上限或下限
                if (rangeParts.Length == 1)
                {
                    //(6,)  (,6)  (,)
                    string str_range = range_Temp.Replace(" ", "").Replace("(-", "upper/").Replace("-)", "/lower").Replace("(", "/lower").Replace(")", "/lower");
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
