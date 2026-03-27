using Liteon_TestProgram.Utilities;
using ScottPlot.WinForms;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.TestFunc.ICT
{
    internal class ShowICTValuePlot
    {

        #region 挨个将每个mac对应的List<TestICTResult>存到字典中

        // 添加键值对到字典中
        public void AddToDictionary(Dictionary<string, List<TestICTResult>> dict, string key, List<TestICTResult> value)
        {
            dict[key] = value;
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


        //查找符合条件的 TestICTResult 对象（如果有多个对象需要遍历）‌：
        //这个函数假设你在一个包含多个 TestICTResult 对象的集合中查找。
        private (double, string )FindRFTestResult(List<TestICTResult> testResults, string str_testItem)
        {
            try
            {
                TestICTResult temp = null;
                double dValue = double.NaN;
                string sRange = string.Empty;

                if (string.IsNullOrEmpty(str_testItem) == false)
                {
                    temp = testResults.FirstOrDefault(result =>
                               result.Type.Contains(str_testItem)
                               );

                    if (temp != null)
                    {

                        if (double.TryParse(temp.Value, out double value))
                        {
                            dValue = value;
                        }
                        else
                        {
                            //dValue = 999.999;
                        }
                        sRange = $"({temp.Range})";
                    }

                }
                return (dValue, sRange);
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Plot FindTestICTResult error:\r\n {ex}", true);
                throw;
            }

        }








        public (List<double>, string )FormatYAxisData(Dictionary<string, List<TestICTResult>> dict, string str_testItem)
        {
            List<double> ArraryValues = new List<double>();
            string str_Range = string.Empty;

            foreach (var key in dict.Keys)
            {
                //一个log中的所有数据
                List<TestICTResult> abc = GetValueFromDictionary(dict, key);

                //一个log符合str_testItem的一个项的测试数据
                var temp_dValue = FindRFTestResult(abc, str_testItem);

                ArraryValues.Add(temp_dValue.Item1);
                str_Range = temp_dValue.Item2;
            }

            return (ArraryValues, str_Range);
        }

        private List<string> FormatXAxisLabels(Dictionary<string, List<TestICTResult>> dict)
        {
            List<string> xlabels_mac = new List<string>();
            foreach (var key in dict.Keys)
            {
                xlabels_mac.Add(key);
            }
            return xlabels_mac;
        }


        public void PlotData(FormsPlot formsPlot, Dictionary<string, List<TestICTResult>> dict, string str_Control_ItemName, ScottPlot.Color color)
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
            DrawLinechart(myPlot, ys, color, str_Control_ItemName, str_Range, null, title);
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
            sp.LegendText = str_Control_ItemName + "_" +str_Range;

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
                txt.LabelFontColor = color;
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
