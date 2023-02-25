namespace MediaOrganizer
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
            this.directoryChooserBtn = new System.Windows.Forms.Button();
            this.sourceDirTxtBox = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.organizeBtn = new System.Windows.Forms.Button();
            this.renameSimilarChecked = new System.Windows.Forms.CheckBox();
            this.removeEmpty = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // directoryChooserBtn
            // 
            this.directoryChooserBtn.Location = new System.Drawing.Point(1160, 53);
            this.directoryChooserBtn.Name = "directoryChooserBtn";
            this.directoryChooserBtn.Size = new System.Drawing.Size(453, 58);
            this.directoryChooserBtn.TabIndex = 1;
            this.directoryChooserBtn.Text = "Choose Directory";
            this.directoryChooserBtn.UseVisualStyleBackColor = true;
            this.directoryChooserBtn.Click += new System.EventHandler(this.directoryChooserBtn_Click);
            // 
            // sourceDirTxtBox
            // 
            this.sourceDirTxtBox.CausesValidation = false;
            this.sourceDirTxtBox.Location = new System.Drawing.Point(40, 59);
            this.sourceDirTxtBox.Name = "sourceDirTxtBox";
            this.sourceDirTxtBox.PlaceholderText = "Enter folder path to organize images and videos";
            this.sourceDirTxtBox.ReadOnly = true;
            this.sourceDirTxtBox.Size = new System.Drawing.Size(1095, 47);
            this.sourceDirTxtBox.TabIndex = 2;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(41, 217);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1572, 58);
            this.progressBar.TabIndex = 3;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(43, 320);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1570, 698);
            this.txtLog.TabIndex = 5;
            // 
            // organizeBtn
            // 
            this.organizeBtn.Enabled = false;
            this.organizeBtn.Location = new System.Drawing.Point(43, 138);
            this.organizeBtn.Name = "organizeBtn";
            this.organizeBtn.Size = new System.Drawing.Size(295, 58);
            this.organizeBtn.TabIndex = 6;
            this.organizeBtn.Text = "Start";
            this.organizeBtn.UseVisualStyleBackColor = true;
            this.organizeBtn.Click += new System.EventHandler(this.organizeBtn_Click);
            // 
            // renameSimilarChecked
            // 
            this.renameSimilarChecked.AutoSize = true;
            this.renameSimilarChecked.Enabled = false;
            this.renameSimilarChecked.Location = new System.Drawing.Point(365, 147);
            this.renameSimilarChecked.Name = "renameSimilarChecked";
            this.renameSimilarChecked.Size = new System.Drawing.Size(259, 45);
            this.renameSimilarChecked.TabIndex = 7;
            this.renameSimilarChecked.Text = "Rename Similar";
            this.renameSimilarChecked.UseVisualStyleBackColor = true;
            // 
            // removeEmpty
            // 
            this.removeEmpty.AutoSize = true;
            this.removeEmpty.Checked = true;
            this.removeEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removeEmpty.Enabled = false;
            this.removeEmpty.Location = new System.Drawing.Point(652, 147);
            this.removeEmpty.Name = "removeEmpty";
            this.removeEmpty.Size = new System.Drawing.Size(405, 45);
            this.removeEmpty.TabIndex = 8;
            this.removeEmpty.Text = "Remove Empty Directories";
            this.removeEmpty.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1665, 1095);
            this.Controls.Add(this.removeEmpty);
            this.Controls.Add(this.renameSimilarChecked);
            this.Controls.Add(this.organizeBtn);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.sourceDirTxtBox);
            this.Controls.Add(this.directoryChooserBtn);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Media Organizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button directoryChooserBtn;
        private TextBox sourceDirTxtBox;
        private ProgressBar progressBar;
        private TextBox txtLog;
        private Button organizeBtn;
        private CheckBox renameSimilarChecked;
        private CheckBox removeEmpty;
    }
}