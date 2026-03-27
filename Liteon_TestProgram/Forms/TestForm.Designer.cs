namespace Liteon_TestProgram.Forms
{
    partial class TestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel8 = new TableLayoutPanel();
            label_BT = new Label();
            textBox_BT = new TextBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            label_Mac = new Label();
            textBox_Mac = new TextBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            label_FPY = new Label();
            label_FPYNum = new Label();
            tableLayoutPanel4 = new TableLayoutPanel();
            label_FAIL = new Label();
            label_FailNum = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            label_PASS = new Label();
            label_PassNum = new Label();
            tableLayoutPanel6 = new TableLayoutPanel();
            label_SN = new Label();
            textBox_SN = new TextBox();
            label_Status = new Label();
            btn_Test = new Button();
            btn_Config = new Button();
            btn_log = new Button();
            splitContainer1 = new SplitContainer();
            groupBox_TestItems = new GroupBox();
            dataGridView_item = new DataGridView();
            index = new DataGridViewTextBoxColumn();
            item = new DataGridViewTextBoxColumn();
            data = new DataGridViewTextBoxColumn();
            Time = new DataGridViewTextBoxColumn();
            result = new DataGridViewTextBoxColumn();
            tableLayoutPanel9 = new TableLayoutPanel();
            groupBox_TestInfo = new GroupBox();
            richTextBox_log = new RichTextBox();
            label_TestTime = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox_TestItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_item).BeginInit();
            tableLayoutPanel9.SuspendLayout();
            groupBox_TestInfo.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(46, 50, 58);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(splitContainer1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 78.84097F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 21.159029F));
            tableLayoutPanel1.Size = new Size(1147, 742);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.4069347F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26.3876247F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26.5594978F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21.64594F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel8, 2, 1);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel7, 1, 1);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel5, 2, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel4, 1, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel6, 0, 1);
            tableLayoutPanel2.Controls.Add(label_Status, 0, 2);
            tableLayoutPanel2.Controls.Add(btn_Test, 3, 2);
            tableLayoutPanel2.Controls.Add(btn_Config, 3, 1);
            tableLayoutPanel2.Controls.Add(btn_log, 3, 0);
            tableLayoutPanel2.Location = new Point(3, 588);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 32.203392F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 34.7457657F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.05085F));
            tableLayoutPanel2.Size = new Size(1141, 151);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 2;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel8.Controls.Add(label_BT, 0, 0);
            tableLayoutPanel8.Controls.Add(textBox_BT, 1, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(593, 51);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.Size = new Size(297, 46);
            tableLayoutPanel8.TabIndex = 8;
            // 
            // label_BT
            // 
            label_BT.AutoSize = true;
            label_BT.Dock = DockStyle.Fill;
            label_BT.ForeColor = Color.FromArgb(0, 193, 255);
            label_BT.Location = new Point(3, 0);
            label_BT.Name = "label_BT";
            label_BT.Size = new Size(83, 46);
            label_BT.TabIndex = 0;
            label_BT.Text = "BT:";
            label_BT.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_BT
            // 
            textBox_BT.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_BT.BackColor = Color.FromArgb(45, 45, 49);
            textBox_BT.BorderStyle = BorderStyle.FixedSingle;
            textBox_BT.Font = new Font("Microsoft YaHei UI", 9F);
            textBox_BT.ForeColor = SystemColors.Control;
            textBox_BT.Location = new Point(92, 11);
            textBox_BT.Name = "textBox_BT";
            textBox_BT.Size = new Size(202, 23);
            textBox_BT.TabIndex = 1;
            textBox_BT.TextAlign = HorizontalAlignment.Center;
            textBox_BT.TextChanged += textBox_BT_TextChanged;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel7.Controls.Add(label_Mac, 0, 0);
            tableLayoutPanel7.Controls.Add(textBox_Mac, 1, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(292, 51);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Size = new Size(295, 46);
            tableLayoutPanel7.TabIndex = 7;
            // 
            // label_Mac
            // 
            label_Mac.AutoSize = true;
            label_Mac.Dock = DockStyle.Fill;
            label_Mac.ForeColor = Color.FromArgb(0, 193, 255);
            label_Mac.Location = new Point(3, 0);
            label_Mac.Name = "label_Mac";
            label_Mac.Size = new Size(82, 46);
            label_Mac.TabIndex = 0;
            label_Mac.Text = "Mac:";
            label_Mac.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_Mac
            // 
            textBox_Mac.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_Mac.BackColor = Color.FromArgb(45, 45, 49);
            textBox_Mac.BorderStyle = BorderStyle.FixedSingle;
            textBox_Mac.Font = new Font("Microsoft YaHei UI", 9F);
            textBox_Mac.ForeColor = SystemColors.Control;
            textBox_Mac.Location = new Point(91, 11);
            textBox_Mac.Name = "textBox_Mac";
            textBox_Mac.Size = new Size(201, 23);
            textBox_Mac.TabIndex = 1;
            textBox_Mac.TextAlign = HorizontalAlignment.Center;
            textBox_Mac.TextChanged += textBox_Mac_TextChanged;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel5.Controls.Add(label_FPY, 0, 0);
            tableLayoutPanel5.Controls.Add(label_FPYNum, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(593, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(297, 42);
            tableLayoutPanel5.TabIndex = 2;
            // 
            // label_FPY
            // 
            label_FPY.AutoSize = true;
            label_FPY.Dock = DockStyle.Fill;
            label_FPY.ForeColor = Color.FromArgb(0, 193, 255);
            label_FPY.Location = new Point(3, 0);
            label_FPY.Name = "label_FPY";
            label_FPY.Size = new Size(83, 42);
            label_FPY.TabIndex = 0;
            label_FPY.Text = "FPY:";
            label_FPY.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_FPYNum
            // 
            label_FPYNum.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label_FPYNum.AutoSize = true;
            label_FPYNum.ForeColor = Color.FromArgb(0, 193, 255);
            label_FPYNum.Location = new Point(92, 12);
            label_FPYNum.Name = "label_FPYNum";
            label_FPYNum.Size = new Size(202, 17);
            label_FPYNum.TabIndex = 1;
            label_FPYNum.Text = "0%";
            label_FPYNum.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel4.Controls.Add(label_FAIL, 0, 0);
            tableLayoutPanel4.Controls.Add(label_FailNum, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(292, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(295, 42);
            tableLayoutPanel4.TabIndex = 1;
            // 
            // label_FAIL
            // 
            label_FAIL.AutoSize = true;
            label_FAIL.Dock = DockStyle.Fill;
            label_FAIL.FlatStyle = FlatStyle.Flat;
            label_FAIL.ForeColor = Color.FromArgb(0, 193, 255);
            label_FAIL.Location = new Point(3, 0);
            label_FAIL.Name = "label_FAIL";
            label_FAIL.Size = new Size(82, 42);
            label_FAIL.TabIndex = 0;
            label_FAIL.Text = "FAIL:";
            label_FAIL.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_FailNum
            // 
            label_FailNum.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label_FailNum.AutoSize = true;
            label_FailNum.FlatStyle = FlatStyle.Flat;
            label_FailNum.ForeColor = Color.FromArgb(0, 193, 255);
            label_FailNum.Location = new Point(91, 12);
            label_FailNum.Name = "label_FailNum";
            label_FailNum.Size = new Size(201, 17);
            label_FailNum.TabIndex = 1;
            label_FailNum.Text = "0";
            label_FailNum.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel3.Controls.Add(label_PASS, 0, 0);
            tableLayoutPanel3.Controls.Add(label_PassNum, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(283, 42);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // label_PASS
            // 
            label_PASS.AutoSize = true;
            label_PASS.Dock = DockStyle.Fill;
            label_PASS.ForeColor = Color.FromArgb(0, 193, 255);
            label_PASS.Location = new Point(3, 0);
            label_PASS.Name = "label_PASS";
            label_PASS.Size = new Size(78, 42);
            label_PASS.TabIndex = 0;
            label_PASS.Text = "PASS:";
            label_PASS.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_PassNum
            // 
            label_PassNum.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label_PassNum.AutoSize = true;
            label_PassNum.ForeColor = Color.FromArgb(0, 193, 255);
            label_PassNum.Location = new Point(87, 12);
            label_PassNum.Name = "label_PassNum";
            label_PassNum.Size = new Size(193, 17);
            label_PassNum.TabIndex = 1;
            label_PassNum.Text = "0";
            label_PassNum.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 2;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel6.Controls.Add(label_SN, 0, 0);
            tableLayoutPanel6.Controls.Add(textBox_SN, 1, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 51);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Size = new Size(283, 46);
            tableLayoutPanel6.TabIndex = 6;
            // 
            // label_SN
            // 
            label_SN.AutoSize = true;
            label_SN.Dock = DockStyle.Fill;
            label_SN.ForeColor = Color.FromArgb(0, 193, 255);
            label_SN.Location = new Point(3, 0);
            label_SN.Name = "label_SN";
            label_SN.Size = new Size(78, 46);
            label_SN.TabIndex = 0;
            label_SN.Text = "SN:";
            label_SN.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_SN
            // 
            textBox_SN.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_SN.BackColor = Color.FromArgb(45, 45, 49);
            textBox_SN.BorderStyle = BorderStyle.FixedSingle;
            textBox_SN.Font = new Font("Microsoft YaHei UI", 9F);
            textBox_SN.ForeColor = SystemColors.Control;
            textBox_SN.Location = new Point(87, 11);
            textBox_SN.Name = "textBox_SN";
            textBox_SN.Size = new Size(193, 23);
            textBox_SN.TabIndex = 1;
            textBox_SN.TextAlign = HorizontalAlignment.Center;
            textBox_SN.TextChanged += textBox_SN_TextChanged;
            // 
            // label_Status
            // 
            label_Status.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label_Status.AutoSize = true;
            label_Status.BackColor = SystemColors.AppWorkspace;
            label_Status.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel2.SetColumnSpan(label_Status, 3);
            label_Status.Font = new Font("Microsoft YaHei UI", 21.75F, FontStyle.Bold);
            label_Status.Location = new Point(3, 105);
            label_Status.Name = "label_Status";
            label_Status.Size = new Size(887, 41);
            label_Status.TabIndex = 9;
            label_Status.Text = "WAIT";
            label_Status.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btn_Test
            // 
            btn_Test.BackColor = Color.FromArgb(37, 42, 100);
            btn_Test.Dock = DockStyle.Fill;
            btn_Test.FlatAppearance.BorderSize = 0;
            btn_Test.FlatStyle = FlatStyle.Flat;
            btn_Test.ForeColor = Color.FromArgb(229, 192, 123);
            btn_Test.Image = (Image)resources.GetObject("btn_Test.Image");
            btn_Test.Location = new Point(896, 103);
            btn_Test.Name = "btn_Test";
            btn_Test.Size = new Size(242, 45);
            btn_Test.TabIndex = 3;
            btn_Test.Text = "开始测试";
            btn_Test.TextImageRelation = TextImageRelation.TextBeforeImage;
            btn_Test.UseVisualStyleBackColor = false;
            btn_Test.Click += btn_Test_Click;
            // 
            // btn_Config
            // 
            btn_Config.BackColor = Color.FromArgb(37, 42, 100);
            btn_Config.Dock = DockStyle.Fill;
            btn_Config.FlatAppearance.BorderSize = 0;
            btn_Config.FlatStyle = FlatStyle.Flat;
            btn_Config.ForeColor = Color.FromArgb(229, 192, 123);
            btn_Config.Image = (Image)resources.GetObject("btn_Config.Image");
            btn_Config.Location = new Point(896, 51);
            btn_Config.Name = "btn_Config";
            btn_Config.Size = new Size(242, 46);
            btn_Config.TabIndex = 5;
            btn_Config.Text = "测试配置";
            btn_Config.TextImageRelation = TextImageRelation.TextBeforeImage;
            btn_Config.UseVisualStyleBackColor = false;
            btn_Config.Click += btn_Config_Click;
            // 
            // btn_log
            // 
            btn_log.BackColor = Color.FromArgb(37, 42, 100);
            btn_log.Dock = DockStyle.Fill;
            btn_log.FlatAppearance.BorderSize = 0;
            btn_log.FlatStyle = FlatStyle.Flat;
            btn_log.ForeColor = Color.FromArgb(229, 192, 123);
            btn_log.Image = (Image)resources.GetObject("btn_log.Image");
            btn_log.Location = new Point(896, 3);
            btn_log.Name = "btn_log";
            btn_log.Size = new Size(242, 42);
            btn_log.TabIndex = 4;
            btn_log.Text = "测试LOG";
            btn_log.TextImageRelation = TextImageRelation.TextBeforeImage;
            btn_log.UseVisualStyleBackColor = false;
            btn_log.Click += btn_log_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox_TestItems);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel9);
            splitContainer1.Size = new Size(1141, 579);
            splitContainer1.SplitterDistance = 287;
            splitContainer1.TabIndex = 1;
            // 
            // groupBox_TestItems
            // 
            groupBox_TestItems.BackColor = Color.FromArgb(46, 50, 58);
            groupBox_TestItems.Controls.Add(dataGridView_item);
            groupBox_TestItems.Dock = DockStyle.Fill;
            groupBox_TestItems.FlatStyle = FlatStyle.Flat;
            groupBox_TestItems.ForeColor = SystemColors.Control;
            groupBox_TestItems.Location = new Point(0, 0);
            groupBox_TestItems.Name = "groupBox_TestItems";
            groupBox_TestItems.Size = new Size(1141, 287);
            groupBox_TestItems.TabIndex = 0;
            groupBox_TestItems.TabStop = false;
            groupBox_TestItems.Text = "测试项目";
            // 
            // dataGridView_item
            // 
            dataGridView_item.AllowUserToAddRows = false;
            dataGridView_item.AllowUserToDeleteRows = false;
            dataGridView_item.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView_item.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_item.BackgroundColor = Color.FromArgb(46, 50, 58);
            dataGridView_item.BorderStyle = BorderStyle.None;
            dataGridView_item.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(46, 50, 58);
            dataGridViewCellStyle1.Font = new Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView_item.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView_item.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_item.Columns.AddRange(new DataGridViewColumn[] { index, item, data, Time, result });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(46, 50, 58);
            dataGridViewCellStyle2.Font = new Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.Control;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView_item.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView_item.Dock = DockStyle.Fill;
            dataGridView_item.EnableHeadersVisualStyles = false;
            dataGridView_item.Location = new Point(3, 19);
            dataGridView_item.Name = "dataGridView_item";
            dataGridView_item.ReadOnly = true;
            dataGridView_item.RowHeadersVisible = false;
            dataGridView_item.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView_item.Size = new Size(1135, 265);
            dataGridView_item.TabIndex = 0;
            // 
            // index
            // 
            index.FillWeight = 10F;
            index.HeaderText = "编号";
            index.MinimumWidth = 50;
            index.Name = "index";
            index.ReadOnly = true;
            index.Width = 57;
            // 
            // item
            // 
            item.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            item.FillWeight = 10F;
            item.HeaderText = "项目";
            item.Name = "item";
            item.ReadOnly = true;
            item.SortMode = DataGridViewColumnSortMode.NotSortable;
            item.Width = 38;
            // 
            // data
            // 
            data.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            data.FillWeight = 76.07843F;
            data.HeaderText = "数据";
            data.MinimumWidth = 50;
            data.Name = "data";
            data.ReadOnly = true;
            data.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Time
            // 
            Time.HeaderText = "用时";
            Time.Name = "Time";
            Time.ReadOnly = true;
            Time.SortMode = DataGridViewColumnSortMode.NotSortable;
            Time.Width = 38;
            // 
            // result
            // 
            result.FillWeight = 3.92157F;
            result.HeaderText = "结果";
            result.MinimumWidth = 70;
            result.Name = "result";
            result.ReadOnly = true;
            result.SortMode = DataGridViewColumnSortMode.NotSortable;
            result.Width = 70;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.BackColor = Color.FromArgb(46, 50, 58);
            tableLayoutPanel9.ColumnCount = 1;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.Controls.Add(groupBox_TestInfo, 0, 1);
            tableLayoutPanel9.Controls.Add(label_TestTime, 0, 0);
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Location = new Point(0, 0);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 2;
            tableLayoutPanel9.RowStyles.Add(new RowStyle());
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.Size = new Size(1141, 288);
            tableLayoutPanel9.TabIndex = 0;
            // 
            // groupBox_TestInfo
            // 
            groupBox_TestInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox_TestInfo.Controls.Add(richTextBox_log);
            groupBox_TestInfo.FlatStyle = FlatStyle.Flat;
            groupBox_TestInfo.ForeColor = SystemColors.Control;
            groupBox_TestInfo.Location = new Point(3, 20);
            groupBox_TestInfo.Name = "groupBox_TestInfo";
            groupBox_TestInfo.Size = new Size(1135, 265);
            groupBox_TestInfo.TabIndex = 0;
            groupBox_TestInfo.TabStop = false;
            groupBox_TestInfo.Text = "测试信息";
            // 
            // richTextBox_log
            // 
            richTextBox_log.BackColor = Color.FromArgb(46, 50, 58);
            richTextBox_log.BorderStyle = BorderStyle.None;
            richTextBox_log.Dock = DockStyle.Fill;
            richTextBox_log.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox_log.ForeColor = SystemColors.Control;
            richTextBox_log.Location = new Point(3, 19);
            richTextBox_log.Name = "richTextBox_log";
            richTextBox_log.ReadOnly = true;
            richTextBox_log.Size = new Size(1129, 243);
            richTextBox_log.TabIndex = 0;
            richTextBox_log.Text = "";
            // 
            // label_TestTime
            // 
            label_TestTime.Anchor = AnchorStyles.Left;
            label_TestTime.AutoSize = true;
            label_TestTime.BackColor = Color.FromArgb(46, 50, 58);
            label_TestTime.ForeColor = SystemColors.Control;
            label_TestTime.Location = new Point(3, 0);
            label_TestTime.Name = "label_TestTime";
            label_TestTime.Size = new Size(68, 17);
            label_TestTime.TabIndex = 1;
            label_TestTime.Text = "测试时间：";
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1147, 742);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "TestForm";
            Text = "TestForm";
            Load += TestForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel8.PerformLayout();
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox_TestItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_item).EndInit();
            tableLayoutPanel9.ResumeLayout(false);
            tableLayoutPanel9.PerformLayout();
            groupBox_TestInfo.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel5;
        private Label label_FPY;
        private Label label_FPYNum;
        private TableLayoutPanel tableLayoutPanel4;
        private Label label_FAIL;
        private Label label_FailNum;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label_PASS;
        private Label label_PassNum;
        private TableLayoutPanel tableLayoutPanel8;
        private Label label_BT;
        private TextBox textBox_BT;
        private TableLayoutPanel tableLayoutPanel7;
        private Label label_Mac;
        private TextBox textBox_Mac;
        private Button btn_Test;
        private Button btn_log;
        private Button btn_Config;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label_SN;
        private TextBox textBox_SN;
        private Label label_Status;
        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel9;
        private GroupBox groupBox_TestInfo;
        private RichTextBox richTextBox_log;
        private Label label_TestTime;
        private GroupBox groupBox_TestItems;
        private DataGridView dataGridView_item;
        private DataGridViewTextBoxColumn index;
        private DataGridViewTextBoxColumn item;
        private DataGridViewTextBoxColumn data;
        private DataGridViewTextBoxColumn Time;
        private DataGridViewTextBoxColumn result;
    }
}