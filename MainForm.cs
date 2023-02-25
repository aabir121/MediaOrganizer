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
                }
            }
        }
    }
}