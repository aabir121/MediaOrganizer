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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1665, 1095);
            this.Controls.Add(this.sourceDirTxtBox);
            this.Controls.Add(this.directoryChooserBtn);
            this.Name = "MainForm";
            this.Text = "Media Organizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button directoryChooserBtn;
        private TextBox sourceDirTxtBox;
    }
}