using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Windows.Forms.LinkLabel;

namespace Liteon_TestProgram.TestFunc.Litepoint
{
    internal class GenerateMultiPortFlow
    {
        private readonly string str_IQ_IP = "192.168.100.254";
        private readonly string[] Arrary_ModifyMark = {
        
        ">APP_ID[Integer]=",
        ">APT_ENABLE[Integer]=",
        ">IQXEL_CONNECTION_TYPE[Integer]=",
        ">VSA_PORT[Integer]=",
        ">VSG_PORT[Integer]=",
        ">IQTESTER_IP01[String]=",
        ">IQTESTER_IP[String]=",

        ">IQTESTER_MODULE_01",
        "_PORT[String]",
    };

        public bool CreateMultiPortFlow(string str_FlowFolder, string str_NewFlowName, int iPortNums)
        {

            try
            {
                string[] Arrary_NewFlowName = str_NewFlowName.Split('/');
                CyclicFlowGeneration(str_FlowFolder, "Verify.txt", Arrary_NewFlowName[1], iPortNums);
                CyclicFlowGeneration(str_FlowFolder, "Write.txt", Arrary_NewFlowName[0], iPortNums);
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowCreateFlowInfo($"Flow create error:\r\n{ex}", true);
                return false;
            }
         
            return true;
        }

        public bool CyclicFlowGeneration(string str_FlowFolder, string str_SourceFlow, string str_NewFlowName, int iPortNums)
        {
            string str_SourceFlowFullPathAndName = str_FlowFolder + $"\\{str_SourceFlow}";

            // 读取文件内容
            string[] lines = File.ReadAllLines(str_SourceFlowFullPathAndName);

            for (int j = 1; j <= iPortNums; j++)  //有多少个port就有多少个flow
            {
                // 遍历每一行，查找并替换
                for (int i = 0; i < lines.Length; i++)
                {
                    try
                    {
                        if (Arrary_ModifyMark.Any(target => lines[i].Replace(" ", "").Contains(target)))
                        {
                            string str_Match = Arrary_ModifyMark.FirstOrDefault(target => lines[i].Replace(" ", "").Contains(target));
                            lines[i] = Flow_RepalceWords(lines[i], str_Match, iPortNums, j);
                        }
                    }
                    catch (Exception ex)
                    {
                        UIHandleHelper.ShowCreateFlowInfo($"Error processing flow line: {lines[i]} \r\n {ex}", true);
                        return false;
                    }
                }
                
               

                // 将修改后的内容写回文件
                if (iPortNums == 2)
                {
                    File.WriteAllLines($"{str_FlowFolder}\\{str_NewFlowName}_{j}RF.txt", lines);
                    UIHandleHelper.ShowCreateFlowInfo($"{str_NewFlowName}_{j}RF.txt create pass.");
                }
                else if (iPortNums == 8)
                {
                    int iFlowPortName_1 = Flow_PortName_Rules(j);
                    string iFlowPortName_2 = j % 2 == 1 ? "A" : "B";
                    File.WriteAllLines($"{str_FlowFolder}\\{str_NewFlowName}_{iFlowPortName_1}{iFlowPortName_2}.txt", lines);
                    UIHandleHelper.ShowCreateFlowInfo($"{str_NewFlowName}_{iFlowPortName_1}{iFlowPortName_2}.txt create pass.");
                }
                else if (iPortNums == 16)
                {
                    int iFlowPortName_1 = Flow_PortName_Rules(j);
                    if(j>=9)
                    {
                        string iFlowPortName_2 = j % 2 == 1 ? "C" : "D";
                        File.WriteAllLines($"{str_FlowFolder}\\{str_NewFlowName}_{iFlowPortName_1}{iFlowPortName_2}.txt", lines);
                        UIHandleHelper.ShowCreateFlowInfo($"{str_NewFlowName}_{iFlowPortName_1}{iFlowPortName_2}.txt create pass.");
                    }
                    else
                    {
                        string iFlowPortName_2 = j % 2 == 1 ? "A" : "B";
                        File.WriteAllLines($"{str_FlowFolder}\\{str_NewFlowName}_{iFlowPortName_1}{iFlowPortName_2}.txt", lines);
                        UIHandleHelper.ShowCreateFlowInfo($"{str_NewFlowName}_{iFlowPortName_1}{iFlowPortName_2}.txt create pass.");
                    }
                }
            }

            return true;
        }

        public string Flow_RepalceWords(string str_line, string str_Match, int iPortNums, int iCurrentPortNum)
        {
            string str_LineTemp = null;
            for (int i = 0; i < Arrary_ModifyMark.Length; i++)
            { 
                if (str_Match == Arrary_ModifyMark[i])
                {
                    switch (i)
                    {
                        case 0:
                            {
                                //>APP_ID[Integer]=
                                str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" {iCurrentPortNum} ";
                            }
                            return str_LineTemp;

                        case 1:
                            {
                                //>APT_ENABLE[Integer]=
                                str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + " 1 ";
                            }
                            return str_LineTemp;

                        case 2:
                            {
                                //>IQXEL_CONNECTION_TYPE[Integer]=
                                if(iPortNums == 2)
                                {
                                    if(iCurrentPortNum == 1)
                                    {
                                        str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + " 1 ";
                                    }
                                    else
                                    {
                                        str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + " 2 ";
                                    }
                                }
                            }
                            return str_LineTemp;

                        case 3:
                            {
                                //>VSA_PORT[Integer]=
                                if (iPortNums == 2)
                                {
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" {iCurrentPortNum + 1} ";
                                }
                                else 
                                {
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" {Flow_PortName_Rules(iCurrentPortNum) + 1} ";
                                }
                                
                            }
                            return str_LineTemp;

                        case 4:
                            {
                                //">VSG_PORT[Integer]=",
                                if (iPortNums == 2)
                                {
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" {iCurrentPortNum + 1} ";
                                }
                                else
                                {
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" {Flow_PortName_Rules(iCurrentPortNum) + 1} ";
                                }
                            }
                            return str_LineTemp;

                        case 5:
                            {
                                //">IQTESTER_IP01[String]=",
                                if (iPortNums == 2)
                                {
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" {str_IQ_IP} ";
                                }
                            }
                            return str_LineTemp;

                        case 6:
                            {
                                //">IQTESTER_IP[String]=",
                                if (iPortNums == 2)
                                {
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" {str_IQ_IP} ";
                                }
                            }
                            return str_LineTemp;

                        case 7:
                            {
                                //>IQTESTER_MODULE_01
                                if (iCurrentPortNum >= 9)
                                {
                                    string str_IPName = iCurrentPortNum % 2 == 1 ? "C" : "D";
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" {str_IQ_IP}:{str_IPName} ";
                                }
                                else
                                {
                                    string str_IPName = iCurrentPortNum % 2 == 1 ? "A" : "B";
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" {str_IQ_IP}:{str_IPName} ";
                                }
                            }
                            return str_LineTemp;

                        case 8:
                            {
                                // >ANT1_PORT [String]  = RF2A  -> _PORT[String]
                                if (iCurrentPortNum >= 9)
                                {
                                    string str_RFPortPart_1 = Flow_PortName_Rules(iCurrentPortNum).ToString();
                                    string str_RFPortPart_2 = iCurrentPortNum % 2 == 1 ? "C" : "D";
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" RF{str_RFPortPart_1}{str_RFPortPart_2} ";
                                }
                                else
                                {
                                    string str_RFPortPart_1 = Flow_PortName_Rules(iCurrentPortNum).ToString();
                                    string str_RFPortPart_2 = iCurrentPortNum % 2 == 1 ? "A" : "B";
                                    str_LineTemp = str_line.Substring(0, str_line.IndexOf("=") + 1) + $" RF{str_RFPortPart_1}{str_RFPortPart_2} ";
                                }
                            }
                            return str_LineTemp;

                        default:
                            return str_LineTemp;
                    }
                }
            }

            return str_LineTemp;
        }



        public int Flow_PortName_Rules(int a)
        {
            switch (a)
            {
                case 1:
                case 2:
                case 9:
                case 10:
                    return 1;
                case 3:
                case 4:
                case 11:
                case 12:
                    return 2;
                case 5:
                case 6:
                case 13:
                case 14:
                    return 3;
                case 7:
                case 8:
                case 15:
                case 16:
                    return 4;
                default:
                    return a; 
            }
        }

       



    }
}
