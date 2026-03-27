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
    public partial class ThroughputInfoConfirm : Form
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

        private string str_2G_Tips = string.Empty;
        private string str_5G_Tips = string.Empty;
        private string str_6G_Tips = string.Empty;
        private string str_7G_Tips = string.Empty;

        public ThroughputInfoConfirm(string Iperf_Server_IP,
                                                    string Check_WIFI_IDs, string Check_BD_IDs,
                                                    bool bTest_WIFI, bool bTest_BT,
                                                    string str_RemoteBTName, string str_BTPicPathAndName,
                                                    bool bTest_2G, bool bTest_5G, bool bTest_6G, bool bTest_7G,
                                                    string str_2G_SSID, string str_5G_SSID, string str_6G_SSID, string str_7G_SSID,
                                                    bool bBT_Transfer_Pic, bool bBT_Transfer_Music, string str_BTMusicPathAndName, string str_BTGuid,
                                                    bool bMesSwitch, string str_MesIP, string str_Station, string str_MO, string str_WorkerNum, string str_UploadInfo,
                                                    int Speed2G_Tx_LowerLimit, int Speed5G_Tx_LowerLimit, int Speed6G_Tx_LowerLimit, int Speed7G_Tx_LowerLimit,
                                                    int Speed2G_Rx_LowerLimit, int Speed5G_Rx_LowerLimit, int Speed6G_Rx_LowerLimit, int Speed7G_Rx_LowerLimit)
        {
            InitializeComponent();


            textBox_Iperf_Server_IP.Text = Iperf_Server_IP;
            textBox_Check_WIFI_IDs.Text = Check_WIFI_IDs;
            textBox_Check_BT_IDs.Text = Check_BD_IDs;


            textBox_2GSSID.Text = str_2G_SSID;
            textBox_5GSSID.Text = str_5G_SSID;
            textBox_6GSSID.Text = str_6G_SSID;
            textBox_7GSSID.Text = str_7G_SSID;

            textBox_RemoteBTName.Text = str_RemoteBTName;
            textBox_BTPicPathAndName.Text = str_BTPicPathAndName;
            checkBox_BTMusic.Checked = bBT_Transfer_Music;
            checkBox_BTPic.Checked = bBT_Transfer_Pic;
            textBox_BTMusicPathAndName.Text = str_BTMusicPathAndName;
            textBox_BTGuid.Text = str_BTGuid;

            checkBox_2G.Checked = bTest_2G;
            checkBox_5G.Checked = bTest_5G;
            checkBox_6G.Checked = bTest_6G;
            checkBox_7G.Checked = bTest_7G;
            checkBox_WIFI.Checked = bTest_WIFI;
            checkBox_BT.Checked = bTest_BT;

            checkBox_MesSwitch.Checked = bMesSwitch;
            textBox_MesIP.Text = str_MesIP;
            comboBox_FTStation.Text = str_Station;
            textBox_MO.Text = str_MO;
            textBox_WorkerNum.Text = str_WorkerNum;
            textBox_UploadInfo.Text = str_UploadInfo;

            if (checkBox_WIFI.Checked == false)
            {
                checkBox_2G.Enabled = false;
                checkBox_5G.Enabled = false;
                checkBox_6G.Enabled = false;
                checkBox_7G.Enabled = false;
                textBox_Check_WIFI_IDs.Enabled = false;

                textBox_2GSSID.Enabled = false;
                textBox_5GSSID.Enabled = false;
                textBox_6GSSID.Enabled = false;
                textBox_7GSSID.Enabled = false;
            }

            if (checkBox_BT.Checked == false)
            {
                textBox_Check_BT_IDs.Enabled = false;
                textBox_RemoteBTName.Enabled = false;
                textBox_BTPicPathAndName.Enabled = false;
                checkBox_BTMusic.Enabled = false;
                checkBox_BTPic.Checked = false;
                textBox_BTMusicPathAndName.Enabled = false;
                textBox_BTGuid.Enabled = false;
            }

            if (checkBox_MesSwitch.Checked == false)
            {
                textBox_MesIP.Enabled = false;
                comboBox_FTStation.Enabled = false;
                textBox_MO.Enabled = false;
                textBox_WorkerNum.Enabled = false;
                textBox_UploadInfo.Enabled = false;
            }


            if (bTest_2G)
            {
                str_2G_Tips = $"2G Tx最低速度 {Speed2G_Tx_LowerLimit} Mbits/sec\r\n2G Rx最低速度 {Speed2G_Rx_LowerLimit} Mbits/sec\r\n";
            }

            if (bTest_5G)
            {
                str_5G_Tips = $"5G Tx最低速度 {Speed5G_Tx_LowerLimit} Mbits/sec\r\n5G Rx最低速度 {Speed5G_Rx_LowerLimit} Mbits/sec\r\n";
            }

            if (bTest_6G)
            {
                str_6G_Tips = $"6G Tx最低速度 {Speed6G_Tx_LowerLimit} Mbits/sec\r\n6G Rx最低速度 {Speed6G_Rx_LowerLimit} Mbits/sec\r\n";
            }

            if (bTest_7G)
            {
                str_7G_Tips = $"7G Tx最低速度 {Speed7G_Tx_LowerLimit} Mbits/sec\r\n7G Rx最低速度 {Speed7G_Rx_LowerLimit} Mbits/sec\r\n";
            }


        }

        private void btn_Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_OK_MouseMove(object sender, MouseEventArgs e)
        {
            string str_Tips_All = string.Empty;

            if (!string.IsNullOrEmpty(str_2G_Tips))
            {
                str_Tips_All += str_2G_Tips;
            }

            if (!string.IsNullOrEmpty(str_5G_Tips))
            {
                str_Tips_All += str_5G_Tips;
            }

            if (!string.IsNullOrEmpty(str_6G_Tips))
            {
                str_Tips_All += str_6G_Tips;
            }

            if (!string.IsNullOrEmpty(str_7G_Tips))
            {
                str_Tips_All += str_7G_Tips;
            }

            toolTip_Msg.SetToolTip(btn_OK, str_Tips_All);
        }

        private void checkBox_MesSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_MesSwitch.Checked == false)
            {
                textBox_MesIP.Enabled = false;
                comboBox_FTStation.Enabled = false;
                textBox_MO.Enabled = false;
                textBox_WorkerNum.Enabled = false;
                textBox_UploadInfo.Enabled = false;
            }
            else
            {
                textBox_MesIP.Enabled = true;
                comboBox_FTStation.Enabled = true;
                textBox_MO.Enabled = true;
                textBox_WorkerNum.Enabled = true;
                textBox_UploadInfo.Enabled = true;
            }
        }



        #region IperfAndBT

        public string _Iperf_Server_IP
        {
            get { return textBox_Iperf_Server_IP.Text?.ToString(); }
        }

        public string _RemoteBTName
        {
            get { return textBox_RemoteBTName.Text?.ToString(); }
        }

        public string _BTPicPathAndName
        {
            get { return textBox_BTPicPathAndName.Text?.ToString(); }
        }

        public bool _BT_Transfer_Pic
        {
            get { return checkBox_BTPic.Checked; }
        }

        public bool _BT_Transfer_Music
        {
            get { return checkBox_BTMusic.Checked; }
        }

        public string _BTMusicPathAndName
        {
            get { return textBox_BTMusicPathAndName.Text?.ToString(); }
        }

        public string _BTGuid
        {
            get { return textBox_BTGuid.Text?.ToString(); }
         }

        #endregion

        #region Mes

        public bool _MesSwitch
        {
            get { return checkBox_MesSwitch.Checked; }
        }

        public string _MesIP
        {
            get { return textBox_MesIP.Text?.ToString(); }
        }

        public string _Station
        {
            get { return comboBox_FTStation.Text?.ToString(); }
        }

        public string _MO
        {
            get { return textBox_MO.Text?.ToString(); }
        }

        public string _WorkerNum
        {
            get { return textBox_WorkerNum.Text?.ToString(); }
        }

        public string _UploadInfo
        {
            get { return textBox_UploadInfo.Text?.ToString(); }
        }

        #endregion

    }
}
