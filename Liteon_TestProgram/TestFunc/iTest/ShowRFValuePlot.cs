using FluentFTP.Helpers;
using Liteon_TestProgram.Utilities;
using OpenTK.Audio.OpenAL;
using ScottPlot;
using ScottPlot.WinForms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.TestFunc.iTest
{
    internal class ShowRFValuePlot
    {
        #region 挨个将每个mac对应的List<RFTestResult>存到字典中

        // 添加键值对到字典中
        public void AddToDictionary(Dictionary<string, List<RFTestResult>> dict, string key, List<RFTestResult> value)
        {
            dict[key] = value;
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


        //查找符合条件的 RFTestResult 对象（如果有多个对象需要遍历）‌：
        //这个函数假设你在一个包含多个 RFTestResult 对象的集合中查找。
        private (double, string) FindRFTestResult(List<RFTestResult> testResults, string str_testItem)
        {
            try
            {
                RFTestResult temp = null;
                double dValue = double.NaN;
                string sRange = string.Empty;
                bool bWifiTx = false;
                bool bWifiRx = false;
                bool bBtTx = false;
                bool bBtRx = false;

                string[] arraryHeadStr = str_testItem.Split('#');

                
                #region WifiTx

                if (arraryHeadStr.Length == 4 &&
                    (arraryHeadStr[arraryHeadStr.Length - 1] == "Power"
                    || arraryHeadStr[arraryHeadStr.Length - 1].Contains("EVM")
                    || arraryHeadStr[arraryHeadStr.Length - 1].Contains("MaskErr")
                    || arraryHeadStr[arraryHeadStr.Length - 1].Contains("FreqErr")
                    ))
                {
                    temp = null;
                    bWifiTx = true;

                    if (bWifiTx)
                    {
                        //temp = testResults.FirstOrDefault(result =>
                        //    result.Frequency.Contains(arraryHeadStr[0]) &&
                        //    result.DataRate.Contains(arraryHeadStr[1]) &&
                        //    result.Antenna.Contains(arraryHeadStr[2]) &&
                        //    result.Metrics_Tx != null
                        //    );

                        temp = testResults.FirstOrDefault(result =>
                                   result != null &&                          // 先检查result本身是否为null
                                   result.Frequency != null &&                // 检查Frequency是否为null
                                   result.Frequency.Contains(arraryHeadStr[0]) &&
                                   result.DataRate != null &&                 // 检查DataRate是否为null
                                   result.DataRate.Contains(arraryHeadStr[1]) &&
                                   result.Antenna != null &&                  // 检查Antenna是否为null
                                   result.Antenna.Contains(arraryHeadStr[2]) &&
                                   result.Metrics_Tx != null);
                    }

                    if (temp != null)
                    {
                        if (arraryHeadStr[arraryHeadStr.Length - 1] == "Power")
                        {
                            if (double.TryParse(temp.Metrics_Tx.Power, out double value))
                            {
                                dValue = value;
                            }
                            else
                            {
                                //dValue = 999.999;
                            }

                            sRange = temp.Metrics_Tx.Power_Range;
                        }

                        if (arraryHeadStr[arraryHeadStr.Length - 1].Contains("EVM"))
                        {
                            if (double.TryParse(temp.Metrics_Tx.EVM, out double value))
                            {
                                dValue = value;
                            }
                            else
                            {
                                //dValue = 999.999;
                            }

                            sRange = temp.Metrics_Tx.EVM_Range;
                        }

                        if (arraryHeadStr[arraryHeadStr.Length - 1].Contains("MaskErr"))
                        {
                            if (double.TryParse(temp.Metrics_Tx.MaskErr, out double value))
                            {
                                dValue = value;
                            }
                            else
                            {
                                //dValue = 999.999;
                            }

                            sRange = temp.Metrics_Tx.MaskErr_Range;
                        }

                        if (arraryHeadStr[arraryHeadStr.Length - 1].Contains("FreqErr"))
                        {
                            if (double.TryParse(temp.Metrics_Tx.FreqError, out double value))
                            {
                                dValue = value;
                            }
                            else
                            {
                                //dValue = 999.999;
                            }

                            sRange = temp.Metrics_Tx.FreqError_Range;
                        }
                    }

                }


                #endregion

                
                #region WifiRx

                if (arraryHeadStr.Length == 4 &&
                    (arraryHeadStr[arraryHeadStr.Length - 1] == "Rx_Power"
                    || arraryHeadStr[arraryHeadStr.Length - 1] == "Rx"
                    ))
                {
                    temp = null;
                    bWifiRx = true;

                    if (bWifiRx)
                    {
                        temp = testResults.FirstOrDefault(result =>
                        result!=null &&
                        result.Frequency != null &&
                        result.DataRate != null &&
                        result.Antenna != null &&
                            result.Frequency.Contains(arraryHeadStr[0]) &&
                            result.DataRate.Contains(arraryHeadStr[1]) &&
                            result.Antenna.Contains(arraryHeadStr[2]) &&
                            result.Metrics_Rx != null
                            );
                    }

                    if (temp != null)
                    {
                        if (arraryHeadStr[arraryHeadStr.Length - 1] == "Rx")
                        {
                            if (double.TryParse(temp.Metrics_Rx.PER.Replace("%", ""), out double value))
                            {
                                dValue = value;
                            }
                            else
                            {
                                //dValue = 999.999;
                            }

                            sRange = temp.Metrics_Rx.PER_Range;
                        }
                    }
                }

               

                #endregion

                #region BtTx

                if (arraryHeadStr.Length == 3 &&
                (arraryHeadStr[arraryHeadStr.Length - 1] == "InitFreqErr"
                || arraryHeadStr[arraryHeadStr.Length - 1] == "Power"
                ))
                {
                    temp = null;
                    bBtTx = true;

                    if (bBtTx)
                    {
                        temp = testResults.FirstOrDefault(result =>
                          result != null &&
                          result.Frequency!=null &&
                          result.DataRate != null &&
                            result.Frequency.Contains(arraryHeadStr[0]) &&
                            result.DataRate.Contains(arraryHeadStr[1]) &&
                            result.Metrics_BtTx != null
                            );
                    }

                    if (temp != null)
                    {
                        if (arraryHeadStr[arraryHeadStr.Length - 1].Contains("InitFreqErr"))
                        {
                            if (double.TryParse(temp.Metrics_BtTx.InitFreqErr, out double value))
                            {
                                dValue = value;
                            }

                            sRange = temp.Metrics_BtTx.InitFreqErr_Range;
                        }

                        if (arraryHeadStr[arraryHeadStr.Length - 1] == "Power")
                        {
                            if (double.TryParse(temp.Metrics_BtTx.Power, out double value))
                            {
                                dValue = value;
                            }
                            else
                            {
                                //dValue = 999.999;
                            }

                            sRange = temp.Metrics_BtTx.Power_Range;
                        }
                    }
                }

               

                #endregion

                #region BtRx

                if (arraryHeadStr.Length == 3 &&
                    (arraryHeadStr[arraryHeadStr.Length - 1] == "Rx_Power"
                    || arraryHeadStr[arraryHeadStr.Length - 1] == "Rx"
                    ))
                {
                    temp = null;
                    bBtRx = true;

                    if (bBtRx)
                    {
                        temp = testResults.FirstOrDefault(result =>
                        result!=null &&
                        result.Frequency!=null &&
                        result.DataRate != null &&
                            result.Frequency.Contains(arraryHeadStr[0]) &&
                            result.DataRate.Contains(arraryHeadStr[1]) &&
                            result.Metrics_BtRx != null
                            );
                    }

                    if (temp != null)
                    {
                        if (arraryHeadStr[arraryHeadStr.Length - 1] == "Rx")
                        {
                            if (double.TryParse(temp.Metrics_BtRx.PER.Replace("%", ""), out double value))
                            {
                                dValue = value;
                            }
                            else
                            {
                                //dValue = 999.999;
                            }

                            sRange = temp.Metrics_BtRx.PER_Range;
                        }
                    }
                }

                

                #endregion
                


                return (dValue, sRange);
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Plot FindRFTestResult error:\r\n {ex}", true);
                throw;
            }

        }


        public (List<double>, string )FormatYAxisData(Dictionary<string, List<RFTestResult>> dict, string str_testItem)
        {
            List<double> ArraryValues = new List<double>();

            string str_Range = string.Empty;



            foreach (var key in dict.Keys)
            {
                //一个log中的所有数据
                List<RFTestResult> abc = GetValueFromDictionary(dict, key);

                //一个log符合str_testItem的一个项的测试数据
                var temp_dValue = FindRFTestResult(abc, str_testItem);

                ArraryValues.Add(temp_dValue.Item1);
                str_Range = temp_dValue.Item2;
            }

            return (ArraryValues, str_Range);
        }

        private List<string> FormatXAxisLabels(Dictionary<string, List<RFTestResult>> dict)
        {
            List<string> xlabels_mac = new List<string>();
            foreach (var key in dict.Keys)
            {
                xlabels_mac.Add(key);
            }
            return xlabels_mac;
        }


        public void PlotData(FormsPlot formsPlot, Dictionary<string, List<RFTestResult>> dict, string str_Control_ItemName, ScottPlot.Color color)
        {
            try
            {
                var plt = formsPlot.Plot;

                var yAxisLabels_Values = FormatYAxisData(dict, str_Control_ItemName);
                List<string> xAxisLabels_mac = FormatXAxisLabels(dict);

                if (yAxisLabels_Values.Item1.Count > 0 && xAxisLabels_mac.Count == yAxisLabels_Values.Item1.Count)
                {

                    DrawLinechart(plt, yAxisLabels_Values.Item1, color, str_Control_ItemName, yAxisLabels_Values.Item2, xAxisLabels_mac);
                    UIHandleHelper.ControlHandle(formsPlot, () => { formsPlot.Refresh(); });

                }
                else
                {
                    //MessageBox.Show("No valid data to plot.");
                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowCollectionLogsRFData($"PlotData Error:\r\n{ex}");
                throw;
            }
        }




        #region 显示点图到plot

        //方法重载，方便调用
        private void DrawLinechart(Plot myPlot, List<double> ys, ScottPlot.Color color, string str_Control_ItemName, string str_Range, List<string> title = null)
        {
            DrawLinechart(myPlot, ys, color, str_Control_ItemName, str_Range,  null, title);
        }

        //原始方法
        private void DrawLinechart(Plot myPlot, List<double> ys, ScottPlot.Color color, string str_Control_ItemName, string str_Range, List<double> xs = null, List<string> title = null)
        {
            bool flag = false;
            bool isallnull = xs == null && title == null;
            if (isallnull)
            {
                throw new Exception("请传入横坐标值或者横坐标轴标签，渲染失败！");
            }
            if (xs != null && xs.Count != ys.Count || title != null && title.Count != ys.Count)
            {
                throw new Exception("横纵坐标长度不同，渲染失败！");
            }

            if (title?.Count != 0 || xs?.Count == 0)
            {
                xs = new List<double>();
                flag = true;//表示xs要渲染成0，1，2，3，4...序列
                for (var i = 0; i < ys.Count; i++)
                {
                    xs.Add(i + 1);
                }
            }

            //1.数据渲染
            var sp = myPlot.Add.Scatter(xs, ys);
            sp.LegendText= str_Control_ItemName + "_" + str_Range;

            //2.平滑处理
            sp.Smooth = true;//平滑折线图，注释掉这行代码可以变成直线图
            sp.LineWidth = 3;
            sp.MarkerSize = 10;
            sp.Color = color;//设置线颜色 , 这里也可由入参控制：
            myPlot.Axes.AntiAlias(true);

            //3.数值标记
            int index = 0;
            foreach (var item in ys)
            {
                var txt = myPlot.Add.Text(item.ToString(), xs[index], item);
                txt.LabelFontSize = 10;
                txt.LabelPadding = 2;
                txt.LabelBold = true;
                txt.LabelFontColor= color;
                // 调整文本位置，使其显示在点的上方
                txt.OffsetY = -25; // 越小越靠上
                txt.OffsetX = -6;// 越小越靠左
                index++;
            }

            // 4.横坐标标记
            if (flag && title != null && title.Count != 0)
            {
                myPlot.Axes.Bottom.SetTicks(xs.ToArray(), title.ToArray());
                myPlot.Axes.Bottom.TickLabelStyle.FontName = "宋体";
                myPlot.Axes.Bottom.TickLabelStyle.FontSize = 13;
                myPlot.Axes.Bottom.TickLabelStyle.Rotation = 0;
                myPlot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleLeft;

                //float largestLabelWidth = 0;
                //using SKPaint paint = new();
                //foreach (string str in title)
                //{
                //    PixelSize size = myPlot.Axes.Bottom.TickLabelStyle.Measure(str, paint).Size;
                //    largestLabelWidth = Math.Max(largestLabelWidth, size.Width);
                //}

                //// ensure axis panels do not get smaller than the largest label
                //myPlot.Axes.Bottom.MinimumSize = largestLabelWidth;
                //myPlot.Axes.Right.MinimumSize = largestLabelWidth;
            }

            // 5.调整图表边距
            myPlot.Axes.Margins(bottom: .1, top: .3);

        }

        #endregion


    }
}
