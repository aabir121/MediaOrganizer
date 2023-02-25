namespace MediaOrganizer
{
    public partial class MainForm : Form
    {
        // Create a CancellationTokenSource to allow for cancelling the task
        CancellationTokenSource cts = new CancellationTokenSource();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If the task is still running, cancel it
            if (!cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
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
            if (organizeBtn.Text == "Cancel")
            {
                cts.Cancel();
                resetUi();
                return;
            }

            directoryChooserBtn.Enabled = false;
            txtLog.Text = "";
            organizeBtn.Text = "Cancel";

            // Run the media organizer in a separate task to avoid blocking the UI thread
            Task.Run(() => FileOrganizer.OrganizeMediaFiles(sourceDirTxtBox.Text, (percentage, message) =>
            {
                // Update progress bar, label, and logs on the UI thread
                this.Invoke(new Action(() =>
                {
                    ReportProgress(percentage, message);
                }));
            }, cts.Token)).ContinueWith(task =>
            {
                // Do something after the async operation is complete
                if (cts.IsCancellationRequested)
                {
                    MessageBox.Show("Organizer was cancelled!");
                    resetUi();
                }
                else
                {
                    MessageBox.Show("Media files organized!");
                    organizeBtn.Text = "Start";
                    directoryChooserBtn.Enabled = true;
                }
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

        private void resetUi()
        {
            this.txtLog.Text = "";
            this.organizeBtn.Text = "Start";
            this.progressBar.Value = 0;
            this.lblProgress.Text = "0%";
            directoryChooserBtn.Enabled = true;
        }
    }
}