using Liteon_TestProgram.CaseProject;
using Liteon_TestProgram.Utilities;
using Liteon_TestProgram.Utilities.PEM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Liteon_TestProgram.Forms
{
    public partial class ToolForm_02 : Form
    {

        protected string _str_CaseName = "";

        private PEMController pEMController = new PEMController();



        public ToolForm_02(MainForm mainForm, string str_CaseName)
        {
            _str_CaseName = str_CaseName;
            InitializeComponent();

            btn_PowerOn.Enabled = false;
            btn_PowerOff.Enabled = false;
            btn_SelectDev.Enabled = false;
            btn_ReadVI.Enabled = false;
            btn_Exit.Enabled = false;
        }


        private void ToolForm_02_Load(object sender, EventArgs e)
        {
            UIHandleHelper.PEMLogBox = richTextBox_PEMInfo;
            pEMController.PEMLogMessage = UIHandleHelper.ShowPEMInfo; // 绑定日志输出方法
        }



        private async void btn_Init_ClickAsync(object sender, EventArgs e)
        {
            if (pEMController == null)
            {
                pEMController = new PEMController();
                UIHandleHelper.PEMLogBox = richTextBox_PEMInfo;
                pEMController.PEMLogMessage = UIHandleHelper.ShowPEMInfo; // 绑定日志输出方法
            }

            UIHandleHelper.ControlHandle(richTextBox_PEMInfo, () => richTextBox_PEMInfo.Text = "");
            var testResult = await Task.Run(() => pEMController.Initialize());

            if (testResult)
            {
                UIHandleHelper.ControlHandle(btn_PowerOn, () => btn_PowerOn.Enabled = true);
                UIHandleHelper.ControlHandle(btn_PowerOff, () => btn_PowerOff.Enabled = true);
                UIHandleHelper.ControlHandle(btn_SelectDev, () => btn_SelectDev.Enabled = true);
                UIHandleHelper.ControlHandle(btn_ReadVI, () => btn_ReadVI.Enabled = true);
                UIHandleHelper.ControlHandle(btn_Exit, () => btn_Exit.Enabled = true);
                UIHandleHelper.ControlHandle(btn_Init, () => btn_Init.Enabled = false);


                Timer_RD_VI.Interval = 2000;
                Timer_RD_VI.Enabled = true;
            }
        }

        private async void btn_SelectDev_ClickAsync(object sender, EventArgs e)
        {
            bool bResult = false;

            var ret = MessageBoxEX.Show("是否使用系统默认的设备Busno和Devno, 使用默认的请选Y\r\n使用指定的请先填写BusNo和DevNo, 选N", false);

            if (ret == DialogResult.Yes)
            {
                return;
            }

            try
            {
                ret = MessageBoxEX.Show("是否查看如何填写BusNo和DevNo", false);

                if (ret == DialogResult.Yes)
                {
                    PictureViewer.Show("BusNo 和 DevNo", "PEM_SelDev.jpg", true);
                    return;
                }

                uint iBusno = uint.Parse(UIHandleHelper.GetControlText(textBox_BusNo));
                uint iDevno = uint.Parse(UIHandleHelper.GetControlText(textBox_DevNo));

                bResult = await Task.Run(() => pEMController.SelectDevice(iBusno, iDevno));

                if (!bResult)
                {
                    UIHandleHelper.ControlHandle(btn_PowerOn, () => btn_PowerOn.Enabled = false);
                    UIHandleHelper.ControlHandle(btn_PowerOff, () => btn_PowerOff.Enabled = false);
                }

            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowPEMInfo($"PEM select device err: {ex.Message}", true);
                UIHandleHelper.ControlHandle(btn_PowerOn, () => btn_PowerOn.Enabled = false);
                UIHandleHelper.ControlHandle(btn_PowerOff, () => btn_PowerOff.Enabled = false);
            }

        }

        private async void btn_PowerOn_ClickAsync(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                pEMController.PowerOn();
            });
        }

        private async void btn_PowerOff_ClickAsync(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                pEMController.PowerOff();
            });
        }

        private void btn_ReadVI_Click(object sender, EventArgs e)
        {
            double[] voltages;
            double[] currents;

            // 调用方法
            var vResult = pEMController.ReadVoltageCurrent(out voltages, out currents);

            // 处理返回值
            if (voltages != null && currents != null)
            {
                UIHandleHelper.ControlHandle(lb_Voltage_3V3_Value, () => lb_Voltage_3V3_Value.Text = voltages[0].ToString("F3"));
                UIHandleHelper.ControlHandle(lb_Current_3V3_Value, () => lb_Current_3V3_Value.Text = currents[0].ToString("F3"));

                if (vResult.Item2 == 0)
                {
                    UIHandleHelper.ControlHandle(lb_Volatge_Value, () => lb_Volatge_Value.Text = voltages[1].ToString("F3"));
                    UIHandleHelper.ControlHandle(lb_Current_Value, () => lb_Current_Value.Text = currents[1].ToString("F3"));

                    UIHandleHelper.ControlHandle(lb_VOLT_TYPE, () => lb_VOLT_TYPE.Text = "1.5V Volatge(V)");
                    UIHandleHelper.ControlHandle(lb_CURR_TYPE, () => lb_CURR_TYPE.Text = "1.5V Current(A)");
                }
                else
                {
                    UIHandleHelper.ControlHandle(lb_Volatge_Value, () => lb_Volatge_Value.Text = voltages[2].ToString("F3"));
                    UIHandleHelper.ControlHandle(lb_Current_Value, () => lb_Current_Value.Text = currents[2].ToString("F3"));

                    UIHandleHelper.ControlHandle(lb_VOLT_TYPE, () => lb_VOLT_TYPE.Text = "12V Volatge(V)");
                    UIHandleHelper.ControlHandle(lb_CURR_TYPE, () => lb_CURR_TYPE.Text = "12V Current(A)");
                }


                //cmd打印信息
                Console.WriteLine("Voltages:");
                for (int i = 0; i < voltages.Length; i++)
                {
                    Console.WriteLine($"  Voltage[{i}]: {voltages[i]:F3} V");
                }

                Console.WriteLine("Currents:");
                for (int i = 0; i < currents.Length; i++)
                {
                    Console.WriteLine($"  Current[{i}]: {currents[i]:F3} A");
                }
            }
            else
            {
                UIHandleHelper.ShowPEMInfo("Failed to read voltage and current.", true);
            }
        }

        private async void btn_Exit_ClickAsync(object sender, EventArgs e)
        {
            if (await Task.Run(() => pEMController.Exit()))
            {
                UIHandleHelper.ControlHandle(btn_PowerOn, () => btn_PowerOn.Enabled = false);
                UIHandleHelper.ControlHandle(btn_PowerOff, () => btn_PowerOff.Enabled = false);
                UIHandleHelper.ControlHandle(btn_SelectDev, () => btn_SelectDev.Enabled = false);
                UIHandleHelper.ControlHandle(btn_ReadVI, () => btn_ReadVI.Enabled = false);
                UIHandleHelper.ControlHandle(btn_Exit, () => btn_Exit.Enabled = false);
                UIHandleHelper.ControlHandle(btn_Init, () => btn_Init.Enabled = true);

                Timer_RD_VI.Stop();
                Timer_RD_VI.Dispose(); // 如果不再需要定时器，可以调用Dispose释放资源

                UIHandleHelper.ControlHandle(lb_Voltage_3V3_Value, () => lb_Voltage_3V3_Value.Text = "0.00");
                UIHandleHelper.ControlHandle(lb_Current_3V3_Value, () => lb_Current_3V3_Value.Text = "0.00");

                UIHandleHelper.ControlHandle(lb_Volatge_Value, () => lb_Volatge_Value.Text = "0.00");
                UIHandleHelper.ControlHandle(lb_Current_Value, () => lb_Current_Value.Text = "0.00");



                pEMController = null;
               
            }

            this.Invalidate();
            this.Refresh();
            return;
        }

        private void Timer_RD_VI_Tick(object sender, EventArgs e)
        {
            Task.Run(() => { btn_ReadVI_Click(sender, e); });           
        }


      

    }
}
