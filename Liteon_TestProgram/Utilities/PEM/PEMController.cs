using NationalInstruments.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.PEM
{
    internal class PEMController
    {


        public bool IsPETSExist { get; private set; }



        public PEMController()
        {
            IsPETSExist = false;
        }


        #region 日志方法

        // 日志输出委托
        public Action<string, bool, bool, string, string, bool, string> UILogMessage { get; set; }

        public Action<string, bool> PEMLogMessage { get; set; }

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

        #endregion

        #region Dll中方法委托

        DllLibraryLoader loader = new DllLibraryLoader();

        // 定义非托管函数的委托签名
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int PETS_INIT();
        private delegate int PETS_VALIDPEM(uint addr);
        private delegate int PETS_CHKSHORT();
        private delegate int PETS_SELPEM(uint addr);
        private delegate int PETS_VALIDDEV();
        private delegate int PETS_POFF();
        private delegate int PETS_PON();
        private delegate int PETS_RETRAINDEV();
        private delegate int PETS_BEEP(int freq, int time);
        private delegate int PETS_WINCHECK();
        private delegate int PETS_WINEN2(int delaytime);
        private delegate int PETS_DEVRESCAN(int delaytime);
        private delegate int PETS_WINDIS(int delaytime);
        private delegate int PETS_SELDEVBD(uint Busno, uint Devno);
        private delegate int PETS_SELDEVFUNC(uint Busno, uint Devno, uint[] Func);
        private delegate int PETS_GETPNTYPE();
        private delegate int PETS_GET3V3V(double[] Volatge);
        private delegate int PETS_GET3V3I(double[] current);
        private delegate int PETS_GET1V5V(double[] Volatge);
        private delegate int PETS_GET1V5I(double[] current);
        private delegate int PETS_GET12V(double[] Volatge);
        private delegate int PETS_GET12I(double[] current);
        private delegate uint PETS_GETDEVHWVID();
        private delegate uint PETS_GETDEVHWDID();
        private delegate uint PETS_GETDEVHWSVID();
        private delegate uint PETS_GETDEVHWSDID();
        private delegate int PETS_EXIT();


        #endregion



        /// <summary>
        /// 初始化PEM
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            //初始化
            if(loader ==null)
            {
                loader = new DllLibraryLoader();
            }
            loader.Load("SolPeTsDll.dll"); // 加载 DLL
            var PETS_INIT_Func = loader.GetFunction<PETS_INIT>("PETS_INIT"); // 获取函数
            int status = -1;
            status = PETS_INIT_Func(); // 调用函数

            if (status != 0)
            {
                ShowPEMInfo("Failed to initialize PEM!", true);
                ShowUIInfo("Failed to initialize PEM!", true);
                return false;
            }

            //选择PEM
            var PETS_VALIDPEM_Func = loader.GetFunction<PETS_VALIDPEM>("PETS_VALIDPEM"); // 获取函数
            status = -1;
            uint currpemno = 0;
            for (uint i = 0; i < 4; i++)
            {
                if (PETS_VALIDPEM_Func(i) == 1)
                {
                    currpemno = i;
                    break;
                }
            }

            var PETS_SELPEM_Func = loader.GetFunction<PETS_SELPEM>("PETS_SELPEM"); // 获取函数
            status = -1;

            status = PETS_SELPEM_Func(currpemno);
            if (status != 0)
            {
                ShowPEMInfo("Failed to select PEM-1x!", true);
                ShowUIInfo("Failed to select PEM-1x!", true);
                return false;
            }


            //var PETS_VALIDDEV_Func = loader.GetFunction<PETS_VALIDDEV>("PETS_VALIDDEV"); // 获取函数
            //status = -1;
            //status = PETS_VALIDDEV_Func();
            //if (status != 1)
            //{
            //    MessageBox.Show("VALIDDEV Fail! Need to Assign A New Device!");
            //}

            var PETS_CHKSHORT_Func = loader.GetFunction<PETS_CHKSHORT>("PETS_CHKSHORT"); // 获取函数
            status = -1;
            status = PETS_CHKSHORT_Func();
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

            var PETS_PON_Func = loader.GetFunction<PETS_PON>("PETS_PON"); // 获取函数
            int status = -1;
            status = PETS_PON_Func();
            Thread.Sleep(500);

            if (status == 4)
            {
                var PETS_CHKSHORT_Func = loader.GetFunction<PETS_CHKSHORT>("PETS_CHKSHORT"); // 获取函数
                status = -1;
                status = PETS_CHKSHORT_Func();
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
                var PETS_RETRAINDEV_Func = loader.GetFunction<PETS_RETRAINDEV>("PETS_RETRAINDEV"); // 获取函数
                PETS_RETRAINDEV_Func();

                var PETS_GETDEVHWVID_Func = loader.GetFunction<PETS_GETDEVHWVID>("PETS_GETDEVHWVID"); // 获取函数
                var PETS_GETDEVHWDID_Func = loader.GetFunction<PETS_GETDEVHWDID>("PETS_GETDEVHWDID"); // 获取函数
                var PETS_GETDEVHWSVID_Func = loader.GetFunction<PETS_GETDEVHWSVID>("PETS_GETDEVHWSVID"); // 获取函数
                var PETS_GETDEVHWSDID_Func = loader.GetFunction<PETS_GETDEVHWSDID>("PETS_GETDEVHWSDID"); // 获取函数
                uint dut_vid = PETS_GETDEVHWVID_Func();
                uint dut_did = PETS_GETDEVHWDID_Func();
                uint dut_svid = PETS_GETDEVHWSVID_Func();
                uint dut_sdid = PETS_GETDEVHWSDID_Func();

                //ShowUIInfo($"Device VID: {dut_vid:X}, DID: {dut_did:X}, SVID: {dut_svid:X}, SDID: {dut_sdid:X}");
                //ShowPEMInfo($"Device VID: {dut_vid:X}, DID: {dut_did:X}, SVID: {dut_svid:X}, SDID: {dut_sdid:X}");

                var PETS_BEEP_Func = loader.GetFunction<PETS_BEEP>("PETS_BEEP"); // 获取函数
                PETS_BEEP_Func(2, 200);

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

            var PETS_POFF_Func = loader.GetFunction<PETS_POFF>("PETS_POFF"); // 获取函数
            int status = -1;
            status = PETS_POFF_Func();
            if (status != 0)
            {
                ShowPEMInfo("PEM Power Off Fail!", true);
                ShowUIInfo("PEM Power Off Fail!", true);
                return false;
            }
            else
            {
                var PETS_BEEP_Func = loader.GetFunction<PETS_BEEP>("PETS_BEEP"); // 获取函数
                PETS_BEEP_Func(1, 200);
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
            //if (!IsPETSExist)
            //{
            //    ShowPEMInfo("PEM card is not initialized!", true);
            //    ShowUIInfo("PEM card is not initialized!", true);
            //    return false;
            //}

            //var PETS_EXIT_Func = loader.GetFunction<PETS_EXIT>("PETS_EXIT"); // 获取函数
            //int status = -1;
            //status = PETS_EXIT_Func();

            //if (status != 0)
            //{
            //    ShowPEMInfo("PEM Exit Fail!", true);
            //    ShowUIInfo("PEM Exit Fail!", true);
            //    return false;
            //}
            //else
            //{
            //    IsPETSExist = false;
            //    loader.Unload(); // 卸载 DLL
            //    ShowPEMInfo("PEM exited.");
            //    ShowUIInfo("PEM exited.");
            //    return true;
            //}

            IsPETSExist = false;
            UILogMessage = null;
            PEMLogMessage = null;
            loader.Unload(); // 卸载 DLL
            ShowPEMInfo("PEM exited.");
            ShowUIInfo("PEM exited.");
            return true;
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

            var PETS_WINCHECK_Func = loader.GetFunction<PETS_WINCHECK>("PETS_WINCHECK"); // 获取函数
            int status = -1;
            status = PETS_WINCHECK_Func();
            if (status != 1)
            {
                var PETS_WINEN2_Func = loader.GetFunction<PETS_WINEN2>("PETS_WINEN2"); // 获取函数
                status = -1;
                status = PETS_WINEN2_Func(1000);
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

            var PETS_DEVRESCAN_Func = loader.GetFunction<PETS_DEVRESCAN>("PETS_DEVRESCAN"); // 获取函数
            PETS_DEVRESCAN_Func(500);
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

            var PETS_WINCHECK_Func = loader.GetFunction<PETS_WINCHECK>("PETS_WINCHECK"); // 获取函数
            int status = -1;
            status = PETS_WINCHECK_Func();
            if (status != 0)
            {
                var PETS_WINDIS_Func = loader.GetFunction<PETS_WINDIS>("PETS_WINDIS"); // 获取函数
                status = -1;
                status = PETS_WINDIS_Func(500);
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

            var PETS_GETPNTYPE_Func = loader.GetFunction<PETS_GETPNTYPE>("PETS_GETPNTYPE"); // 获取函数
            int card_type = -1;
            card_type = PETS_GETPNTYPE_Func();

            var PETS_GET3V3V_Func = loader.GetFunction<PETS_GET3V3V>("PETS_GET3V3V"); // 获取函数
            var PETS_GET3V3I_Func = loader.GetFunction<PETS_GET3V3I>("PETS_GET3V3I"); // 获取函数
            PETS_GET3V3V_Func(g_3v3_v);
            PETS_GET3V3I_Func(g_3v3_i);

            if (card_type == 0)
            {
                var PETS_GET1V5V_Func = loader.GetFunction<PETS_GET1V5V>("PETS_GET1V5V"); // 获取函数
                var PETS_GET1V5I_Func = loader.GetFunction<PETS_GET1V5I>("PETS_GET1V5I"); // 获取函数
                PETS_GET1V5V_Func(g_1v5_v);
                PETS_GET1V5I_Func(g_1v5_i);

                voltages[0] = g_3v3_v[0];
                voltages[1] = g_1v5_v[0];
                currents[0] = g_3v3_i[0];
                currents[1] = g_1v5_i[0];

                //ShowUIInfo($"3.3V: {g_3v3_v[0]:F3}V, {g_3v3_i[0]:F3}A | 1.5V: {g_1v5_v[0]:F3}V, {g_1v5_i[0]:F3}A");
                //ShowPEMInfo($"3.3V: {g_3v3_v[0]:F3}V, {g_3v3_i[0]:F3}A | 1.5V: {g_1v5_v[0]:F3}V, {g_1v5_i[0]:F3}A");
            }
            else
            {
                var PETS_GET12V_Func = loader.GetFunction<PETS_GET12V>("PETS_GET12V"); // 获取函数
                var PETS_GET12I_Func = loader.GetFunction<PETS_GET12I>("PETS_GET12I"); // 获取函数
                PETS_GET12V_Func(g_12v_v);
                PETS_GET12I_Func(g_12v_i);

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
                var PETS_SELDEVBD_Func = loader.GetFunction<PETS_SELDEVBD>("PETS_SELDEVBD"); // 获取函数
                int status = -1;
                status = PETS_SELDEVBD_Func(Busno, Devno);

                if (status != 0)
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
                ShowPEMInfo($"PEM select device err (Busno: {Busno}, Devno: {Devno}) : {ex.Message}", true);
                ShowUIInfo($"PEM select device err (Busno: {Busno}, Devno: {Devno}) : {ex.Message}", true);
            }

            return false;

        }




    }
}
