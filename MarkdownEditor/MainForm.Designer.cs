using Microsoft.Web.WebView2.WinForms;

namespace MarkdownEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip = new MenuStrip();
            fToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            exportToHTMLToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            previewCheckBox = new CheckBox();
            richTextBox = new RichTextBox();
            webView = new WebView2();
            
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) webView).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Dock = DockStyle.None;
            menuStrip.Items.AddRange(new ToolStripItem[] { fToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(45, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // fToolStripMenuItem
            // 
            fToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, exportToHTMLToolStripMenuItem, exitToolStripMenuItem });
            fToolStripMenuItem.Name = "fToolStripMenuItem";
            fToolStripMenuItem.Size = new Size(37, 20);
            fToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(180, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(180, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(180, 22);
            saveAsToolStripMenuItem.Text = "Save as";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // exportToHTMLToolStripMenuItem
            // 
            exportToHTMLToolStripMenuItem.Name = "exportToHTMLToolStripMenuItem";
            exportToHTMLToolStripMenuItem.Size = new Size(180, 22);
            exportToHTMLToolStripMenuItem.Text = "Export to HTML";
            exportToHTMLToolStripMenuItem.Click += exportToHTMLToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // previewCheckBox
            // 
            previewCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            previewCheckBox.AutoSize = true;
            previewCheckBox.Location = new Point(672, 5);
            previewCheckBox.Name = "previewCheckBox";
            previewCheckBox.Size = new Size(67, 19);
            previewCheckBox.TabIndex = 1;
            previewCheckBox.Text = "Preview";
            previewCheckBox.UseVisualStyleBackColor = true;
            previewCheckBox.AutoCheck = true;
            previewCheckBox.CheckedChanged += previewCheckBox_CheckedChanged;
            // 
            // richTextBox
            // 
            richTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox.Location = new Point(0, 27);
            richTextBox.Name = "richTextBox";
            richTextBox.Size = new Size(739, 603);
            richTextBox.TabIndex = 2;
            richTextBox.Text = "";
            richTextBox.Visible = true;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Location = new Point(0, 27);
            webView.Name = "webView";
            webView.Size = new Size(739, 603);
            webView.TabIndex = 2;
            webView.Visible = false;            
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(739, 631);
            Controls.Add(menuStrip);
            Controls.Add(previewCheckBox);            
            Controls.Add(richTextBox);
            Controls.Add(webView);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            Text = "MarkdownEditor - New Document";
            Load += MainForm_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) webView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem exportToHTMLToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private CheckBox previewCheckBox;
        private RichTextBox richTextBox;
        private WebView2 webView;

        private string? curentFile = null;
        private string curentFileName = "New Document";
        private string defaultFileExtension = "md";

        private void ChangeWindowText(string? filePath)
        {
            if (filePath == null)
            {
                curentFile = null;
                curentFileName = "New Document";
            }
            else
            {
                curentFile = Path.GetFullPath(filePath);
                curentFileName = Path.GetFileNameWithoutExtension(filePath);
            }
            Text = $"MarkdownEditor - {curentFileName}";
        }
    }
}
