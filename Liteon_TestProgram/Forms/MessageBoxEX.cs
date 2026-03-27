using Liteon_TestProgram.Utilities;
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
    public partial class MessageBoxEX : Form
    {
        [DllImport("user32.dll")]  //需添加using System.Runtime.InteropServices
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;



        public MessageBoxEX(string text, bool bOnlyConfirm)
        {
            InitializeComponent();
            this.MouseDown += Form_Base_MouseDown;
            DialogResult = DialogResult.None;

            _contentText = text;

            if (bOnlyConfirm)
            {
                button_NO.Visible = false;
            }

        }

        private void Form_Base_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
        }


        private string _contentText = "暂无信息!";

        public string ContentText
        {
            get { return _contentText; }
            set { _contentText = value; }
        }



        private void MessageBoxEX_Load(object sender, EventArgs e)
        {
            if (this._contentText.Trim() != "")
            {
                this.lblMessage.Text = this._contentText;
            }
        }


        public static DialogResult Show(string text, bool bOnlyConfirm)
        {
            MessageBoxEX msgbox = new MessageBoxEX(text, bOnlyConfirm);
            return msgbox.ShowDialog();
        }


        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Max_Click(object sender, EventArgs e)
        {
            // 判断窗口是否处于最大化状态
            if (this.WindowState == FormWindowState.Maximized)
            {
                // 执行取消最大化的操作
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void button_Min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button_YES_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void button_NO_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Close();
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {
            using (MsgBoxInfoDetails msgBoxInfoDetails = new MsgBoxInfoDetails("MsgBoxInfoDetails", this._contentText))
            {
                msgBoxInfoDetails.ShowDialog();
            }
        }


    }
}
