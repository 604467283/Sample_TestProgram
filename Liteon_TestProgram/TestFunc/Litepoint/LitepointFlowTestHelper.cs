using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Liteon_TestProgram.Utilities;

namespace Liteon_TestProgram.TestFunc.Litepoint
{
    internal class LitepointFlowTestHelper
    {
        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(
           IntPtr hwnd,
           string lpOperation,
           string lpFile,
           string lpParameters,
           string lpDirectory,
           ShowCommands nShowCmd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);



        public (bool, string )CreateLitePointBat(bool bWrite, string str_FlowPath, string str_IQfactStudioExe, string str_PortName, bool bFailStop, 
                                                                string str_Flow_MoreParameters = "", bool bSampleMode = false)
        {
            string str_Temp = "";
            string str_FlowAndBatPartName = bWrite ? "IQ_WriteTest" : "IQ_VerifyTest";

            if (bSampleMode)
            {
                str_FlowAndBatPartName = "IQ_VerifyTestSample";
            }

            string str_FailStop = bFailStop ? "1" : "0";


            try
            {
               
                if (bWrite)
                {
                    UIHandleHelper.ShowRunLog("Create Write + Verify Flow...");
                    str_Temp = "cd /d " + str_FlowPath + "\\Bin\r\n" +
                   str_IQfactStudioExe + " -run " + str_FlowAndBatPartName + "_" + str_PortName + ".txt " + str_Flow_MoreParameters + " -ONFAIL " + str_FailStop + " -EXIT";

                }
                else
                {
                    UIHandleHelper.ShowRunLog("Create Only Verify Flow...");
                    str_Temp = "cd /d" + str_FlowPath + "\\Bin\r\n" +
                   str_IQfactStudioExe + " -run " + str_FlowAndBatPartName + "_" + str_PortName + ".txt " + str_Flow_MoreParameters + " -ONFAIL " + str_FailStop + " -EXIT";

                }

                TextWriter Filewriter;//以寫方式打開文件
                Filewriter = File.CreateText($"{FileProcessHelper.GetCurrentExeDirectory()}\\{str_FlowAndBatPartName}_{str_PortName}.bat");//創建或打開一個UTF-8的文件
                Filewriter.Write(str_Temp);//寫入
                Filewriter.Close();

            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Create LitePoint Bat error: {ex}", true);
                return (false, null);
            }

            UIHandleHelper.ShowRunLog("Create LitePoint flow bat ok.");
            return (true, $"{str_FlowAndBatPartName}_{str_PortName}.bat");
        }


       

        public bool ChangeSerialMAC(string str_IQTestProgramFolderPath, string str_Mac, string str_BD)
        {
            UIHandleHelper.ShowRunLog($"Set mac to Serial_MAC.txt...");

            if (File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\log\\Log_All.txt"))
                File.Delete($"{str_IQTestProgramFolderPath}\\Bin\\log\\Log_All.txt");
            if (File.Exists($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt"))
                File.Delete($"{str_IQTestProgramFolderPath}\\Bin\\log\\logOutput.txt");

            string strLine = "";
            string strOldMac = "";
            string strOldBT = "";
            string InputMAC = "STATIC_MAC_ADDRESS	= " + str_Mac;
            string InputBT = "STATIC_BD_MAC_ADDRESS	= " + str_BD;
            string strFilePath = $"{str_IQTestProgramFolderPath}\\Bin\\Serial_MAC.txt";
            try
            {
                StreamReader sr = File.OpenText(strFilePath);
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (strLine.IndexOf("STATIC_MAC_ADDRESS") >= 0 && strLine.Contains("="))
                    {
                        strOldMac = strLine;

                    }
                    if (strLine.IndexOf("STATIC_BD_MAC_ADDRESS") >= 0 && strLine.Contains("="))
                    {
                        strOldBT = strLine;
                        break;
                    }
                }
                sr.Close();
                if (File.Exists(strFilePath))
                {
                    string strContent = File.ReadAllText(strFilePath);
                    strContent = Regex.Replace(strContent, strOldMac, InputMAC);
                    strContent = Regex.Replace(strContent, strOldBT, InputBT);
                    File.WriteAllText(strFilePath, strContent);

                    Thread.Sleep(1000);

                    System.IO.TextReader filereader;//以讀方式打開文件
                    filereader = System.IO.File.OpenText(strFilePath);
                    string tempmac = filereader.ReadToEnd();
                    filereader.Close();
                    string pstrMAC = str_Mac;
                    if (tempmac.IndexOf(pstrMAC) < 0)
                    {
                        UIHandleHelper.ShowRunLog($"Check set mac to Serial_MAC.txt fail", true);
                        return false;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"Check set mac to Serial_MAC.txt pass");
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Set Mac ID to Serial_MAC.txt Error: {ex}", true);
                return false;
            }

            return false;
        }

        public bool CallFlowTest(string str_FlowBatPathAndName, string str_FlowLogPathAndName, int iRunFlowTimeout = 150, string str_FlowRunExeNotContainsSuffix = "IQfactRun_Console")
        {
            if (File.Exists(str_FlowLogPathAndName))
                File.Delete(str_FlowLogPathAndName);

            try
            {
                #region 调用flow脚本

                ProcessHelper.KillProcessByName("cmd");
                Thread.Sleep(50);
                IntPtr hwndApp = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");
                if (hwndApp == IntPtr.Zero)
                {
                    ShellExecute(IntPtr.Zero, "open", str_FlowBatPathAndName, "", "", ShowCommands.SW_SHOW);
                    Thread.Sleep(5000);
                    hwndApp = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");
                }
                else
                {
                    ProcessHelper.KillProcessByName("cmd");
                    Thread.Sleep(50);
                    ShellExecute(IntPtr.Zero, "open", str_FlowBatPathAndName, "", "", ShowCommands.SW_SHOW);
                    Thread.Sleep(5000);
                    hwndApp = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");
                }

                #endregion

                #region 循环判断是否运行结束

                int iCount = 0;
                while (true)
                {
                    Thread.Sleep(1000);

                    if (File.Exists(str_FlowLogPathAndName))
                    {
                        Thread.Sleep(1000);

                        ProcessHelper.KillProcessByName(str_FlowRunExeNotContainsSuffix);
                        ProcessHelper.KillProcessByName("cmd");
                        break;
                    }
                    else
                    {
                        hwndApp = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");
                        if (hwndApp == IntPtr.Zero)
                        {
                            UIHandleHelper.ShowRunLog("Test flow unexpectedly shuts down.", true);
                            return false;
                        }
                    }
                    

                    if (iCount > iRunFlowTimeout)
                    {
                        ProcessHelper.KillProcessByName(str_FlowRunExeNotContainsSuffix);
                        ProcessHelper.KillProcessByName("cmd");
                        UIHandleHelper.ShowRunLog("Run test flow timeout.", true);
                        return false;
                    }

                    iCount++;
                }

                #endregion

                #region 处理测试log

                int iPassTest= -1;
                String strLine = "";
                StreamReader sr = File.OpenText(str_FlowLogPathAndName);
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (strLine.ToLower().IndexOf("--- [failed]") >= 0 || strLine.ToLower().IndexOf("return error") >= 0)
                    {
                        UIHandleHelper.ShowRunLog($"fail info: \r\n {strLine}", true);
                        iPassTest = 0;
                    }

                    if (strLine.IndexOf("*  P A S S  *") >= 0)
                    {
                        if (iPassTest == 0)
                        {
                            //nothing
                        }
                        else
                        {
                            iPassTest = 1;
                        }
                    }

                    if (strLine.IndexOf("*  F A I L  *") >= 0)
                    {
                        iPassTest = 0;
                    }

                }
                sr.Close();

                if (iPassTest == 1)
                {
                    UIHandleHelper.ShowRunLog("Run test flow result is pass.");
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Run test flow result is fail.", true);
                    return false;
                }

                #endregion

            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Run test flow error: {ex}", true);
                return false;
            }

            //return false;
        }




        public (bool, string) CreateLitePointReadMacBat(string str_FlowPath, string str_FlowAndBatPartName, string str_IQfactStudioExe, bool bFailStop, string str_Flow_MoreParameters = "")
        {
            string str_Temp = "";
            string str_FailStop = bFailStop ? "1" : "0";

            try
            {

                UIHandleHelper.ShowRunLog("Create Read Flow...");
                str_Temp = "cd /d " + str_FlowPath + "\\Bin\r\n" +
               str_IQfactStudioExe + " -run " + str_FlowAndBatPartName + ".txt " + str_Flow_MoreParameters + " -ONFAIL " + str_FailStop + " -EXIT";



                TextWriter Filewriter;//以寫方式打開文件
                Filewriter = File.CreateText($"{FileProcessHelper.GetCurrentExeDirectory()}\\{str_FlowAndBatPartName}.bat");//創建或打開一個UTF-8的文件
                Filewriter.Write(str_Temp);//寫入
                Filewriter.Close();

            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Create LitePoint read mac Bat error: {ex}", true);
                return (false, null);
            }

            UIHandleHelper.ShowRunLog("Create LitePoint read mac flow bat ok.");
            return (true, $"{str_FlowAndBatPartName}.bat");
        }



        public bool CallFlowTest_ReadMac(string str_FlowBatPathAndName, string str_FlowLogPathAndName, string str_MarkMac, string str_MarkBt, int iRunFlowTimeout = 150, string str_FlowRunExeNotContainsSuffix = "IQfactRun_Console")
        {
            if (File.Exists(str_FlowLogPathAndName))
                File.Delete(str_FlowLogPathAndName);

            try
            {
                #region 调用flow脚本

                ProcessHelper.KillProcessByName("cmd");
                Thread.Sleep(50);
                IntPtr hwndApp = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");
                if (hwndApp == IntPtr.Zero)
                {
                    ShellExecute(IntPtr.Zero, "open", str_FlowBatPathAndName, "", "", ShowCommands.SW_SHOW);
                    Thread.Sleep(5000);
                    hwndApp = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");
                }
                else
                {
                    ProcessHelper.KillProcessByName("cmd");
                    Thread.Sleep(50);
                    ShellExecute(IntPtr.Zero, "open", str_FlowBatPathAndName, "", "", ShowCommands.SW_SHOW);
                    Thread.Sleep(5000);
                    hwndApp = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");
                }

                #endregion

                #region 循环判断是否运行结束

                int iCount = 0;
                while (true)
                {
                    Thread.Sleep(1000);

                    if (File.Exists(str_FlowLogPathAndName))
                    {
                        Thread.Sleep(1000);

                        ProcessHelper.KillProcessByName(str_FlowRunExeNotContainsSuffix);
                        ProcessHelper.KillProcessByName("cmd");
                        break;
                    }
                    else
                    {
                        hwndApp = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");
                        if (hwndApp == IntPtr.Zero)
                        {
                            UIHandleHelper.ShowRunLog("Test flow unexpectedly shuts down.", true);
                            return false;
                        }
                    }


                    if (iCount > iRunFlowTimeout)
                    {
                        ProcessHelper.KillProcessByName(str_FlowRunExeNotContainsSuffix);
                        ProcessHelper.KillProcessByName("cmd");
                        UIHandleHelper.ShowRunLog("Run test flow timeout.", true);
                        return false;
                    }

                    iCount++;
                }

                #endregion

                #region 处理测试log

                int iPassTest = -1;
                String strLine = "";
                string str_AllLines = "";
                StreamReader sr = File.OpenText(str_FlowLogPathAndName);
                while ((strLine = sr.ReadLine()) != null)
                {
                    str_AllLines += strLine;
                    if (strLine.ToLower().IndexOf("--- [failed]") >= 0 || strLine.ToLower().IndexOf("return error") >= 0)
                    {
                        UIHandleHelper.ShowRunLog($"fail info: \r\n {strLine}", true);
                        iPassTest = 0;
                    }

                    if (strLine.IndexOf("*  P A S S  *") >= 0)
                    {
                        if (iPassTest == 0)
                        {
                            //nothing
                        }
                        else
                        {
                            iPassTest = 1;
                        }
                    }

                    if (strLine.IndexOf("*  F A I L  *") >= 0)
                    {
                        iPassTest = 0;
                    }

                }
                sr.Close();

                if (iPassTest == 1)
                {
                    UIHandleHelper.ShowRunLog("Run test flow result is pass.");
                    //return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog("Run test flow result is fail.", true);
                    return false;
                }



                bool bCheckTest_Mac = false;
                bool bCheckTest_Bt = false;
                str_AllLines = str_AllLines.Replace(" ", "");

                if (string.IsNullOrEmpty(str_MarkMac) == false)
                {
                    if (str_AllLines.Contains(str_MarkMac))
                    {
                        UIHandleHelper.ShowRunLog($"Check pass: {str_MarkMac}");
                        bCheckTest_Mac = true;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"Check mac fail", true);
                    }
                }


                if (string.IsNullOrEmpty(str_MarkBt) == false)
                {
                    if (str_AllLines.Contains(str_MarkBt))
                    {
                        UIHandleHelper.ShowRunLog($"Check pass: {str_MarkBt}");
                        bCheckTest_Bt = true;
                    }
                    else
                    {
                        UIHandleHelper.ShowRunLog($"Check bt fail", true);
                    }
                }


                if (string.IsNullOrEmpty(str_MarkBt) == false && string.IsNullOrEmpty(str_MarkMac) == false)
                {
                    if (bCheckTest_Mac && bCheckTest_Bt)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (string.IsNullOrEmpty(str_MarkMac) == false)
                {
                    if (bCheckTest_Mac)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (string.IsNullOrEmpty(str_MarkBt) == false)
                {
                    if (bCheckTest_Bt)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }



                #endregion

            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"Run test flow error: {ex}", true);
                return false;
            }

            return false;
        }

    }
}
