using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Liteon_TestProgram.Forms
{
    public partial class LogCollectSettingForm : Form
    {
        public LogCollectSettingForm()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        #region 通用

        public bool _CheckBox_Standard
        {
            get { return checkBox_Standard.Checked; }
        }

        public bool _CheckBox_Result
        {
            get { return checkBox_Result.Checked; }
        }

        #endregion

        #region Wifi

        public bool _CheckBox_EVM
        {
            get { return checkBox_EVM.Checked; }
        }

        public bool _CheckBox_FreqError
        {
            get { return checkBox_FreqError.Checked; }
        }

        public bool _CheckBox_SymClkError
        {
            get { return checkBox_SymClkError.Checked; }
        }

        public bool _CheckBox_Power
        {
            get { return checkBox_Power.Checked; }
        }

        public bool _CheckBox_LOLeakage
        {
            get { return checkBox_LOLeakage.Checked; }
        }

        public bool _CheckBox_SpectrumMask
        {
            get { return checkBox_SpectrumMask.Checked; }
        }

        public bool _CheckBox_RxPower
        {
            get { return checkBox_WifiRXPower.Checked; }
        }

        public bool _CheckBox_RxPer
        {
            get { return checkBox_WifiRxPer.Checked; }
        }

        public bool _CheckBox_Rssi
        {
            get { return checkBox_Rssi.Checked; }
        }


        #endregion


        #region BT

        public bool _CheckBox_BtFreqOffset
        {
            get { return checkBox_BtFreqOffset.Checked; }
        }

        public bool _CheckBox_BtPower
        {
            get { return checkBox_BtPower.Checked; }
        }

        public bool _CheckBox_BtInitFreqErr
        {
            get { return checkBox_BtInitFreqErr.Checked; }
        }

        public bool _CheckBox_BtRxPower
        {
            get { return checkBox_BtRxPower.Checked; }
        }

        public bool _CheckBox_BtRxPer
        {
            get { return checkBox_BtRxPer.Checked; }
        }

        #endregion

        #region ICT

        public bool _CheckBox_IctUint
        {
            get { return checkBox_IctUint.Checked; }
        }

        public bool _CheckBox_IctValue
        {
            get { return checkBox_IctValue.Checked; }
        }

        #endregion



    }
}
