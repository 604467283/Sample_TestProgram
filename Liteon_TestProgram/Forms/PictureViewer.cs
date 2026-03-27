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
    public partial class PictureViewer : Form
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


        private string _contentPic = null;

        public string ContentText
        {
            get { return _contentPic; }
            set { _contentPic = value; }
        }

        public PictureViewer(string title, string picPath, bool bOnlyConfirm)
        {
            InitializeComponent();

            DialogResult = DialogResult.None;

            _contentPic = picPath;

            //标题
            lb_Title.Text = title;

            if (bOnlyConfirm)
            {
                btn_No.Visible = false;
            }
        }


        private void btn_Min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_Max_Click(object sender, EventArgs e)
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

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_No_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Close();
        }

        private void btn_Yes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            this.Close();
        }


        public static DialogResult Show(string title, string picPath, bool bOnlyConfirm)
        {
            PictureViewer picbox = new PictureViewer(title, picPath, bOnlyConfirm);
            return picbox.ShowDialog();
        }

        private void PictureViewer_Load(object sender, EventArgs e)
        {
            // 设置 PictureBox 的 SizeMode 属性以适应图片大小
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // 加载图片
            try
            {
                // 使用 Image.FromFile 方法加载图片
                pictureBox.Image = Image.FromFile(_contentPic);
            }
            catch (Exception ex)
            {
                MessageBoxEX.Show("加载图片时出错: " + ex.Message, true);
            }
        }


    }
}
