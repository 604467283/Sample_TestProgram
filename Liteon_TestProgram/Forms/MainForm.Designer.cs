namespace Liteon_TestProgram
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            splitContainer1 = new SplitContainer();
            tabControl_test = new TabControl();
            tableLayoutPanel2 = new TableLayoutPanel();
            textBox_Info4 = new TextBox();
            label_Info4 = new Label();
            textBox_Info3 = new TextBox();
            label_Info3 = new Label();
            textBox_Info2 = new TextBox();
            label_Info2 = new Label();
            textBox_Tips_Title = new TextBox();
            textBox_Tips_Content = new TextBox();
            label_Info1 = new Label();
            textBox_Info1 = new TextBox();
            pictureBox1 = new PictureBox();
            panel_Info = new Panel();
            label_info5 = new Label();
            textBox_info5 = new TextBox();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel4 = new ToolStripStatusLabel();
            toolStripStatusLabel_Time = new ToolStripStatusLabel();
            toolStripStatusLabel6 = new ToolStripStatusLabel();
            toolStripStatusLabel_ReleaseTime = new ToolStripStatusLabel();
            toolStripStatusLabel7 = new ToolStripStatusLabel();
            toolStripStatusLabel_ToolVersion = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            toolStripStatusLabel_Robot = new ToolStripStatusLabel();
            toolStripStatusLabel5 = new ToolStripStatusLabel();
            toolStripStatusLabel_Multi = new ToolStripStatusLabel();
            tableLayoutPanel3 = new TableLayoutPanel();
            button_Close = new Button();
            button_Max = new Button();
            button_Min = new Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            label1 = new Label();
            timer_info5 = new System.Windows.Forms.Timer(components);
            timer_RollScreenInfoIni = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel_Info.SuspendLayout();
            statusStrip1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500F));
            tableLayoutPanel1.Controls.Add(splitContainer1, 0, 1);
            tableLayoutPanel1.Controls.Add(statusStrip1, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Location = new Point(0, 5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.99995F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 89.28936F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 3.71069241F));
            tableLayoutPanel1.Size = new Size(1074, 655);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 48);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tabControl_test);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel2);
            splitContainer1.Size = new Size(1068, 578);
            splitContainer1.SplitterDistance = 710;
            splitContainer1.TabIndex = 1;
            // 
            // tabControl_test
            // 
            tabControl_test.Dock = DockStyle.Fill;
            tabControl_test.Font = new Font("Microsoft YaHei UI", 9F);
            tabControl_test.Location = new Point(0, 0);
            tabControl_test.Name = "tabControl_test";
            tabControl_test.SelectedIndex = 0;
            tabControl_test.Size = new Size(710, 578);
            tabControl_test.SizeMode = TabSizeMode.Fixed;
            tabControl_test.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26.83616F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 73.16384F));
            tableLayoutPanel2.Controls.Add(textBox_Info4, 1, 5);
            tableLayoutPanel2.Controls.Add(label_Info4, 0, 5);
            tableLayoutPanel2.Controls.Add(textBox_Info3, 1, 4);
            tableLayoutPanel2.Controls.Add(label_Info3, 0, 4);
            tableLayoutPanel2.Controls.Add(textBox_Info2, 1, 3);
            tableLayoutPanel2.Controls.Add(label_Info2, 0, 3);
            tableLayoutPanel2.Controls.Add(textBox_Tips_Title, 0, 0);
            tableLayoutPanel2.Controls.Add(textBox_Tips_Content, 0, 1);
            tableLayoutPanel2.Controls.Add(label_Info1, 0, 2);
            tableLayoutPanel2.Controls.Add(textBox_Info1, 1, 2);
            tableLayoutPanel2.Controls.Add(pictureBox1, 0, 7);
            tableLayoutPanel2.Controls.Add(panel_Info, 0, 6);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 8;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 69.3188248F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.88504028F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 22.7961311F));
            tableLayoutPanel2.Size = new Size(354, 578);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // textBox_Info4
            // 
            textBox_Info4.BackColor = Color.FromArgb(45, 45, 49);
            textBox_Info4.BorderStyle = BorderStyle.FixedSingle;
            textBox_Info4.Dock = DockStyle.Fill;
            textBox_Info4.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_Info4.ForeColor = SystemColors.ControlLight;
            textBox_Info4.Location = new Point(98, 419);
            textBox_Info4.Name = "textBox_Info4";
            textBox_Info4.ReadOnly = true;
            textBox_Info4.Size = new Size(253, 25);
            textBox_Info4.TabIndex = 9;
            textBox_Info4.TextAlign = HorizontalAlignment.Center;
            // 
            // label_Info4
            // 
            label_Info4.AutoSize = true;
            label_Info4.Dock = DockStyle.Fill;
            label_Info4.Font = new Font("Microsoft YaHei UI", 10.5F);
            label_Info4.ForeColor = SystemColors.Control;
            label_Info4.Location = new Point(3, 416);
            label_Info4.Name = "label_Info4";
            label_Info4.Size = new Size(89, 31);
            label_Info4.TabIndex = 8;
            label_Info4.Text = "测试端口:";
            label_Info4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_Info3
            // 
            textBox_Info3.BackColor = Color.FromArgb(45, 45, 49);
            textBox_Info3.BorderStyle = BorderStyle.FixedSingle;
            textBox_Info3.Dock = DockStyle.Fill;
            textBox_Info3.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_Info3.ForeColor = SystemColors.ControlLight;
            textBox_Info3.Location = new Point(98, 388);
            textBox_Info3.Name = "textBox_Info3";
            textBox_Info3.ReadOnly = true;
            textBox_Info3.Size = new Size(253, 25);
            textBox_Info3.TabIndex = 7;
            textBox_Info3.TextAlign = HorizontalAlignment.Center;
            // 
            // label_Info3
            // 
            label_Info3.AutoSize = true;
            label_Info3.Dock = DockStyle.Fill;
            label_Info3.Font = new Font("Microsoft YaHei UI", 10.5F);
            label_Info3.ForeColor = SystemColors.Control;
            label_Info3.Location = new Point(3, 385);
            label_Info3.Name = "label_Info3";
            label_Info3.Size = new Size(89, 31);
            label_Info3.TabIndex = 6;
            label_Info3.Text = "测试SFC:";
            label_Info3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_Info2
            // 
            textBox_Info2.BackColor = Color.FromArgb(45, 45, 49);
            textBox_Info2.BorderStyle = BorderStyle.FixedSingle;
            textBox_Info2.Dock = DockStyle.Fill;
            textBox_Info2.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_Info2.ForeColor = SystemColors.ControlLight;
            textBox_Info2.Location = new Point(98, 357);
            textBox_Info2.Name = "textBox_Info2";
            textBox_Info2.ReadOnly = true;
            textBox_Info2.Size = new Size(253, 25);
            textBox_Info2.TabIndex = 5;
            textBox_Info2.TextAlign = HorizontalAlignment.Center;
            // 
            // label_Info2
            // 
            label_Info2.AutoSize = true;
            label_Info2.Dock = DockStyle.Fill;
            label_Info2.Font = new Font("Microsoft YaHei UI", 10.5F);
            label_Info2.ForeColor = SystemColors.Control;
            label_Info2.Location = new Point(3, 354);
            label_Info2.Name = "label_Info2";
            label_Info2.Size = new Size(89, 31);
            label_Info2.TabIndex = 4;
            label_Info2.Text = "测试版本:";
            label_Info2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_Tips_Title
            // 
            textBox_Tips_Title.BackColor = Color.FromArgb(46, 50, 58);
            textBox_Tips_Title.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel2.SetColumnSpan(textBox_Tips_Title, 2);
            textBox_Tips_Title.Dock = DockStyle.Fill;
            textBox_Tips_Title.ForeColor = SystemColors.Control;
            textBox_Tips_Title.Location = new Point(3, 3);
            textBox_Tips_Title.Name = "textBox_Tips_Title";
            textBox_Tips_Title.ReadOnly = true;
            textBox_Tips_Title.Size = new Size(348, 23);
            textBox_Tips_Title.TabIndex = 0;
            // 
            // textBox_Tips_Content
            // 
            textBox_Tips_Content.BackColor = Color.FromArgb(46, 50, 58);
            textBox_Tips_Content.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel2.SetColumnSpan(textBox_Tips_Content, 2);
            textBox_Tips_Content.Dock = DockStyle.Fill;
            textBox_Tips_Content.ForeColor = SystemColors.Control;
            textBox_Tips_Content.Location = new Point(3, 32);
            textBox_Tips_Content.Multiline = true;
            textBox_Tips_Content.Name = "textBox_Tips_Content";
            textBox_Tips_Content.ReadOnly = true;
            textBox_Tips_Content.ScrollBars = ScrollBars.Both;
            textBox_Tips_Content.Size = new Size(348, 288);
            textBox_Tips_Content.TabIndex = 1;
            // 
            // label_Info1
            // 
            label_Info1.AutoSize = true;
            label_Info1.Dock = DockStyle.Fill;
            label_Info1.Font = new Font("Microsoft YaHei UI", 10.5F);
            label_Info1.ForeColor = SystemColors.Control;
            label_Info1.Location = new Point(3, 323);
            label_Info1.Name = "label_Info1";
            label_Info1.Size = new Size(89, 31);
            label_Info1.TabIndex = 2;
            label_Info1.Text = "测试名称:";
            label_Info1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_Info1
            // 
            textBox_Info1.BackColor = Color.FromArgb(45, 45, 49);
            textBox_Info1.BorderStyle = BorderStyle.FixedSingle;
            textBox_Info1.Dock = DockStyle.Fill;
            textBox_Info1.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_Info1.ForeColor = SystemColors.ControlLight;
            textBox_Info1.Location = new Point(98, 326);
            textBox_Info1.Name = "textBox_Info1";
            textBox_Info1.ReadOnly = true;
            textBox_Info1.Size = new Size(253, 25);
            textBox_Info1.TabIndex = 3;
            textBox_Info1.TextAlign = HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            tableLayoutPanel2.SetColumnSpan(pictureBox1, 2);
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 483);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(348, 92);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            // 
            // panel_Info
            // 
            tableLayoutPanel2.SetColumnSpan(panel_Info, 2);
            panel_Info.Controls.Add(label_info5);
            panel_Info.Controls.Add(textBox_info5);
            panel_Info.Dock = DockStyle.Fill;
            panel_Info.Location = new Point(3, 450);
            panel_Info.Name = "panel_Info";
            panel_Info.Size = new Size(348, 27);
            panel_Info.TabIndex = 12;
            // 
            // label_info5
            // 
            label_info5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label_info5.AutoSize = true;
            label_info5.BackColor = Color.FromArgb(46, 50, 58);
            label_info5.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            label_info5.ForeColor = Color.FromArgb(229, 192, 123);
            label_info5.Location = new Point(329, 6);
            label_info5.Name = "label_info5";
            label_info5.Size = new Size(159, 19);
            label_info5.TabIndex = 1;
            label_info5.Text = "光寶科技(常州)有限公司";
            // 
            // textBox_info5
            // 
            textBox_info5.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_info5.BackColor = Color.FromArgb(46, 50, 58);
            textBox_info5.BorderStyle = BorderStyle.None;
            textBox_info5.Enabled = false;
            textBox_info5.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox_info5.Location = new Point(0, 2);
            textBox_info5.Name = "textBox_info5";
            textBox_info5.Size = new Size(348, 25);
            textBox_info5.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = Color.FromArgb(46, 50, 58);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel4, toolStripStatusLabel_Time, toolStripStatusLabel6, toolStripStatusLabel_ReleaseTime, toolStripStatusLabel7, toolStripStatusLabel_ToolVersion, toolStripStatusLabel2, toolStripStatusLabel_Robot, toolStripStatusLabel5, toolStripStatusLabel_Multi });
            statusStrip1.Location = new Point(0, 633);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1074, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel4
            // 
            toolStripStatusLabel4.BackColor = Color.FromArgb(46, 50, 58);
            toolStripStatusLabel4.ForeColor = SystemColors.Control;
            toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            toolStripStatusLabel4.Size = new Size(44, 17);
            toolStripStatusLabel4.Text = "时间：";
            // 
            // toolStripStatusLabel_Time
            // 
            toolStripStatusLabel_Time.ForeColor = SystemColors.Control;
            toolStripStatusLabel_Time.Name = "toolStripStatusLabel_Time";
            toolStripStatusLabel_Time.Size = new Size(141, 17);
            toolStripStatusLabel_Time.Text = "2024年9月14日22:47:31";
            // 
            // toolStripStatusLabel6
            // 
            toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            toolStripStatusLabel6.Size = new Size(19, 17);
            toolStripStatusLabel6.Text = " | ";
            // 
            // toolStripStatusLabel_ReleaseTime
            // 
            toolStripStatusLabel_ReleaseTime.ForeColor = SystemColors.Control;
            toolStripStatusLabel_ReleaseTime.Name = "toolStripStatusLabel_ReleaseTime";
            toolStripStatusLabel_ReleaseTime.Size = new Size(81, 17);
            toolStripStatusLabel_ReleaseTime.Text = "ReleaseTime";
            // 
            // toolStripStatusLabel7
            // 
            toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            toolStripStatusLabel7.Size = new Size(19, 17);
            toolStripStatusLabel7.Text = " | ";
            // 
            // toolStripStatusLabel_ToolVersion
            // 
            toolStripStatusLabel_ToolVersion.ForeColor = SystemColors.Control;
            toolStripStatusLabel_ToolVersion.Name = "toolStripStatusLabel_ToolVersion";
            toolStripStatusLabel_ToolVersion.Size = new Size(52, 17);
            toolStripStatusLabel_ToolVersion.Text = "Version";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(19, 17);
            toolStripStatusLabel2.Text = " | ";
            // 
            // toolStripStatusLabel_Robot
            // 
            toolStripStatusLabel_Robot.ForeColor = SystemColors.Control;
            toolStripStatusLabel_Robot.Name = "toolStripStatusLabel_Robot";
            toolStripStatusLabel_Robot.Size = new Size(107, 17);
            toolStripStatusLabel_Robot.Text = "Robot: Connect...";
            // 
            // toolStripStatusLabel5
            // 
            toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            toolStripStatusLabel5.Size = new Size(19, 17);
            toolStripStatusLabel5.Text = " | ";
            // 
            // toolStripStatusLabel_Multi
            // 
            toolStripStatusLabel_Multi.ForeColor = SystemColors.Control;
            toolStripStatusLabel_Multi.Name = "toolStripStatusLabel_Multi";
            toolStripStatusLabel_Multi.Size = new Size(44, 17);
            toolStripStatusLabel_Multi.Text = "Multi: ";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 23;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.347826F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.681648F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.588015F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.681648F));
            tableLayoutPanel3.Controls.Add(button_Close, 22, 0);
            tableLayoutPanel3.Controls.Add(button_Max, 21, 0);
            tableLayoutPanel3.Controls.Add(button_Min, 20, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(1068, 39);
            tableLayoutPanel3.TabIndex = 2;
            tableLayoutPanel3.MouseDown += Form_Base_MouseDown;
            // 
            // button_Close
            // 
            button_Close.BackgroundImage = (Image)resources.GetObject("button_Close.BackgroundImage");
            button_Close.BackgroundImageLayout = ImageLayout.Zoom;
            button_Close.Dock = DockStyle.Fill;
            button_Close.FlatAppearance.BorderSize = 0;
            button_Close.FlatStyle = FlatStyle.Flat;
            button_Close.Location = new Point(1020, 3);
            button_Close.Name = "button_Close";
            button_Close.Size = new Size(45, 33);
            button_Close.TabIndex = 0;
            button_Close.UseVisualStyleBackColor = true;
            button_Close.Click += button_Close_Click;
            // 
            // button_Max
            // 
            button_Max.BackgroundImage = (Image)resources.GetObject("button_Max.BackgroundImage");
            button_Max.BackgroundImageLayout = ImageLayout.Zoom;
            button_Max.Dock = DockStyle.Fill;
            button_Max.FlatAppearance.BorderSize = 0;
            button_Max.FlatStyle = FlatStyle.Flat;
            button_Max.Location = new Point(972, 3);
            button_Max.Name = "button_Max";
            button_Max.Size = new Size(42, 33);
            button_Max.TabIndex = 1;
            button_Max.UseVisualStyleBackColor = true;
            button_Max.Click += button_Max_Click;
            // 
            // button_Min
            // 
            button_Min.BackgroundImage = (Image)resources.GetObject("button_Min.BackgroundImage");
            button_Min.BackgroundImageLayout = ImageLayout.Zoom;
            button_Min.Dock = DockStyle.Fill;
            button_Min.FlatAppearance.BorderSize = 0;
            button_Min.FlatStyle = FlatStyle.Flat;
            button_Min.Location = new Point(923, 3);
            button_Min.Name = "button_Min";
            button_Min.Size = new Size(43, 33);
            button_Min.TabIndex = 2;
            button_Min.UseVisualStyleBackColor = true;
            button_Min.Click += button_Min_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel3.SetColumnSpan(tableLayoutPanel4, 10);
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Controls.Add(label1, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(454, 33);
            tableLayoutPanel4.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Verdana", 18F, FontStyle.Bold);
            label1.ForeColor = Color.CornflowerBlue;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(448, 33);
            label1.TabIndex = 0;
            label1.Text = "Liteon Test Program";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            label1.MouseDown += Form_Base_MouseDown;
            // 
            // timer_info5
            // 
            timer_info5.Enabled = true;
            timer_info5.Interval = 200;
            timer_info5.Tick += timer_info5_Tick;
            // 
            // timer_RollScreenInfoIni
            // 
            timer_RollScreenInfoIni.Enabled = true;
            timer_RollScreenInfoIni.Interval = 10000;
            timer_RollScreenInfoIni.Tick += timer_RollScreenInfoIni_Tick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 50, 58);
            ClientSize = new Size(1075, 662);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LITEON_TestProgram";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel_Info.ResumeLayout(false);
            panel_Info.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private ToolStripStatusLabel toolStripStatusLabel_Time;
        private SplitContainer splitContainer1;
        private TabControl tabControl_test;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox textBox_Tips_Title;
        private TextBox textBox_Tips_Content;
        private Label label_Info1;
        private TextBox textBox_Info4;
        private Label label_Info4;
        private TextBox textBox_Info3;
        private Label label_Info3;
        private TextBox textBox_Info2;
        private Label label_Info2;
        private TextBox textBox_Info1;
        private PictureBox pictureBox1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel_Robot;
        private ToolStripStatusLabel toolStripStatusLabel5;
        private ToolStripStatusLabel toolStripStatusLabel_Multi;
        private ToolStripStatusLabel toolStripStatusLabel_ReleaseTime;
        private ToolStripStatusLabel toolStripStatusLabel7;
        private ToolStripStatusLabel toolStripStatusLabel_ToolVersion;
        private ToolStripStatusLabel toolStripStatusLabel6;
        private TableLayoutPanel tableLayoutPanel3;
        private Button button_Close;
        private Button button_Max;
        private Button button_Min;
        private TableLayoutPanel tableLayoutPanel4;
        private Label label1;
        private Panel panel_Info;
        private TextBox textBox_info5;
        private Label label_info5;
        private System.Windows.Forms.Timer timer_info5;
        private System.Windows.Forms.Timer timer_RollScreenInfoIni;
    }
}