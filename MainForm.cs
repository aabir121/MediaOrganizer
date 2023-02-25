namespace MediaOrganizer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void directoryChooserBtn_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select a folder to organize media files";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sourceDirTxtBox.Text = dialog.SelectedPath;
                    organizeBtn.Enabled = true;
                }
            }
        }

        private void organizeBtn_Click(object sender, EventArgs e)
        {
            directoryChooserBtn.Enabled = false;
            organizeBtn.Enabled = false;
            txtLog.Text = "";

            // Run the media organizer in a separate task to avoid blocking the UI thread
            Task.Run(() => FileOrganizer.OrganizeMediaFiles(sourceDirTxtBox.Text, (percentage, message) =>
            {
                // Update progress bar, label, and logs on the UI thread
                this.Invoke(new Action(() =>
                {
                    ReportProgress(percentage, message);
                }));
            })).ContinueWith(task =>
            {
                // Do something after the async operation is complete
                MessageBox.Show("Media files organized!");
                directoryChooserBtn.Enabled = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ReportProgress(int percentage, string message)
        {
            // Update the progress bar and label
            progressBar.Value = percentage;
            lblProgress.Text = $"{percentage}%";

            // Update the logs
            txtLog.AppendText($"{message}{Environment.NewLine}");
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }
    }
}