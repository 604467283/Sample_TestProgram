using NationalInstruments.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.PEM
{
    internal class PEMController_1 : IDisposable
    {
        // 日志输出委托
        public Action<string, bool, bool, string, string, bool, string> UILogMessage { get; set; }

        public Action<string, bool> PEMLogMessage { get; set; }

        public bool IsPETSExist { get; private set; }

        #region 加载DLL接口

        // DLL Imports
        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_INIT")]
        private static extern int PETS_INIT();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_SELPEM")]
        private static extern int PETS_SELPEM(uint addr);

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_VALIDPEM")]
        private static extern int PETS_VALIDPEM(uint addr);

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_GETPWRSTS")]
        private static extern int PETS_GETPWRSTS();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_POFF")]
        private static extern int PETS_POFF();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_PON")]
        private static extern int PETS_PON();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_EXIT")]
        private static extern int PETS_EXIT();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_BEEP")]
        private static extern int PETS_BEEP(int freq, int time);

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_GETPNTYPE")]
        private static extern int PETS_GETPNTYPE();

        [DllImport("SolPeTsDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PETS_GET3V3V(double[] voltage);

        [DllImport("SolPeTsDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PETS_GET3V3I(double[] current);

        [DllImport("SolPeTsDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PETS_GET1V5V(double[] voltage);

        [DllImport("SolPeTsDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PETS_GET1V5I(double[] current);

        [DllImport("SolPeTsDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PETS_GET12V(double[] voltage);

        [DllImport("SolPeTsDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PETS_GET12I(double[] current);

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_WINCHECK")]
        private static extern int PETS_WINCHECK();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_WINEN2")]
        private static extern int PETS_WINEN2(int delaytime);

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_WINDIS")]
        private static extern int PETS_WINDIS(int delaytime);

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_CHKSHORT")]
        private static extern int PETS_CHKSHORT();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_SELDEVBD")]
        private static extern int PETS_SELDEVBD(uint Busno, uint Devno);

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_VALIDDEV")]
        private static extern int PETS_VALIDDEV();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_RETRAINDEV")]
        private static extern int PETS_RETRAINDEV();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_GETDEVHWVID")]
        private static extern uint PETS_GETDEVHWVID();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_GETDEVHWDID")]
        private static extern uint PETS_GETDEVHWDID();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_GETDEVHWSVID")]
        private static extern uint PETS_GETDEVHWSVID();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_GETDEVHWSDID")]
        private static extern uint PETS_GETDEVHWSDID();

        [DllImport("SolPeTsDll.dll", EntryPoint = "PETS_DEVRESCAN")]
        private static extern int PETS_DEVRESCAN(int delaytime);

        #endregion



        public PEMController_1()
        {
            IsPETSExist = false;
            components = new Container(); // 初始化组件容器
        }

        // 日志输出方法
        private void ShowUIInfo(string message, bool failColor = false, bool showGridView = false,
                         string testItemName = null, string testItemContent = null,
                         bool testItemResult = false, string errorCode = "Err000")
        {
            if (UILogMessage != null)
            {
                // 确保线程安全
                if (UILogMessage.Target is Control control && control.InvokeRequired)
                {
                    control.Invoke(new Action(() => UILogMessage(message, failColor, showGridView, testItemName, testItemContent, testItemResult, errorCode)));
                }
                else
                {
                    UILogMessage(message, failColor, showGridView, testItemName, testItemContent, testItemResult, errorCode);
                }
            }
        }

        // PEM 信息输出方法
        private void ShowPEMInfo(string content, bool isFail = false)
        {
            if (PEMLogMessage != null)
            {
                // 确保线程安全
                if (PEMLogMessage.Target is Control control && control.InvokeRequired)
                {
                    control.Invoke(new Action(() => PEMLogMessage(content, isFail)));
                }
                else
                {
                    PEMLogMessage(content, isFail);
                }
            }
        }


        private void HandleShortCircuit(int status)
        {
            if (status == 1)
            {
                ShowPEMInfo("1.5V Short!", true);
                ShowUIInfo("1.5V Short!", true);
            }
            else if (status == 2)
            {
                ShowPEMInfo("3.3V Short!", true);
                ShowUIInfo("3.3V Short!", true);
            }
            else if (status == 4)
            {
                ShowPEMInfo("3.3AUX Short!", true);
                ShowUIInfo("3.3AUX Short!", true);
            }
            
        }

        /// <summary>
        /// 初始化PEM
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            int status = PETS_INIT();
            if (status != 0)
            {
                ShowPEMInfo("Failed to initialize PEM!", true);
                ShowUIInfo("Failed to initialize PEM!", true);
                return false;
            }

            uint currpemno = 0;
            for (uint i = 0; i < 4; i++)
            {
                if (PETS_VALIDPEM(i) == 1)
                {
                    currpemno = i;
                    break;
                }
            }

            status = PETS_SELPEM(currpemno);
            if (status != 0)
            {
                ShowPEMInfo("Failed to select PEM-1x!", true);
                ShowUIInfo("Failed to select PEM-1x!", true);
                return false;
            }

            //status = PETS_VALIDDEV();
            //if (status != 1)
            //{
            //    MessageBox.Show("VALIDDEV Fail! Need to Assign A New Device!");
            //}

            status = PETS_CHKSHORT();
            if (status != 0)
            {
                HandleShortCircuit(status);
                return false;
            }

            IsPETSExist = true;

            ShowPEMInfo("Init PEM OK.");
            ShowUIInfo("Init PEM OK.");
            return true;
        }

        /// <summary>
        /// PEM上电, 返回结果和VID,DID,SVID,SDID信息，ID之间使用分号隔开
        /// </summary>
        public (bool, string) PowerOn()
        {
            if (!IsPETSExist)
            {
                ShowPEMInfo("PEM card is not initialized!", true);
                ShowUIInfo("PEM card is not initialized!", true);
                return (false, null);
            }

            int status = PETS_PON();
            Thread.Sleep(500);

            if (status == 4)
            {
                status = PETS_CHKSHORT();
                if (status != 0)
                {
                    ShowPEMInfo("PEM Power On Fail!", true);
                    ShowUIInfo("PEM Power On Fail!", true);
                    HandleShortCircuit(status);
                    return (false, null);
                }
            }

            if (status == 0)
            {
                Thread.Sleep(1000);
                PETS_RETRAINDEV();

                uint dut_vid = PETS_GETDEVHWVID();
                uint dut_did = PETS_GETDEVHWDID();
                uint dut_svid = PETS_GETDEVHWSVID();
                uint dut_sdid = PETS_GETDEVHWSDID();

                ShowUIInfo($"Device VID: {dut_vid:X}, DID: {dut_did:X}, SVID: {dut_svid:X}, SDID: {dut_sdid:X}");
                ShowPEMInfo($"Device VID: {dut_vid:X}, DID: {dut_did:X}, SVID: {dut_svid:X}, SDID: {dut_sdid:X}");

                PETS_BEEP(1, 200);

                if (!EnableDriver())
                {
                    return (false, null);
                }

                ShowPEMInfo("PEM Power On OK!");
                ShowUIInfo("PEM Power On OK!");
                return (true, $"{dut_vid};{dut_did};{dut_svid};{dut_sdid}");
            }
            else
            {
                ShowPEMInfo("PEM Power On Fail!", true);
                ShowUIInfo("PEM Power On Fail!", true);
                return (false, null);
            }
        }


        /// <summary>
        /// PEM断电
        /// </summary>
        /// <returns></returns>
        public bool PowerOff()
        {
            if (!IsPETSExist)
            {
                ShowPEMInfo("PEM card is not initialized!", true);
                ShowUIInfo("PEM card is not initialized!", true);
                return false;
            }

            if (!DisableDriver()) 
            { 
                return false;
            }
                
            int status = PETS_POFF();
            if (status != 0)
            {
                ShowPEMInfo("PEM Power Off Fail!", true);
                ShowUIInfo("PEM Power Off Fail!", true);
                return false;
            }
            else
            {
                PETS_BEEP(1, 200);
                ShowPEMInfo("PEM Power Off Success.");
                ShowUIInfo("PEM Power Off Success.");
                return true;
            }
        }

        /// <summary>
        /// PEM退出
        /// </summary>
        /// <returns></returns>
        public bool Exit()
        {
            if (!IsPETSExist)
            {
                ShowPEMInfo("PEM card is not initialized!", true);
                ShowUIInfo("PEM card is not initialized!", true);
                return false;
            }

            int status = PETS_EXIT();

            if (status != 0)
            {
                ShowPEMInfo("PEM Exit Fail!", true);
                ShowUIInfo("PEM Exit Fail!", true);
                return false;
            }
            else
            {
                IsPETSExist = false;
                Dispose();
                ShowPEMInfo("PEM exited.");
                ShowUIInfo("PEM exited.");
                return true;
            }
        }


        /// <summary>
        /// PEM 启用device，需要先设定busno和devno
        /// </summary>
        private bool EnableDriver()
        {
            if (!IsPETSExist)
            {
                ShowPEMInfo("PEM card is not initialized!", true);
                ShowUIInfo("PEM card is not initialized!", true);
                return false;
            }

            int status = PETS_WINCHECK();
            if (status != 1)
            {
                status = PETS_WINEN2(1000);
                if (status != 0)
                {
                    ShowPEMInfo("PEM Failed to enable DUT driver!", true);
                    ShowUIInfo("PEM Failed to enable DUT driver!", true);
                    return false;
                }
                else
                {
                    ShowPEMInfo("PEM: DUT driver enabled-0.");
                    ShowUIInfo("PEM: DUT driver enabled-0.");
                    return true;
                }
            }

            PETS_DEVRESCAN(500);
            ShowPEMInfo("PEM: DUT driver enabled-1.");
            ShowUIInfo("PEM: DUT driver enabled.-1");
            return true;
        }

        /// <summary>
        /// PEM 禁用device，需要先设定busno和devno
        /// </summary>
        private bool DisableDriver()
        {
            if (!IsPETSExist)
            {
                ShowPEMInfo("PEM card is not initialized!", true);
                ShowUIInfo("PEM card is not initialized!", true);
                return false;
            }

            int status = PETS_WINCHECK();
            if (status != 0)
            {
                status = PETS_WINDIS(500);
                if (status != 0)
                {
                    ShowUIInfo("Failed to disable DUT driver!", true); // 失败时使用红色
                    ShowPEMInfo("Failed to disable DUT driver!", true); // 同时输出到 PEM 信息
                }
                else
                {
                    ShowUIInfo("PEM: DUT driver disabled-0.");
                    ShowPEMInfo("PEM: DUT driver disabled-0.");
                }
            }

            ShowPEMInfo("PEM: DUT driver disabled-1.");
            ShowUIInfo("PEM: DUT driver disabled.-1");
            return true;

        }


        /// <summary>
        /// PEM读取VI信息, voltages和currents各有3各元素，用于存放3.3V 1.5V 12V的电压电流值，元素0存放3.3的值，元素1存放1.5的值，元素2存放12的值；
        /// 如果返回的int值是0，返回的数据是3.3V和1.5V的
        /// 如果返回的int值是1，返回的数据是3.3V和12V的
        /// </summary>
        /// <param name="voltages"></param>
        /// <param name="currents"></param>
        /// <returns></returns>
        public (bool, int) ReadVoltageCurrent(out double[] voltages, out double[] currents)
        {
            voltages = new double[3];
            currents = new double[3];

            if (!IsPETSExist)
            {
                ShowPEMInfo("PEM card is not initialized!", true);
                ShowUIInfo("PEM card is not initialized!", true);
                return (false, 0);
            }

            double[] g_3v3_v = new double[1];
            double[] g_3v3_i = new double[1];
            double[] g_1v5_v = new double[1];
            double[] g_1v5_i = new double[1];
            double[] g_12v_v = new double[1];
            double[] g_12v_i = new double[1];

            int card_type = PETS_GETPNTYPE();

            PETS_GET3V3V(g_3v3_v);
            PETS_GET3V3I(g_3v3_i);

            if (card_type == 0)
            {
                PETS_GET1V5V(g_1v5_v);
                PETS_GET1V5I(g_1v5_i);

                voltages[0] = g_3v3_v[0];
                voltages[1] = g_1v5_v[0];
                currents[0] = g_3v3_i[0];
                currents[1] = g_1v5_i[0];

                //ShowUIInfo($"3.3V: {g_3v3_v[0]:F3}V, {g_3v3_i[0]:F3}A | 1.5V: {g_1v5_v[0]:F3}V, {g_1v5_i[0]:F3}A");
                //ShowPEMInfo($"3.3V: {g_3v3_v[0]:F3}V, {g_3v3_i[0]:F3}A | 1.5V: {g_1v5_v[0]:F3}V, {g_1v5_i[0]:F3}A");
            }
            else
            {
                PETS_GET12V(g_12v_v);
                PETS_GET12I(g_12v_i);

                voltages[0] = g_3v3_v[0];
                voltages[2] = g_12v_v[0];
                currents[0] = g_3v3_i[0];
                currents[2] = g_12v_i[0];

                //ShowUIInfo($"3.3V: {g_3v3_v[0]:F3}V, {g_3v3_i[0]:F3}A | 12V: {g_12v_v[0]:F3}V, {g_12v_i[0]:F3}A");
                //ShowPEMInfo($"3.3V: {g_3v3_v[0]:F3}V, {g_3v3_i[0]:F3}A | 12V: {g_12v_v[0]:F3}V, {g_12v_i[0]:F3}A");
            }

            return (true, card_type);
        }


        public bool SelectDevice(uint Busno, uint Devno)
        {
            if (!IsPETSExist)
            {
                ShowPEMInfo("PEM card is not initialized!", true);
                ShowUIInfo("PEM card is not initialized!", true);
                return false;
            }

            try
            {
                int a = PETS_SELDEVBD(Busno, Devno);

                if (a != 0) 
                {
                    ShowPEMInfo($"PEM select device (Busno: {Busno}, Devno: {Devno}) fail.", true);
                    ShowUIInfo($"PEM select device (Busno: {Busno}, Devno: {Devno}) fail.", true);
                    return false;
                }


                ShowPEMInfo($"PEM select device (Busno: {Busno}, Devno: {Devno}) OK.");
                ShowUIInfo($"PEM select device (Busno: {Busno}, Devno: {Devno}) OK.");
                return true;

            }
            catch (Exception ex)
            {
                ShowPEMInfo($"PEM select device err (Busno: {Busno}, Devno: {Devno}) : {ex}", true);
                ShowUIInfo($"PEM select device err (Busno: {Busno}, Devno: {Devno}) : {ex}", true);
            }

            return false;

        }







        // 标记是否已释放资源
        private bool _disposed = false;

        // 组件容器（如果需要）
        private IContainer components;

        // 实现 IDisposable 接口
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // 释放资源
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    if (components != null)
                    {
                        components.Dispose();
                        components = null;
                    }

                    // 释放其他托管资源
                    UILogMessage = null;
                    PEMLogMessage = null;
                }

                // 释放非托管资源（如果有）
                //PETS_EXIT();

                _disposed = true;
            }
        }

        // 析构函数
        ~PEMController_1()
        {
            Dispose(false);
        }







    }
}
