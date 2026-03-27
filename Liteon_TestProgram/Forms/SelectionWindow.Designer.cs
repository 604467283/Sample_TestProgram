namespace Liteon_TestProgram.CaseProject
{
    partial class SelectionWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectionWindow));
            tableLayoutPanel1 = new TableLayoutPanel();
            label_Title = new Label();
            comboBox_Items = new ComboBox();
            btn_OK = new Button();
            btn_Exit = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(29, 29, 29);
            tableLayoutPanel1.ColumnCount = 7;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857113F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel1.Controls.Add(label_Title, 0, 0);
            tableLayoutPanel1.Controls.Add(comboBox_Items, 1, 2);
            tableLayoutPanel1.Controls.Add(btn_OK, 2, 4);
            tableLayoutPanel1.Controls.Add(btn_Exit, 6, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 18.87755F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 11.22449F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 17.8571434F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.65306139F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 28.5714283F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.7959185F));
            tableLayoutPanel1.Size = new Size(319, 196);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.MouseDown += Form_Base_MouseDown;
            // 
            // label_Title
            // 
            label_Title.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(label_Title, 6);
            label_Title.Dock = DockStyle.Fill;
            label_Title.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_Title.ForeColor = Color.CornflowerBlue;
            label_Title.Location = new Point(3, 0);
            label_Title.Name = "label_Title";
            label_Title.Size = new Size(264, 37);
            label_Title.TabIndex = 0;
            label_Title.Text = "Title";
            label_Title.TextAlign = ContentAlignment.MiddleLeft;
            label_Title.MouseDown += Form_Base_MouseDown;
            // 
            // comboBox_Items
            // 
            comboBox_Items.BackColor = Color.FromArgb(45, 45, 49);
            tableLayoutPanel1.SetColumnSpan(comboBox_Items, 5);
            comboBox_Items.Dock = DockStyle.Fill;
            comboBox_Items.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Items.FlatStyle = FlatStyle.Flat;
            comboBox_Items.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox_Items.ForeColor = SystemColors.HighlightText;
            comboBox_Items.FormattingEnabled = true;
            comboBox_Items.Location = new Point(48, 62);
            comboBox_Items.Name = "comboBox_Items";
            comboBox_Items.Size = new Size(219, 29);
            comboBox_Items.TabIndex = 1;
            // 
            // btn_OK
            // 
            btn_OK.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel1.SetColumnSpan(btn_OK, 3);
            btn_OK.Dock = DockStyle.Fill;
            btn_OK.FlatAppearance.BorderSize = 0;
            btn_OK.FlatStyle = FlatStyle.Flat;
            btn_OK.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_OK.ForeColor = Color.FromArgb(229, 192, 123);
            btn_OK.Image = (Image)resources.GetObject("btn_OK.Image");
            btn_OK.Location = new Point(93, 112);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new Size(129, 50);
            btn_OK.TabIndex = 0;
            btn_OK.Text = "确定";
            btn_OK.TextImageRelation = TextImageRelation.TextBeforeImage;
            btn_OK.UseVisualStyleBackColor = false;
            btn_OK.Click += btn_OK_Click;
            // 
            // btn_Exit
            // 
            btn_Exit.BackgroundImage = (Image)resources.GetObject("btn_Exit.BackgroundImage");
            btn_Exit.BackgroundImageLayout = ImageLayout.Zoom;
            btn_Exit.Dock = DockStyle.Fill;
            btn_Exit.FlatAppearance.BorderSize = 0;
            btn_Exit.FlatStyle = FlatStyle.Flat;
            btn_Exit.Location = new Point(273, 3);
            btn_Exit.Name = "btn_Exit";
            btn_Exit.Size = new Size(43, 31);
            btn_Exit.TabIndex = 3;
            btn_Exit.UseVisualStyleBackColor = true;
            btn_Exit.Click += btn_Exit_Click;
            // 
            // SelectionWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 50, 58);
            ClientSize = new Size(319, 196);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SelectionWindow";
            StartPosition = FormStartPosition.CenterParent;
            Text = "NOAH_PC";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label_Title;
        private ComboBox comboBox_Items;
        private Button btn_OK;
        private Button btn_Exit;
    }
}