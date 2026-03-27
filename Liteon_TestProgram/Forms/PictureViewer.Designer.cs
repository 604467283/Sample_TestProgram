namespace Liteon_TestProgram.Forms
{
    partial class PictureViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PictureViewer));
            tableLayoutPanel1 = new TableLayoutPanel();
            btn_Close = new Button();
            btn_Max = new Button();
            lb_Title = new Label();
            btn_Min = new Button();
            pictureBox = new PictureBox();
            btn_No = new Button();
            btn_Yes = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.Controls.Add(btn_Close, 9, 0);
            tableLayoutPanel1.Controls.Add(btn_Max, 8, 0);
            tableLayoutPanel1.Controls.Add(lb_Title, 0, 0);
            tableLayoutPanel1.Controls.Add(btn_Min, 7, 0);
            tableLayoutPanel1.Controls.Add(pictureBox, 0, 2);
            tableLayoutPanel1.Controls.Add(btn_No, 1, 15);
            tableLayoutPanel1.Controls.Add(btn_Yes, 7, 15);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 18;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.55555534F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.779661F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.779661F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.779661F));
            tableLayoutPanel1.Size = new Size(527, 431);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_Close
            // 
            btn_Close.BackgroundImage = (Image)resources.GetObject("btn_Close.BackgroundImage");
            btn_Close.BackgroundImageLayout = ImageLayout.Zoom;
            btn_Close.Dock = DockStyle.Fill;
            btn_Close.FlatAppearance.BorderSize = 0;
            btn_Close.FlatStyle = FlatStyle.Flat;
            btn_Close.Location = new Point(471, 3);
            btn_Close.Name = "btn_Close";
            tableLayoutPanel1.SetRowSpan(btn_Close, 2);
            btn_Close.Size = new Size(53, 40);
            btn_Close.TabIndex = 3;
            btn_Close.Text = "button3";
            btn_Close.UseVisualStyleBackColor = true;
            btn_Close.Click += btn_Close_Click;
            // 
            // btn_Max
            // 
            btn_Max.BackgroundImage = (Image)resources.GetObject("btn_Max.BackgroundImage");
            btn_Max.BackgroundImageLayout = ImageLayout.Zoom;
            btn_Max.Dock = DockStyle.Fill;
            btn_Max.FlatAppearance.BorderSize = 0;
            btn_Max.FlatStyle = FlatStyle.Flat;
            btn_Max.Location = new Point(419, 3);
            btn_Max.Name = "btn_Max";
            tableLayoutPanel1.SetRowSpan(btn_Max, 2);
            btn_Max.Size = new Size(46, 40);
            btn_Max.TabIndex = 2;
            btn_Max.UseVisualStyleBackColor = true;
            btn_Max.Click += btn_Max_Click;
            // 
            // lb_Title
            // 
            lb_Title.AutoSize = true;
            lb_Title.BackColor = Color.FromArgb(46, 50, 58);
            tableLayoutPanel1.SetColumnSpan(lb_Title, 7);
            lb_Title.Dock = DockStyle.Fill;
            lb_Title.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lb_Title.ForeColor = Color.CornflowerBlue;
            lb_Title.Location = new Point(3, 0);
            lb_Title.Name = "lb_Title";
            tableLayoutPanel1.SetRowSpan(lb_Title, 2);
            lb_Title.Size = new Size(358, 46);
            lb_Title.TabIndex = 0;
            lb_Title.Text = "Title";
            lb_Title.TextAlign = ContentAlignment.MiddleLeft;
            lb_Title.MouseDown += Form_Base_MouseDown;
            // 
            // btn_Min
            // 
            btn_Min.BackgroundImage = (Image)resources.GetObject("btn_Min.BackgroundImage");
            btn_Min.BackgroundImageLayout = ImageLayout.Zoom;
            btn_Min.Dock = DockStyle.Fill;
            btn_Min.FlatAppearance.BorderSize = 0;
            btn_Min.FlatStyle = FlatStyle.Flat;
            btn_Min.Location = new Point(367, 3);
            btn_Min.Name = "btn_Min";
            tableLayoutPanel1.SetRowSpan(btn_Min, 2);
            btn_Min.Size = new Size(46, 40);
            btn_Min.TabIndex = 1;
            btn_Min.UseVisualStyleBackColor = true;
            btn_Min.Click += btn_Min_Click;
            // 
            // pictureBox
            // 
            tableLayoutPanel1.SetColumnSpan(pictureBox, 10);
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Location = new Point(3, 49);
            pictureBox.Name = "pictureBox";
            tableLayoutPanel1.SetRowSpan(pictureBox, 13);
            pictureBox.Size = new Size(521, 293);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 4;
            pictureBox.TabStop = false;
            // 
            // btn_No
            // 
            btn_No.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel1.SetColumnSpan(btn_No, 2);
            btn_No.Dock = DockStyle.Fill;
            btn_No.FlatAppearance.BorderSize = 0;
            btn_No.FlatStyle = FlatStyle.Flat;
            btn_No.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_No.ForeColor = Color.FromArgb(229, 192, 123);
            btn_No.Image = (Image)resources.GetObject("btn_No.Image");
            btn_No.Location = new Point(55, 348);
            btn_No.Name = "btn_No";
            tableLayoutPanel1.SetRowSpan(btn_No, 2);
            btn_No.Size = new Size(98, 50);
            btn_No.TabIndex = 5;
            btn_No.Text = "NO";
            btn_No.TextImageRelation = TextImageRelation.TextBeforeImage;
            btn_No.UseVisualStyleBackColor = false;
            btn_No.Click += btn_No_Click;
            // 
            // btn_Yes
            // 
            btn_Yes.BackColor = Color.FromArgb(37, 42, 100);
            tableLayoutPanel1.SetColumnSpan(btn_Yes, 2);
            btn_Yes.Dock = DockStyle.Fill;
            btn_Yes.FlatAppearance.BorderSize = 0;
            btn_Yes.FlatStyle = FlatStyle.Flat;
            btn_Yes.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Yes.ForeColor = Color.FromArgb(229, 192, 123);
            btn_Yes.Image = (Image)resources.GetObject("btn_Yes.Image");
            btn_Yes.Location = new Point(367, 348);
            btn_Yes.Name = "btn_Yes";
            tableLayoutPanel1.SetRowSpan(btn_Yes, 2);
            btn_Yes.Size = new Size(98, 50);
            btn_Yes.TabIndex = 6;
            btn_Yes.Text = "YES";
            btn_Yes.TextImageRelation = TextImageRelation.TextBeforeImage;
            btn_Yes.UseVisualStyleBackColor = false;
            btn_Yes.Click += btn_Yes_Click;
            // 
            // PictureViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(527, 431);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "PictureViewer";
            StartPosition = FormStartPosition.CenterParent;
            Text = "PictureViewer";
            TopMost = true;
            Load += PictureViewer_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label lb_Title;
        private Button btn_Close;
        private Button btn_Max;
        private Button btn_Min;
        private PictureBox pictureBox;
        private Button btn_No;
        private Button btn_Yes;
    }
}