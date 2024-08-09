using Markdig;

namespace MarkdownEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            webView.EnsureCoreWebView2Async(null);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                shortcutForSave();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void shortcutForSave()
        {
            if (curentFile == null)
            {
                saveAsToolStripMenuItem_Click();
            }
            else
            {
                try
                {
                    File.WriteAllText(curentFile, richTextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curentFile == null)
            {
                saveAsToolStripMenuItem_Click(sender, e);
            } 
            else
            {
                try
                {
                    File.WriteAllText(curentFile, richTextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsToolStripMenuItem_Click();
        }
        private void saveAsToolStripMenuItem_Click()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Markdown files (*.md)|*.md";
                saveFileDialog.DefaultExt = "md";
                saveFileDialog.AddExtension = true;
                saveFileDialog.Title = "Save markdown file";
                saveFileDialog.FileName = curentFileName;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = saveFileDialog.FileName;
                        File.WriteAllText(filePath, richTextBox.Text);
                        MessageBox.Show("File saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ChangeWindowText(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Markdown files (*.md)|*.md";
                openFileDialog.DefaultExt = "md";
                openFileDialog.AddExtension = true;
                openFileDialog.Title = "Open markdown file";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;
                        richTextBox.Text = File.ReadAllText(filePath);
                        ChangeWindowText(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        
        private void exportToHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "HTML files (*.html)|*.html";
                saveFileDialog.DefaultExt = "html";
                saveFileDialog.AddExtension = true;
                saveFileDialog.Title = "Save html file";
                saveFileDialog.FileName = curentFileName;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = saveFileDialog.FileName;
                        File.WriteAllText(filePath, Markdown.ToHtml(richTextBox.Text));
                        MessageBox.Show("Export successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting a file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void previewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (previewCheckBox.Checked)
            {
                string markdownText = richTextBox.Text;
                string htmlText = Markdown.ToHtml(markdownText);
                webView.NavigateToString(htmlText);
                richTextBox.Visible = false;
                webView.Visible = true;
            }
            else
            {
                webView.NavigateToString("");
                richTextBox.Visible = true;
                webView.Visible = false;
            }
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeWindowText(null);
            richTextBox.Text = ""; 
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
