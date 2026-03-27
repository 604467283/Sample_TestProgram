namespace Liteon_TestProgram.Forms
{
    partial class MsgBoxInfoDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgBoxInfoDetails));
            tableLayoutPanel1 = new TableLayoutPanel();
            richTextBox_Msg = new RichTextBox();
            btn_Quit = new Button();
            label_Title = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80.5460739F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19.4539242F));
            tableLayoutPanel1.Controls.Add(richTextBox_Msg, 0, 1);
            tableLayoutPanel1.Controls.Add(btn_Quit, 1, 0);
            tableLayoutPanel1.Controls.Add(label_Title, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 13.1034479F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 86.89655F));
            tableLayoutPanel1.Size = new Size(493, 296);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // richTextBox_Msg
            // 
            richTextBox_Msg.BackColor = Color.FromArgb(29, 29, 29);
            richTextBox_Msg.BorderStyle = BorderStyle.None;
            tableLayoutPanel1.SetColumnSpan(richTextBox_Msg, 2);
            richTextBox_Msg.Dock = DockStyle.Fill;
            richTextBox_Msg.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox_Msg.ForeColor = SystemColors.HighlightText;
            richTextBox_Msg.Location = new Point(3, 41);
            richTextBox_Msg.Name = "richTextBox_Msg";
            richTextBox_Msg.ReadOnly = true;
            richTextBox_Msg.Size = new Size(487, 252);
            richTextBox_Msg.TabIndex = 0;
            richTextBox_Msg.Text = "";
            // 
            // btn_Quit
            // 
            btn_Quit.BackgroundImage = (Image)resources.GetObject("btn_Quit.BackgroundImage");
            btn_Quit.BackgroundImageLayout = ImageLayout.Zoom;
            btn_Quit.Dock = DockStyle.Fill;
            btn_Quit.FlatAppearance.BorderSize = 0;
            btn_Quit.FlatStyle = FlatStyle.Flat;
            btn_Quit.Location = new Point(400, 3);
            btn_Quit.Name = "btn_Quit";
            btn_Quit.Size = new Size(90, 32);
            btn_Quit.TabIndex = 1;
            btn_Quit.UseVisualStyleBackColor = true;
            btn_Quit.Click += btn_Quit_Click;
            // 
            // label_Title
            // 
            label_Title.AutoSize = true;
            label_Title.Dock = DockStyle.Fill;
            label_Title.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_Title.ForeColor = Color.CornflowerBlue;
            label_Title.Location = new Point(3, 0);
            label_Title.Name = "label_Title";
            label_Title.Size = new Size(391, 38);
            label_Title.TabIndex = 2;
            label_Title.Text = "Title";
            label_Title.TextAlign = ContentAlignment.MiddleLeft;
            label_Title.MouseDown += Form_Base_MouseDown;
            // 
            // MsgBoxInfoDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(29, 29, 29);
            ClientSize = new Size(493, 296);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MsgBoxInfoDetails";
            StartPosition = FormStartPosition.CenterParent;
            Text = "MsgBoxInfoDetails";
            TopMost = true;
            MouseDown += Form_Base_MouseDown;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private RichTextBox richTextBox_Msg;
        private Button btn_Quit;
        private Label label_Title;
    }
}