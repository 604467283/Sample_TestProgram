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
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Liteon_TestProgram.CaseProject
{
    public partial class SelectionWindow : Form
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

        public SelectionWindow(string str_Title, string str_CanSelectedItems, string str_CurrentItems = null)
        {
            InitializeComponent();

            //标题
            label_Title.Text = str_Title;

            //可供选择的items
            string[] items = str_CanSelectedItems.Split(';');
            foreach (string item in items)
            {
                comboBox_Items.Items.Add(item);
            }

            //默认选中第一个
            comboBox_Items.SelectedIndex = 0;

            //用于显示预设你想要的一个item
            if (string.IsNullOrEmpty(str_CurrentItems) == false)
            {
                for (int i = 0; i < comboBox_Items.Items.Count; i++)
                {
                    // 检查当前项是否包含目标值
                    if (comboBox_Items.Items[i].ToString().Contains(str_CurrentItems))
                    {
                        // 找到匹配项，设置SelectedIndex以选中它
                        comboBox_Items.SelectedIndex = i;
                        break; // 找到后即可退出循环
                    }
                }
            }

            btn_OK.Focus();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        // 定义一个公共属性来存储选中的值
        public string SelectedValue
        {
            get { return comboBox_Items.SelectedItem?.ToString(); }
        }

    }
}
