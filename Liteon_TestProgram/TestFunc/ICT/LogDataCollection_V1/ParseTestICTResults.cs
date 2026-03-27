using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Liteon_TestProgram.TestFunc.ICT.LogDataCollection_V1
{
    internal class ParseTestICTResults
    {
        public (List<TestICTResult>, bool) ParseWifiTestResults(string filePath)
        {
            bool bSuccess = false;
            var results = new List<TestICTResult>();

            TestICTResult currentResult = null;
            

            //获取文件Mac
            string str_FileMac = FileProcessHelper.GetFileName(filePath).Substring(0, FileProcessHelper.GetFileName(filePath).IndexOf("_") - 1);
            


            string[] lines = File.ReadAllLines(filePath);
            bool processNextLines = false;

            int iCount = CountMarkOccurrences(lines, "#1,");

            int iFoundMarkTimes = 0;
            foreach (var line in lines)
            {

                try
                {
                    if (line.Replace(" ", "").IndexOf("#1,") != -1)  //PASS,EVO   #1,
                    {
                        iFoundMarkTimes++;
                        if (iFoundMarkTimes == iCount)  //找lines 存在几个",EVO#1,"， 如果存在多个，则从最后一个开始处理
                        {
                            processNextLines = true;
                        }

                    }


                    if (processNextLines)
                    {
                        

                        //v--3,800,680mV,500,OK;
                        if ((line.IndexOf("v-") != -1 || line.IndexOf("P") != -1) 
                            && (line.IndexOf("OK;") != -1 || line.IndexOf("NG;") != -1) 
                            && line.IndexOf("V,") != -1)
                        {
                            currentResult = new TestICTResult();
                            currentResult.Mac = str_FileMac;

                            string[] strings = line.Split(',');

                            if (line.IndexOf("v-") != -1)
                            {
                                currentResult.Type = strings[0].Substring(strings[0].IndexOf("v-"));
                            }
                            else if (line.IndexOf("P") != -1)
                            {
                                currentResult.Type = strings[0].Substring(strings[0].IndexOf("P"));
                            }

                            
                            currentResult.Value = SplitContinuousNumbersAndLetters(strings[2])[0];
                            currentResult.Uint = SplitContinuousNumbersAndLetters(strings[2])[1];
                            currentResult.Result = strings[4].Replace(";", "");
                            currentResult.Range = strings[3] + "-" + strings[1];

                        }


                        //R-19,93.0,71.0kR,55.0,OK;
                        if (line.IndexOf("R-") != -1
                            && (line.IndexOf("OK;") != -1 || line.IndexOf("NG;") != -1) 
                            && line.IndexOf("R,") != -1)
                        {
                            currentResult = new TestICTResult();
                            currentResult.Mac = str_FileMac;

                            string[] strings = line.Split(',');
                            currentResult.Type = strings[0].Substring(strings[0].IndexOf("R-"));
                            currentResult.Value = SplitContinuousNumbersAndLetters(strings[2])[0];
                            currentResult.Uint = SplitContinuousNumbersAndLetters(strings[2])[1];
                            currentResult.Result = strings[4].Replace(";", "");
                            currentResult.Range = strings[3] + "-" + strings[1];

                        }


                        if (currentResult!=null)
                        {
                            results.Add(currentResult);
                            currentResult = null;
                        }
                        

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

        /// <summary>
        /// 使用正则表达式（Regex）来将一个由连续数字和连续字母组成的字符串分开
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<string> SplitContinuousNumbersAndLetters(string input)
        {
            List<string> result = new List<string>();

            // 正则表达式模式，匹配一个或多个连续的数字（可能包含小数点），或一个或多个连续的字母
            string pattern = @"[\d.]+|[a-zA-Z]+";
            MatchCollection matches = Regex.Matches(input, pattern);

            // 将匹配的结果添加到结果列表中
            foreach (Match match in matches)
            {
                result.Add(match.Value);
            }

            return result;
        }

        public int CountMarkOccurrences(string[] lines, string str_mark)
        {
            int count = 0;

            foreach (var line in lines)
            {
                if (line.Replace(" ", "").Contains(str_mark))
                {
                    count++;
                }
            }

            return count;
        }

    }
}
