using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SajetConnect;

namespace SFCHelpers
{
    public partial class SFCHelpers : Form
    {

        #region 鼠标拖动窗口， 需要加到MouseDown的事件中

        [DllImport("user32.dll")]  //需添加using System.Runtime.InteropServices
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void Form_Base_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
        }

        #endregion



        public SFCHelpers()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 解析命令行参数为键值对
        /// </summary>
        public Dictionary<string, string> ParseCommandLineArgs(string[] args)
        {
            var parsedArgs = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    string key = args[i].TrimStart('-');
                    string value = i + 1 < args.Length && !args[i + 1].StartsWith("-") ? args[i + 1] : null;
                    parsedArgs[key] = value;
                }
            }
            return parsedArgs;
        }

        /// <summary>
        /// 处理命令行参数
        /// </summary>
        public void ProcessCommandLineArgs(Dictionary<string, string> args)
        {

            if (args.ContainsKey("help"))
            {
                ShowHelp();
            }

            #region Sajet_CheckSFCI.dll

            bool b_init = false;
            bool b_check = false;
            string stationValue = string.Empty;
            string ipValue = string.Empty;
            bool b_destroy = false;
            string macValue = string.Empty;
            string moValue = string.Empty;

            if (args.ContainsKey("init"))
            {
                b_init = true;
            }

            if (args.ContainsKey("check"))
            {
                b_check = true;
            }

            if (args.ContainsKey("ip"))
            {
                ipValue = args["ip"];
            }

            if (args.ContainsKey("station"))
            {
                stationValue = args["station"];
            }

            if (args.ContainsKey("destroy"))
            {
                b_destroy = true;
            }

            if (args.ContainsKey("mac"))
            {
                macValue = args["mac"];
            }

            if (args.ContainsKey("mo"))
            {
                moValue = args["mo"];
            }


            //实现方法
            bool bresult = false;
            if (b_init)
            {
                int iResult = -1;
                iResult = SFC_CommFunc.Sajet_Initial(stationValue, ipValue);
                if (iResult == 0)
                {
                    Console.WriteLine("Result: Init SFC OK");
                    bresult = true;
                }
                else
                {
                    Console.WriteLine("Result: Init SFC NG");
                    Console.WriteLine("===END===");
                }
            }


            if (bresult)
            {
                try
                {
                    if (b_check)
                    {
                        Sajet_Check(macValue, moValue);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Sajet Check Err: {ex.Message}");
                }
                finally
                {
                    int iResult = -1;
                    iResult = SFC_CommFunc.Sajet_Destroy();
                    if (iResult == 0)
                    {
                        Console.WriteLine("Result: Destroy SFC OK");
                    }
                    else
                    {
                        Console.WriteLine("Result: Destroy SFC NG");
                    }

                    Console.WriteLine("===END===");
                }
            }


            #endregion


            #region SajetConnect.dll

            bool b_open = false;
            bool b_close = false;
            string cmdValue = string.Empty;
            string uploadValue = string.Empty;

            string str_Result = string.Empty;


            if (args.ContainsKey("open"))
            {
                b_open = true;
            }

            if (args.ContainsKey("close"))
            {
                b_close = true;
            }

            if (args.ContainsKey("upload"))
            {
                uploadValue = args["upload"];
            }

            if (args.ContainsKey("cmd"))
            {
                cmdValue = args["cmd"];
            }



            if (b_open)
            {
                SajetInterFace ISajet;
                bresult = false;
                Thread.Sleep(100);

                try
                {
                    ISajet = new SajetConnect.SajetConnect();
                    Thread.Sleep(100);
                    str_Result = ISajet.SajetTransStart();

                    if (str_Result.Contains("OK"))
                    {
                        Console.WriteLine($"Result: Open MES OK: {str_Result}");
                        bresult = true;
                    }
                    else
                    {
                        Console.WriteLine($"Result: Open MES NG: {str_Result}");
                        Console.WriteLine("===END===");
                    }

                }
                catch (Exception ex)
                {
                    bresult = false;
                    Console.WriteLine($"MES Open Err: {ex.Message}");
                    Console.WriteLine("===END===");
                    return;
                }


                if (bresult)
                {
                    try
                    {
                        str_Result = string.Empty;
                        str_Result = ISajet.SajetTransData(cmdValue, uploadValue);   //-- 過站作業
                        if (str_Result.Contains("OK"))
                        {
                            Console.WriteLine($"Result: MES Upload OK: {str_Result}");
                        }
                        else
                        {
                            Console.WriteLine($"Result: MES Upload NG: {str_Result}");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"MES Upload Err: {ex.Message}");
                    }
                    finally
                    {
                        str_Result = string.Empty;
                        str_Result = ISajet.SajetTransClose();

                        if (str_Result.Contains("OK"))
                        {
                            Console.WriteLine($"Result: Close MES OK: {str_Result}");
                        }
                        else
                        {
                            Console.WriteLine($"Result: Close MES NG: {str_Result}");
                        }

                        Console.WriteLine("===END===");
                    }
                }

            }





            #endregion


        }



        public void Sajet_Check(string macValue, string moValue)
        {
            int iResult = -1;
            iResult = SFC_CommFunc.Check_SFCS($"4;{macValue};{moValue};");

            string str_Result = iResult.ToString();
            Console.WriteLine($"Result: Code {str_Result}");
            int iResult_Code = int.Parse(str_Result.Substring(0, 2));

            switch (iResult_Code)
            {
                case 10:
                    Console.WriteLine("Result: Check SFC OK");
                    break;
                case 11:
                    Console.WriteLine("Result: Command not define");
                    break;
                case 12:
                    Console.WriteLine("Result: SN NG");
                    break;
                case 13:
                    Console.WriteLine("Result: Route NG");
                    break;
                case 14:
                    Console.WriteLine("Result: Version discrepance");
                    break;
                case 15:
                    Console.WriteLine("Result: Use program has not found");
                    break;
                case 16:
                    Console.WriteLine("Result: Carton closed");
                    break;
                case 17:
                    Console.WriteLine("Result: Model error");
                    break;
                case 18:
                    Console.WriteLine("Result: ATE Model error");
                    break;
                case 19:
                    Console.WriteLine("Result: Program No not found");
                    break;
                case 20:
                    Console.WriteLine("Result: Emp error");
                    break;
                case 21:
                    Console.WriteLine("Result: Machine error");
                    break;
                case 22:
                    Console.WriteLine("Result: SN Scrap");
                    break;
                case 23:
                    Console.WriteLine("Result: SN Hold");
                    break;
                case 24:
                    Console.WriteLine("Result: SN Ndf");
                    break;
                case 25:
                    Console.WriteLine("Result: SN Fail");
                    break;
                case 26:
                    Console.WriteLine("Result: Goto Rework");
                    break;
                case 27:
                    Console.WriteLine("Result: Pallet closed");
                    break;
                case 28:
                    Console.WriteLine("Result: Go NG");
                    break;
                case 29:
                    Console.WriteLine("Result: Insert burnin_detail error");
                    break;
                case 30:
                    Console.WriteLine("Result: Closed Pallet error");
                    break;
                case 31:
                    Console.WriteLine("Result: Pallet No Not Exist");
                    break;
                case 40:
                    Console.WriteLine("Result: Heavy Code");
                    break;
                default:
                    Console.WriteLine("Result: default not define");
                    break;
            }
        }


        private void ShowHelp()
        {
            Console.WriteLine(new string(' ', 55));
            Console.WriteLine("可用命令行参数：");
            Console.WriteLine(new string(' ', 55));
            Console.WriteLine($"     {"-help",-25} {"显示帮助信息",-10}");
            Console.WriteLine(new string('-', 55));  // 输出分隔线
            Console.WriteLine($"{"以下是Sajet_CheckSFCI.dll使用的指令(SFC)，主要用于检查站别结果",-10}");
            Console.WriteLine(new string(' ', 55));


            //Sajet_CheckSFCI.dll
            Console.WriteLine($"     {"-init",-25} {"初始化SFC",-10}");
            Console.WriteLine($"     {"",-25} {"初始化格式: -station <value>  -ip <value>",-10}");
            Console.WriteLine($"{"",-8} {"-ip <value>",-21} {"初始化SFC使用的IP",-10}");
            Console.WriteLine($"{"",-8} {"-station <value>",-21} {"初始化SFC使用的站别",-10}");
            Console.WriteLine($"{"",-8} {"",-21} {"目前(20250225)使用的站别:",-10}");
            Console.WriteLine($"{"",-8} {"",-21} {"FT1:60007831 FT2:60007020 FT3:60007634 FT4:60007370 FT5:60007395 FT6:60007090",-10}");

            Console.WriteLine(new string(' ', 55));

            Console.WriteLine($"     {"-check",-25} {"检查站别",-10}");
            Console.WriteLine($"     {"",-25} {"检查站别格式: -check -mac <value> -mo <value>   返回代号是10开头，标识检查站别pass",-10}");
            Console.WriteLine($"{"",-8} {"-mac <value>",-21} {"检查站别的条码",-10}");
            Console.WriteLine($"{"",-8} {"-mo <value>",-21} {"检查站别的工单",-10}");

            Console.WriteLine(new string(' ', 55));

            Console.WriteLine($"     {"-destroy",-25} {"关闭SFC (目前已经强制执行, 该参数可以不用添加)",-10}");

            Console.WriteLine(new string(' ', 55));
            Console.WriteLine($"{"",-4} {"example: SFCHelpers.exe  -init -station 60007831 -ip 10.141.106.194 -check -mac 443E075B0216 -mo 97603-002070A000 -destroy",-10}");
            Console.WriteLine($"{"",-4} {"检查OK的返回值: Check SFC OK",-10}");
            Console.WriteLine(new string(' ', 55));



            //SajetConnect.dll
            Console.WriteLine(new string('-', 55));  // 输出分隔线
            Console.WriteLine($"{"以下是SajetConnect.dll使用的指令(One Mes)，主要用于上传测试结果",-10}");
            Console.WriteLine(new string(' ', 55));
            Console.WriteLine($"     {"-open",-25} {"初始化MES",-10}");
            Console.WriteLine($"     {"-close",-25} {"关闭MES  (目前已经强制执行, 该参数可以不用添加)",-10}");

            Console.WriteLine($"     {"-cmd <value>",-25} {"MES指令头",-10}");

            Console.WriteLine(new string(' ', 55));

            Console.WriteLine($"     {"-upload <value>",-25} {"MES上传测试结果",-10}");

            Console.WriteLine(new string(' ', 55));

            Console.WriteLine($"     {"",-25} {"上传格式(OQC): -cmd <value> -upload <value>",-10}");
            Console.WriteLine($"     {"",-25} {"upload value格式-> 工號;SN;結果(OK & NG);狀態(0 & 1--0:站, 1:寫測試值);測試小項; (英文分号结尾)",-10}");
            Console.WriteLine($"     {"",-25} {"upload value例子-> 11099253;700894FF86AB;OK;0;TPOOC-003;",-10}");
            Console.WriteLine($"     {"",-25} {"cmd value 需要使用51",-10}");

            Console.WriteLine(new string(' ', 55));

            Console.WriteLine($"     {"",-25} {"OQC example: SFCHelpers.exe  -open -cmd 51 -upload 11099253;700894FF86AB;OK;0;TPOOC-003; -close",-10}");
            Console.WriteLine($"     {"",-25} {"上传OK的返回值: MES Upload OK     上传NG的返回值: MES Upload NG",-10}");

            Console.WriteLine(new string(' ', 55));
            Console.WriteLine(new string(' ', 55));
            Console.WriteLine(new string(' ', 55));


            Console.WriteLine($"     {"",-25} {"上传格式(Sample): -cmd <value> -upload <value>",-10}");
            Console.WriteLine($"     {"",-25} {"Tips：使用之前需要使用Terminal.exe设定机台编号",-10}");
            Console.WriteLine($"     {"",-25} {"upload value格式-> 工號;SN;程式名称;程式版本;不良代码(测试PASS可不放);测试結果(OK & NG);(英文分号结尾)",-10}");
            Console.WriteLine($"     {"",-25} {"upload value测试Pass例子-> 41003391;700894FF86AB;WCBN811L-L6_Test;V0.0.0.1;;OK;SAMPLETest;",-10}");
            Console.WriteLine($"     {"",-25} {"upload value测试Fail例子-> 41003391;700894FF86AB;WCBN811L-L6_Test;V0.0.0.1;WFCV001;NG;SAMPLETest;",-10}");
            Console.WriteLine($"     {"",-25} {"cmd value 需要使用5",-10}");

            Console.WriteLine(new string(' ', 55));

            Console.WriteLine($"     {"",-25} {"Sample example: SFCHelpers.exe  -open -cmd 5 -upload  41003391;700894FF86AB;WCBN811L-L6_Test;V0.0.0.1;WFCV001;NG;SAMPLETest; -close",-10}");
            Console.WriteLine($"     {"",-25} {"上传OK的返回值: MES Upload OK     上传NG的返回值: MES Upload NG",-10}");

            Console.WriteLine(new string(' ', 55));



            Console.WriteLine("===END===");

        }

    }
}
