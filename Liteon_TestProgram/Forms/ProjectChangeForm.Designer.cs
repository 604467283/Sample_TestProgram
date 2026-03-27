namespace Liteon_TestProgram.Forms
{
    partial class ProjectChangeForm
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
            richTextBox_ChangeList = new RichTextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            label_ProjectName = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox_ChangeList
            // 
            richTextBox_ChangeList.BackColor = SystemColors.WindowText;
            richTextBox_ChangeList.BorderStyle = BorderStyle.None;
            richTextBox_ChangeList.Dock = DockStyle.Fill;
            richTextBox_ChangeList.ForeColor = SystemColors.ControlLightLight;
            richTextBox_ChangeList.Location = new Point(3, 49);
            richTextBox_ChangeList.Name = "richTextBox_ChangeList";
            richTextBox_ChangeList.ReadOnly = true;
            richTextBox_ChangeList.Size = new Size(794, 398);
            richTextBox_ChangeList.TabIndex = 0;
            richTextBox_ChangeList.Text = "";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(richTextBox_ChangeList, 0, 1);
            tableLayoutPanel1.Controls.Add(label_ProjectName, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10.227273F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 89.7727356F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label_ProjectName
            // 
            label_ProjectName.AutoSize = true;
            label_ProjectName.BackColor = Color.FromArgb(46, 50, 58);
            label_ProjectName.Dock = DockStyle.Fill;
            label_ProjectName.FlatStyle = FlatStyle.Flat;
            label_ProjectName.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_ProjectName.Location = new Point(3, 0);
            label_ProjectName.Name = "label_ProjectName";
            label_ProjectName.Size = new Size(794, 46);
            label_ProjectName.TabIndex = 1;
            label_ProjectName.Text = "  <变更记录>";
            label_ProjectName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ProjectChangeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            ForeColor = SystemColors.ControlLightLight;
            FormBorderStyle = FormBorderStyle.None;
            Name = "ProjectChangeForm";
            Text = "ProjectChangeForm";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox_ChangeList;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label_ProjectName;
    }
}