using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaOrganizer
{
    public partial class MainForm : Form
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public MainForm()
        {
            InitializeComponent();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            await Task.Delay(50);
        }

        private void directoryChooserBtn_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Select a folder to organize media files"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                sourceDirTxtBox.Text = dialog.SelectedPath;
                organizeBtn.Enabled = true;
                renameSimilarChecked.Enabled = true;
                removeEmpty.Enabled = true;
            }
        }

        private void organizeBtn_Click(object sender, EventArgs e)
        {
            if (organizeBtn.Text == "Cancel")
            {
                _cancellationTokenSource.Cancel();
                ResetUi();
                return;
            }

            directoryChooserBtn.Enabled = false;
            txtLog.Text = "";
            organizeBtn.Text = "Cancel";
            progressBar.Style = ProgressBarStyle.Continuous;

            Task.Run(() => FileOrganizer.OrganizeMediaFiles(
                GetRunConfiguration(),
                (percentage, message) => ReportProgress(percentage, message),
                _cancellationTokenSource.Token))
            .ContinueWith(task => HandleOrganizeMediaFilesCompletion(task), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void HandleOrganizeMediaFilesCompletion(Task task)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                MessageBox.Show("Organizer was cancelled!");
                ResetUi();
            }
            else if (task.IsFaulted)
            {
                MessageBox.Show($"An error occurred during the operation: {task?.Exception?.InnerException?.Message}");
                ResetUi();
            }
            else
            {
                MessageBox.Show("Media files organized!");
                ResetUi(true);
            }
        }

        private RunConfig GetRunConfiguration()
        {
            RunConfig config = new()
            {
                SourceDirectory = sourceDirTxtBox.Text,
                RenameSimilar = renameSimilarChecked.Checked,
                RemoveEmptyDirectory = removeEmpty.Checked
            };

            return config;
        }

        private void ReportProgress(int percentage, string message)
        {
            progressBar.Invoke(new Action(() =>
            {
                progressBar.Value = percentage;
            }));

            using (var graphics = progressBar.CreateGraphics())
            {
                var font = new Font("Arial", 10, FontStyle.Italic);
                var brush = new SolidBrush(Color.Black);

                var text = $"{percentage}%";
                var textSize = graphics.MeasureString(text, font);

                var x = (progressBar.Width / 2) - (textSize.Width / 2);
                var y = (progressBar.Height / 2) - (textSize.Height / 2);

                graphics.DrawString(text, font, brush, x, y);
            }

            txtLog.Invoke(new Action(() =>
            {
                txtLog.AppendText($"{message}{Environment.NewLine}");
                txtLog.SelectionStart = txtLog.Text.Length;
                txtLog.ScrollToCaret();
            }));
        }

        private void ResetUi(bool retainLogs = false)
        {
            if (!retainLogs)
            {
                txtLog.Clear();
            }
            organizeBtn.Text = "Start";
            directoryChooserBtn.Enabled = true;
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 0;
            removeEmpty.Enabled = false;
            renameSimilarChecked.Enabled = false;
        }
    }
}
