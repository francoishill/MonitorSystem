namespace MonitorSystem
{
	partial class MonitoredFilesChanged
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.textBox_Description = new System.Windows.Forms.RichTextBox();
			this.contextMenu_TotalFile = new System.Windows.Forms.ContextMenu();
			this.menuItem_ClearMessages = new System.Windows.Forms.MenuItem();
			this.menuItem_DiscardEmpty = new System.Windows.Forms.MenuItem();
			this.contextMenu_FileModification = new System.Windows.Forms.ContextMenu();
			this.menuItem_ClearThisMessage = new System.Windows.Forms.MenuItem();
			this.menuItem_Accept = new System.Windows.Forms.MenuItem();
			this.menuItem_Discard = new System.Windows.Forms.MenuItem();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.richTextBox_FileContents = new ScintillaNet.Scintilla();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.richTextBox_FileContents)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textBox_Description);
			this.splitContainer1.Size = new System.Drawing.Size(558, 459);
			this.splitContainer1.SplitterDistance = 138;
			this.splitContainer1.TabIndex = 2;
			this.splitContainer1.TabStop = false;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.ShowLines = false;
			this.treeView1.Size = new System.Drawing.Size(558, 138);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			this.treeView1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MonitoredFilesChanged_PreviewKeyDown);
			// 
			// textBox_Description
			// 
			this.textBox_Description.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox_Description.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
			this.textBox_Description.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
			this.textBox_Description.Location = new System.Drawing.Point(0, 0);
			this.textBox_Description.Name = "textBox_Description";
			this.textBox_Description.Size = new System.Drawing.Size(558, 317);
			this.textBox_Description.TabIndex = 1;
			this.textBox_Description.Text = "";
			this.textBox_Description.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.textBox_Description.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_Description_KeyDown);
			this.textBox_Description.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MonitoredFilesChanged_PreviewKeyDown);
			// 
			// contextMenu_TotalFile
			// 
			this.contextMenu_TotalFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_ClearMessages,
            this.menuItem_DiscardEmpty});
			// 
			// menuItem_ClearMessages
			// 
			this.menuItem_ClearMessages.Checked = true;
			this.menuItem_ClearMessages.Index = 0;
			this.menuItem_ClearMessages.Text = "&Clear messages";
			this.menuItem_ClearMessages.Click += new System.EventHandler(this.menuItem_ClearMessages_Click);
			// 
			// menuItem_DiscardEmpty
			// 
			this.menuItem_DiscardEmpty.Index = 1;
			this.menuItem_DiscardEmpty.Text = "Discard &empty";
			this.menuItem_DiscardEmpty.Click += new System.EventHandler(this.menuItem_DiscardEmpty_Click);
			// 
			// contextMenu_FileModification
			// 
			this.contextMenu_FileModification.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_ClearThisMessage,
            this.menuItem_Accept,
            this.menuItem_Discard});
			// 
			// menuItem_ClearThisMessage
			// 
			this.menuItem_ClearThisMessage.Index = 0;
			this.menuItem_ClearThisMessage.Text = "Clear &this message";
			this.menuItem_ClearThisMessage.Click += new System.EventHandler(this.menuItem_ClearThisMessage_Click);
			// 
			// menuItem_Accept
			// 
			this.menuItem_Accept.Index = 1;
			this.menuItem_Accept.Text = "&Accept";
			this.menuItem_Accept.Click += new System.EventHandler(this.menuItem_Accept_Click);
			// 
			// menuItem_Discard
			// 
			this.menuItem_Discard.Index = 2;
			this.menuItem_Discard.Text = "Dis&card";
			this.menuItem_Discard.Click += new System.EventHandler(this.menuItem_Discard_Click);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer2.Location = new System.Drawing.Point(12, 12);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.richTextBox_FileContents);
			this.splitContainer2.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.splitContainer2.Size = new System.Drawing.Size(1093, 459);
			this.splitContainer2.SplitterDistance = 558;
			this.splitContainer2.TabIndex = 3;
			// 
			// richTextBox_FileContents
			// 
			this.richTextBox_FileContents.ConfigurationManager.Language = "mssql";
			this.richTextBox_FileContents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox_FileContents.Folding.Flags = ((ScintillaNet.FoldFlag)((ScintillaNet.FoldFlag.LineBeforeContracted | ScintillaNet.FoldFlag.LineAfterContracted)));
			this.richTextBox_FileContents.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
			this.richTextBox_FileContents.IsBraceMatching = true;
			this.richTextBox_FileContents.IsReadOnly = true;
			this.richTextBox_FileContents.LineWrap.Mode = ScintillaNet.WrapMode.Word;
			this.richTextBox_FileContents.LineWrap.VisualFlags = ((ScintillaNet.WrapVisualFlag)((ScintillaNet.WrapVisualFlag.End | ScintillaNet.WrapVisualFlag.Start)));
			this.richTextBox_FileContents.Location = new System.Drawing.Point(0, 0);
			this.richTextBox_FileContents.Margins.FoldMarginColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.richTextBox_FileContents.Margins.Margin0.Width = 30;
			this.richTextBox_FileContents.Margins.Margin2.Width = 20;
			this.richTextBox_FileContents.Name = "richTextBox_FileContents";
			this.richTextBox_FileContents.Size = new System.Drawing.Size(531, 459);
			this.richTextBox_FileContents.TabIndex = 1;
			this.richTextBox_FileContents.UseFont = true;
			this.richTextBox_FileContents.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MonitoredFilesChanged_PreviewKeyDown);
			// 
			// MonitoredFilesChanged
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1117, 483);
			this.Controls.Add(this.splitContainer2);
			this.KeyPreview = true;
			this.Name = "MonitoredFilesChanged";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MonitoredFilesChanged";
			this.Shown += new System.EventHandler(this.MonitoredFilesChanged_Shown);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MonitoredFilesChanged_PreviewKeyDown);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.richTextBox_FileContents)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		public System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.MenuItem menuItem_ClearMessages;
		private System.Windows.Forms.MenuItem menuItem_DiscardEmpty;
		public System.Windows.Forms.ContextMenu contextMenu_TotalFile;
		public System.Windows.Forms.ContextMenu contextMenu_FileModification;
		private System.Windows.Forms.MenuItem menuItem_ClearThisMessage;
		private System.Windows.Forms.MenuItem menuItem_Accept;
		private System.Windows.Forms.MenuItem menuItem_Discard;
		private System.Windows.Forms.SplitContainer splitContainer2;
		public System.Windows.Forms.RichTextBox textBox_Description;
		private ScintillaNet.Scintilla richTextBox_FileContents;
	}
}