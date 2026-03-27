namespace Liteon_TestProgram.Forms
{
    partial class ToolForm_01
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolForm_01));
            splitContainer1 = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            label_EncryptIni = new Label();
            textBox_Md5Key = new TextBox();
            label3 = new Label();
            btn_SetMd5 = new Button();
            groupBox3 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            comboBox_IQPortNums = new ComboBox();
            textBox_FlowFolder = new TextBox();
            button_FlowFolder = new Button();
            label2 = new Label();
            label4 = new Label();
            textBox_FlowName = new TextBox();
            richTextBox_FlowsData = new RichTextBox();
            button_MakeFlowStart = new Button();
            groupBox2 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            textBox_TargetFolder = new TextBox();
            button_DealWithLogs = new Button();
            groupBox4 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            label6 = new Label();
            comboBox_FileMark = new ComboBox();
            label7 = new Label();
            comboBox_excludedFolders = new ComboBox();
            richTextBox_LogsData = new RichTextBox();
            btn_LogCollectSettings = new Button();
            groupBox5 = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            textBox_SelectFile_Md5 = new TextBox();
            richTextBox_FileMd5 = new RichTextBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            formsPlot_RFValues = new ScottPlot.WinForms.FormsPlot();
            comboBox_RFMode = new ComboBox();
            button_ScottPlotClear = new Button();
            label_Upper = new Label();
            label_Lower = new Label();
            textBox_LowerLimit = new TextBox();
            textBox_UpperLimit = new TextBox();
            label5 = new Label();
            label_CPK = new Label();
            button_CPK = new Button();
            toolTip_MSG = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            groupBox5.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackgroundImage = (Image)resources.GetObject("splitContainer1.Panel2.BackgroundImage");
            splitContainer1.Panel2.BackgroundImageLayout = ImageLayout.Stretch;
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel5);
            splitContainer1.Size = new Size(811, 530);
            splitContainer1.SplitterDistance = 283;
            splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = SystemColors.WindowText;
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.Stretch;
            tableLayoutPanel1.ColumnCount = 12;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox3, 8, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 4, 0);
            tableLayoutPanel1.Controls.Add(groupBox5, 0, 5);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 10;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10.2112675F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 9.507042F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.Size = new Size(811, 283);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(46, 50, 58);
            tableLayoutPanel1.SetColumnSpan(groupBox1, 4);
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.FlatStyle = FlatStyle.Flat;
            groupBox1.ForeColor = SystemColors.Control;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            tableLayoutPanel1.SetRowSpan(groupBox1, 5);
            groupBox1.Size = new Size(262, 134);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "加密文件MD5";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.046875F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26.953125F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(label_EncryptIni, 1, 0);
            tableLayoutPanel2.Controls.Add(textBox_Md5Key, 1, 1);
            tableLayoutPanel2.Controls.Add(label3, 0, 1);
            tableLayoutPanel2.Controls.Add(btn_SetMd5, 1, 2);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 35.51402F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 30.8411217F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 32.71028F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(256, 112);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(53, 40);
            label1.TabIndex = 0;
            label1.Text = "文件: ";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_EncryptIni
            // 
            label_EncryptIni.AutoSize = true;
            tableLayoutPanel2.SetColumnSpan(label_EncryptIni, 3);
            label_EncryptIni.Dock = DockStyle.Fill;
            label_EncryptIni.Font = new Font("Microsoft YaHei UI", 7.5F);
            label_EncryptIni.Location = new Point(62, 0);
            label_EncryptIni.Name = "label_EncryptIni";
            label_EncryptIni.Size = new Size(191, 40);
            label_EncryptIni.TabIndex = 1;
            label_EncryptIni.Text = "label2";
            label_EncryptIni.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_Md5Key
            // 
            textBox_Md5Key.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_Md5Key.BackColor = Color.FromArgb(45, 45, 49);
            textBox_Md5Key.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel2.SetColumnSpan(textBox_Md5Key, 3);
            textBox_Md5Key.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_Md5Key.ForeColor = SystemColors.Control;
            textBox_Md5Key.Location = new Point(62, 44);
            textBox_Md5Key.Name = "textBox_Md5Key";
            textBox_Md5Key.PasswordChar = '*';
            textBox_Md5Key.Size = new Size(191, 25);
            textBox_Md5Key.TabIndex = 2;
            textBox_Md5Key.TextAlign = HorizontalAlignment.Center;
            textBox_Md5Key.TextChanged += textBox_Md5Key_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(3, 40);
            label3.Name = "label3";
            label3.Size = new Size(53, 34);
            label3.TabIndex = 3;
            label3.Text = "密码: ";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btn_SetMd5
            // 
            btn_SetMd5.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel2.SetColumnSpan(btn_SetMd5, 3);
            btn_SetMd5.Dock = DockStyle.Fill;
            btn_SetMd5.Enabled = false;
            btn_SetMd5.FlatAppearance.BorderSize = 0;
            btn_SetMd5.FlatStyle = FlatStyle.Flat;
            btn_SetMd5.ForeColor = Color.FromArgb(229, 192, 123);
            btn_SetMd5.Image = (Image)resources.GetObject("btn_SetMd5.Image");
            btn_SetMd5.Location = new Point(62, 77);
            btn_SetMd5.Name = "btn_SetMd5";
            btn_SetMd5.Size = new Size(191, 32);
            btn_SetMd5.TabIndex = 4;
            btn_SetMd5.Text = "变更MD5";
            btn_SetMd5.TextImageRelation = TextImageRelation.TextBeforeImage;
            btn_SetMd5.UseVisualStyleBackColor = false;
            btn_SetMd5.Click += btn_SetMd5_Click;
            // 
            // groupBox3
            // 
            groupBox3.BackColor = Color.FromArgb(46, 50, 58);
            tableLayoutPanel1.SetColumnSpan(groupBox3, 4);
            groupBox3.Controls.Add(tableLayoutPanel4);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.FlatStyle = FlatStyle.Flat;
            groupBox3.ForeColor = SystemColors.ControlLight;
            groupBox3.Location = new Point(539, 3);
            groupBox3.Name = "groupBox3";
            tableLayoutPanel1.SetRowSpan(groupBox3, 10);
            groupBox3.Size = new Size(269, 277);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Litepoint批量生成Flow";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 4;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.4375F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.046875F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.515625F));
            tableLayoutPanel4.Controls.Add(comboBox_IQPortNums, 3, 1);
            tableLayoutPanel4.Controls.Add(textBox_FlowFolder, 0, 0);
            tableLayoutPanel4.Controls.Add(button_FlowFolder, 3, 0);
            tableLayoutPanel4.Controls.Add(label2, 0, 1);
            tableLayoutPanel4.Controls.Add(label4, 0, 2);
            tableLayoutPanel4.Controls.Add(textBox_FlowName, 2, 2);
            tableLayoutPanel4.Controls.Add(richTextBox_FlowsData, 0, 4);
            tableLayoutPanel4.Controls.Add(button_MakeFlowStart, 0, 3);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 19);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 6;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 13.333333F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 13.333333F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 13.7254906F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 15.2941179F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 26.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(263, 255);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // comboBox_IQPortNums
            // 
            comboBox_IQPortNums.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            comboBox_IQPortNums.BackColor = Color.FromArgb(45, 45, 49);
            comboBox_IQPortNums.FlatStyle = FlatStyle.Flat;
            comboBox_IQPortNums.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox_IQPortNums.ForeColor = SystemColors.ControlLight;
            comboBox_IQPortNums.FormattingEnabled = true;
            comboBox_IQPortNums.Items.AddRange(new object[] { "2", "8", "16" });
            comboBox_IQPortNums.Location = new Point(189, 37);
            comboBox_IQPortNums.Name = "comboBox_IQPortNums";
            comboBox_IQPortNums.Size = new Size(71, 28);
            comboBox_IQPortNums.TabIndex = 4;
            comboBox_IQPortNums.Text = "2";
            // 
            // textBox_FlowFolder
            // 
            textBox_FlowFolder.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_FlowFolder.BackColor = Color.FromArgb(45, 45, 49);
            textBox_FlowFolder.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel4.SetColumnSpan(textBox_FlowFolder, 3);
            textBox_FlowFolder.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_FlowFolder.ForeColor = SystemColors.ControlLight;
            textBox_FlowFolder.Location = new Point(3, 4);
            textBox_FlowFolder.Name = "textBox_FlowFolder";
            textBox_FlowFolder.Size = new Size(180, 25);
            textBox_FlowFolder.TabIndex = 0;
            textBox_FlowFolder.Text = "请选择模版flow文件夹(Write.txt/Verify.txt)";
            // 
            // button_FlowFolder
            // 
            button_FlowFolder.BackColor = Color.FromArgb(37, 42, 100);
            button_FlowFolder.Dock = DockStyle.Fill;
            button_FlowFolder.FlatAppearance.BorderSize = 0;
            button_FlowFolder.FlatStyle = FlatStyle.Flat;
            button_FlowFolder.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            button_FlowFolder.ForeColor = Color.FromArgb(229, 192, 123);
            button_FlowFolder.Location = new Point(189, 3);
            button_FlowFolder.Name = "button_FlowFolder";
            button_FlowFolder.Size = new Size(71, 28);
            button_FlowFolder.TabIndex = 1;
            button_FlowFolder.Text = "...";
            button_FlowFolder.UseVisualStyleBackColor = false;
            button_FlowFolder.Click += button_FlowFolder_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            tableLayoutPanel4.SetColumnSpan(label2, 3);
            label2.Dock = DockStyle.Fill;
            label2.ForeColor = SystemColors.ControlLight;
            label2.Location = new Point(3, 34);
            label2.Name = "label2";
            label2.Size = new Size(180, 34);
            label2.TabIndex = 2;
            label2.Text = "IQ仪器端口数量:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            tableLayoutPanel4.SetColumnSpan(label4, 2);
            label4.Dock = DockStyle.Fill;
            label4.ForeColor = SystemColors.ControlLight;
            label4.Location = new Point(3, 68);
            label4.Name = "label4";
            label4.Size = new Size(120, 35);
            label4.TabIndex = 5;
            label4.Text = "生成的Flow名称:";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_FlowName
            // 
            textBox_FlowName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_FlowName.BackColor = Color.FromArgb(45, 45, 49);
            textBox_FlowName.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel4.SetColumnSpan(textBox_FlowName, 2);
            textBox_FlowName.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_FlowName.ForeColor = SystemColors.ControlLight;
            textBox_FlowName.Location = new Point(129, 73);
            textBox_FlowName.Name = "textBox_FlowName";
            textBox_FlowName.Size = new Size(131, 25);
            textBox_FlowName.TabIndex = 6;
            textBox_FlowName.Text = "IQ_WriteTest/IQ_VerifyTest";
            // 
            // richTextBox_FlowsData
            // 
            richTextBox_FlowsData.BackColor = Color.Black;
            richTextBox_FlowsData.BorderStyle = BorderStyle.None;
            tableLayoutPanel4.SetColumnSpan(richTextBox_FlowsData, 4);
            richTextBox_FlowsData.Dock = DockStyle.Fill;
            richTextBox_FlowsData.Font = new Font("Microsoft YaHei UI", 6.75F);
            richTextBox_FlowsData.ForeColor = SystemColors.ControlLight;
            richTextBox_FlowsData.Location = new Point(3, 145);
            richTextBox_FlowsData.Name = "richTextBox_FlowsData";
            richTextBox_FlowsData.ReadOnly = true;
            tableLayoutPanel4.SetRowSpan(richTextBox_FlowsData, 2);
            richTextBox_FlowsData.Size = new Size(257, 107);
            richTextBox_FlowsData.TabIndex = 8;
            richTextBox_FlowsData.Text = "";
            // 
            // button_MakeFlowStart
            // 
            button_MakeFlowStart.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel4.SetColumnSpan(button_MakeFlowStart, 4);
            button_MakeFlowStart.Dock = DockStyle.Fill;
            button_MakeFlowStart.FlatAppearance.BorderSize = 0;
            button_MakeFlowStart.FlatStyle = FlatStyle.Flat;
            button_MakeFlowStart.ForeColor = Color.FromArgb(229, 192, 123);
            button_MakeFlowStart.Location = new Point(3, 106);
            button_MakeFlowStart.Name = "button_MakeFlowStart";
            button_MakeFlowStart.Size = new Size(257, 33);
            button_MakeFlowStart.TabIndex = 7;
            button_MakeFlowStart.Text = "START";
            button_MakeFlowStart.UseVisualStyleBackColor = false;
            button_MakeFlowStart.Click += button_MakeFlowStart_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.FromArgb(46, 50, 58);
            tableLayoutPanel1.SetColumnSpan(groupBox2, 4);
            groupBox2.Controls.Add(tableLayoutPanel3);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.FlatStyle = FlatStyle.Flat;
            groupBox2.ForeColor = SystemColors.ControlLight;
            groupBox2.Location = new Point(271, 3);
            groupBox2.Name = "groupBox2";
            tableLayoutPanel1.SetRowSpan(groupBox2, 10);
            groupBox2.Size = new Size(262, 277);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "批量Log数据提取";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.26562F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27.7343769F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.265625F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.515625F));
            tableLayoutPanel3.Controls.Add(textBox_TargetFolder, 0, 0);
            tableLayoutPanel3.Controls.Add(button_DealWithLogs, 3, 0);
            tableLayoutPanel3.Controls.Add(groupBox4, 0, 1);
            tableLayoutPanel3.Controls.Add(richTextBox_LogsData, 0, 5);
            tableLayoutPanel3.Controls.Add(btn_LogCollectSettings, 0, 4);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 8;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel3.Size = new Size(256, 255);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // textBox_TargetFolder
            // 
            textBox_TargetFolder.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_TargetFolder.BackColor = Color.FromArgb(45, 45, 49);
            textBox_TargetFolder.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel3.SetColumnSpan(textBox_TargetFolder, 3);
            textBox_TargetFolder.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_TargetFolder.ForeColor = SystemColors.ControlLight;
            textBox_TargetFolder.Location = new Point(3, 3);
            textBox_TargetFolder.Name = "textBox_TargetFolder";
            textBox_TargetFolder.Size = new Size(176, 25);
            textBox_TargetFolder.TabIndex = 0;
            textBox_TargetFolder.Text = "请选择log文件夹";
            // 
            // button_DealWithLogs
            // 
            button_DealWithLogs.BackColor = Color.FromArgb(37, 42, 100);
            button_DealWithLogs.Dock = DockStyle.Fill;
            button_DealWithLogs.FlatAppearance.BorderSize = 0;
            button_DealWithLogs.FlatStyle = FlatStyle.Flat;
            button_DealWithLogs.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            button_DealWithLogs.ForeColor = Color.FromArgb(229, 192, 123);
            button_DealWithLogs.Location = new Point(185, 3);
            button_DealWithLogs.Name = "button_DealWithLogs";
            button_DealWithLogs.Size = new Size(68, 25);
            button_DealWithLogs.TabIndex = 1;
            button_DealWithLogs.Text = "...";
            button_DealWithLogs.UseVisualStyleBackColor = false;
            button_DealWithLogs.Click += button_DealWithLogs_Click;
            // 
            // groupBox4
            // 
            tableLayoutPanel3.SetColumnSpan(groupBox4, 4);
            groupBox4.Controls.Add(tableLayoutPanel6);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.ForeColor = SystemColors.ControlLight;
            groupBox4.Location = new Point(3, 34);
            groupBox4.Name = "groupBox4";
            tableLayoutPanel3.SetRowSpan(groupBox4, 3);
            groupBox4.Size = new Size(250, 87);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "选择文件夹设定";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 4;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57.37705F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.7540979F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.3442621F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13.1147537F));
            tableLayoutPanel6.Controls.Add(label6, 0, 0);
            tableLayoutPanel6.Controls.Add(comboBox_FileMark, 1, 0);
            tableLayoutPanel6.Controls.Add(label7, 0, 1);
            tableLayoutPanel6.Controls.Add(comboBox_excludedFolders, 1, 1);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 19);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel6.Size = new Size(244, 65);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(3, 0);
            label6.Name = "label6";
            label6.Size = new Size(134, 32);
            label6.TabIndex = 0;
            label6.Text = "目标文件的后缀:";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox_FileMark
            // 
            comboBox_FileMark.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            comboBox_FileMark.BackColor = Color.FromArgb(45, 45, 49);
            tableLayoutPanel6.SetColumnSpan(comboBox_FileMark, 3);
            comboBox_FileMark.FlatStyle = FlatStyle.Flat;
            comboBox_FileMark.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox_FileMark.ForeColor = SystemColors.ControlLight;
            comboBox_FileMark.FormattingEnabled = true;
            comboBox_FileMark.Items.AddRange(new object[] { "*UI.txt", "*IQ.txt" });
            comboBox_FileMark.Location = new Point(143, 3);
            comboBox_FileMark.Name = "comboBox_FileMark";
            comboBox_FileMark.Size = new Size(98, 28);
            comboBox_FileMark.TabIndex = 1;
            comboBox_FileMark.Text = "*IQ.txt";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(3, 32);
            label7.Name = "label7";
            label7.Size = new Size(134, 33);
            label7.TabIndex = 2;
            label7.Text = "跳过指定文件夹:";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox_excludedFolders
            // 
            comboBox_excludedFolders.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            comboBox_excludedFolders.BackColor = Color.FromArgb(45, 45, 49);
            tableLayoutPanel6.SetColumnSpan(comboBox_excludedFolders, 3);
            comboBox_excludedFolders.FlatStyle = FlatStyle.Flat;
            comboBox_excludedFolders.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox_excludedFolders.ForeColor = SystemColors.ControlLight;
            comboBox_excludedFolders.FormattingEnabled = true;
            comboBox_excludedFolders.Items.AddRange(new object[] { "FAIL" });
            comboBox_excludedFolders.Location = new Point(143, 35);
            comboBox_excludedFolders.Name = "comboBox_excludedFolders";
            comboBox_excludedFolders.Size = new Size(98, 28);
            comboBox_excludedFolders.TabIndex = 3;
            comboBox_excludedFolders.Text = "FAIL";
            // 
            // richTextBox_LogsData
            // 
            richTextBox_LogsData.BackColor = Color.Black;
            richTextBox_LogsData.BorderStyle = BorderStyle.None;
            tableLayoutPanel3.SetColumnSpan(richTextBox_LogsData, 4);
            richTextBox_LogsData.Dock = DockStyle.Fill;
            richTextBox_LogsData.Font = new Font("Microsoft YaHei UI", 6.75F);
            richTextBox_LogsData.ForeColor = SystemColors.ControlLight;
            richTextBox_LogsData.Location = new Point(3, 158);
            richTextBox_LogsData.Name = "richTextBox_LogsData";
            richTextBox_LogsData.ReadOnly = true;
            tableLayoutPanel3.SetRowSpan(richTextBox_LogsData, 3);
            richTextBox_LogsData.Size = new Size(250, 94);
            richTextBox_LogsData.TabIndex = 2;
            richTextBox_LogsData.Text = "";
            // 
            // btn_LogCollectSettings
            // 
            btn_LogCollectSettings.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel3.SetColumnSpan(btn_LogCollectSettings, 4);
            btn_LogCollectSettings.Dock = DockStyle.Fill;
            btn_LogCollectSettings.FlatAppearance.BorderSize = 0;
            btn_LogCollectSettings.FlatStyle = FlatStyle.Flat;
            btn_LogCollectSettings.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btn_LogCollectSettings.ForeColor = Color.FromArgb(229, 192, 123);
            btn_LogCollectSettings.Location = new Point(3, 127);
            btn_LogCollectSettings.Name = "btn_LogCollectSettings";
            btn_LogCollectSettings.Size = new Size(250, 25);
            btn_LogCollectSettings.TabIndex = 4;
            btn_LogCollectSettings.Text = "Settings";
            btn_LogCollectSettings.UseVisualStyleBackColor = false;
            btn_LogCollectSettings.Click += btn_LogCollectSettings_Click;
            // 
            // groupBox5
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox5, 4);
            groupBox5.Controls.Add(tableLayoutPanel7);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.ForeColor = SystemColors.HighlightText;
            groupBox5.Location = new Point(3, 143);
            groupBox5.Name = "groupBox5";
            tableLayoutPanel1.SetRowSpan(groupBox5, 5);
            groupBox5.Size = new Size(262, 137);
            groupBox5.TabIndex = 3;
            groupBox5.TabStop = false;
            groupBox5.Text = "文件MD5计算";
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Controls.Add(textBox_SelectFile_Md5, 0, 0);
            tableLayoutPanel7.Controls.Add(richTextBox_FileMd5, 0, 1);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 19);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 33.04348F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 66.95652F));
            tableLayoutPanel7.Size = new Size(256, 115);
            tableLayoutPanel7.TabIndex = 0;
            // 
            // textBox_SelectFile_Md5
            // 
            textBox_SelectFile_Md5.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_SelectFile_Md5.BackColor = Color.FromArgb(45, 45, 49);
            textBox_SelectFile_Md5.BorderStyle = BorderStyle.None;
            textBox_SelectFile_Md5.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_SelectFile_Md5.ForeColor = SystemColors.HighlightText;
            textBox_SelectFile_Md5.Location = new Point(3, 6);
            textBox_SelectFile_Md5.Name = "textBox_SelectFile_Md5";
            textBox_SelectFile_Md5.Size = new Size(250, 26);
            textBox_SelectFile_Md5.TabIndex = 0;
            textBox_SelectFile_Md5.Text = "双击此处选择文件";
            textBox_SelectFile_Md5.TextAlign = HorizontalAlignment.Center;
            textBox_SelectFile_Md5.MouseDoubleClick += textBox_SelectFile_Md5_MouseDoubleClickAsync;
            // 
            // richTextBox_FileMd5
            // 
            richTextBox_FileMd5.BackColor = Color.FromArgb(45, 45, 49);
            richTextBox_FileMd5.BorderStyle = BorderStyle.None;
            richTextBox_FileMd5.Dock = DockStyle.Fill;
            richTextBox_FileMd5.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox_FileMd5.ForeColor = SystemColors.HighlightText;
            richTextBox_FileMd5.Location = new Point(3, 41);
            richTextBox_FileMd5.Name = "richTextBox_FileMd5";
            richTextBox_FileMd5.Size = new Size(250, 71);
            richTextBox_FileMd5.TabIndex = 1;
            richTextBox_FileMd5.Text = "";
            richTextBox_FileMd5.MouseUp += richTextBox_FileMd5_MouseUp;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.FromArgb(46, 50, 58);
            tableLayoutPanel5.ColumnCount = 12;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel5.Controls.Add(formsPlot_RFValues, 2, 0);
            tableLayoutPanel5.Controls.Add(comboBox_RFMode, 0, 2);
            tableLayoutPanel5.Controls.Add(button_ScottPlotClear, 0, 0);
            tableLayoutPanel5.Controls.Add(label_Upper, 0, 4);
            tableLayoutPanel5.Controls.Add(label_Lower, 1, 4);
            tableLayoutPanel5.Controls.Add(textBox_LowerLimit, 0, 5);
            tableLayoutPanel5.Controls.Add(textBox_UpperLimit, 1, 5);
            tableLayoutPanel5.Controls.Add(label5, 0, 6);
            tableLayoutPanel5.Controls.Add(label_CPK, 1, 6);
            tableLayoutPanel5.Controls.Add(button_CPK, 0, 7);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 10;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.Size = new Size(811, 243);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // formsPlot_RFValues
            // 
            tableLayoutPanel5.SetColumnSpan(formsPlot_RFValues, 10);
            formsPlot_RFValues.DisplayScale = 1F;
            formsPlot_RFValues.Dock = DockStyle.Fill;
            formsPlot_RFValues.Location = new Point(137, 3);
            formsPlot_RFValues.Margin = new Padding(3, 3, 3, 0);
            formsPlot_RFValues.Name = "formsPlot_RFValues";
            tableLayoutPanel5.SetRowSpan(formsPlot_RFValues, 10);
            formsPlot_RFValues.Size = new Size(671, 240);
            formsPlot_RFValues.TabIndex = 0;
            // 
            // comboBox_RFMode
            // 
            comboBox_RFMode.BackColor = Color.FromArgb(45, 45, 49);
            tableLayoutPanel5.SetColumnSpan(comboBox_RFMode, 2);
            comboBox_RFMode.FlatStyle = FlatStyle.Flat;
            comboBox_RFMode.ForeColor = SystemColors.ControlLight;
            comboBox_RFMode.FormattingEnabled = true;
            comboBox_RFMode.Location = new Point(3, 51);
            comboBox_RFMode.Name = "comboBox_RFMode";
            comboBox_RFMode.Size = new Size(128, 25);
            comboBox_RFMode.TabIndex = 1;
            comboBox_RFMode.DropDown += comboBox_RFMode_DropDown;
            comboBox_RFMode.TextChanged += comboBox_RFMode_TextChanged;
            // 
            // button_ScottPlotClear
            // 
            button_ScottPlotClear.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel5.SetColumnSpan(button_ScottPlotClear, 2);
            button_ScottPlotClear.Dock = DockStyle.Fill;
            button_ScottPlotClear.FlatAppearance.BorderSize = 0;
            button_ScottPlotClear.FlatStyle = FlatStyle.Flat;
            button_ScottPlotClear.ForeColor = Color.FromArgb(229, 192, 123);
            button_ScottPlotClear.Location = new Point(3, 3);
            button_ScottPlotClear.Name = "button_ScottPlotClear";
            tableLayoutPanel5.SetRowSpan(button_ScottPlotClear, 2);
            button_ScottPlotClear.Size = new Size(128, 42);
            button_ScottPlotClear.TabIndex = 3;
            button_ScottPlotClear.Text = "清空所有曲线";
            button_ScottPlotClear.UseVisualStyleBackColor = false;
            button_ScottPlotClear.Click += button_ScottPlotClear_Click;
            // 
            // label_Upper
            // 
            label_Upper.AutoSize = true;
            label_Upper.Dock = DockStyle.Fill;
            label_Upper.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_Upper.ForeColor = Color.FromArgb(229, 192, 123);
            label_Upper.Location = new Point(3, 96);
            label_Upper.Name = "label_Upper";
            label_Upper.Size = new Size(61, 24);
            label_Upper.TabIndex = 4;
            label_Upper.Text = "下限";
            label_Upper.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_Lower
            // 
            label_Lower.AutoSize = true;
            label_Lower.Dock = DockStyle.Fill;
            label_Lower.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_Lower.ForeColor = Color.FromArgb(229, 192, 123);
            label_Lower.Location = new Point(70, 96);
            label_Lower.Name = "label_Lower";
            label_Lower.Size = new Size(61, 24);
            label_Lower.TabIndex = 5;
            label_Lower.Text = "上限";
            label_Lower.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_LowerLimit
            // 
            textBox_LowerLimit.BorderStyle = BorderStyle.None;
            textBox_LowerLimit.Dock = DockStyle.Fill;
            textBox_LowerLimit.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox_LowerLimit.Location = new Point(3, 123);
            textBox_LowerLimit.Name = "textBox_LowerLimit";
            textBox_LowerLimit.Size = new Size(61, 21);
            textBox_LowerLimit.TabIndex = 6;
            textBox_LowerLimit.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox_UpperLimit
            // 
            textBox_UpperLimit.BorderStyle = BorderStyle.None;
            textBox_UpperLimit.Dock = DockStyle.Fill;
            textBox_UpperLimit.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox_UpperLimit.Location = new Point(70, 123);
            textBox_UpperLimit.Name = "textBox_UpperLimit";
            textBox_UpperLimit.Size = new Size(61, 21);
            textBox_UpperLimit.TabIndex = 7;
            textBox_UpperLimit.TextAlign = HorizontalAlignment.Center;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.FlatStyle = FlatStyle.Flat;
            label5.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(229, 192, 123);
            label5.Location = new Point(3, 144);
            label5.Name = "label5";
            label5.Size = new Size(61, 24);
            label5.TabIndex = 8;
            label5.Text = "CPK: ";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_CPK
            // 
            label_CPK.AutoSize = true;
            label_CPK.BackColor = Color.Black;
            label_CPK.Dock = DockStyle.Fill;
            label_CPK.FlatStyle = FlatStyle.Flat;
            label_CPK.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_CPK.ForeColor = Color.FromArgb(229, 192, 123);
            label_CPK.Location = new Point(70, 144);
            label_CPK.Name = "label_CPK";
            label_CPK.Size = new Size(61, 24);
            label_CPK.TabIndex = 9;
            label_CPK.Text = "NULL";
            label_CPK.TextAlign = ContentAlignment.MiddleCenter;
            label_CPK.MouseMove += label_CPK_MouseMove;
            // 
            // button_CPK
            // 
            button_CPK.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel5.SetColumnSpan(button_CPK, 2);
            button_CPK.Dock = DockStyle.Fill;
            button_CPK.FlatAppearance.BorderSize = 0;
            button_CPK.FlatStyle = FlatStyle.Flat;
            button_CPK.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button_CPK.ForeColor = Color.FromArgb(229, 192, 123);
            button_CPK.Location = new Point(3, 171);
            button_CPK.Name = "button_CPK";
            tableLayoutPanel5.SetRowSpan(button_CPK, 2);
            button_CPK.Size = new Size(128, 42);
            button_CPK.TabIndex = 10;
            button_CPK.Text = "手动CPK";
            button_CPK.UseVisualStyleBackColor = false;
            button_CPK.Click += button_CPK_Click;
            // 
            // ToolForm_01
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(811, 530);
            Controls.Add(splitContainer1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ToolForm_01";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ToolForm";
            Load += ToolForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            groupBox4.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            groupBox5.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private Label label_EncryptIni;
        private TextBox textBox_Md5Key;
        private Label label3;
        private Button btn_SetMd5;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox textBox_TargetFolder;
        private Button button_DealWithLogs;
        private RichTextBox richTextBox_LogsData;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel4;
        private TextBox textBox_FlowFolder;
        private Button button_FlowFolder;
        private Label label2;
        private ComboBox comboBox_IQPortNums;
        private Label label4;
        private TextBox textBox_FlowName;
        private Button button_MakeFlowStart;
        private RichTextBox richTextBox_FlowsData;
        private TableLayoutPanel tableLayoutPanel5;
        private ScottPlot.WinForms.FormsPlot formsPlot_RFValues;
        private ComboBox comboBox_RFMode;
        private Button button_ScottPlotClear;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label6;
        private ComboBox comboBox_FileMark;
        private Label label7;
        private ComboBox comboBox_excludedFolders;
        private Label label_Upper;
        private Label label_Lower;
        private TextBox textBox_LowerLimit;
        private TextBox textBox_UpperLimit;
        private Label label5;
        private Label label_CPK;
        private Button button_CPK;
        private ToolTip toolTip_MSG;
        private GroupBox groupBox5;
        private TableLayoutPanel tableLayoutPanel7;
        private TextBox textBox_SelectFile_Md5;
        private RichTextBox richTextBox_FileMd5;
        private Button btn_LogCollectSettings;
    }
}