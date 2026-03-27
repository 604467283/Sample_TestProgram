namespace Liteon_TestProgram.Forms
{
    partial class ToolForm_02
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            lb_Voltage_3V3_Value = new Label();
            lb_Current_3V3_Value = new Label();
            lb_VOLT_TYPE = new Label();
            lb_Volatge_Value = new Label();
            lb_CURR_TYPE = new Label();
            lb_Current_Value = new Label();
            label5 = new Label();
            label6 = new Label();
            textBox_DevNo = new TextBox();
            textBox_BusNo = new TextBox();
            btn_Init = new Button();
            btn_SelectDev = new Button();
            btn_PowerOn = new Button();
            btn_PowerOff = new Button();
            richTextBox_PEMInfo = new RichTextBox();
            btn_Exit = new Button();
            btn_ReadVI = new Button();
            Timer_RD_VI = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = SystemColors.ControlText;
            tableLayoutPanel1.ColumnCount = 17;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.882352F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.ForeColor = SystemColors.ControlLight;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 16;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.25F));
            tableLayoutPanel1.Size = new Size(826, 532);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.FromArgb(46, 50, 58);
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel2, 8);
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel2, 16);
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(378, 526);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(46, 50, 58);
            tableLayoutPanel2.SetColumnSpan(groupBox1, 2);
            groupBox1.Controls.Add(tableLayoutPanel3);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.ForeColor = SystemColors.Control;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(372, 520);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "PEM";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 11;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.289618F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.562841F));
            tableLayoutPanel3.Controls.Add(label1, 1, 0);
            tableLayoutPanel3.Controls.Add(label2, 6, 0);
            tableLayoutPanel3.Controls.Add(lb_Voltage_3V3_Value, 1, 2);
            tableLayoutPanel3.Controls.Add(lb_Current_3V3_Value, 6, 2);
            tableLayoutPanel3.Controls.Add(lb_VOLT_TYPE, 1, 4);
            tableLayoutPanel3.Controls.Add(lb_Volatge_Value, 1, 6);
            tableLayoutPanel3.Controls.Add(lb_CURR_TYPE, 6, 4);
            tableLayoutPanel3.Controls.Add(lb_Current_Value, 6, 6);
            tableLayoutPanel3.Controls.Add(label5, 0, 9);
            tableLayoutPanel3.Controls.Add(label6, 6, 9);
            tableLayoutPanel3.Controls.Add(textBox_DevNo, 9, 9);
            tableLayoutPanel3.Controls.Add(textBox_BusNo, 3, 9);
            tableLayoutPanel3.Controls.Add(btn_Init, 1, 12);
            tableLayoutPanel3.Controls.Add(btn_SelectDev, 6, 12);
            tableLayoutPanel3.Controls.Add(btn_PowerOn, 1, 15);
            tableLayoutPanel3.Controls.Add(btn_PowerOff, 6, 15);
            tableLayoutPanel3.Controls.Add(richTextBox_PEMInfo, 0, 21);
            tableLayoutPanel3.Controls.Add(btn_Exit, 6, 18);
            tableLayoutPanel3.Controls.Add(btn_ReadVI, 1, 18);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 29;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.21285152F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.61445785F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 3.4482758F));
            tableLayoutPanel3.Size = new Size(366, 498);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            tableLayoutPanel3.SetColumnSpan(label1, 4);
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(36, 0);
            label1.Name = "label1";
            tableLayoutPanel3.SetRowSpan(label1, 2);
            label1.Size = new Size(126, 34);
            label1.TabIndex = 0;
            label1.Text = "3.3V Voltage (V)";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            tableLayoutPanel3.SetColumnSpan(label2, 4);
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(201, 0);
            label2.Name = "label2";
            tableLayoutPanel3.SetRowSpan(label2, 2);
            label2.Size = new Size(126, 34);
            label2.TabIndex = 1;
            label2.Text = "3.3V Current (A)";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lb_Voltage_3V3_Value
            // 
            lb_Voltage_3V3_Value.AutoSize = true;
            lb_Voltage_3V3_Value.BackColor = Color.Black;
            tableLayoutPanel3.SetColumnSpan(lb_Voltage_3V3_Value, 4);
            lb_Voltage_3V3_Value.Dock = DockStyle.Fill;
            lb_Voltage_3V3_Value.Location = new Point(36, 34);
            lb_Voltage_3V3_Value.Name = "lb_Voltage_3V3_Value";
            tableLayoutPanel3.SetRowSpan(lb_Voltage_3V3_Value, 2);
            lb_Voltage_3V3_Value.Size = new Size(126, 34);
            lb_Voltage_3V3_Value.TabIndex = 2;
            lb_Voltage_3V3_Value.Text = "0.00";
            lb_Voltage_3V3_Value.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lb_Current_3V3_Value
            // 
            lb_Current_3V3_Value.AutoSize = true;
            lb_Current_3V3_Value.BackColor = Color.Black;
            tableLayoutPanel3.SetColumnSpan(lb_Current_3V3_Value, 4);
            lb_Current_3V3_Value.Dock = DockStyle.Fill;
            lb_Current_3V3_Value.Location = new Point(201, 34);
            lb_Current_3V3_Value.Name = "lb_Current_3V3_Value";
            tableLayoutPanel3.SetRowSpan(lb_Current_3V3_Value, 2);
            lb_Current_3V3_Value.Size = new Size(126, 34);
            lb_Current_3V3_Value.TabIndex = 3;
            lb_Current_3V3_Value.Text = "0.00";
            lb_Current_3V3_Value.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lb_VOLT_TYPE
            // 
            lb_VOLT_TYPE.AutoSize = true;
            tableLayoutPanel3.SetColumnSpan(lb_VOLT_TYPE, 4);
            lb_VOLT_TYPE.Dock = DockStyle.Fill;
            lb_VOLT_TYPE.Location = new Point(36, 68);
            lb_VOLT_TYPE.Name = "lb_VOLT_TYPE";
            tableLayoutPanel3.SetRowSpan(lb_VOLT_TYPE, 2);
            lb_VOLT_TYPE.Size = new Size(126, 34);
            lb_VOLT_TYPE.TabIndex = 4;
            lb_VOLT_TYPE.Text = "1.5V Volatge (V)";
            lb_VOLT_TYPE.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lb_Volatge_Value
            // 
            lb_Volatge_Value.AutoSize = true;
            lb_Volatge_Value.BackColor = Color.Black;
            tableLayoutPanel3.SetColumnSpan(lb_Volatge_Value, 4);
            lb_Volatge_Value.Dock = DockStyle.Fill;
            lb_Volatge_Value.Location = new Point(36, 102);
            lb_Volatge_Value.Name = "lb_Volatge_Value";
            tableLayoutPanel3.SetRowSpan(lb_Volatge_Value, 2);
            lb_Volatge_Value.Size = new Size(126, 34);
            lb_Volatge_Value.TabIndex = 6;
            lb_Volatge_Value.Text = "0.00";
            lb_Volatge_Value.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lb_CURR_TYPE
            // 
            lb_CURR_TYPE.AutoSize = true;
            tableLayoutPanel3.SetColumnSpan(lb_CURR_TYPE, 4);
            lb_CURR_TYPE.Dock = DockStyle.Fill;
            lb_CURR_TYPE.Location = new Point(201, 68);
            lb_CURR_TYPE.Name = "lb_CURR_TYPE";
            tableLayoutPanel3.SetRowSpan(lb_CURR_TYPE, 2);
            lb_CURR_TYPE.Size = new Size(126, 34);
            lb_CURR_TYPE.TabIndex = 5;
            lb_CURR_TYPE.Text = "1.5V Current (A)";
            lb_CURR_TYPE.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lb_Current_Value
            // 
            lb_Current_Value.AutoSize = true;
            lb_Current_Value.BackColor = Color.Black;
            tableLayoutPanel3.SetColumnSpan(lb_Current_Value, 4);
            lb_Current_Value.Dock = DockStyle.Fill;
            lb_Current_Value.Location = new Point(201, 102);
            lb_Current_Value.Name = "lb_Current_Value";
            tableLayoutPanel3.SetRowSpan(lb_Current_Value, 2);
            lb_Current_Value.Size = new Size(126, 34);
            lb_Current_Value.TabIndex = 7;
            lb_Current_Value.Text = "0.00";
            lb_Current_Value.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            tableLayoutPanel3.SetColumnSpan(label5, 3);
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(3, 153);
            label5.Name = "label5";
            tableLayoutPanel3.SetRowSpan(label5, 2);
            label5.Size = new Size(93, 34);
            label5.TabIndex = 8;
            label5.Text = "BusNo :";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            tableLayoutPanel3.SetColumnSpan(label6, 3);
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(201, 153);
            label6.Name = "label6";
            tableLayoutPanel3.SetRowSpan(label6, 2);
            label6.Size = new Size(93, 34);
            label6.TabIndex = 10;
            label6.Text = "DevNo :";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_DevNo
            // 
            textBox_DevNo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_DevNo.BackColor = SystemColors.Window;
            textBox_DevNo.BorderStyle = BorderStyle.None;
            tableLayoutPanel3.SetColumnSpan(textBox_DevNo, 2);
            textBox_DevNo.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_DevNo.Location = new Point(300, 157);
            textBox_DevNo.Name = "textBox_DevNo";
            tableLayoutPanel3.SetRowSpan(textBox_DevNo, 2);
            textBox_DevNo.Size = new Size(63, 26);
            textBox_DevNo.TabIndex = 11;
            textBox_DevNo.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox_BusNo
            // 
            textBox_BusNo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_BusNo.BorderStyle = BorderStyle.None;
            tableLayoutPanel3.SetColumnSpan(textBox_BusNo, 2);
            textBox_BusNo.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_BusNo.Location = new Point(102, 157);
            textBox_BusNo.Name = "textBox_BusNo";
            tableLayoutPanel3.SetRowSpan(textBox_BusNo, 2);
            textBox_BusNo.Size = new Size(60, 26);
            textBox_BusNo.TabIndex = 9;
            textBox_BusNo.TextAlign = HorizontalAlignment.Center;
            // 
            // btn_Init
            // 
            btn_Init.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel3.SetColumnSpan(btn_Init, 4);
            btn_Init.Dock = DockStyle.Fill;
            btn_Init.FlatAppearance.BorderSize = 0;
            btn_Init.FlatStyle = FlatStyle.Flat;
            btn_Init.ForeColor = Color.FromArgb(229, 192, 123);
            btn_Init.Location = new Point(36, 207);
            btn_Init.Name = "btn_Init";
            tableLayoutPanel3.SetRowSpan(btn_Init, 2);
            btn_Init.Size = new Size(126, 28);
            btn_Init.TabIndex = 12;
            btn_Init.Text = "初始化PEM卡";
            btn_Init.UseVisualStyleBackColor = false;
            btn_Init.Click += btn_Init_ClickAsync;
            // 
            // btn_SelectDev
            // 
            btn_SelectDev.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel3.SetColumnSpan(btn_SelectDev, 4);
            btn_SelectDev.Dock = DockStyle.Fill;
            btn_SelectDev.FlatAppearance.BorderSize = 0;
            btn_SelectDev.FlatStyle = FlatStyle.Flat;
            btn_SelectDev.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_SelectDev.ForeColor = Color.FromArgb(192, 0, 0);
            btn_SelectDev.Location = new Point(201, 207);
            btn_SelectDev.Name = "btn_SelectDev";
            tableLayoutPanel3.SetRowSpan(btn_SelectDev, 2);
            btn_SelectDev.Size = new Size(126, 28);
            btn_SelectDev.TabIndex = 13;
            btn_SelectDev.Text = "选择设备(慎用)";
            btn_SelectDev.UseVisualStyleBackColor = false;
            btn_SelectDev.Click += btn_SelectDev_ClickAsync;
            // 
            // btn_PowerOn
            // 
            btn_PowerOn.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel3.SetColumnSpan(btn_PowerOn, 4);
            btn_PowerOn.Dock = DockStyle.Fill;
            btn_PowerOn.FlatAppearance.BorderSize = 0;
            btn_PowerOn.FlatStyle = FlatStyle.Flat;
            btn_PowerOn.ForeColor = Color.FromArgb(229, 192, 123);
            btn_PowerOn.Location = new Point(36, 258);
            btn_PowerOn.Name = "btn_PowerOn";
            tableLayoutPanel3.SetRowSpan(btn_PowerOn, 2);
            btn_PowerOn.Size = new Size(126, 28);
            btn_PowerOn.TabIndex = 14;
            btn_PowerOn.Text = "打开电源并启卡";
            btn_PowerOn.UseVisualStyleBackColor = false;
            btn_PowerOn.Click += btn_PowerOn_ClickAsync;
            // 
            // btn_PowerOff
            // 
            btn_PowerOff.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel3.SetColumnSpan(btn_PowerOff, 4);
            btn_PowerOff.Dock = DockStyle.Fill;
            btn_PowerOff.FlatAppearance.BorderSize = 0;
            btn_PowerOff.FlatStyle = FlatStyle.Flat;
            btn_PowerOff.ForeColor = Color.FromArgb(229, 192, 123);
            btn_PowerOff.Location = new Point(201, 258);
            btn_PowerOff.Name = "btn_PowerOff";
            tableLayoutPanel3.SetRowSpan(btn_PowerOff, 2);
            btn_PowerOff.Size = new Size(126, 28);
            btn_PowerOff.TabIndex = 15;
            btn_PowerOff.Text = "禁卡并关闭电源";
            btn_PowerOff.UseVisualStyleBackColor = false;
            btn_PowerOff.Click += btn_PowerOff_ClickAsync;
            // 
            // richTextBox_PEMInfo
            // 
            richTextBox_PEMInfo.BackColor = SystemColors.WindowText;
            richTextBox_PEMInfo.BorderStyle = BorderStyle.None;
            tableLayoutPanel3.SetColumnSpan(richTextBox_PEMInfo, 11);
            richTextBox_PEMInfo.Dock = DockStyle.Fill;
            richTextBox_PEMInfo.ForeColor = SystemColors.ControlLight;
            richTextBox_PEMInfo.Location = new Point(3, 360);
            richTextBox_PEMInfo.Name = "richTextBox_PEMInfo";
            richTextBox_PEMInfo.ReadOnly = true;
            tableLayoutPanel3.SetRowSpan(richTextBox_PEMInfo, 8);
            richTextBox_PEMInfo.Size = new Size(360, 135);
            richTextBox_PEMInfo.TabIndex = 17;
            richTextBox_PEMInfo.Text = "";
            // 
            // btn_Exit
            // 
            btn_Exit.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel3.SetColumnSpan(btn_Exit, 4);
            btn_Exit.FlatAppearance.BorderSize = 0;
            btn_Exit.FlatStyle = FlatStyle.Flat;
            btn_Exit.ForeColor = Color.FromArgb(229, 192, 123);
            btn_Exit.Location = new Point(201, 309);
            btn_Exit.Name = "btn_Exit";
            tableLayoutPanel3.SetRowSpan(btn_Exit, 2);
            btn_Exit.Size = new Size(126, 28);
            btn_Exit.TabIndex = 16;
            btn_Exit.Text = "退出PEM卡";
            btn_Exit.UseVisualStyleBackColor = false;
            btn_Exit.Click += btn_Exit_ClickAsync;
            // 
            // btn_ReadVI
            // 
            btn_ReadVI.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel3.SetColumnSpan(btn_ReadVI, 4);
            btn_ReadVI.Dock = DockStyle.Fill;
            btn_ReadVI.FlatAppearance.BorderSize = 0;
            btn_ReadVI.FlatStyle = FlatStyle.Flat;
            btn_ReadVI.ForeColor = Color.FromArgb(229, 192, 123);
            btn_ReadVI.Location = new Point(36, 309);
            btn_ReadVI.Name = "btn_ReadVI";
            tableLayoutPanel3.SetRowSpan(btn_ReadVI, 2);
            btn_ReadVI.Size = new Size(126, 28);
            btn_ReadVI.TabIndex = 18;
            btn_ReadVI.Text = "读取VI数据";
            btn_ReadVI.UseVisualStyleBackColor = false;
            btn_ReadVI.Click += btn_ReadVI_Click;
            // 
            // Timer_RD_VI
            // 
            Timer_RD_VI.Interval = 1000;
            Timer_RD_VI.Tick += Timer_RD_VI_Tick;
            // 
            // ToolForm_02
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(826, 532);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ToolForm_02";
            Text = "ToolForm_02";
            Load += ToolForm_02_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label1;
        private Label label2;
        private Label lb_Voltage_3V3_Value;
        private Label lb_Current_3V3_Value;
        private Label lb_VOLT_TYPE;
        private Label lb_CURR_TYPE;
        private Label lb_Volatge_Value;
        private Label lb_Current_Value;
        private Label label5;
        private TextBox textBox_BusNo;
        private Label label6;
        private TextBox textBox_DevNo;
        private Button btn_Init;
        private Button btn_SelectDev;
        private Button btn_PowerOn;
        private Button btn_PowerOff;
        private Button btn_Exit;
        private RichTextBox richTextBox_PEMInfo;
        private System.Windows.Forms.Timer Timer_RD_VI;
        private Button btn_ReadVI;
    }
}