namespace Liteon_TestProgram.Forms
{
    partial class CaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            textBox_NGSample_PWD = new TextBox();
            comboBox_Case = new ComboBox();
            label1 = new Label();
            label_PWD = new Label();
            btn_LoadCase = new Button();
            btn_NGSample = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            button_Close = new Button();
            button_Max = new Button();
            button_Min = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            label2 = new Label();
            checkBox_LogCollection = new CheckBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.181818F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54.54545F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27.2727242F));
            tableLayoutPanel1.Controls.Add(textBox_NGSample_PWD, 1, 2);
            tableLayoutPanel1.Controls.Add(comboBox_Case, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Controls.Add(label_PWD, 0, 2);
            tableLayoutPanel1.Controls.Add(btn_LoadCase, 2, 1);
            tableLayoutPanel1.Controls.Add(btn_NGSample, 2, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 2, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Location = new Point(2, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25.67648F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 39.1887627F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 35.1347542F));
            tableLayoutPanel1.Size = new Size(528, 156);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.MouseDown += Form_Base_MouseDown;
            // 
            // textBox_NGSample_PWD
            // 
            textBox_NGSample_PWD.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox_NGSample_PWD.BackColor = Color.FromArgb(45, 45, 49);
            textBox_NGSample_PWD.BorderStyle = BorderStyle.FixedSingle;
            textBox_NGSample_PWD.Font = new Font("Microsoft YaHei UI", 15.75F);
            textBox_NGSample_PWD.ForeColor = SystemColors.HighlightText;
            textBox_NGSample_PWD.Location = new Point(99, 111);
            textBox_NGSample_PWD.Name = "textBox_NGSample_PWD";
            textBox_NGSample_PWD.PasswordChar = '*';
            textBox_NGSample_PWD.Size = new Size(282, 34);
            textBox_NGSample_PWD.TabIndex = 0;
            textBox_NGSample_PWD.TextAlign = HorizontalAlignment.Center;
            textBox_NGSample_PWD.Visible = false;
            // 
            // comboBox_Case
            // 
            comboBox_Case.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            comboBox_Case.BackColor = Color.FromArgb(45, 45, 49);
            comboBox_Case.FlatStyle = FlatStyle.Flat;
            comboBox_Case.Font = new Font("Microsoft YaHei UI", 15.75F);
            comboBox_Case.ForeColor = SystemColors.HighlightText;
            comboBox_Case.FormattingEnabled = true;
            comboBox_Case.Location = new Point(99, 52);
            comboBox_Case.Name = "comboBox_Case";
            comboBox_Case.Size = new Size(282, 36);
            comboBox_Case.Sorted = true;
            comboBox_Case.TabIndex = 1;
            comboBox_Case.DropDown += comboBox_Case_DropDown;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("微软雅黑", 15.75F);
            label1.ForeColor = Color.FromArgb(0, 193, 255);
            label1.Location = new Point(3, 56);
            label1.Name = "label1";
            label1.Size = new Size(90, 28);
            label1.TabIndex = 2;
            label1.Text = "机种：";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_PWD
            // 
            label_PWD.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label_PWD.AutoSize = true;
            label_PWD.Font = new Font("微软雅黑", 15.75F);
            label_PWD.ForeColor = Color.FromArgb(0, 193, 255);
            label_PWD.Location = new Point(3, 114);
            label_PWD.Name = "label_PWD";
            label_PWD.Size = new Size(90, 28);
            label_PWD.TabIndex = 2;
            label_PWD.Text = "密码：";
            label_PWD.TextAlign = ContentAlignment.MiddleCenter;
            label_PWD.Visible = false;
            // 
            // btn_LoadCase
            // 
            btn_LoadCase.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btn_LoadCase.BackColor = Color.FromArgb(0, 193, 255);
            btn_LoadCase.FlatAppearance.BorderSize = 0;
            btn_LoadCase.FlatStyle = FlatStyle.Flat;
            btn_LoadCase.Font = new Font("Microsoft YaHei UI", 15.75F);
            btn_LoadCase.Location = new Point(387, 47);
            btn_LoadCase.Name = "btn_LoadCase";
            btn_LoadCase.Size = new Size(138, 47);
            btn_LoadCase.TabIndex = 3;
            btn_LoadCase.Text = "加载机种";
            btn_LoadCase.UseVisualStyleBackColor = false;
            btn_LoadCase.Click += btn_LoadCase_Click;
            // 
            // btn_NGSample
            // 
            btn_NGSample.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btn_NGSample.BackColor = Color.FromArgb(0, 193, 255);
            btn_NGSample.FlatAppearance.BorderSize = 0;
            btn_NGSample.FlatStyle = FlatStyle.Flat;
            btn_NGSample.Font = new Font("Microsoft YaHei UI", 10.5F);
            btn_NGSample.Location = new Point(387, 105);
            btn_NGSample.Name = "btn_NGSample";
            btn_NGSample.Size = new Size(138, 47);
            btn_NGSample.TabIndex = 3;
            btn_NGSample.Text = "离线测试(待开发)";
            btn_NGSample.UseVisualStyleBackColor = false;
            btn_NGSample.Click += btn_NGSample_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(button_Close, 2, 0);
            tableLayoutPanel2.Controls.Add(button_Max, 1, 0);
            tableLayoutPanel2.Controls.Add(button_Min, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(387, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(138, 34);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // button_Close
            // 
            button_Close.BackgroundImage = (Image)resources.GetObject("button_Close.BackgroundImage");
            button_Close.BackgroundImageLayout = ImageLayout.Zoom;
            button_Close.Dock = DockStyle.Fill;
            button_Close.FlatAppearance.BorderSize = 0;
            button_Close.FlatStyle = FlatStyle.Flat;
            button_Close.Location = new Point(95, 3);
            button_Close.Name = "button_Close";
            button_Close.Size = new Size(40, 28);
            button_Close.TabIndex = 2;
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
            button_Max.Location = new Point(49, 3);
            button_Max.Name = "button_Max";
            button_Max.Size = new Size(40, 28);
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
            button_Min.Location = new Point(3, 3);
            button_Min.Name = "button_Min";
            button_Min.Size = new Size(40, 28);
            button_Min.TabIndex = 0;
            button_Min.UseVisualStyleBackColor = true;
            button_Min.Click += button_Min_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel3, 2);
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Controls.Add(label2, 0, 0);
            tableLayoutPanel3.Controls.Add(checkBox_LogCollection, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(378, 34);
            tableLayoutPanel3.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Verdana", 18F, FontStyle.Bold);
            label2.ForeColor = Color.CornflowerBlue;
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(242, 34);
            label2.TabIndex = 0;
            label2.Text = "==机种选择==";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            label2.MouseDown += Form_Base_MouseDown;
            // 
            // checkBox_LogCollection
            // 
            checkBox_LogCollection.AutoSize = true;
            checkBox_LogCollection.Dock = DockStyle.Fill;
            checkBox_LogCollection.FlatStyle = FlatStyle.Flat;
            checkBox_LogCollection.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBox_LogCollection.ForeColor = SystemColors.ControlLight;
            checkBox_LogCollection.Location = new Point(251, 3);
            checkBox_LogCollection.Name = "checkBox_LogCollection";
            checkBox_LogCollection.Size = new Size(124, 28);
            checkBox_LogCollection.TabIndex = 1;
            checkBox_LogCollection.Text = "Log整理模式";
            checkBox_LogCollection.UseVisualStyleBackColor = true;
            checkBox_LogCollection.CheckedChanged += checkBox_LogCollection_CheckedChanged;
            // 
            // CaseForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 50, 58);
            ClientSize = new Size(539, 170);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CaseForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CaseForm";
            TopMost = true;
            Load += CaseForm_Load;
            MouseDown += Form_Base_MouseDown;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TextBox textBox_NGSample_PWD;
        private ComboBox comboBox_Case;
        private Label label1;
        private Label label_PWD;
        private Button btn_LoadCase;
        private Button btn_NGSample;
        private TableLayoutPanel tableLayoutPanel2;
        private Button button_Close;
        private Button button_Max;
        private Button button_Min;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label2;
        private CheckBox checkBox_LogCollection;
    }
}